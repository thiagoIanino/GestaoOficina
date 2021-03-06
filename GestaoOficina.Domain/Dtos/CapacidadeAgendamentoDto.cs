using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CapacidadeAgendamentoDto
    {
        public DateTime Data { get; set; }
        public int CargaDisponivel { get; set; }
    }
}
