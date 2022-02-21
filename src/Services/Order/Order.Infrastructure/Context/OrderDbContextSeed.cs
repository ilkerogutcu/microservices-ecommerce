using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Order.Domain.AggregateModels.BuyerAggregate;
using Order.Domain.AggregateModels.OrderAggregate;
using Order.Domain.SeedWork;
using Serilog;


namespace Order.Infrastructure.Context
{
    public class OrderDbContextSeed
    {
        public async Task SeedAsync(OrderDbContext context)
        {
            var policy = CreatePolicy(nameof(OrderDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                var useCustomizationData = false;
                var contentRootPath = "Seeding/Setup";

                await using (context)
                {
                    await context.Database.MigrateAsync();

                    if (!context.CardTypes.Any())
                    {
                        context.CardTypes.AddRange(useCustomizationData
                            ? GetCardTypesFromFile(contentRootPath)
                            : GetPredefinedCardTypes());
                    }

                    if (!context.OrderStatuses.Any())
                    {
                        context.OrderStatuses.AddRange(useCustomizationData
                            ? GetOrderStatusesFromFile(contentRootPath)
                            : GetPredefinedOrderStatuses());
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private IEnumerable<OrderStatus> GetOrderStatusesFromFile(string contentRootPath)
        {
            var fileName = "OrderStatus.txt";
            if (!File.Exists(fileName))
            {
                return GetPredefinedOrderStatuses();
            }

            var fileContent = File.ReadAllLines(fileName);
            var id = 1;
            return fileContent.Select(c => new OrderStatus(id++, c)).Where(x => true);
        }

        private IEnumerable<OrderStatus> GetPredefinedOrderStatuses()
        {
            return Enumeration.GetAll<OrderStatus>();
        }

        private IEnumerable<CardType> GetCardTypesFromFile(string contentRootPath)
        {
            var fileName = "CardTypes.txt";
            if (!File.Exists(fileName))
            {
                return GetPredefinedCardTypes();
            }

            var fileContent = File.ReadAllLines(fileName);
            var id = 1;
            return fileContent.Select(c => new CardType(id++, c)).Where(x => true);
        }

        private IEnumerable<CardType> GetPredefinedCardTypes()
        {
            return Enumeration.GetAll<CardType>();
        }

        private AsyncRetryPolicy CreatePolicy(string prefix, int retries = 10)
        {
            return Policy.Handle<SqlException>()
                .WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: ((exception, timespan, retry, context) =>
                    {
                        Log.Error($"{prefix} Exception {exception.GetType().Name} with message {exception.Message}");
                    })
                );
        }
    }
}