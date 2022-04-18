using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Interfaces
{
    public interface IDominioAgendamentoService
    {
        void ValidarDadosEntradaAgendamento(DateTime dataAgendamento, TipoServico? tipoServico);

        Task ValidarDisponibilidadeDiaAgendamento(Guid idOficina, DateTime dataAgendamento, TipoServico tipoServico);
    }
}
