using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Interfaces
{
    public interface IDominioOficinaService
    {
        void ValidarDadosEntradaOficina(int? carga, string nome);
    }
}
