using Dapper;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OficinaRepository : BaseRepository, IOficinaRepository
    {
        public OficinaRepository()
        {

        }

        const string INSERIR_OFICINA_EXECUTE = "insert into Oficina (id,nome,carga,cnpj,senha) values (@id, @nome, @carga,@cnpj,@senha)";
        const string ATUALIZAR_OFICINA_EXECUTE = "Update Oficina set senha = @senha where id = @id";
        const string OBTER_OFICINA_POR_CNPJ_E_SENHA_QUERY = "select id, nome, carga, cnpj, senha from Oficina where cnpj = @cnpj";
        const string OBTER_CARGA_OFICINA_EXECUTE = "select carga from Oficina where id = @id";

        public async Task InserirOficina(Oficina oficina)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", oficina.Id, DbType.Guid);
            parms.Add("@nome", oficina.Nome, DbType.AnsiString);
            parms.Add("@carga", oficina.Carga, DbType.Int16);
            parms.Add("@cnpj", oficina.Cnpj, DbType.AnsiStringFixedLength);
            parms.Add("@senha", oficina.Senha, DbType.AnsiString);

            await ExecutarAsync(INSERIR_OFICINA_EXECUTE, parms);
        }
        public async Task AtualizarSenhaOficina(Oficina oficina)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", oficina.Id, DbType.Guid);
            parms.Add("@senha", oficina.Senha, DbType.AnsiString);

            await ExecutarAsync(ATUALIZAR_OFICINA_EXECUTE, parms);
        }

        public async Task<Oficina> ObterOficinaPorCnpj(string cnpj)
        {
            DynamicParameters parametros = new DynamicParameters();
            // ansiString == varchar / ansiStringfixedLength == char
            parametros.Add("@cnpj", cnpj, DbType.AnsiStringFixedLength);

            return await ObterAsync<Oficina>(OBTER_OFICINA_POR_CNPJ_E_SENHA_QUERY, parametros);
        }

        public async Task<int> ObterCargaOficina(Guid idOficina)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", idOficina, DbType.Guid);

            return await ObterAsync<int>(OBTER_CARGA_OFICINA_EXECUTE, parms);
        }
    }
}
