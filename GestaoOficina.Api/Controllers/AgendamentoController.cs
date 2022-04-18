using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GestaoOficina.Api.Controllers
{
    [Route("api/agendamento")]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoApplication _agendamentoApplication;
        public AgendamentoController(IAgendamentoApplication agendamentoApplication)
        {
            _agendamentoApplication = agendamentoApplication;
        }

        [HttpPost]
        public async Task<IActionResult> RealizarAgendamento([FromBody] Agendamentoinput agendamentoInput)
        {
            var agendamento = await _agendamentoApplication.RealizarAgendamento(agendamentoInput);

            return Ok(agendamento);
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirAgendamento(Guid idOficina)
        {
            await _agendamentoApplication.ExcluirAgendamento(idOficina);

            return Ok("Agendamento excluido com sucesso");
        }
    }
}
