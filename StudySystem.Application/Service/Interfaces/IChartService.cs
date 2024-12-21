using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IChartService : IBaseService
    {
        Task<StatisticResponseModel> GetStatisticResponse(int month);
    }
}
