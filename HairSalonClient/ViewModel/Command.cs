using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HairSalonClient.ViewModel
{
    class Command : ICommand
    {
        #region PROPERTISES & FIELDS
        private Action<object> _action;
        private Func<object, bool> _func;
        #endregion

        #region CONSTRUCTOR
        public Command(Action<object> action)
        {
            _action = action;
        }
        public Command(Action<object> action, Func<object, bool> func)
        {
            this._action = action;
            this._func = func;
        }
        #endregion

        #region METHOD
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _action(parameter);
        }
        #endregion
    }
}
