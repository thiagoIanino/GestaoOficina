using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RelatorioDto
    {
        public DateTime Data { get; set; }
        public int TotalAgendamentos { get; set; }
        public int AgendamentosFinalizados { get; set; }
        public int AgendamentosNaoRealizados { get; set; }
        public int AgendamentosEmProcesso { get; set; }
        public List<HistoricoAgendamentoDto> Agendamentos { get; set;}
    }
}
