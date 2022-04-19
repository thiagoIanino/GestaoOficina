using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Enums;
using GestaoOficina.Domain.Extensions;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Services
{
    public class DominioAgendamentoService : IDominioAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        public DominioAgendamentoService(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public void ValidarDadosEntradaAgendamento(DateTime? dataAgendamento, TipoServico? tipoServico)
        {
            if (dataAgendamento.Equals(DateTime.MinValue))
                throw new ArgumentNullException(nameof(dataAgendamento));
            else if (tipoServico is null)
                throw new ArgumentNullException(nameof(tipoServico));
        }
        public void ValidarDadosEntradaAlteracaoStatus(Guid? idAgendamento, StatusAgendamento? status)
        {
            if (idAgendamento == Guid.Empty)
                throw new ArgumentNullException(nameof(idAgendamento));
            else if (status == null)
                throw new ArgumentNullException(nameof(status));
        }

        public async Task ValidarDisponibilidadeDiaAgendamento(Guid idOficina, DateTime dataAgendamento, TipoServico tipoServico)
        {
            if (!dataAgendamento.EhDiaUtil() || !dataAgendamento.EhValidaPeloLimite())
                throw new Exception("Data inválida para agendamento. Tente agendar para um dia útil com uma semana de antecedencia");

            try
            {
                var cargaAtualAgendamento = ListarCargaDiariaJaPreenchida(dataAgendamento, idOficina);
                var capacidadeAgndamento = _agendamentoRepository.ObterCargaServicoEOficina(tipoServico, idOficina);

                await Task.WhenAll(cargaAtualAgendamento, capacidadeAgndamento);

                var totalCargaJaPreenchida = cargaAtualAgendamento.Result?.Sum() ?? 0;
                var capacidadeBonus = CalcularCapacidadeBonus(dataAgendamento, capacidadeAgndamento.Result.CargaOficina);

                if (totalCargaJaPreenchida + capacidadeAgndamento.Result.CargaServico > (capacidadeAgndamento.Result.CargaOficina + capacidadeBonus))
                {
                    throw new Exception("A oficina não possui carga suficiente para esse serviço no dia");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Task<List<int>> ListarCargaDiariaJaPreenchida(DateTime dataAgendamento, Guid idOficina)
        {
            var dataMinimaConsulta = dataAgendamento.Date;
            var dataMaximaConsulta = dataAgendamento.Date.AddDays(1);
            return _agendamentoRepository.ListarCargaDiariaJaPreenchida(idOficina, dataMinimaConsulta, dataMaximaConsulta);
        }

        public int CalcularCapacidadeBonus(DateTime data, int capacidadeOficina)
        {
            if (data.EhDiaDeAltaDemanda())
            {
                var capacidadeBonus = (int)Math.Floor(capacidadeOficina * 0.3);
                return capacidadeBonus;
            }

            return 0;
        }

        public async Task PreencherRelatorioAgendamento(Guid idAgendamento, StatusAgendamento status)
        {
            if (status == StatusAgendamento.EmAndamento)
            {
                await _agendamentoRepository.InserirRelatorioAgendamento(idAgendamento, DateTime.Now);
            }
            else if (status == StatusAgendamento.Finalizado)
            {
                await _agendamentoRepository.AlterarRelatorioAgendamento(idAgendamento, DateTime.Now);
            }
        }

        public async Task<List<RelatorioDto>> ComporRelatorioAgendamento(List<RelatorioAgendamentoDto> agendamentos)
        {
            var agendamentosPorDiaDictionary = AgruparAgendamentoPorDiaDictionary(agendamentos);

            var relatorios = new List<RelatorioDto>();

            foreach (var agnts in agendamentosPorDiaDictionary)
            {
                var totalAgendamentos = agnts.Value.Count();
                var agendamentosFinalizados = agnts.Value.Count(x => x.Status == StatusAgendamento.Finalizado);
                var agendamentosNaoRealizados = agnts.Value.Count(x => x.Status == StatusAgendamento.NaoRealizado);
                var agendamentosEmAndamento = agnts.Value.Count(x => x.Status == StatusAgendamento.EmAndamento);

                var historicoAgendamentoList = new List<HistoricoAgendamentoDto>();
                foreach (var historico in agnts.Value)
                {
                    historicoAgendamentoList.Add(new HistoricoAgendamentoDto
                    {
                        IdAgendamento = historico.Id,
                        Servico = historico.Servico,
                        Status = historico.Status,
                        DuracaoEmMinutos = historico.Status != StatusAgendamento.NaoRealizado? (int)(historico.DataFim - historico.DataInicio).TotalMinutes : 0
                    });
                }

                relatorios.Add(new RelatorioDto
                {
                    TotalAgendamentos = totalAgendamentos,
                    AgendamentosFinalizados = agendamentosFinalizados,
                    AgendamentosNaoRealizados = agendamentosNaoRealizados,
                    AgendamentosEmProcesso = agendamentosEmAndamento,
                    Agendamentos = historicoAgendamentoList,
                    Data = agnts.Key

                });
            }

            return relatorios;
        }

        private Dictionary<DateTime, List<RelatorioAgendamentoDto>> AgruparAgendamentoPorDiaDictionary(List<RelatorioAgendamentoDto> agendamentos)
        {
            var agendamentosPorDia = new Dictionary<DateTime, List<RelatorioAgendamentoDto>>();
            foreach (var agendamento in agendamentos)
            {
                if (agendamentosPorDia.ContainsKey(agendamento.DataAgendamento.Date))
                {
                    var agList = agendamentosPorDia.FirstOrDefault(x => x.Key == agendamento.DataAgendamento.Date);

                    agList.Value.Add(agendamento);
                }
                else
                {
                    agendamentosPorDia.Add(agendamento.DataAgendamento.Date, new List<RelatorioAgendamentoDto> { agendamento });
                }
            }

            return agendamentosPorDia;
        }
    }
}
