using ArchiverWpf.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Archiver.Archivers;

namespace ArchiverWpf.ViewModels
{
  public class MainViewModel : INotifyPropertyChanged
  {
    #region Variables
    private RelayCommand _sourcePathArchiveCommand;
    private string _sourcePathArchive;
    private RelayCommand _destinationPathArchiveCommand;
    private string _destinationPathArchive;
    private RelayCommand _sourcePathDeArchiveCommand;
    private string _sourcePathDeArchive;
    private RelayCommand _destinationPathDeArchiveCommand;
    private string _destinationPathDeArchive;
    private RelayCommand _archiveCommand;
    private RelayCommand _deArchiveCommand;
    private long _allFileSerialize;
    private long _doneSerialize;    
    private long _allFileDeSerialize;
    private long _doneDeSerialize;
    #endregion

    #region Constructor
    public MainViewModel()
    {
      InitializeCommand();
    }

    #endregion
    #region Public property
    public RelayCommand SourcePathArchiveCommand
    {
      get
      {
        return _sourcePathArchiveCommand;
      }
    }

    public RelayCommand DestinationPathArchiveCommand
    {
      get
      {
        return _destinationPathArchiveCommand;
      }
    }
    public RelayCommand SourcePathDeArchiveCommand
    {
      get
      {
        return _sourcePathDeArchiveCommand;
      }
    }

    public RelayCommand DestinationPathDeArchiveCommand
    {
      get
      {
        return _destinationPathDeArchiveCommand;
      }
    }

    public RelayCommand ArchiveCommand
    {
      get
      {
        return _archiveCommand;
      }
    }
    public RelayCommand DeArchiveCommand
    {
      get
      {
        return _deArchiveCommand;
      }
    }
    public long AllFileSerialize
    {
      get
      {
        return _allFileSerialize;
      }
      set
      {
        _allFileSerialize = value;
        OnPropertytChanged("AllFileSerialize");
      }
    }  
    public long DoneSerialize
    {
      get
      {
        return _doneSerialize;
      }
      set
      {
        _doneSerialize = value;
        OnPropertytChanged("DoneSerialize");
      }
    }

    public long AllFileDeSerialize
    {
      get
      {
        return _allFileDeSerialize;
      }
      set
      {
        _allFileDeSerialize = value;
        OnPropertytChanged("AllFileDeSerialize");
      }
    }
    public long DoneDeSerialize
    {
      get
      {
        return _doneDeSerialize;
      }
      set
      {
        _doneDeSerialize = value;
        OnPropertytChanged("DoneDeSerialize");
      }
    }

    #endregion
    #region Private Ьethod
    private void InitializeCommand()
    {
      _sourcePathArchiveCommand = new RelayCommand(SourcePathCommand_Execute, SourcePathCommand_CanExecute);
      _destinationPathArchiveCommand = new RelayCommand(DestinationPathCommand_Execute, DestinationPathCommand_CanExecute);
      _sourcePathDeArchiveCommand = new RelayCommand(SourcePathDeArchiveCommand_Execute, SourcePathDeArchiveCommand_CanExecute);
      _destinationPathDeArchiveCommand = new RelayCommand(DestinationPathDeArchiveCommand_Execute, DestinationPathDeArchiveCommand_CanExecute);
      _archiveCommand = new RelayCommand(ArchiveCommand_Execute, ArchiveCommand_CanExecute);
      _deArchiveCommand = new RelayCommand(DeArchiveCommand_Execute, DeArchiveCommand_CanExecute);
    }

    private void SourcePathCommand_Execute(object obj)
    {
      using (var fbd = new FolderBrowserDialog())
      {
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
          _sourcePathArchive = fbd.SelectedPath;
          var dirInfo = new DirectoryInfo(_sourcePathArchive);
          //AllFileSerialize = (int)DirSize(dirInfo)/(1024*1024);
        }
      }
    }

    private bool SourcePathCommand_CanExecute(object obj)
    {
      return true;
    }
    private void DestinationPathCommand_Execute(object obj)
    {
      Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
      {
        FileName = "Serialize"
      };
      Nullable<bool> result = dlg.ShowDialog();
      if (result == true)
      {
        _destinationPathArchive = dlg.FileName;
      }
    }

    private bool DestinationPathCommand_CanExecute(object obj)
    {
      return true;
    }

    private void SourcePathDeArchiveCommand_Execute(object obj)
    {
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      Nullable<bool> result = dlg.ShowDialog();
      if (result == true)
      {
        _sourcePathDeArchive = dlg.FileName;
        var fileInfo = new FileInfo(_sourcePathDeArchive);
        AllFileDeSerialize = (int)fileInfo.Length / (1024 * 1024);
      }
    }

    private bool SourcePathDeArchiveCommand_CanExecute(object obj)
    {
      return true;
    }

    private void DestinationPathDeArchiveCommand_Execute(object obj)
    {
      using (var fbd = new FolderBrowserDialog())
      {
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
          _destinationPathDeArchive = fbd.SelectedPath;
        }
      }
    }
    private bool DestinationPathDeArchiveCommand_CanExecute(object obj)
    {
      return true;
    }

    private void ArchiveCommand_Execute(object obj)
    {
      Archive(_sourcePathArchive, _destinationPathArchive);
    }

    private bool ArchiveCommand_CanExecute(object obj)
    {
      return !String.IsNullOrEmpty(_destinationPathArchive) && !String.IsNullOrEmpty(_sourcePathArchive) ? true : false;
    }

    private void DeArchiveCommand_Execute(object obj)
    {
      DeArchive(_sourcePathDeArchive, _destinationPathDeArchive);
    }
    private bool DeArchiveCommand_CanExecute(object obj)
    {
      return !String.IsNullOrEmpty(_destinationPathDeArchive) && !String.IsNullOrEmpty(_sourcePathDeArchive) ? true : false;
    }

    #endregion

    #region INotifyPropertyChanged
    /// <summary>sss
    /// Event propertyChanged for property
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// Method when event is changed property
    /// </summary>
    /// <param name="prop"></param>
    public void OnPropertytChanged([CallerMemberName]string prop = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(prop));
      }
    }
    #endregion
    #region Public Method
    public Task Archive(string sourcePath, string destinationPath)
    {
      IArchiver archiver = new BinaryArchiver();
      AllFileDeSerialize = 0;
      DoneSerialize = 0;
      archiver.ReportArchivationProgress += (o, e) =>
      {
        AllFileSerialize = e.MaxProgress;
        DoneSerialize += e.Progress;        
      };
      var task = archiver.Archive(sourcePath, destinationPath);
      return (Task)task;
    }

    public Task DeArchive(string sourcePath, string destinationPath)
    {
      IArchiver archiver = new BinaryArchiver();
      AllFileDeSerialize = 0;
      DoneSerialize = 0;
      archiver.ReportDeArchivationProgress += (o, e) =>
      {
        AllFileDeSerialize = e.MaxProgress;
        DoneDeSerialize += e.Progress;        
      };
      var task = archiver.DeArchive(sourcePath, destinationPath);
      return (Task)task;
    }
    #endregion
  }
}
