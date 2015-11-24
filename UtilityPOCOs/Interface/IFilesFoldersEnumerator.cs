using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDomain.Interface
{
   public interface IFilesFoldersEnumerator
   {

       List<System.IO.DirectoryInfo> GetDirectories(string rootDirectory);
       
       List<System.IO.FileInfo> GetFiles(string directoryPath);

   }
}
