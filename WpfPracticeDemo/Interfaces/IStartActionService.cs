using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Interfaces
{
    internal interface IStartActionService
    {
        ObservableCollection<IStartAction> StartActions { get; }

        Task<StartActionResult> ExcuteStartActions();

        void ShowBootAdvancedView();

        void CloseBootAdvancedView();
    }
}
