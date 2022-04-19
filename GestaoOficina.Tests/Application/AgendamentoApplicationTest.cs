using GestaoOficina.Application;
using GestaoOficina.Application.Interfaces;
using GestaoOficina.Application.Models;
using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Enums;
using GestaoOficina.Domain.Interfaces;
using GestaoOficina.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GestaoOficina.Tests.Application
{
    public class AgendamentoApplicationTest
    {
        private readonly IAgendamentoApplication _agendamentoApplication;
        private readonly Mock<IDominioAgendamentoService> _dominioAgendamentoService;
        private readonly Mock<IAgendamentoRepository> _agendamentoRepository;
        private readonly Mock<IContextoRepository> _contextoService;

        public AgendamentoApplicationTest()
        {
            _dominioAgendamentoService = new Mock<IDominioAgendamentoService>();
            _agendamentoRepository = new Mock<IAgendamentoRepository>();
            _contextoService = new Mock<IContextoRepository>();

            _agendamentoApplication = new AgendamentoApplication(
                _dominioAgendamentoService.Object,
                _agendamentoRepository.Object,
                _contextoService.Object);
        }


        [Fact]
        public async void RealizarAgendamentoComSucesso()
        {
            _contextoService.Setup(mock => mock.ObterIdOficinaAutenticada()).Returns(Guid.NewGuid());
            await _agendamentoApplication.RealizarAgendamento(new Agendamentoinput());
            _agendamentoRepository.Verify(mock => mock.InserirAgendamento(It.IsAny<Agendamento>()), Times.Once);
        }

        [Fact]
        public async void AtualizarAgendamentoNaoRealizados_Sucesso()
        {
             _agendamentoApplication.AtualizarAgendamentosNaoRealizadosJob();

            _agendamentoRepository.Verify(mock => mock.AtualizarAgendamentosNaoRealizados(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async void ComporRelatorios_Sucesso()
        {
            _contextoService.Setup(mock => mock.ObterIdOficinaAutenticada()).Returns(Guid.NewGuid());
            _agendamentoRepository.Setup(mock => mock.ListarAgendamentosParaRelatorio(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(new List<RelatorioAgendamentoDto>());

            await _agendamentoApplication.ComporRelatorioAgendamentos(5);

            _dominioAgendamentoService.Verify(mock => mock.ComporRelatorioAgendamento(It.IsAny<List<RelatorioAgendamentoDto>>()), Times.Once);
        }

        [Fact]
        public async void AlterarAgendamento_Sucesso()
        {
            await _agendamentoApplication.AlterarStatusAgendamento(new AgendamentoAlteracaoInput());

            _agendamentoRepository.Verify(mock => mock.AlterarStatusAgendamento(It.IsAny<Guid>(), It.IsAny<StatusAgendamento>()), Times.Once);
            _dominioAgendamentoService.Verify(mock => mock.PreencherRelatorioAgendamento(It.IsAny<Guid>(), It.IsAny<StatusAgendamento>()), Times.Once);
        }

        [Fact]
        public async void AlterarAgendamento_ErroAoInserirNoBanco()
        {
            _agendamentoRepository.Setup(mock => mock.AlterarStatusAgendamento(It.IsAny<Guid>(), It.IsAny<StatusAgendamento>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(async () => await _agendamentoApplication.AlterarStatusAgendamento(new AgendamentoAlteracaoInput()));
        }

        [Fact]
        public async void ExcluirAgendamentoComSucesso()
        {
            await _agendamentoApplication.ExcluirAgendamento(Guid.NewGuid());
            _agendamentoRepository.Verify(mock => mock.ExcluirAgendamento(It.IsAny<Guid>()), Times.Once);
        }
        [Fact]
        public async void ExcluirAgendamento_Erro()
        {
            await Assert.ThrowsAsync<Exception>(async () => await _agendamentoApplication.ExcluirAgendamento(Guid.Empty));
        }
        [Fact]
        public async void ListarAgendamentosDiarios()
        {
            _contextoService.Setup(mock => mock.ObterIdOficinaAutenticada()).Returns(Guid.NewGuid());
            _agendamentoRepository.Setup(mock => mock.ListarAgendamentos(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).ReturnsAsync(new List<Agendamento>());

            await _agendamentoApplication.ListarAgendamentosDiarios();
            _agendamentoRepository.Verify(mock => mock.ListarAgendamentos(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>()), Times.Once);
        }



    }
}
