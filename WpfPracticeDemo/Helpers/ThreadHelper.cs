using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfPracticeDemo.Helpers
{
    internal static class ThreadHelper
    {
        public static void ExcutedInUiThread(Action action)
        {
            if (action == null)
            {
                return;
            }

            Dispatcher.CurrentDispatcher.Invoke(action);
        }
    }
}
