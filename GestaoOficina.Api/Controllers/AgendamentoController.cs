using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GestaoOficina.Api.Controllers
{
    [Route("api/agendamentos")]
    [ExcludeFromCodeCoverage]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoApplication _agendamentoApplication;
        public AgendamentoController(IAgendamentoApplication agendamentoApplication)
        {
            _agendamentoApplication = agendamentoApplication;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarAgendamentosDiarios()
        {
            var agendamentos = await _agendamentoApplication.ListarAgendamentosDiarios();
            return Ok(agendamentos);
        }
        [Route("relatorios")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ComporRelatorio(int quantidadeDias)
        {
            var relatorios = await _agendamentoApplication.ComporRelatorioAgendamentos(quantidadeDias);
            return Ok(relatorios);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RealizarAgendamento([FromBody] Agendamentoinput agendamentoInput)
        {
            var agendamento = await _agendamentoApplication.RealizarAgendamento(agendamentoInput);
            return Ok(agendamento);
        }
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> AlterarStatusAgendamento([FromBody] AgendamentoAlteracaoInput agendamentoInput)
        {
            await _agendamentoApplication.AlterarStatusAgendamento(agendamentoInput);
            return Ok("Status alterado com sucesso");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> ExcluirAgendamento(Guid idAgendamento)
        {
            await _agendamentoApplication.ExcluirAgendamento(idAgendamento);
            return Ok("Agendamento excluido com sucesso");
        }
    }
}
