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
    public class DominioAgendamentoServiceTest
    {
        private readonly Mock<IAgendamentoRepository> _agendamentoRepository;
        private readonly IDominioAgendamentoService _dominioAgendamentoService;

        public DominioAgendamentoServiceTest()
        {
            _agendamentoRepository = new Mock<IAgendamentoRepository>();

            _dominioAgendamentoService = new DominioAgendamentoService(_agendamentoRepository.Object);
        }

        [Fact]
        public void ValidarDadosEntradaAgendamento_DataInvalida()
        {

            Assert.Throws<ArgumentNullException>(() => _dominioAgendamentoService.ValidarDadosEntradaAgendamento(DateTime.MinValue, TipoServico.Lavacao));

        }
        [Fact]
        public void ValidarDadosEntradaAgendamento_Sucesso()
        {
            _dominioAgendamentoService.ValidarDadosEntradaAgendamento(DateTime.Now, TipoServico.Lavacao);

        }

        [Fact]
        public void ValidarDadosEntradaAgendamento_StatusInvalido()
        {

            Assert.Throws<ArgumentNullException>(() => _dominioAgendamentoService.ValidarDadosEntradaAgendamento(DateTime.Now, null));

        }
        [Fact]
        public void ValidarDadosEntradaAlteracaoStatus_IdInvalido()
        {

            Assert.Throws<ArgumentNullException>(() => _dominioAgendamentoService.ValidarDadosEntradaAlteracaoStatus(Guid.Empty, StatusAgendamento.EmAndamento));

        }

        [Fact]
        public void ValidarDadosEntradaAlteracaoStatus_Sucesso()
        {

            _dominioAgendamentoService.ValidarDadosEntradaAlteracaoStatus(Guid.NewGuid(), StatusAgendamento.EmAndamento);

        }

        [Fact]
        public void ValidarDadosEntradaAlteracaoStatus_StatusInvalida()
        {

            Assert.Throws<ArgumentNullException>(() => _dominioAgendamentoService.ValidarDadosEntradaAlteracaoStatus(Guid.NewGuid(), null));

        }


        [Fact]
        public void ValidarDisponibilidadeDiaAgendamentoAltaDemanda_Sucesso()
        {
            var dataReferencia = DateTime.Now.AddDays(7);
            var dataValida = dataReferencia.EhDiaUtil() ? ProximaDiaDaSemana(dataReferencia, DayOfWeek.Thursday) : ProximaDiaDaSemana(dataReferencia.AddDays(2), DayOfWeek.Thursday);

            _agendamentoRepository.Setup(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<int> { 2, 3 });
            _agendamentoRepository.Setup(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>())).ReturnsAsync(new CargaAgendamentoDto { CargaOficina = 10, CargaServico = 1 });

            _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(Guid.NewGuid(), dataValida, TipoServico.RevisaoBasica);

            _agendamentoRepository.Verify(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _agendamentoRepository.Verify(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public void ValidarDisponibilidadeDiaAgendamento_Sucesso()
        {
            var dataReferencia = DateTime.Now.AddDays(7);
            var dataValida = dataReferencia.EhDiaUtil() ? ProximaDiaDaSemana(dataReferencia, DayOfWeek.Monday) : ProximaDiaDaSemana(dataReferencia.AddDays(2), DayOfWeek.Monday);

            _agendamentoRepository.Setup(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<int> { 2, 3 });
            _agendamentoRepository.Setup(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>())).ReturnsAsync(new CargaAgendamentoDto { CargaOficina = 10, CargaServico = 1 });

            _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(Guid.NewGuid(), dataValida, TipoServico.RevisaoBasica);

            _agendamentoRepository.Verify(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            _agendamentoRepository.Verify(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>()), Times.Once);
        }


        [Fact]
        public void ValidarDisponibilidadeFimDeSemana_Erro()
        {
            var dataReferencia = DateTime.Now.AddDays(7);
            var dataValida = dataReferencia.EhDiaUtil() ? ProximaDiaDaSemana(dataReferencia, DayOfWeek.Saturday) : ProximaDiaDaSemana(dataReferencia.AddDays(2), DayOfWeek.Saturday);

            _agendamentoRepository.Setup(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<int> { 2, 3 });
            _agendamentoRepository.Setup(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>())).ReturnsAsync(new CargaAgendamentoDto { CargaOficina = 10, CargaServico = 1 });

            Assert.ThrowsAsync<Exception>(async () => await _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(Guid.NewGuid(), dataValida, TipoServico.RevisaoBasica));
        }

        [Fact]
        public void ValidarDisponibilidadeSemAntecedencia_Erro()
        {
            var dataReferencia = DateTime.Now;
            var dataValida = dataReferencia.EhDiaUtil() ? dataReferencia : dataReferencia.AddDays(2);

            _agendamentoRepository.Setup(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<int> { 2, 3 });
            _agendamentoRepository.Setup(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>())).ReturnsAsync(new CargaAgendamentoDto { CargaOficina = 10, CargaServico = 1 });

            Assert.ThrowsAsync<Exception>(async () => await _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(Guid.NewGuid(), dataValida, TipoServico.RevisaoBasica));
        }

        [Fact]
        public void ValidarDisponibilidadeOficinaSemCarga_Erro()
        {
            var dataReferencia = DateTime.Now;
            var dataValida = dataReferencia.EhDiaUtil() ? ProximaDiaDaSemana(dataReferencia, DayOfWeek.Monday) : ProximaDiaDaSemana(dataReferencia.AddDays(2), DayOfWeek.Monday);

            _agendamentoRepository.Setup(mock => mock.ListarCargaDiariaJaPreenchida(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<int> { 2, 3, 3 });
            _agendamentoRepository.Setup(mock => mock.ObterCargaServicoEOficina(It.IsAny<TipoServico>(), It.IsAny<Guid>())).ReturnsAsync(new CargaAgendamentoDto { CargaOficina = 10, CargaServico = 3 });

            Assert.ThrowsAsync<Exception>(async () => await _dominioAgendamentoService.ValidarDisponibilidadeDiaAgendamento(Guid.NewGuid(), dataValida, TipoServico.RevisaoBasica));
        }
        [Fact]
        public void PreencherRelatorioAgendamentoEmAndamento_Sucesso()
        {
            _dominioAgendamentoService.PreencherRelatorioAgendamento(Guid.NewGuid(),StatusAgendamento.EmAndamento);

            _agendamentoRepository.Verify(mock => mock.InserirRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Once);
            _agendamentoRepository.Verify(mock => mock.AlterarRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
        }
        [Fact]
        public void PreencherRelatorioAgendamentoFinalizado_Sucesso()
        {
            _dominioAgendamentoService.PreencherRelatorioAgendamento(Guid.NewGuid(), StatusAgendamento.Finalizado);

            _agendamentoRepository.Verify(mock => mock.InserirRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
            _agendamentoRepository.Verify(mock => mock.AlterarRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Once);
        }
        [Fact]
        public void PreencherRelatorioAgendamentoNaoRealizado_Sucesso()
        {
            _dominioAgendamentoService.PreencherRelatorioAgendamento(Guid.NewGuid(), StatusAgendamento.NaoRealizado);

            _agendamentoRepository.Verify(mock => mock.InserirRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
            _agendamentoRepository.Verify(mock => mock.AlterarRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async void ComporRelatorioAgendamento_Sucesso()
        {
            var id = Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d");
            var idOficina = Guid.Parse("c37423c7-89c0-4e2b-8fc4-cb2f64888e8d");

            var relatorioMock = new List<RelatorioAgendamentoDto> {
                new RelatorioAgendamentoDto {
                DataAgendamento = DateTime.Now.AddDays(-3).Date,
                DataFim = DateTime.Now.AddHours(-5),
                DataInicio = DateTime.Now.AddHours(-7),
                Id = id,IdOficina =idOficina, Servico = TipoServico.Lavacao, Status = StatusAgendamento.Finalizado}
            };

            var result = await _dominioAgendamentoService.ComporRelatorioAgendamento(relatorioMock);
            var expected = new List<RelatorioDto> { new RelatorioDto {
                TotalAgendamentos = 1,
                AgendamentosEmProcesso = 0,
                AgendamentosFinalizados = 1,
                AgendamentosNaoRealizados = 0,
                Data = DateTime.Now.AddDays(-3).Date,
                Agendamentos = new List<HistoricoAgendamentoDto>{
                    new HistoricoAgendamentoDto{
                    DuracaoEmMinutos = 119,
                    IdAgendamento = id,
                 Servico = TipoServico.Lavacao,
                    Status =StatusAgendamento.Finalizado
                    }
                } 
                }
            };

            Assert.Equal(expected.First().TotalAgendamentos, result.First().TotalAgendamentos);
            Assert.Equal(expected.First().Data, result.First().Data);
            Assert.Equal(expected.First().Agendamentos.Count, result.First().Agendamentos.Count);
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
