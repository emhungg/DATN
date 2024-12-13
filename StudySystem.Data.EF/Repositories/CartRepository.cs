using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public CartResponseModel GetCart(string userId)
        {
            CartResponseModel cartResponseModel = new CartResponseModel();
            var rs = from c in _context.Carts
                     join u in _context.UserDetails
                     on c.UserId equals u.UserID
                     join ci in _context.CartItems
                     on c.CartId equals ci.CartId
                     join p in _context.Products
                     on ci.ProductId equals p.ProductId
                     join i in _context.Images
                     on p.ProductId equals i.ProductId
                     where u.UserID.Equals(userId)
                     group c by new { p.ProductId, p.ProductName, p.ProductPrice, p.PriceSell, i.ImageDes, ci.Quantity, p.ProductQuantity } into g
                     select new CartDataModel
                     {
                         ProductId = g.Key.ProductId,
                         ProductName = g.Key.ProductName,
                         Price = g.Key.ProductPrice,
                         PriceSell = g.Key.PriceSell,
                         ImagePath = g.Key.ImageDes,
                         TotalQuantity = g.Key.ProductQuantity,
                         Quantity = g.Key.Quantity
                     };
            
            cartResponseModel.CartData = rs.GroupBy(x => x.ProductId)
                               .Select(g => g.First())
                               .ToList();
            return cartResponseModel;
        }

        public async Task<Cart> InsertCartUser(string userId, string cartId)
        {
            var rs = await _context.Set<Cart>().SingleOrDefaultAsync(x => x.UserId.Equals(userId)).ConfigureAwait(false);
            if (rs != null)
            {
                return rs;
            }
            else
            {
                await _context.AddAsync(new Cart { CartId = cartId, UserId = userId }).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return rs;
            }
        }
    }
}
