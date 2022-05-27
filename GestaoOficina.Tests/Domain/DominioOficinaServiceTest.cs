using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Enums;
using GestaoOficina.Domain.Extensions;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using GestaoOficina.Domain.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GestaoOficina.Tests.Domain
{
    public class DominioOficinaServiceTest
    {
        private readonly Mock<IDominioAgendamentoService> _dominioAgendamentoService;
        private readonly Mock<ITokenRepository> _tokenRepository;
        private readonly IDominioOficinaService _dominioOficinaService;
        private readonly Mock<IOficinaRepository>_oficinaRepository;
        private readonly Mock<IHashService> _hashService;
        public DominioOficinaServiceTest()
        {
            _dominioAgendamentoService = new Mock<IDominioAgendamentoService>();
            _tokenRepository = new Mock<ITokenRepository>();
            _oficinaRepository = new Mock<IOficinaRepository>();
            _hashService = new Mock<IHashService>();

            _dominioOficinaService = new DominioOficinaService(
                _dominioAgendamentoService.Object,
                _tokenRepository.Object,
                _oficinaRepository.Object,
                _hashService.Object);
        }

        [Fact]
        public void CalcularCapacidadeDisponivel_Sucesso()
        {
            var result =  _dominioOficinaService.CalcularCapacidadeDisponivel(MockAgendamentiServico(),10, DateTime.Now, DateTime.Now.AddDays(4));

            var expected = new List<CapacidadeAgendamentoDto> {
                new CapacidadeAgendamentoDto {CargaDisponivel = 7, Data = DateTime.Now.AddDays(4).Date}
            };

            Assert.Equal(4, result.Count);
            Assert.Equal(7, result.First().CargaDisponivel);

        }

        [Fact]
        public void CalcularDataLimiteCincoDiasPadrao_Sucesso()
        {
            var result = _dominioOficinaService.CalcularDataLimite(DateTime.MinValue);

            Assert.Equal(DateTime.Now.AdicionarCincoDiasUteis().Date, result.Date);

        }

        [Fact]
        public void CalcularDataLimiteDataPersonalizada_Sucesso()
        {
            var result = _dominioOficinaService.CalcularDataLimite(DateTime.Now.AddDays(7).Date);

            Assert.Equal(DateTime.Now.AddDays(7).Date, result);

        }

        [Fact]
        public void CalcularDataLimiteDataPassada_Erro()
        {
            Assert.Throws<Exception>(()=> _dominioOficinaService.CalcularDataLimite(DateTime.Now.AddDays(-7).Date));

        }

        private List<AgendamentoServicoDto> MockAgendamentiServico()
        {
            var id = Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d");
            var idOficina = Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d");
            return new List<AgendamentoServicoDto>
            {
                new AgendamentoServicoDto
                {
                    Id = id,
                    IdOficina = idOficina,
                    CargaRequirida = 3,
                    DataAgendamento = ProximaDiaDaSemana(DateTime.Now, DayOfWeek.Tuesday),
                    Servico = TipoServico.RevisaoBasica,
                    Status = StatusAgendamento.Agendado
                }
            };
        }

        public DateTime ProximaDiaDaSemana(DateTime data, DayOfWeek diaDaSemana)
        {
            var dataRetorno = data;
            while (dataRetorno.DayOfWeek != diaDaSemana)
            {
                dataRetorno = dataRetorno.AddDays(1);
            }
            return dataRetorno;
        }
    }
}
