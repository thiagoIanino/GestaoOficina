using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class OficinaInput
    {
        public string Nome { get; set; }
        public int Carga { get; set; }
        public string Cnpj { get; set; }
        public string Senha { get; set; }
    }
}
