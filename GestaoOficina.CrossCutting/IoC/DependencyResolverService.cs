using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    public static class DependencyResolverservice
    {
        public static void AddDependencyResolverService(this IServiceCollection services)
        {
            services.AddScoped<IDominioOficinaService, DominioOficinaService>();
            services.AddScoped<IDominioAgendamentoService, DominioAgendamentoService>();
        }
    }
}
