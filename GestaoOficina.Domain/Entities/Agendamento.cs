using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Entities
{
    public class Agendamento
    {
        public Agendamento(Guid idOficina,DateTime dataAgendameto, TipoServico tipoServico)
        {
            Id = Guid.NewGuid();
            IdOficina = idOficina;
            DataAgendamento = dataAgendameto;
            TipoServico = tipoServico;
            Status = StatusAgendamento.Agendado;
        }

        public Guid Id { get; set; }
        public Guid IdOficina { get; set; }
        public DateTime DataAgendamento { get; set; }
        public TipoServico TipoServico { get; set; }
        public StatusAgendamento Status { get; set; }
    }
}
