using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.CrossCutting.IoC
{
    [ExcludeFromCodeCoverage]
    public static class DependencyResolver
    {
        public static void AddDependencyResolver(this IServiceCollection services)
        {
            services.AddDependencyResolverApplication();
            services.AddDependencyResolverRepository();
            services.AddDependencyResolverService();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }

}