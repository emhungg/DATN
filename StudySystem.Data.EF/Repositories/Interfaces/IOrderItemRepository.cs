using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<UpdateProductQuantityDataModel> ReturnProductChanged(string orderId);
    }
}
