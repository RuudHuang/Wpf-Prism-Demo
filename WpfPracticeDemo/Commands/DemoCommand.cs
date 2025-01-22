using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfPracticeDemo.Commands
{
    internal class DemoCommand : ICommand
    {
        Action _commandExcuteAction;

        public DemoCommand(Action commandExcuteAction)
        {
            _commandExcuteAction = commandExcuteAction;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(null))
            {
                _commandExcuteAction?.Invoke();
            }            
        }
    }
}
