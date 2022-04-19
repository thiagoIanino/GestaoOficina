using GestaoOficina.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GestaoOficina.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ContextoRepository : IContextoRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextoRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid ObterIdOficinaAutenticada()
        {

            var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(id);
        }

    }
}
