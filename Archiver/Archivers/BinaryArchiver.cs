using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Archiver.Nodes;
using Archiver.Utils;

namespace Archiver.Archivers
{
  public class BinaryArchiver : IArchiver
  {
    public Task Archive(string sourcePath, string destinationPath)
    {
      return Task.Factory.StartNew(() =>
      {
        if (File.Exists(sourcePath))
        {
          var fileName = Path.GetFileName(sourcePath);
          var folderName = Path.GetFileNameWithoutExtension(sourcePath);
          var folderNode = new FolderNode(folderName);
          var fileNode = new FileNode(relativePath: string.Format(@"{0}\{1}", folderName, fileName));
          folderNode.AddChildNode(fileNode);
          fileNode.Load(sourcePath);
          folderNode.Serialize(destinationPath);
        }
        if (Directory.Exists(sourcePath))
        {
          var folderName = Path.GetFileName(sourcePath);
          var folderNode = new FolderNode(relativePath: folderName);
          folderNode.Load(sourcePath);
          folderNode.Serialize(destinationPath);
        }
      });
    }

    public Task DeArchive(string sourcePath, string destinationPath)
    {
      return Task.Factory.StartNew(() =>
      {
        var node = BinarySerializer.Deserialize<FolderNode>(sourcePath, new List<Type>() { typeof(Node), typeof(FileNode), typeof(FolderNode) });
        node.Save(destinationPath);
      });
    }
  }
}
