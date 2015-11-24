
using CustomMalUpdaterDomain.Interfaces;

namespace CustomMalUpdater
{
    using System;
    using CustomMalUpdaterServices;
    using UtilityImplementations;

    internal class Program
    {
        private static string _username = System.Configuration.ConfigurationManager.AppSettings["username"];
        private static string _password = System.Configuration.ConfigurationManager.AppSettings["password"];
        private static string _basepath = System.Configuration.ConfigurationManager.AppSettings["basepath"];
        
        private static int _lowerNumberToSleep =
            Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["lowerNumberToSleep"]);

        private static int _upperNumberToSleep =
            Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["upperNumberToSleep"]);
        private static string _animeSynopsisFileName = System.Configuration.ConfigurationManager.AppSettings["SynopsisFileName"];
        
        private static ILogger _logger;
        private static IAnimeRepo _animeRepo;
        private static AnimeManager _animeManager;
        private static IAnimeFolderManager _animeFolderManager;
        

        private static void Initialisation()
        {
            _animeRepo = new AnimeRepo();
            _logger = new ConsoleLogger();
            _animeFolderManager = new AnimeFolderManager(_basepath, _logger);
            _animeManager = new AnimeManager(new MalDataParser(new XmlSerializerAndDeserializer())
                , _logger, _animeRepo, _username, _password, _animeFolderManager
                , new AnimeSynopsisManager(_animeFolderManager)
                ,_animeSynopsisFileName);
        }


        private static void Main(string[] args)
        {
            //var query = args[0];
            Initialisation();
            _animeManager.ProcessNewSearchTerms(_lowerNumberToSleep, _upperNumberToSleep);
            _animeManager.UpdateAnimeDetailsFromApi(_lowerNumberToSleep, _upperNumberToSleep);
            _animeManager.PrepFolders(_lowerNumberToSleep, _upperNumberToSleep);

            Console.WriteLine("Done. press any key to exit");
            Console.Read();
        }
    }

    internal class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(DateTime.Now + " " +  message);
        }
    }
}
