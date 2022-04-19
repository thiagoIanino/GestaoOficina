using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RelatorioAgendamentoDto : Agendamento
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
