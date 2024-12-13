using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class StatisticResonseModel
    {
        public double RevenusThisYear { get; set; }
        public double RevenusThisDay { get; set; }
        public int TotalProductQuantity { get; set; }
        public int TotalOrderQuantity { get; set; }
        public List<float>? RevenusByMonth { get; set; }
    }
    
}
