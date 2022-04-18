using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Application.Models
{
    public class Agendamentoinput
    {
        public DateTime DataAgendamento { get; set; }
        public TipoServico TipoServico { get; set; }

    }
}
