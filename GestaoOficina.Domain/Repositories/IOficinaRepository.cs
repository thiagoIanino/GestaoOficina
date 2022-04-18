using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Repositories
{
    public interface IOficinaRepository
    {
        Task InserirOficina(Oficina oficina);
    }
}
