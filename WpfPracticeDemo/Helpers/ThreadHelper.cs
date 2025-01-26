using System;
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
