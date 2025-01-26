using System.Threading;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;

namespace WpfPracticeDemo.StartActions
{
    internal class StartActionInitializeData : IStartAction
    {
        public string ActionName => "Initialize Data";

        public double StartActionProgressPercentage => 10;

        public StartActionResult Excute()
        {
            Thread.Sleep(5000);

            return default;
        }
    }
}
