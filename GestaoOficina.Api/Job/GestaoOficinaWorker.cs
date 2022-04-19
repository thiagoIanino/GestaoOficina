using GestaoOficina.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoOficina.Api.Job
{
    [ExcludeFromCodeCoverage]
    public class GestaoOficinaWorker : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer = null!;

        public GestaoOficinaWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _agendamentoService = scope.ServiceProvider.GetRequiredService<IAgendamentoApplication>();

                 _agendamentoService.AtualizarAgendamentosNaoRealizadosJob();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
