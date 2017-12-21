using Archiver.EventArgs;
using System.Threading.Tasks;

namespace Archiver.Archivers
{
  public interface IArchiver
  {
    Task Archive(string sourcePath, string destinationPath);
    Task DeArchive(string sourcePath, string destinationPath);

    event ReportProgress ReportArchivationProgress;
    event ReportProgress ReportDeArchivationProgress;
  }
}
