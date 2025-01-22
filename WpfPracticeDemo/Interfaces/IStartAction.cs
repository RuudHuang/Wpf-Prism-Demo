using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Interfaces
{
    internal interface IStartAction
    {
        string ActionName { get;}

        StartActionResult Excute();

        double StartActionProgressPercentage { get; }

    }
}
