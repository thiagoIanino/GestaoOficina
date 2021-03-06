using GestaoOficina.Application.Models;
using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Application.Interfaces
{
    public interface IOficinaApplication
    {
        Task<Oficina> CadastrarOficina(OficinaInput oficinaInput);
        Task<List<CapacidadeAgendamentoDto>> ListarCargaDisponivel(DateTime dataReferencia);
        Task<OficinaOutput> AutenticarOficina(OficinaAutenticacaoInput oficinaInput);
    }
}
