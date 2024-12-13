using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.Models.Response
{
    public class StatisticResponseModel
    {
        public double RevenusThisYear { get; set; }
        public double RevenusThisDay { get; set; }
        public int TotalProductQuantity { get; set; }
        public int TotalOrderQuantity { get; set; }
        public List<double> RevenusByMonth { get; set; }
        public List<double> OverviewCustomer { get; set; }
        public CompareBestSelling CompareBestSellingData { get; set; }
        public CompareLeastSold LeastSoldData { get; set; }
    }

    public class CompareBestSelling
    {
        public string NameProductFirst { get; set; }
        public double DataProductFirst { get; set; }
        public string NameProductSecond { get; set; }
        public double DataProductSecond { get; set; }
        public string NameProductLast { get; set; }
        public double DataProductLast { get; set; }
    }

    public class CompareLeastSold
    {
        public string NameProductFirst { get; set; }
        public double DataProductFirst { get; set; }
        public string NameProductSecond { get; set; }
        public double DataProductSecond { get; set; }
        public string NameProductLast { get; set; }
        public double DataProductLast { get; set; }
    }
}
