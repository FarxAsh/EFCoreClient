using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using EFCoreClient.Data.Entities;
using System.Data;
using Microsoft.Data.SqlClient.Server;
using System.Linq;
using EFCoreClient.Services;
using Castle.Core.Configuration;

namespace EFCoreClient.Data
{
    public class OrderRepository
    {
        readonly BookStoreContext dbContext;
        readonly DiscountService discountService;

        public OrderRepository(BookStoreContext dbContext, DiscountService discountService)
        {
            this.dbContext = dbContext;
            this.discountService = discountService;
        }

        public async Task<Order> CreateOrderAsync(Order order, List<int> bookIds)
        {
            try
            {
                List<SqlParameter> spParams = GenerateSPCreateOrderParams(order, bookIds);
                await dbContext.Database.ExecuteSqlRawAsync(
                          "uspCreateOrder @UserId, @PaymentTypeId, @DeleveryDate, @OrderDate, @PaymentStatusId, @OrderedBooks, @OrderId OUT,  @DiscountRate", spParams);

                return await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == Convert.ToInt32(spParams[6].Value));
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        private SqlParameter CreateOrderedBooksParameter(string parametrName, List<int> bookIds)
        {

            var rows = new List<SqlDataRecord>();
            var sqlMetaData = new SqlMetaData("Id", SqlDbType.Int);
            foreach (var id in bookIds)
            {
                var row = new SqlDataRecord(sqlMetaData);
                row.SetValues(id);
                rows.Add(row);
            }

            var orderedBooks = new SqlParameter
            {
                ParameterName = "@OrderedBooks",
                SqlDbType = SqlDbType.Structured,
                TypeName = "[dbo].[IdentifierList]",
                Direction = ParameterDirection.Input,
                Value = rows
            };

            return orderedBooks;
        }

        private List<SqlParameter> GenerateSPCreateOrderParams(Order order, List<int> bookIds)
        {
            List<SqlParameter> spParams = new List<SqlParameter>();

            var userId = new SqlParameter("@UserId", order.UserId);
            var paymentTypeId = new SqlParameter("@PaymentTypeId", order.PaymentTypeId);
            var deleveryDate = new SqlParameter("@DeleveryDate", order.DeleveryDate);
            var orderDate = new SqlParameter("@OrderDate", order.OrderDate);
            var paymentStatusId = new SqlParameter("@PaymentStatusId", order.PaymentStatusId);
            var discountRate = new SqlParameter("@DiscountRate", discountService.GetDiscountRate());
            var orderedBooks = CreateOrderedBooksParameter("@OrderedBooks", bookIds);
            var createdOrderId = new SqlParameter
            {
                ParameterName = "@OrderId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            spParams.Add(userId);
            spParams.Add(paymentTypeId);
            spParams.Add(deleveryDate);
            spParams.Add(orderDate);
            spParams.Add(paymentStatusId);
            spParams.Add(orderedBooks);
            spParams.Add(createdOrderId);
            spParams.Add(discountRate);

            return spParams;
        }

    }
}
