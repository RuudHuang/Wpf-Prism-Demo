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

        Action<object> _commandExcutedActionWithParameter;

        public DemoCommand(Action commandExcuteAction)
        {
            _commandExcuteAction = commandExcuteAction;
        }

        public DemoCommand(Action<object> commandExcutedActionWithParameter)
        {
            _commandExcutedActionWithParameter = commandExcutedActionWithParameter;
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
                if (parameter == null)
                {
                    _commandExcuteAction?.Invoke();
                }
                else
                {
                    _commandExcutedActionWithParameter?.Invoke(parameter);
                }
            }                        
        }
    }
}
