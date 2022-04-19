using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GestaoOficina.Api.Controllers
{
    [Route("api/oficinas")]
    [ExcludeFromCodeCoverage]
    public class OficinaController : ControllerBase
    {
        private readonly IOficinaApplication _oficinaApplication;

        public OficinaController(IOficinaApplication oficinaApplication)
        {
            _oficinaApplication = oficinaApplication;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CadastrarOficina([FromBody] OficinaInput oficinaInput)
        {
            var oficina = await _oficinaApplication.CadastrarOficina(oficinaInput);

            return Ok(oficina);
        }
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AutenticarOficinaOficina([FromBody] OficinaAutenticacaoInput oficinaInput)
        {
            var oficina = await _oficinaApplication.AutenticarOficina(oficinaInput);

            return Ok(oficina);
        }
        [Route("carga")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarCargaDisponivel(DateTime dataLimite)
        {
            var oficina = await _oficinaApplication.ListarCargaDisponivel(dataLimite);

            return Ok(oficina);
        }
    }
}
