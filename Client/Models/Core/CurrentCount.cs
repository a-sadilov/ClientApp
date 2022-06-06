using Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Core
{
    public class CurrentCount : ObservableObject
    {
        private static int counter;
        public string Counter
        {
            get
            {
                return counter.ToString();
            } 
            set
            {
                int.Parse(value);
                OnPropertyChanged("Counter");
            }
        }
        public CurrentCount()
        {
            counter = 0;
        }
    }
}
