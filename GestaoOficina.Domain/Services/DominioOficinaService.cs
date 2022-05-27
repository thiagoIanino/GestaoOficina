using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
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
    public class DominioOficinaService : IDominioOficinaService
    {
        private readonly IDominioAgendamentoService _dominioAgendamentoService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly IHashService _hashService;
        public DominioOficinaService(IDominioAgendamentoService dominioAgendamentoService, ITokenRepository tokenRepository, IOficinaRepository oficinaRepository, IHashService hashService)
        {
            _dominioAgendamentoService = dominioAgendamentoService;
            _tokenRepository = tokenRepository;
            _oficinaRepository = oficinaRepository;
            _hashService = hashService;
        }

        public void ValidarDadosAutenticacao(string cnpj, string senha)
        {
            if (string.IsNullOrWhiteSpace(cnpj) || cnpj?.Length != 14)
                throw new ArgumentNullException(nameof(cnpj));
            else if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentNullException(nameof(senha));
        }
        public void ValidarDadosEntradaOficina(int? carga, string nome, string cnpj, string senha)
        {
            if (carga == null)
                throw new ArgumentNullException(nameof(carga));
            else if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentNullException(nameof(carga));
            else if (string.IsNullOrWhiteSpace(cnpj) || cnpj?.Length != 14)
                throw new ArgumentNullException(nameof(cnpj));
            else if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentNullException(nameof(senha));
        }
        public string GerarTokenAutenticacao(Oficina oficina)
        {
            return _tokenRepository.GenerateToken(oficina.Id);
        }

        public async Task AutenticarOficina(Oficina oficina, Oficina oficinaConsultada)
        {
            if (oficinaConsultada is null || !(await _hashService.ValidaEAtualizaHashAsync(oficina, oficinaConsultada.Senha)))
            throw new Exception("Usuario não encontrado");

        }

        public DateTime CalcularDataLimite(DateTime dataLimite)
        {
            if (dataLimite != DateTime.MinValue)
            {
                if (!dataLimite.EhDataFutura())
                    throw new Exception("Só é permitido carga de agendamento de datas futuras");

                return dataLimite;
            }
            return DateTime.Now.AdicionarCincoDiasUteis();
        }

        public List<CapacidadeAgendamentoDto> CalcularCapacidadeDisponivel(List<AgendamentoServicoDto> agendamentos, int cargaOficina, DateTime dataMinima, DateTime dataLimite)
        {
            var capacidadeAgendamentoList = new List<CapacidadeAgendamentoDto>();
            var dataReferencia = dataMinima.Date;
            while (dataReferencia <= dataLimite)
            {
                if (dataReferencia.EhDiaUtil())
                {
                    var capacidadeAgendamento = new CapacidadeAgendamentoDto
                    {
                        CargaDisponivel = cargaOficina + _dominioAgendamentoService.CalcularCapacidadeBonus(dataReferencia, cargaOficina),
                        Data = dataReferencia
                    };

                    foreach (var agendamento in agendamentos)
                    {
                        if (agendamento.DataAgendamento.Date == dataReferencia.Date)
                            capacidadeAgendamento.CargaDisponivel -= agendamento.CargaRequirida;
                    }
                    capacidadeAgendamentoList.Add(capacidadeAgendamento);
                }
                dataReferencia = dataReferencia.AddDays(1);
            }

            return capacidadeAgendamentoList;
        }
    }
}
