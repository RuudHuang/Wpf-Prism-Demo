using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
