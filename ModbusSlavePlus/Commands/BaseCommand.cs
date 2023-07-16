using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModbusSlavePlus.Commands
{
    internal class BaseCommand : ICommand
    {
        private readonly Action<object> execAction;
        private readonly Func<object, bool> changeFunc;

        public BaseCommand(Action<object> execAction,Func<object, bool> changeFunc)
        {
            this.execAction = execAction;
            this.changeFunc = changeFunc;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return changeFunc.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            this.execAction.Invoke(parameter);
        }
    }
}
