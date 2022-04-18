using Dapper;
using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Enums;
using GestaoOficina.Domain.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Repositories
{
    public class AgendamentoRepository : BaseRepository, IAgendamentoRepository
    {
        const string LISTAR_CARGA_DIARIA_JA_PREENCHIDA_QUERY = "select s.cargaRequirida as CargaAgendamento from Agendamento a join Servico s on a.servico = s.id where a.idOficina = @idOficina AND a.status = 1 AND a.dataAgendamento BETWEEN @dataMinima and @dataMaxima";
        const string OBTER_CARGA_REQUIRIDA_SERVICO_E_CARGA_OFICINA_QUERY = @"select cargaRequirida as CargaServico from Servico where id = @idServico
                                                             select carga from Oficina Where id = @idOficina";
        const string INSERIR_AGENDAMENTO_EXECUTE = "insert into Agendamento(id,idOficina,dataAgendamento,servico,status) values (@id,@idOficina,@dataAgendamento,@servico,@status)";
        const string EXCLUIR_AGENDAMENTO_EXECUTE = "delete from Agendamento where id = @id";


        const string CONNECTION_STRING = "Server=localhost,1433;Database=GestaoOficina;User Id=sa;Password=1q2w3e4r@#$";

        public async Task<List<int>> ListarCargaDiariaJaPreenchida(Guid idOficina, DateTime dataMinima, DateTime dataMaxima)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@idOficina", idOficina, DbType.Guid);
            parms.Add("@dataMinima", dataMinima, DbType.DateTime);
            parms.Add("@dataMaxima", dataMaxima, DbType.DateTime);


            return (await ListarAsync<int>(LISTAR_CARGA_DIARIA_JA_PREENCHIDA_QUERY, parms)).ToList();
        }

        public async Task<CapacidadeAgendamentoDto> ObterCargaServicoEOficina(TipoServico servico, Guid idOficina)
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

                    return new CapacidadeAgendamentoDto
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
            parms.Add("@servico", (int)agendamento.TipoServico, DbType.Int16);
            parms.Add("@status", (int)agendamento.Status, DbType.Int16);

            return await ExecutarAsync(INSERIR_AGENDAMENTO_EXECUTE, parms);
        }

        public async Task<int> ExcluirAgendamento(Guid idAgendamento)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", idAgendamento, DbType.Guid);
           

            return await ExecutarAsync(EXCLUIR_AGENDAMENTO_EXECUTE, parms);
        }
    }
}

