using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class OficinaOutput
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Carga { get; set; }
        public string Token { get; set; }
    }
}
