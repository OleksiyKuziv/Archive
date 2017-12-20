using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Archiver.Utils;

namespace Archiver.Nodes
{
  [DataContract]
  [KnownType(typeof(FolderNode))]
  internal class FolderNode : Node
  {

    #region Variables

    [DataMember]
    private readonly string _relativePath;

    [DataMember]
    private List<Node> _childNodes;

    #endregion

    #region Constructors

    public FolderNode()
    {

    }

    public FolderNode(string relativePath)
    {
      _relativePath = relativePath;
    }

    #endregion

    #region Public Methods

    public void AddChildNode(Node node)
    {
      if (_childNodes == null)
      {
        _childNodes = new List<Node>();
      }
      _childNodes.Add(node);
    }

    #endregion

    #region Node's Implementation

    public override void Load(string folderPath)
    {
      if (!Directory.Exists(folderPath))
      {
        Console.WriteLine("This Directory does not exist");
        return;
      }

      _childNodes = new List<Node>();
      string[] fileEntries = Directory.GetFiles(folderPath);

      foreach (var filePath in fileEntries)
      {
        var fileName = Path.GetFileName(filePath);
        var fileNode = new FileNode(relativePath: string.Format(@"{0}\{1}", _relativePath, fileName));
        _childNodes.Add(fileNode);
        fileNode.Load(filePath);
      }

      string[] subdirectoryEntries = Directory.GetDirectories(folderPath);
      foreach (string subdirectoryPath in subdirectoryEntries)
      {
        var folderName = Path.GetFileName(subdirectoryPath);
        var folderNode = new FolderNode(relativePath: string.Format(@"{0}\{1}", _relativePath, folderName));
        _childNodes.Add(folderNode);
        folderNode.Load(subdirectoryPath);
      }

    }

    public override void Serialize(string filePath)
    {
      BinarySerializer.Serialize(this, filePath);
    }

    public override void Save(string filePath)
    {
      Directory.CreateDirectory(string.Format(@"{0}\{1}", filePath, _relativePath));
      if (!_childNodes.Any()) return;
      foreach (var node in _childNodes)
      {
        node.Save(filePath);
      }
    }

    #endregion
  }
}
