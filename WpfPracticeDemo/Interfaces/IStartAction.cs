using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Interfaces
{
    internal interface IStartAction
    {
        string ActionName { get; }

        StartActionResult Excute();

        double StartActionProgressPercentage { get; }

    }
}
