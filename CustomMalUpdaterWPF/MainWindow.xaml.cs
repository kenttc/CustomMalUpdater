
namespace CustomMalUpdaterWPF
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using CustomMalUpdaterDomain;
    using CustomMalUpdaterDomain.Interfaces;
    using CustomMalUpdaterServices;
    using UtilityImplementations;
    using UtilityImplementations.Extensions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _username = System.Configuration.ConfigurationManager.AppSettings["username"];
        private static string _password = System.Configuration.ConfigurationManager.AppSettings["password"];
        private static string _basePath = System.Configuration.ConfigurationManager.AppSettings["basepath"].ToString();
        private static string _animeSynopsisFileName = System.Configuration.ConfigurationManager.AppSettings["SynopsisFileName"];
        private static IAnimeFolderManager _animeFolderManager;
        private ILogger _logger;
        private IAnimeRepo _animeRepo;
        private AnimeManager _animeManager;

        public MainWindow()
        {
            InitializeComponent();
            CreateAutoMapperMaps();
            _logger = new TextLogger(_basePath + "CustomMalUpdaterWPF.log");
            _animeFolderManager = new AnimeFolderManager(_basePath, _logger);
            _animeRepo = new AnimeRepo();

            
            
            _animeManager = new AnimeManager(new MalDataParser(new XmlSerializerAndDeserializer())
                , _logger, _animeRepo, _username, _password, _animeFolderManager
                , new AnimeSynopsisManager(_animeFolderManager)
                , _animeSynopsisFileName);
        }

        private void CreateAutoMapperMaps()
        {
            AutoMapper.Mapper.CreateMap<AnimeEntry, AnimeResult>()
                      .ForMember(x => x.Year, opt => opt.MapFrom(src => src.StrStartDate.GetValidDate().Year))
                      .ForMember(x => x.PathToCreate,
                                 opt =>
                                 opt.MapFrom(src => _animeFolderManager
                                                        .FolderPath(_basePath, src.StrStartDate.GetValidDate().Year,
                                                                    _animeFolderManager.GetSeasonString(
                                                                        src.StrStartDate.GetValidDate()),
                                                                    src.Title.ValidAnimeFolder(),
                                                                    src.AnimeType)))
                      .ForMember(x => x.Synopsis,
                                 opt =>
                                 opt.MapFrom(src => "English title:" 
                                     + src.EnglishTitle + "\r\nSynonyms: " 
                                     + src.Synonyms + "\t" + "Rating: " + src.Score + "\r\n" + src.Synopsis));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        private void DoSearch()
        {
            var query = TextBoxSearch.Text;
            if (string.IsNullOrEmpty(query))
            {
                LblMessage.Content = "Empty query";
                return;
            }
            ListViewResult.Items.Clear();
            lblAnimeId.Content = "";

            var list = _animeRepo.Search(query);

            if (list.Count == 0)
                LblMessage.Content = "no results found";

            foreach (var anime in list)
            {
                var animeEntity = AutoMapper.Mapper.Map<AnimeResult>(anime);
                ListViewResult.Items.Add(animeEntity);
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                //Do your stuff
                var obj = (AnimeResult) ListViewResult.SelectedItems[0];

                TextBoxFolderPath.Text = (obj.PathToCreate);
                TextboxSynopsis.Text = obj.Synopsis;
                lblAnimeId.Content = obj.Id;
                Clipboard.SetText((obj.PathToCreate));
            }
        }

        private void TextBoxFolderPath_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFolderPath.Text)) return;
            Clipboard.SetText(TextBoxFolderPath.Text);
        }

        private void TextBoxSearch_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBoxSearch.Text = "";
        }

        private void TextBoxSearch_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DoSearch();
        }

        private void SaveSearchTerm(object sender, RoutedEventArgs e)
        {
            if (TextBoxSearch.Text.Trim().Length > 0)
            {
                LblMessage.Content = _animeRepo.AddNewSearchTerm(TextBoxSearch.Text.Trim());
                _animeManager.ProcessNewSearchTerms(1,2);
                _animeManager.PrepFolders(1,2);
                LblMessage.Content = "processed new items";

            }
                
        }

        private void ForceRefresh(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(lblAnimeId.Content))) return;
            int id;
            if (!int.TryParse(Convert.ToString(lblAnimeId.Content), out id)) return;

            LblMessage.Content = _animeRepo.MarkForDataUpdate(id);
            _animeManager.UpdateAnimeDetailsFromApi(1,2);
            _animeManager.PrepFolders(1,2);
            LblMessage.Content = "details updated";

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TextBoxFolderPath.Text.Trim().Length > 0)
                Process.Start(TextBoxFolderPath.Text.Trim());
        }
    }

    internal class AnimeResult
    {
        public AnimeResult()
        {
        }

        public AnimeResult(string title, int year, string season, string pathToCreate, int episodes, string synopsis,
                           int id)
        {
            Title = title;
            Year = year;
            Season = season;
            PathToCreate = pathToCreate;
            Episodes = episodes;
            Synopsis = synopsis;
            Id = id;
        }

        public int Episodes { get; set; }
        public string PathToCreate { get; set; }
        public string Synopsis { get; set; }

        public string Title { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }

        public int Id { get; set; }

        public string AnimeType { get; set; }
    }
}