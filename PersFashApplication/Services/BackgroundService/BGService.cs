using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.UserSubscriptionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BackgroundServices
{
    public class BGService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BGService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var customerSubscriptionService = scope.ServiceProvider.GetRequiredService<ICustomerSubscriptionService>();

                    var message = customerSubscriptionService.AutoUpdatingCustomerSubscriptionStatus().GetAwaiter().GetResult();

                    Console.WriteLine($"Message: {message}");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
