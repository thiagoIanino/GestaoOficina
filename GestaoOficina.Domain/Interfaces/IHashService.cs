using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Domain.Interfaces
{
    public interface IHashService
    {
        string GerarHash(Oficina oficina);
        Task<bool> ValidaEAtualizaHashAsync(Oficina oficina, string hash);
    }
}
