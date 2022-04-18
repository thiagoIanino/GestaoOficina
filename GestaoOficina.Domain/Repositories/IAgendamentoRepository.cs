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
        Task<CapacidadeAgendamentoDto> ObterCargaServicoEOficina(TipoServico servico, Guid idOficina);
        Task<List<int>> ListarCargaDiariaJaPreenchida(Guid idOficina, DateTime dataMinima, DateTime dataMaxima);
        Task<int> InserirAgendamento(Agendamento agendamento);
        Task<int> ExcluirAgendamento(Guid idAgendamento);
    }
}
