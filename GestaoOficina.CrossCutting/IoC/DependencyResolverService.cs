using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Services;
using GestaoOficina.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolverservice
    {
        public static void AddDependencyResolverService(this IServiceCollection services)
        {
            services.AddScoped<IDominioOficinaService, DominioOficinaService>();
            services.AddScoped<IDominioAgendamentoService, DominioAgendamentoService>();
            services.AddScoped<IHashService, HashService>();
        }
    }
}
