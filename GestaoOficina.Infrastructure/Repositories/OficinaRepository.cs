using Dapper;
using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Infrastructure.Repositories
{
    public class OficinaRepository : BaseRepository, IOficinaRepository
    {
        public OficinaRepository()
        {

        }

        const string INSERIR_OFICINA_EXECUTE = "insert into Oficina (id,nome,carga) values (@id, @nome, @carga)";

        public async Task InserirOficina(Oficina oficina)
        {
            DynamicParameters parms = new DynamicParameters();

            parms.Add("@id", oficina.Id, DbType.Guid);
            parms.Add("@nome", oficina.Nome, DbType.AnsiString);
            parms.Add("@carga", oficina.Carga, DbType.Int16);

            await ExecutarAsync(INSERIR_OFICINA_EXECUTE, parms);
        }
    }
}
