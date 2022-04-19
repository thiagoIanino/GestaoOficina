using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Repositories
{
    public interface IAgendamentoRepository
    {
        Task<CargaAgendamentoDto> ObterCargaServicoEOficina(TipoServico servico, Guid idOficina);
        Task<List<int>> ListarCargaDiariaJaPreenchida(Guid idOficina, DateTime dataMinima, DateTime dataMaxima);
        Task<int> InserirAgendamento(Agendamento agendamento);
        Task<int> ExcluirAgendamento(Guid idAgendamento);
        Task<List<Agendamento>> ListarAgendamentos(DateTime dataMinima, DateTime dataMaxima, Guid idOficina);
        Task<List<AgendamentoServicoDto>> ListarAgendamentosComCarga(DateTime dataMinima, DateTime dataMaxima, Guid idOficina);
        Task AlterarStatusAgendamento(Guid idAgendamento, StatusAgendamento status);
        Task InserirRelatorioAgendamento(Guid idAgendamento, DateTime dataInicio);
        Task AlterarRelatorioAgendamento(Guid idAgendamento, DateTime dataFim);
        Task<List<RelatorioAgendamentoDto>> ListarAgendamentosParaRelatorio(DateTime dataMinima, DateTime dataMaxima, Guid idOficina);
        Task AtualizarAgendamentosNaoRealizados(DateTime dataReferencia);
    }
}
