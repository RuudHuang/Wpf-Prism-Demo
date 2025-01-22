using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace WpfPracticeDemo.Models
{
    internal class StartActionProgressChangedEventArgs
    {
        public string StartActionName { get; set; }

        public double PercentageInAllAction {  get; set; }
    }
}
