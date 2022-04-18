using GestaoOficina.Domain.Entities;
using GestaoOficina.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Services
{
    public class DominioOficinaService : IDominioOficinaService
    {

        public DominioOficinaService()
        {
        }

        public void ValidarDadosEntradaOficina(int? carga, string nome)
        {
            if (carga == null)
                throw new ArgumentNullException(nameof(carga));
            else if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentNullException(nameof(carga));
        }
    }
}
