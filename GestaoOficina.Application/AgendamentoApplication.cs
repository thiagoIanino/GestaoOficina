using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Application
{
    public class AgendamentoApplication : IAgendamentoApplication
    {
        private readonly IDominioAgendamentoService _dominioAgendamentoService;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public AgendamentoApplication(IDominioAgendamentoService dominioAgendamentoService, IAgendamentoRepository agendamentoRepository)
        {
            _dominioAgendamentoService = dominioAgendamentoService;
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<Agendamento> RealizarAgendamento(Agendamentoinput agendamentoinput)
        {
            _dominioAgendamentoService.ValidarDadosEntradaAgendamento(
                agendamentoinput.DataAgendamento,
                agendamentoinput.TipoServico);

            await _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(
                Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d"),
                agendamentoinput.DataAgendamento,
                agendamentoinput.TipoServico);

            var agendamento = new Agendamento(
                Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d"),
                agendamentoinput.DataAgendamento,
                agendamentoinput.TipoServico);

            await _agendamentoRepository.InserirAgendamento(agendamento);

            return agendamento;

        }

        public async Task ExcluirAgendamento(Guid idAgendamento)
        {
            if (idAgendamento == Guid.Empty)
                throw new Exception("Identificador da oficina vazio");

            await _agendamentoRepository.ExcluirAgendamento(idAgendamento);
        }
    }
}
