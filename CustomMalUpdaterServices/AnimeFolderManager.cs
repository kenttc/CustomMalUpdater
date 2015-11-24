using CustomMalUpdaterDomain;
using CustomMalUpdaterDomain.Interfaces;

namespace CustomMalUpdaterServices
{
    using System;
    using System.Net;
    using UtilityImplementations.Extensions;

    public class AnimeFolderManager : IAnimeFolderManager
    {
        private readonly string _baseFolderPath;
        private readonly ILogger _logger;

        public AnimeFolderManager(string baseFolderPath, ILogger logger)
        {
            _baseFolderPath = baseFolderPath;
            _logger = logger;
        }

        public string GetSeasonString(DateTime? date)
        {
            if (date == null) return "";
            var monthNumber = date.Value.Month;
            switch (monthNumber)
            {
                case 1:
                    return "01_winter";
                    break;
                case 2:
                    return "01_winter";
                    break;
                case 3:
                    return "01_winter";
                    break;
                case 4:
                    return "02_spring";
                    break;
                case 5:
                    return "02_spring";
                    break;
                case 6:
                    return "02_spring";
                    break;
                case 7:
                    return "03_summer";
                    break;
                case 8:
                    return "03_summer";
                    break;
                case 9:
                    return "03_summer";
                    break;
                case 10:
                    return "04_fall";
                    break;
                case 11:
                    return "04_fall";
                    break;
                case 12:
                    return "04_fall";
                    break;
            }
            return "";
        }

        public string FolderPath(string basePath, int year, string season, string animeTitleFolderPath, string animeType)
        {
            return string.Format("{0}{1}\\{2}\\{3} ({4})", basePath, year, season, animeTitleFolderPath, animeType);
        }

        public string CreateFolder(AnimeEntry anime)
        {
            var animeStartDate = Convert.ToDateTime(anime.StrStartDate);
            var year = animeStartDate.Year;
            var season = this.GetSeasonString(animeStartDate);
            var folderPath = anime.Title.ValidAnimeFolder();
            var animeType = anime.AnimeType;

            var pathToCreate = this.FolderPath(_baseFolderPath, year, season, folderPath, animeType);

            if (!System.IO.Directory.Exists(pathToCreate))
                System.IO.Directory.CreateDirectory(pathToCreate);

            return pathToCreate;
        }

        public bool CreateTextFile(string pathToCreate, string content, bool overwrite)
        {
            if (overwrite && System.IO.File.Exists(pathToCreate))
                System.IO.File.Delete(pathToCreate);

            System.IO.File.WriteAllText(pathToCreate, content);

            return true;
        }

        public bool DownloadImageFile(string imageUrl, string fullImageFilePath, string username = "",
                                      string password = "")
        {
            using (var wc = new WebClient())
            {
                try
                {
                    if (!(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)))
                        wc.Credentials = new NetworkCredential(username, password);

                    wc.DownloadFile(imageUrl, fullImageFilePath);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.Log(ex.Message);
                    return false;
                }
            }
        }
    }
}
