using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver.EventArgs
{
  public class ArchivationProgressEventArgs
  {
    #region Private Field

    private readonly long _progress;
    private readonly long _maxProgress;

    #endregion

    #region Public method

    public ArchivationProgressEventArgs(long progress, long maxProgress)
    {
      _progress = progress;
      _maxProgress = maxProgress;
    }

    public long Progress
    {
      get
      {
        return _progress;
      }
    }

    public long MaxProgress
    {
      get
      {
        return _maxProgress;
      }
    }

    #endregion

  }

  public delegate void ReportProgress(object sender, ArchivationProgressEventArgs e);

}
