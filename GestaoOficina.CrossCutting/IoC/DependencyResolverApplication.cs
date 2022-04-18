using GestaoOficina.Application;
using GestaoOficina.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    public static class DependencyResolverApplication
    {
        public static void AddDependencyResolverApplication(this IServiceCollection services)
        {
             services.AddScoped<IOficinaApplication, OficinaApplication>();
             services.AddScoped<IAgendamentoApplication, AgendamentoApplication>();
        }
    }
}
