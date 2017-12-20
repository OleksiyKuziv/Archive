using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArchiverWpf.Model
{
  /// <summary>
  /// Class which is inherited Icommand  to Execute command
  /// </summary>
  public class RelayCommand:ICommand
  {
    #region Private field
    /// <summary>
    /// Private Action than command execute
    /// </summary>
    private Action<object> execute;
    /// <summary>
    /// Private func than command can execute
    /// </summary>
    private Func<object, bool> canExecute;
    #endregion
    #region Public Property
    /// <summary>
    /// Event Can Execute chaned
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }
    /// <summary>
    /// Constructor wit execute and canExecute Commnad. To create command
    /// </summary>
    /// <param name="execute">Execute command action</param>
    /// <param name="canExecute">Can it execute command func</param>
    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }
    /// <summary>
    /// Method if it can execute command
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter)
    {
      return this.canExecute == null || this.canExecute(parameter);
    }
    /// <summary>
    /// Method if if execute command
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(object parameter)
    {
      this.execute(parameter);
    }
    #endregion
  }
}

