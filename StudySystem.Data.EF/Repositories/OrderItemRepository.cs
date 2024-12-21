using LinqToDB;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UpdateProductQuantityDataModel> ReturnProductChanged(string orderId)
        {
            UpdateProductQuantityDataModel product = new UpdateProductQuantityDataModel();
            product.ProductChangedData = new List<ProductChangedDataModel>();
            var rs = await _context.Set<OrderItem>().Where(x => x.OrderId.Equals(orderId)).ToListAsync();
            rs.ForEach(x =>
            product.ProductChangedData.Add(
                new ProductChangedDataModel { ProductId = x.ProductId, Quantity = x.Quantity }
                ));
            return product;
        }
    }
}
