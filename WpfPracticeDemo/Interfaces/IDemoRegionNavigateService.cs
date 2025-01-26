
namespace WpfPracticeDemo.Interfaces
{
    public interface IDemoRegionNavigateService
    {
        void NavigateAllRegionToDefaultView();

        void NavigateToSpecificView<T>();
    }
}
