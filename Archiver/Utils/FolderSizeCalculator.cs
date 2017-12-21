using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.Utils
{
  class FolderSizeCalculator
  {
    #region Public Method

    public static long GetFolderSize(string path)
    {
      var dirInfo = new DirectoryInfo(path);
      return DirSize(dirInfo);
    }

    public static long GetFileSize(string filePath)
    {
      var fileInfo = new FileInfo(filePath);
      return fileInfo.Length;
    }

    #endregion

    #region Private Method

    private static long DirSize(DirectoryInfo dirInfo)
    {
      //File size
      long size = 0;

      //FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
      FileInfo[] files = dirInfo.GetFiles();
      foreach (var file in files)
      {
        size += file.Length;
      }
      ///Folder size
      DirectoryInfo[] directoryInfos = dirInfo.GetDirectories();
      foreach (DirectoryInfo directory in directoryInfos)
      {
        size += DirSize(directory);
      }
      return size;
    }
    #endregion
  }
}
