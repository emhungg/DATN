using Microsoft.Extensions.Logging;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class ChartService : BaseService, IChartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChartService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public ChartService(IUnitOfWork unitOfWork, ILogger<ChartService> logger) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _orderRepository = unitOfWork.OrderRepository;
            _productRepository = unitOfWork.ProductRepository;
            _userRepository = unitOfWork.UserRepository;
            _orderItemRepository = unitOfWork.OrderItemRepository;
        }

        /// <summary>
        /// GetStatisticResponse
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<StatisticResponseModel> GetStatisticResponse(int month)
        {
            try
            {
                double revenusYear = await GetRevenusYear();
                double revenusDay = await GetRevenusDay();
                int totalProduct = await TotalProduct();
                int totalOrderQuantity = await TotalOrderQuantity();
                StatisticResponseModel rs = new StatisticResponseModel
                {
                    RevenusThisYear = revenusYear,
                    RevenusThisDay = revenusDay,
                    TotalOrderQuantity = totalOrderQuantity,
                    TotalProductQuantity = totalProduct,
                    RevenusByMonth = month == 0 ? RevenusByMonth() : RevenusDayOfMonth(month),
                    OverviewCustomer = await CustomerOverview(),
                    CompareBestSellingData = GetCompare(),
                    LeastSoldData = GetLeastCompare()
                };
                return rs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        private async Task<double> GetRevenusYear()
        {
            int currentYear = DateTime.UtcNow.Year;
            var query = await _orderRepository.FindAllAsync(x => x.StatusReceive == OrderStatusReceive.IsShipped);
            var rs = query.Where(x => DatetimeUtils.TimeZoneUTC(x.UpdateDateAt).Year == currentYear)
                .Sum(x => Convert.ToDouble(x.TotalAmount));
            return rs;
        }

        private async Task<double> GetRevenusDay()
        {
            int currentYear = DateTime.UtcNow.Day;
            var query = await _orderRepository.FindAllAsync(x => x.StatusReceive == OrderStatusReceive.IsShipped);
            var rs = query.Where(x => DatetimeUtils.TimeZoneUTC(x.UpdateDateAt).Day == currentYear)
                .Sum(x => Convert.ToDouble(x.TotalAmount));
            return rs;
        }

        private async Task<int> TotalOrderQuantity()
        {
            int currentYear = DateTime.UtcNow.Day;
            var query = await _orderRepository.FindAllAsync(x => x.StatusReceive == OrderStatusReceive.IsShipped).ConfigureAwait(false);
            var rs = query.Where(x => DatetimeUtils.TimeZoneUTC(x.CreateDateAt).Day == currentYear).Count();
            return rs;
        }

        private async Task<int> TotalProduct()
        {
            var query = await _productRepository.GetAllAsyn();
            var rs = query.Sum(x => x.ProductQuantity);
            return rs;
        }

        private List<double> RevenusByMonth()
        {
            int currentYear = DateTime.UtcNow.Year;
            List<int> allMonths = Enumerable.Range(1, 12).ToList();
            var monthlyTotals = allMonths
            .GroupJoin(
                _orderRepository.FindAll(order => order.StatusReceive == OrderStatusReceive.IsShipped && order.UpdateDateAt.Year == currentYear),
                month => month,
                order => order.UpdateDateAt.Month,
                (month, orderGroup) => new
                {
                    Month = month,
                    TotalAmount = orderGroup.Sum(order => Convert.ToDouble(order.TotalAmount))
                })
            .OrderBy(result => result.Month);
            List<double> result = monthlyTotals.Select(item => (double)item.TotalAmount).ToList();

            return result;
        }

        /// <summary>
        /// func lấy data cho biểu đồ doanh thu theo ngày trong tháng
        /// </summary>
        /// <returns></returns>
        private List<double> RevenusDayOfMonth(int month)
        {
            int currentYear = DateTime.UtcNow.Year;
            int days = DateTime.DaysInMonth(currentYear, month);
            List<int> allDays = Enumerable.Range(1, days).ToList();

            var dailyTotals = allDays
                .GroupJoin(
                    _orderRepository.FindAll(order =>
                        order.StatusReceive == OrderStatusReceive.IsShipped &&
                        order.UpdateDateAt.Year == currentYear &&
                        order.UpdateDateAt.Month == month),
                    day => day,
                    order => order.UpdateDateAt.Day,
                    (day, orderGroup) => new
                    {
                        Day = day,
                        TotalAmount = orderGroup.Sum(order => Convert.ToDouble(order.TotalAmount))
                    })
                .OrderBy(result => result.Day);

            List<double> result = dailyTotals.Select(item => (double)item.TotalAmount).ToList();

            return result;
        }

        private async Task<List<double>> CustomerOverview()
        {
            var userResiger = await _userRepository.FindAllAsync(x => !x.UserID.Contains(CommonConstant.UserIdSession))
                .ConfigureAwait(false);
            var orderUser = await _orderRepository.GetAllAsyn().ConfigureAwait(false);
            List<double> result = new List<double> { userResiger.Count(), orderUser.DistinctBy(x => x.UserId).Count() };
            return result;
        }

        private CompareBestSelling GetCompare()
        {
            var orderDone = _orderRepository.FindAll(order => order.StatusReceive == OrderStatusReceive.IsShipped);
            var orders = _orderItemRepository.FindAll(x => orderDone.Select(o => o.OrderId).Contains(x.OrderId));
            var topProducts = orders
            .GroupBy(order => order.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                TotalQuantity = group.Sum(order => order.Quantity)
            })
            .OrderByDescending(item => item.TotalQuantity)
            .Take(3)
            .ToList();

            CompareBestSelling compareData = new CompareBestSelling
            {
                NameProductFirst = topProducts.Count() > 0 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[0].ProductId)).ProductName : "",
                DataProductFirst = topProducts.Count() > 0 ? (double)topProducts[0].TotalQuantity : 0,
                NameProductSecond = topProducts.Count() > 1 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[1].ProductId)).ProductName : "",
                DataProductSecond = topProducts.Count() > 1 ? (double)topProducts[1].TotalQuantity : 0,
                NameProductLast = topProducts.Count() > 2 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[2].ProductId)).ProductName : "",
                DataProductLast = topProducts.Count() > 2 ? (double)topProducts[2].TotalQuantity : 0,
            };

            return compareData;
        }

        private CompareLeastSold GetLeastCompare()
        {
            var orderDone = _orderRepository.FindAll(order => order.StatusReceive == OrderStatusReceive.IsShipped);
            var orders = _orderItemRepository.FindAll(x => orderDone.Select(o => o.OrderId).Contains(x.OrderId));
            var topProducts = orders
            .GroupBy(order => order.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                TotalQuantity = group.Sum(order => order.Quantity)
            })
            .OrderBy(item => item.TotalQuantity)
            .Take(3)
            .ToList();

            CompareLeastSold compareData = new CompareLeastSold
            {
                NameProductFirst = topProducts.Count() > 0 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[0].ProductId)).ProductName : "",
                DataProductFirst = topProducts.Count() > 0 ? (double)topProducts[0].TotalQuantity : 0,
                NameProductSecond = topProducts.Count() > 1 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[1].ProductId)).ProductName : "",
                DataProductSecond = topProducts.Count() > 1 ? (double)topProducts[1].TotalQuantity : 0,
                NameProductLast = topProducts.Count() > 2 ? _productRepository.Find(x => x.ProductId.Equals(topProducts[2].ProductId)).ProductName : "",
                DataProductLast = topProducts.Count() > 2 ? (double)topProducts[2].TotalQuantity : 0,
            };

            return compareData;
        }
    }
}
