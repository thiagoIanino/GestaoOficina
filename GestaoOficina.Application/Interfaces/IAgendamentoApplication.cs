using GestaoOficina.Application.Models;
using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestaoOficina.Application.Interfaces
{
    public interface IAgendamentoApplication
    {
        Task<Agendamento> RealizarAgendamento(Agendamentoinput agendamentoinput);
        Task ExcluirAgendamento(Guid idAgendamento);
    }
}
