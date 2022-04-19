using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class HistoricoAgendamentoDto
    {
        public Guid IdAgendamento { get; set; }
        public StatusAgendamento Status { get; set; }
        public TipoServico Servico { get; set; }
        public int DuracaoEmMinutos { get; set; }

    }
}
