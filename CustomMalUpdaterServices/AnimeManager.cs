    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
    using CustomMalUpdaterDomain;
    using CustomMalUpdaterDomain.Interfaces;
    using UtilityImplementations.Extensions;

namespace CustomMalUpdaterServices
{
    public class AnimeManager
    {
        
        private readonly IMalDataParser _malDataParser;
        private readonly ILogger _logger;
        private readonly IAnimeRepo _animeRepo;
        private readonly string _username;
        private readonly string _password;
        private readonly IAnimeFolderManager _animeFolderManager;
        private readonly IAnimeSynopsisManager _animeSynopsisManager;
        private readonly string _synopsisFileName;

        public AnimeManager(IMalDataParser malDataParser, ILogger logger,
                            IAnimeRepo animeRepo
                            , string username, string password,IAnimeFolderManager animeFolderManager
                           ,IAnimeSynopsisManager animeSynopsisManager, string synopsisFileName )
        {
            
            _malDataParser = malDataParser;
            _logger = logger;
            _animeRepo = animeRepo;
            _username = username;
            _password = password;
            _animeFolderManager = animeFolderManager;
            _animeSynopsisManager = animeSynopsisManager;
            _synopsisFileName = synopsisFileName;
        }

        public void ProcessNewSearchTerms(int lowerNumberToSleep, int upperNumberToSleep )
        {
            var titles = _animeRepo.GetSearchTerms();


            foreach (var title in titles)
            {
                var entries = this.ProcessSearch(title, _username, _password);
                _logger.Log("Found: " + entries.Count() + " for " + title);
                var count = this.AddNewEntries(entries);
                _logger.Log("Added: " + count + " for " + title + "  to db.");

                ThreadSleep(lowerNumberToSleep, upperNumberToSleep);
                
    
            }
        }


        public void UpdateAnimeDetailsFromApi(int lowerNumberToSleep, int upperNumberToSleep)
        {
            var dbObjs = _animeRepo.GetDetailsToUpdate();
            foreach (var anime in dbObjs)
            {
                var objs = this.ProcessSearch(anime.Title, _username, _password);
                var newObj = objs.FirstOrDefault(x => x.Id == anime.Id);
                if (newObj == null) continue;
                _animeRepo.UpdateDetails(newObj);
                
                ThreadSleep(lowerNumberToSleep, upperNumberToSleep);
                
            }
        }

        private  void ThreadSleep(int lowerNumberToSleep, int upperNumberToSleep)
        {
            if (lowerNumberToSleep == 0 && upperNumberToSleep == 0) return;
            
            var rnd = new Random();
            var num= rnd.Next(Convert.ToInt32(lowerNumberToSleep), Convert.ToInt32(upperNumberToSleep));
            System.Threading.Thread.Sleep(num * 1000);
            _logger.Log("Slept for " + num + " secs");
        }
        
        
        /// <summary>
        /// does a search on the api and parses them into anime entries objects
        /// </summary>
        /// <param name="query"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<AnimeEntry> ProcessSearch(string query, string username, string password)
        {
            var url = "http://myanimelist.net/api/anime/search.xml?q=" + query;
            var urlData = "";
            using (var wc = new WebClient())
            {
                if (!(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)))
                    wc.Credentials = new NetworkCredential(username, password);

                wc.Encoding = new UTF8Encoding();

                urlData = wc.DownloadString(url);
            }
            /*here to mark downloaded?*/
            _animeRepo.MarkQueryDownloaded(query);

            if (string.IsNullOrEmpty(urlData))
            {
                _logger.Log("unable to get data for " + query);
                return new List<AnimeEntry>();
            }

            var container = _malDataParser.ParseFromXml(urlData);
            _logger.Log("Found: " + container.Entries.Count() + " for " + query);


            return container.Entries.ToList();
        }

        public int AddNewEntries(List<AnimeEntry> entries)
        {
            var notInDbs = _animeRepo.GetNotInDb(entries);
            
            if (notInDbs.Count == 0) return 0;

            return _animeRepo.AddEntries(notInDbs);
        }


        public void PrepFolders(int lowerNumberToSleep, int upperNumberToSleep)
    {
        var animes = _animeRepo.GetNewEntriesToGenerate();

        foreach (var anime in animes)
        {
            

            var pathToCreate = _animeFolderManager.CreateFolder(anime);

            _logger.Log(DateTime.Now + " generating " + pathToCreate + " " + anime.Title);

            var synopsisContent = _animeSynopsisManager.PlainText(anime);

            var created = _animeFolderManager.CreateTextFile(pathToCreate + "\\" + _synopsisFileName, synopsisContent,
                                                                 anime.SynopsisFileLastGenerated == null);

            _animeRepo.UpdateSynopsisFileLastGenerated(anime.Id, DateTime.Now);

            var imageFileName = anime.ImageUrl.Split('/').Last();
            var fullImageFilePath = pathToCreate + "/" + imageFileName;

            if (System.IO.File.Exists(fullImageFilePath)) continue;

            var downloaded = _animeFolderManager.DownloadImageFile(anime.ImageUrl, fullImageFilePath);

            ThreadSleep(lowerNumberToSleep, upperNumberToSleep);
        }
    }


      


    }
}