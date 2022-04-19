using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GestaoOficina.Application
{
    public class AgendamentoApplication : IAgendamentoApplication
    {
        private readonly IDominioAgendamentoService _dominioAgendamentoService;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IContextoRepository _contextoService;

        public AgendamentoApplication(IDominioAgendamentoService dominioAgendamentoService, IAgendamentoRepository agendamentoRepository,
            IContextoRepository contextoService)
        {
            _dominioAgendamentoService = dominioAgendamentoService;
            _agendamentoRepository = agendamentoRepository;
            _contextoService = contextoService;
        }

        public async Task<Agendamento> RealizarAgendamento(Agendamentoinput agendamentoinput)
        {
            _dominioAgendamentoService.ValidarDadosEntradaAgendamento(
                agendamentoinput?.DataAgendamento,
                agendamentoinput?.TipoServico);

            var idOficina = _contextoService.ObterIdOficinaAutenticada();

            await _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(
                idOficina,
                agendamentoinput.DataAgendamento,
                agendamentoinput.TipoServico);

            var agendamento = new Agendamento(
                idOficina,
                agendamentoinput.DataAgendamento,
                agendamentoinput.TipoServico);

            await _agendamentoRepository.InserirAgendamento(agendamento);

            return agendamento;

        }

        public void AtualizarAgendamentosNaoRealizadosJob()
        {
            var dataReferencia = DateTime.Now.Date;
            _ = _agendamentoRepository.AtualizarAgendamentosNaoRealizados(dataReferencia);
        }

        public async Task AlterarStatusAgendamento(AgendamentoAlteracaoInput agendamentoAlteracao)
        {
            _dominioAgendamentoService.ValidarDadosEntradaAlteracaoStatus(
              agendamentoAlteracao?.IdAgendamento,
              agendamentoAlteracao?.Status);

            try
            {
                using (var transacao = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _agendamentoRepository.AlterarStatusAgendamento(agendamentoAlteracao.IdAgendamento,
                        agendamentoAlteracao.Status);

                    await _dominioAgendamentoService.PreencherRelatorioAgendamento(agendamentoAlteracao.IdAgendamento,
                        agendamentoAlteracao.Status);

                    transacao.Complete();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro de integração com banco de dados", ex);
            }

        }

        public async Task ExcluirAgendamento(Guid idAgendamento)
        {
            if (idAgendamento == Guid.Empty)
                throw new Exception("Identificador da oficina vazio");

            await _agendamentoRepository.ExcluirAgendamento(idAgendamento);
        }

        public async Task<List<Agendamento>> ListarAgendamentosDiarios()
        {
            var dataMinimaConsulta = DateTime.Now.Date;
            var dataMaximaConsulta = DateTime.Now.Date.AddDays(1);

            var idOficina = _contextoService.ObterIdOficinaAutenticada();
            return await _agendamentoRepository.ListarAgendamentos(dataMinimaConsulta, dataMaximaConsulta, idOficina);
        }

        public async Task<List<RelatorioDto>> ComporRelatorioAgendamentos(int dias)
        {
            var dataMinimaConsulta = DateTime.Now.Date.AddDays(-dias - 1);
            var dataMaximaConsulta = DateTime.Now.Date.AddDays(1);
            var idOficina = _contextoService.ObterIdOficinaAutenticada();

            var agendamentos = await _agendamentoRepository.ListarAgendamentosParaRelatorio(dataMinimaConsulta, dataMaximaConsulta, idOficina);

            return await _dominioAgendamentoService.ComporRelatorioAgendamento(agendamentos);
        }
    }
}
