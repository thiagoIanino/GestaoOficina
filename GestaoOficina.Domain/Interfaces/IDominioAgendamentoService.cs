using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Interfaces
{
    public interface IDominioAgendamentoService
    {
        void ValidarDadosEntradaAgendamento(DateTime? dataAgendamento, TipoServico? tipoServico);

        Task ValidarDisponibilidadeDiaAgendamento(Guid idOficina, DateTime dataAgendamento, TipoServico tipoServico);
        int CalcularCapacidadeBonus(DateTime data, int capacidadeOficina);
        void ValidarDadosEntradaAlteracaoStatus(Guid? idAgendamento, StatusAgendamento? status);
        Task PreencherRelatorioAgendamento(Guid idAgendamento, StatusAgendamento status);
        Task<List<RelatorioDto>> ComporRelatorioAgendamento(List<RelatorioAgendamentoDto> agendamentos);
    }
}
