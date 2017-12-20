using Archiver.Utils;
using System;
using System.IO;
using System.Runtime.Serialization;


namespace Archiver.Nodes
{
  [DataContract]
  [KnownType(typeof(FileNode))]
  internal class FileNode : Node, IDisposable
  {

    #region Variables

    [DataMember]
    private readonly string _relativePath;

    [DataMember]
    private MemoryStream _fileMemoryStream;

    #endregion

    #region Constructors

    public FileNode()
    {

    }

    public FileNode(string relativePath)
    {
      _relativePath = relativePath;
    }

    #endregion

    #region Node's Implementation

    public override void Load(string filePath)
    {
      if (!File.Exists(filePath))
      {
        Console.WriteLine("This File does not exist");
        return;
      }

      using (var binaryFile = File.OpenRead(filePath))
      {
        _fileMemoryStream = new MemoryStream();
        binaryFile.CopyTo(_fileMemoryStream);
      }
    }

    public override void Serialize(string filePath)
    {
      BinarySerializer.Serialize(this, filePath);
    }

    public override void Save(string filePath)
    {
      using (var stream = File.Create(string.Format(@"{0}\{1}", filePath, _relativePath)))
      {
        _fileMemoryStream.Position = 0;
        _fileMemoryStream.CopyTo(stream);
      }
    }

    #endregion

    #region IDisposable's Implementation

    public void Dispose()
    {
      _fileMemoryStream?.Dispose();
    }

    #endregion
  }

}
