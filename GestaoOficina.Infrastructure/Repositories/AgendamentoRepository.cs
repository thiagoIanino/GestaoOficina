using Dapper;
using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Enums;
using GestaoOficina.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AgendamentoRepository : BaseRepository, IAgendamentoRepository
    {
        const string LISTAR_CARGA_DIARIA_JA_PREENCHIDA_QUERY = "select s.cargaRequirida as CargaAgendamento from Agendamento a join Servico s on a.servico = s.id where a.idOficina = @idOficina AND a.status = 1 AND a.dataAgendamento BETWEEN @dataMinima and @dataMaxima";
        const string LISTAR_AGENDAMENTOS_QUERY = "select id, idOficina, dataAgendamento, servico, status from Agendamento where idOficina = @idOficina AND status = 1 AND dataAgendamento BETWEEN @dataMinima and @dataMaxima";
        const string LISTAR_RELATORIO_AGENDAMENTOS_QUERY = "select id, idOficina, dataAgendamento, servico, status, dataInicio,dataFim from Agendamento a join RelatorioAgendamento r on a.id = r.idAgendamento where idOficina = @idOficina AND status <> 1 AND dataAgendamento BETWEEN @dataMinima and @dataMaxima";
        const string LISTAR_AGENDAMENTOS_COM_CARGA_QUERY = "select a.id, a.idOficina, a.dataAgendamento, a.servico, a.status, s.cargaRequirida as CargaRequirida from Agendamento a join Servico s on a.servico = s.id  where idOficina = @idOficina AND status = 1 AND dataAgendamento BETWEEN @dataMinima and @dataMaxima";
        const string OBTER_CARGA_REQUIRIDA_SERVICO_E_CARGA_OFICINA_QUERY = @"select cargaRequirida as CargaServico from Servico where id = @idServico
                                                             select carga from Oficina Where id = @idOficina";
        const string INSERIR_AGENDAMENTO_EXECUTE = "insert into Agendamento(id,idOficina,dataAgendamento,servico,status) values (@id,@idOficina,@dataAgendamento,@servico,@status)";
        const string INSERIR_RELATORIO_AGENDAMENTO_EXECUTE = "insert into RelatorioAgendamento(idAgendamento,dataInicio) values (@idAgendamento,@dataInicio)";
        const string ATUALIZAR_AGENDAMENTOS_NAO_REALIZADOS_EXECUTE = "update  Agendamento set status = 3 where  status in(1,4) AND dataAgendamento < @dataReferencia";
        const string ALTERAR_RELATORIO_AGENDAMENTO_EXECUTE = "Update RelatorioAgendamento set dataFim = @dataFim where idAgendamento = @idAgendamento";
        const string EXCLUIR_AGENDAMENTO_EXECUTE = "delete from Agendamento where id = @id";
        const string ALTERAR_STATUS_AGENDAMENTO_EXECUTE = "update Agendamento set status = @status where id = @id";

        const string CONNECTION_STRING = "Server=localhost,1433;Database=GestaoOficina;User Id=sa;Password=1q2w3e4r@#$";

        public async Task<List<int>> ListarCargaDiariaJaPreenchida(Guid idOficina, DateTime dataMinima, DateTime dataMaxima)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@idOficina", idOficina, DbType.Guid);
            parms.Add("@dataMinima", dataMinima, DbType.DateTime);
            parms.Add("@dataMaxima", dataMaxima, DbType.DateTime);


            return (await ListarAsync<int>(LISTAR_CARGA_DIARIA_JA_PREENCHIDA_QUERY, parms)).ToList();
        }
        
        public async Task AlterarStatusAgendamento(Guid idAgendamento, StatusAgendamento status)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", idAgendamento, DbType.Guid);
            parms.Add("@status", (int)status, DbType.Int32);


             await ExecutarAsync(ALTERAR_STATUS_AGENDAMENTO_EXECUTE, parms);
        }

        public async Task<CargaAgendamentoDto> ObterCargaServicoEOficina(TipoServico servico, Guid idOficina)
        {
            DynamicParameters parms = new DynamicParameters();
            parms.Add("@idServico", (int)servico, DbType.Int32);
            parms.Add("@idOficina", idOficina, DbType.Guid);

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {

                using (var multi = conn.QueryMultiple(OBTER_CARGA_REQUIRIDA_SERVICO_E_CARGA_OFICINA_QUERY, parms))
                {
                    var cargaServico = (await multi.ReadAsync<int>()).FirstOrDefault();

                    var cargaOficina = (await multi.ReadAsync<int>()).FirstOrDefault();

                    return new CargaAgendamentoDto
                    {
                        CargaOficina = cargaOficina,
                        CargaServico = cargaServico
                    };
                }

            }
        }

        public async Task<int> InserirAgendamento(Agendamento agendamento)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", agendamento.Id, DbType.Guid);
            parms.Add("@idOficina", agendamento.IdOficina, DbType.Guid);
            parms.Add("@dataAgendamento", agendamento.DataAgendamento, DbType.DateTime);
            parms.Add("@servico", (int)agendamento.Servico, DbType.Int16);
            parms.Add("@status", (int)agendamento.Status, DbType.Int16);

            return await ExecutarAsync(INSERIR_AGENDAMENTO_EXECUTE, parms);
        }

        public async Task<int> ExcluirAgendamento(Guid idAgendamento)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", idAgendamento, DbType.Guid);
           

            return await ExecutarAsync(EXCLUIR_AGENDAMENTO_EXECUTE, parms);
        }

        public async Task<List<Agendamento>> ListarAgendamentos(DateTime dataMinima, DateTime dataMaxima, Guid idOficina)
        {
            DynamicParameters parms = new DynamicParameters();


            parms.Add("@idOficina", idOficina, DbType.Guid);
            parms.Add("@dataMinima", dataMinima, DbType.DateTime);
            parms.Add("@dataMaxima", dataMaxima, DbType.DateTime);

            return (await ListarAsync<Agendamento>(LISTAR_AGENDAMENTOS_QUERY, parms)).ToList();
        } 
        
        public async Task AtualizarAgendamentosNaoRealizados(DateTime dataReferencia)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@dataReferencia", dataReferencia, DbType.DateTime);

             await ExecutarAsync(ATUALIZAR_AGENDAMENTOS_NAO_REALIZADOS_EXECUTE, parms);
        }  
        public async Task<List<RelatorioAgendamentoDto>> ListarAgendamentosParaRelatorio(DateTime dataMinima, DateTime dataMaxima, Guid idOficina)
        {
            DynamicParameters parms = new DynamicParameters();


            parms.Add("@idOficina", idOficina, DbType.Guid);
            parms.Add("@dataMinima", dataMinima, DbType.DateTime);
            parms.Add("@dataMaxima", dataMaxima, DbType.DateTime);

            return (await ListarAsync<RelatorioAgendamentoDto>(LISTAR_RELATORIO_AGENDAMENTOS_QUERY, parms)).ToList();
        }

        public async Task<List<AgendamentoServicoDto>> ListarAgendamentosComCarga(DateTime dataMinima, DateTime dataMaxima, Guid idOficina)
        {
            DynamicParameters parms = new DynamicParameters();


            parms.Add("@idOficina", idOficina, DbType.Guid);
            parms.Add("@dataMinima", dataMinima, DbType.DateTime);
            parms.Add("@dataMaxima", dataMaxima, DbType.DateTime);

            return (await ListarAsync<AgendamentoServicoDto>(LISTAR_AGENDAMENTOS_COM_CARGA_QUERY, parms)).ToList();
        } 
        
        public async Task InserirRelatorioAgendamento(Guid idAgendamento, DateTime dataInicio)
        {
            DynamicParameters parms = new DynamicParameters();


            parms.Add("@idAgendamento", idAgendamento, DbType.Guid);
            parms.Add("@dataInicio", dataInicio, DbType.DateTime);

            await ExecutarAsync(INSERIR_RELATORIO_AGENDAMENTO_EXECUTE, parms);
        }

        public async Task AlterarRelatorioAgendamento(Guid idAgendamento, DateTime dataFim)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@idAgendamento", idAgendamento, DbType.Guid);
            parms.Add("@dataFim", dataFim, DbType.DateTime);

            await ExecutarAsync(ALTERAR_RELATORIO_AGENDAMENTO_EXECUTE, parms);
        }
    }
}

