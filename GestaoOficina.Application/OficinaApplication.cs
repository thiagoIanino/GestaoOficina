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

namespace GestaoOficina.Application
{
    public class OficinaApplication : IOficinaApplication
    {
        private readonly IDominioOficinaService _dominioOficinaService;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IContextoRepository _contextoService;

        public OficinaApplication(
            IDominioOficinaService dominioOficinaService,
            IOficinaRepository oficinaRepository,
            IAgendamentoRepository agendamentoRepository,
            IContextoRepository contextoService)
        {
            _dominioOficinaService = dominioOficinaService;
            _oficinaRepository = oficinaRepository;
            _agendamentoRepository = agendamentoRepository;
            _contextoService = contextoService;
        }

        public async Task<OficinaOutput> AutenticarOficina(OficinaAutenticacaoInput oficinaInput)
        {
            _dominioOficinaService.ValidarDadosAutenticacao(oficinaInput.Cnpj, oficinaInput.Senha);

            var oficina = await _oficinaRepository.ObteroficinaPorCnpjESenha(oficinaInput.Cnpj, oficinaInput.Senha);
            var token = _dominioOficinaService.AutenticarOficina(oficina);

            return new OficinaOutput
            {
                Id = oficina.Id,
                Carga = oficina.Carga,
                Nome = oficina.Nome,
                Token = token
            };
        }

        public async Task<Oficina> CadastrarOficina(OficinaInput oficinaInput)
        {
            _dominioOficinaService.ValidarDadosEntradaOficina(oficinaInput.Carga, oficinaInput.Nome, oficinaInput.Cnpj, oficinaInput.Senha);

            var oficina = new Oficina(
                oficinaInput.Nome,
                oficinaInput.Carga,
                oficinaInput.Cnpj,
                oficinaInput.Senha
                );

            await _oficinaRepository.InserirOficina(oficina);

            return oficina;
        }

        public async Task<List<CapacidadeAgendamentoDto>> ListarCargaDisponivel(DateTime dataReferencia)
        {
            var dataLimite = _dominioOficinaService.CalcularDataLimite(dataReferencia);

            var dataMinimaConsulta = DateTime.Now.Date;
            var dataMaximaConsulta = dataLimite.AddDays(1).Date;

            var idOficina = _contextoService.ObterIdOficinaAutenticada();

            var agendamentos = await _agendamentoRepository.ListarAgendamentosComCarga(dataMinimaConsulta, dataMaximaConsulta, Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d"));
            var cargaOficina = await _oficinaRepository.ObterCargaOficina(idOficina);

            return _dominioOficinaService.CalcularCapacidadeDisponivel(agendamentos, cargaOficina, dataMinimaConsulta, dataLimite);
        }
    }
}
