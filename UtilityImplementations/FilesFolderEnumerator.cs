using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDomain.Interface;

namespace UtilityImplementations
{
    public class FilesFolderEnumerator : IFilesFoldersEnumerator
    {
        public List<DirectoryInfo> GetDirectories(string rootDirectory)
        {
            if (rootDirectory.Length > 248) return new List<DirectoryInfo>();
            var subDirectories = Directory.EnumerateDirectories(rootDirectory).ToList();
            var result = new List<DirectoryInfo>();
            subDirectories.ForEach(dir =>
            {
                result.Add(new System.IO.DirectoryInfo(dir));
                result.AddRange(GetDirectories(dir));
            });
            return result;
        }

        public List<FileInfo> GetFiles(string directoryPath)
        {
            if (directoryPath.Length > 248) return new List<FileInfo>();
            
            return Directory.EnumerateFiles(directoryPath).Select(x =>new FileInfo(x)).ToList();

        }
    }
}
