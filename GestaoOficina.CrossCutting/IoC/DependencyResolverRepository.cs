using GestaoOficina.Domain.Repositories;
using GestaoOficina.Infrastructure.Repositories;
using IFoody.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolverRepository
    {
        public static void AddDependencyResolverRepository(this IServiceCollection services)
        {
            services.AddScoped<IOficinaRepository, OficinaRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IContextoRepository, ContextoRepository>();
        }
    }
}
