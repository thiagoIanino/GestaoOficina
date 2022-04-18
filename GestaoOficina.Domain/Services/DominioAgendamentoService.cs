using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Enums;
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
    public class DominioAgendamentoService : IDominioAgendamentoService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        public DominioAgendamentoService(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public void ValidarDadosEntradaAgendamento(DateTime dataAgendamento, TipoServico? tipoServico)
        {
            if (dataAgendamento.Equals(DateTime.MinValue))
                throw new ArgumentNullException(nameof(dataAgendamento));
            else if (tipoServico is null)
                throw new ArgumentNullException(nameof(tipoServico));
        }

        public async Task ValidarDisponibilidadeDiaAgendamento(Guid idOficina, DateTime dataAgendamento, TipoServico tipoServico)
        {
            if (!dataAgendamento.EhDiaUtil())
                throw new Exception("Não é possível marcar um agendamento aos fins de semana");

            try
            {
                var cargaAtualAgendamento = ListarCargaDiariaJaPreenchida(dataAgendamento, idOficina);
                var capacidadeAgndamento = _agendamentoRepository.ObterCargaServicoEOficina(tipoServico,idOficina);

                await Task.WhenAll(cargaAtualAgendamento, capacidadeAgndamento);

                var totalCargaJaPreenchida = cargaAtualAgendamento.Result?.Sum() ?? 0;
                var capacidadeBonus = CalcularCapacidadeBonus(dataAgendamento, capacidadeAgndamento.Result.CargaOficina);

                if (totalCargaJaPreenchida + capacidadeAgndamento.Result.CargaServico > (capacidadeAgndamento.Result.CargaOficina + capacidadeBonus))
                {
                    throw new Exception("A oficina não possui carga suficiente para esse serviço no dia");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private Task<List<int>> ListarCargaDiariaJaPreenchida(DateTime dataAgendamento, Guid idOficina)
        {
            var dataMinimaConsulta = dataAgendamento.Date;
            var dataMaximaConsulta = dataAgendamento.Date.AddDays(1);
            return _agendamentoRepository.ListarCargaDiariaJaPreenchida(idOficina, dataMinimaConsulta, dataMaximaConsulta);
        }

        private int CalcularCapacidadeBonus (DateTime data, int capacidadeOficina)
        {
            if(data.EhDiaDeAltaDemanda())
            {
                var capacidadeBonus = (int)Math.Floor(capacidadeOficina * 1.3);
                return capacidadeBonus;
            }

            return 0;
        }
    }
}
