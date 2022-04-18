using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GestaoOficina.Api.Controllers
{
    [Route("api/oficina")]
    public class OficinaController : ControllerBase
    {
        private readonly IOficinaApplication _oficinaApplication;

        public OficinaController(IOficinaApplication oficinaApplication)
        {
            _oficinaApplication = oficinaApplication;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarOficina([FromBody] OficinaInput oficinaInput)
        {
            var oficina = await _oficinaApplication.CadastrarOficina(oficinaInput);

            return Ok(oficina);
        }
    }
}
