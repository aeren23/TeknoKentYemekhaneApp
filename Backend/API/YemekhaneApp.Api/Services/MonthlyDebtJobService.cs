using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using YemekhaneApp.Application.CQRS.Commands.MealRecord;
using YemekhaneApp.Application.CQRS.Commands.UserDebt;

public class MonthlyDebtJobService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MonthlyDebtJobService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddMinutes(5);
            var delay = nextRun - now;
            if (delay < TimeSpan.Zero)
                delay = TimeSpan.FromMinutes(1);

            await Task.Delay(delay, stoppingToken);

            // Sadece ayın ilk günü çalışacak
            if (DateTime.Now.Day == 1)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    int year = now.Year;
                    int month = now.Month;

                    var createDebtsCommand = new CreateUserDebtsForMonthCommand(year, month);
                    await mediator.Send(createDebtsCommand, stoppingToken);
                }
            }
        }
    }
}