using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Npgsql;
using StudySystem.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace StudySystem.Data.EF.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// CreatedOrder
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<bool> CreatedOrder(Order order)
        {
            await _context.AddAsync(order).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// admin 
        /// DeleteOrder
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOrder(string orderId)
        {
            var rs = await _context.Set<Order>().SingleOrDefaultAsync(x => x.OrderId.Equals(orderId)).ConfigureAwait(false);
            var orderAddress = await _context.Set<AddressBook>().FirstOrDefaultAsync(x => x.OrderId.Equals(orderId)).ConfigureAwait(false);
            var userOrder = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(rs.CreateUser)).ConfigureAwait(false);
            if (rs != null && rs.Status != CommonConstant.OrderStatusPaymented && userOrder.UserID.Contains(CommonConstant.UserIdSession))
            {
                _context.Remove(rs);
                _context.Remove(userOrder);
                if (orderAddress != null)
                {
                    _context.Remove(orderAddress);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<InvoiceResponseModel> GetInvoice(string orderId)
        {
            using (var connection = new NpgsqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();

                var query = @"select 
                o.""OrderId"" as InvoiceId,
                TO_CHAR(o.""CreateDateAt"" , 'DD-MM-YYYY') as Date,
	                ud.""UserFullName"" as Buyer, case when o.""Payment"" = '1' then 'CK' else 'TM' end as PaymentMethod,
	                ab.""AddressReceive"" as Address, '' as AmountInWords, o.""TotalAmount"" as ToTalAmountNotVAT , 0  as ToTalAmountVAT,
	                o.""TotalAmount"" as ToTalAmount, p.""ProductName"" as ItemName, 'CAI' as Unit, oi.""Quantity"" as Quantity, oi.""Price"" as UnitPrice, oi.""Price"" as AmountNotVAT, 0 as VATRate, 0 as VAT, 
	                (oi.""Quantity"" * oi.""Price"") as Amount
                from ""Orders"" o 
                left join ""OrderItems"" oi  on o.""OrderId"" = oi.""OrderId"" 
                left join ""UserDetails"" ud on ud.""UserID""  = o.""UserId"" 
                left join ""AddressBooks"" ab on o.""OrderId""  = ab.""OrderId""
                left join ""Products"" p  on oi.""ProductId"" = p.""ProductId""
                where o.""OrderId"" = @orderId";
                var invoiceDictionary = new Dictionary<string, InvoiceResponseModel>();

                var result = await connection.QueryAsync<InvoiceResponseModel, ItemInvoiceResponseModel, InvoiceResponseModel>(
                    query,
                    (invoice, item) =>
                    {
                        if (!invoiceDictionary.TryGetValue(invoice.InvoiceId, out var currentInvoice))
                        {
                            currentInvoice = invoice;
                            currentInvoice.Items = new List<ItemInvoiceResponseModel>();
                            invoiceDictionary.Add(currentInvoice.InvoiceId, currentInvoice);
                        }

                        currentInvoice.Items.Add(item);
                        return currentInvoice;
                    },
                    param: new { orderId = orderId },
                    splitOn: "ItemName"
                );

                return invoiceDictionary.Values.FirstOrDefault();
            }
        }

        /// <summary>
        /// GetOrderCustomser
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderInformationByUserResponseModel> GetOrderCustomser(string userId)
        {
            var query = await (from i in _context.Orders
                               join j in _context.OrderItems on i.OrderId equals j.OrderId
                               where i.UserId.Equals(userId)
                               group new { i, j } by new { i.UserId } into grOrder
                               select new OrderInformationByUserResponseModel
                               {
                                   QuantityOrder = grOrder.Count(),
                                   TotalAmount = grOrder.Where(x => x.i.StatusReceive == OrderStatusReceive.IsShipped).Distinct().Sum(x => Convert.ToDouble(x.i.TotalAmount)),
                                   GroupOrderItems = grOrder.Select(x => new GroupOrderItems
                                   {
                                       OrderId = x.i.OrderId,
                                       ProductId = x.j.ProductId,
                                       ImageOrder = _context.Images.First(i => i.ProductId.Equals(x.j.ProductId)).ImageDes,
                                       NameOrder = _context.Products.First(i => i.ProductId.Equals(x.j.ProductId)).ProductName,
                                       QuantityOtherItems = grOrder.Count(y => y.i.OrderId == x.i.OrderId),
                                       StatusReceiveOrder = x.i.StatusReceive,
                                       TotalAmountOrder = Convert.ToDouble(x.i.TotalAmount),
                                       OrderAt = x.i.CreateDateAt.ToString("dd/MM/yyyy HH:mm")
                                   }).ToList(),
                               }).FirstOrDefaultAsync().ConfigureAwait(false);

            if (query != null)
            {
                query.GroupOrderItems = query.GroupOrderItems.DistinctBy(x => x.OrderId).ToList();
                return query;
            }
            return null;
        }

        public async Task<OrderDetailByOderIdResponseModel> GetOrderDetails(string orderId)
        {
            var query = await (from i in _context.OrderItems
                               join p in _context.Products
                               on i.ProductId equals p.ProductId
                               where i.OrderId == orderId
                               select new ProductOrderDetails
                               {
                                   ProductId = i.ProductId,
                                   Image = _context.Images.Where(x => x.ProductId.Equals(i.ProductId)).Select(x => x.ImageDes).First(),
                                   NameProduct = p.ProductName,
                                   TotalPriceByProduct = i.Price.ToString(),
                                   TotalQuantityByProduct = i.Quantity.ToString(),
                               }).ToListAsync();
            OrderDetailByOderIdResponseModel rs = new OrderDetailByOderIdResponseModel();
            rs.Data = query;
            return rs;
        }

        /// <summary>
        /// admin GetOrders
        /// </summary>
        /// <returns></returns>
        public async Task<OrdersAllResponseModel> GetOrders()
        {
            var orderItems = await _context.OrderItems.ToListAsync().ConfigureAwait(false);
            using (var connection = new NpgsqlConnection(AppSetting.ConnectionString))
            {
                // Mở kết nối
                connection.Open();
                var query = @"SELECT 
                                    o.""OrderId"" ,
                                    max(u.""UserFullName"") AS CustomerName,
                                    max(u.""PhoneNumber"") AS CustomerPhone,
                                    max(u.""Email"") AS CustomerEmail,
                                    max(o.""ReceiveType"") AS ReciveType,
                                    COALESCE(CONCAT(max(a.""AddressReceive""), ' ', max(a.""District"") , ' ', max(a.""Province"")), '') AS AddressReceive,
                                    max(o.""Note"") as Note ,
                                    max(o.""Status"") AS StatusOrder,
                                    max(o.""Payment"") AS MethodPayment,
                                    (o.""TotalAmount"") as TotalAmount,
                                    null AS ProductOrderListDataModels,
                                    COALESCE(TO_CHAR(o.""CreateDateAt"" AT TIME ZONE 'UTC', 'DD/MM/YYYY HH24:MI'), '') AS OrderDateAt,
                                    COALESCE(o.""StatusReceive"", 3) AS StatusReceive
                                FROM ""Orders"" o
                                JOIN ""UserDetails"" u ON o.""UserId""  = u.""UserID"" 
                                LEFT JOIN ""AddressBooks"" a ON o.""OrderId"" = a.""OrderId"" 
                                LEFT JOIN ""OrderItems"" oi ON o.""OrderId"" = oi.""OrderId"" 
                                LEFT JOIN ""Products"" p ON oi.""ProductId"" = p.""ProductId"" 
                                GROUP BY o.""OrderId"" , u.""UserID"" 
                                ORDER BY o.""OrderId"" ;";
                var data = await connection.QueryAsync<OrdersResponseDataModel>(query);

                foreach (var item in data)
                {
                    item.ProductOrderListDataModels = orderItems.Where(x => x.OrderId.Equals(item.OrderId)).Select(x => new ProductOrderListDataModel
                    {
                        ProductName = _context.Products.First(i => i.ProductId.Equals(x.ProductId)).ProductName,
                        Quantity = x.Quantity,
                        Price = x.Price,
                    }).ToList();
                }

                return new OrdersAllResponseModel()
                {
                    Orders = data.ToList()
                };
                connection.Close();
            }

        }
        /// <summary>
        /// admin
        /// UpdateStatusOrder
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <param name="statusReceive"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStatusOrder(string orderId, string orderStatus, int statusReceive)
        {
            var rs = await _context.Set<Order>().SingleOrDefaultAsync(x => x.OrderId.Equals(orderId)).ConfigureAwait(false);
            if (rs != null)
            {
                rs.Status = orderStatus;
                rs.StatusReceive = statusReceive;
                rs.UpdateDateAt = DateTime.UtcNow;
                if (orderStatus == "2" || statusReceive == OrderStatusReceive.IsCanceled)
                {
                    rs.StatusReceive = 2;
                    rs.Status = CommonConstant.OrderStatusCancelPayment;
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }


    }
}
