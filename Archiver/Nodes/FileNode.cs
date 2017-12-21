using Archiver.EventArgs;
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
    private readonly long _archiveSize;

    [DataMember]
    private readonly string _relativePath;

    [DataMember]
    private MemoryStream _fileMemoryStream;

    #endregion

    #region Events

    public event ReportProgress ReportArchivationProgress;

    public event ReportProgress ReportDeArchivationProgress;

    #endregion

    #region Constructors

    public FileNode()
    {

    }

    public FileNode(string relativePath, long archiveSize)
    {
      _relativePath = relativePath;
      _archiveSize = archiveSize;
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
      var fileSize = FolderSizeCalculator.GetFileSize(filePath);
      using (var binaryFile = File.OpenRead(filePath))
      {
        _fileMemoryStream = new MemoryStream();
        binaryFile.CopyTo(_fileMemoryStream);
      }
      if (ReportArchivationProgress != null)
      {
        ReportArchivationProgress(this, new ArchivationProgressEventArgs(fileSize, _archiveSize));
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
        if (ReportDeArchivationProgress != null)
        {
          ReportDeArchivationProgress(this, new ArchivationProgressEventArgs(_fileMemoryStream.Length, _archiveSize));
        }
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
