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
    public class OficinaApplication : IOficinaApplication
    {
        private readonly IDominioOficinaService _dominioOficinaService;
        private readonly IOficinaRepository _oficinaRepository;

        public OficinaApplication(
            IDominioOficinaService dominioOficinaService,
            IOficinaRepository oficinaRepository)
        {
            _dominioOficinaService = dominioOficinaService;
            _oficinaRepository = oficinaRepository;
        }

        public async Task<Oficina> CadastrarOficina(OficinaInput oficinaInput)
        {
            _dominioOficinaService.ValidarDadosEntradaOficina(oficinaInput.Carga, oficinaInput.Nome);

            var oficina = new Oficina(
                oficinaInput.Nome,
                oficinaInput.Carga
                );

            await _oficinaRepository.InserirOficina(oficina);

            return oficina;
        }
    }
}
