using GestaoOficina.Domain.Dtos;
using GestaoOficina.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Interfaces
{
    public interface IDominioOficinaService
    {
        void ValidarDadosEntradaOficina(int? carga, string nome, string cnpj, string senha);
        DateTime CalcularDataLimite(DateTime dataLimite);
        List<CapacidadeAgendamentoDto> CalcularCapacidadeDisponivel(List<AgendamentoServicoDto> agendamentos, int cargaMaxima, DateTime dataMinima, DateTime dataLimite);
        void ValidarDadosAutenticacao(string cnpj, string senha);
        string AutenticarOficina(Oficina oficina);
    }
}
