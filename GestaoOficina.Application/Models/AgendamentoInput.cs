﻿using GestaoOficina.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Application.Models
{
    [ExcludeFromCodeCoverage]
    public class Agendamentoinput
    {
        public DateTime DataAgendamento { get; set; }
        public TipoServico TipoServico { get; set; }

    }
}
