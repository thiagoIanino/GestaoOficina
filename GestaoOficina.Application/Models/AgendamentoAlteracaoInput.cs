using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class AgendamentoAlteracaoInput
    {
        public Guid IdAgendamento { get; set; }
        public StatusAgendamento Status { get; set; }
    }
}
