using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IImageProductRepository : IRepository<Image>
    {
        Task<bool> UpdateImageProduct(string productId, List<string> imageName, string user);
        Task<bool> DeleteImageProduct(string productId);
    }
}
