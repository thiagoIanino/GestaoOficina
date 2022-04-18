using GestaoOficina.Domain.Repositories;
using GestaoOficina.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    public static class DependencyResolverRepository
    {
        public static void AddDependencyResolverRepository(this IServiceCollection services)
        {
            services.AddScoped<IOficinaRepository, OficinaRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
        }
    }
}
