using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CargaAgendamentoDto
    {
        public int CargaOficina { get; set; }
        public int CargaServico { get; set; }
    }
}
