

using CustomMalUpdaterDomain;
using CustomMalUpdaterDomain.Interfaces;

namespace CustomMalUpdaterServices
{
    using System;
    using System.Web;

    public class AnimeSynopsisManager : IAnimeSynopsisManager
    {
        private readonly IAnimeFolderManager _folderManager;

        public AnimeSynopsisManager(IAnimeFolderManager folderManager)
        {
            _folderManager = folderManager;
        }

        public string PlainText(AnimeEntry anime)
        {

            var animeStartDate = Convert.ToDateTime(anime.StrStartDate);
            var animeEndDate = string.IsNullOrEmpty(anime.StrEndDate)
                                   ? ""
                                   : Convert.ToDateTime(anime.StrEndDate).ToString("ddd dd-MMM-yyyy");
            
            var season = _folderManager.GetSeasonString(animeStartDate);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("============================================================");
            sb.AppendLine("Title:" + anime.Title);

            sb.AppendLine("English title:" + anime.EnglishTitle);
            sb.AppendLine("Other names:" + anime.Synonyms);
            sb.AppendLine("Rating:" + anime.Score + " Season:" + season);
            sb.AppendLine("release date:" + animeStartDate.ToString("ddd dd-MMM-yyyy"));
            sb.AppendLine("ended date:" + animeEndDate);
            sb.AppendLine("============================================================");
            sb.AppendLine("total eps:" + anime.Episodes);
            sb.AppendLine("Anime type:" + anime.AnimeType);
            sb.AppendLine("============================================================");
            sb.AppendLine("synopsis:");
            sb.AppendLine(HttpUtility.HtmlDecode(anime.Synopsis));
            sb.AppendLine("============================================================");
            return sb.ToString();
        }

        public string Html(AnimeEntry anime)
        {
            throw new NotImplementedException();
        }
    }
}
