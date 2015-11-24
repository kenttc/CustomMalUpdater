using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomMalUpdaterDomain.Interfaces
{
    public interface IAnimeFolderManager
    {

        string GetSeasonString(DateTime? date);

        string FolderPath(string basePath, int year, string season, string animeTitleFolderPath, string animeType);
        
        string CreateFolder(AnimeEntry anime);

        bool CreateTextFile(string pathToCreate, string content, bool overwrite);
        bool DownloadImageFile(string imageUrl, string fullImageFilePath, string username = "", string password = "");
    }
}
