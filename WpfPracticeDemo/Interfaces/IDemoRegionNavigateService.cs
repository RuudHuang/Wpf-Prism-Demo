using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPracticeDemo.Interfaces
{
    public interface IDemoRegionNavigateService
    {
        void NavigateAllRegionToDefaultView();

        void NavigateToSpecificView<T>();
    }
}
