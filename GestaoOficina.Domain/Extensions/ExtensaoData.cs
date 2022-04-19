using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExtensaoData
    {

        public static bool EhDiaUtil(this DateTime data)
        {
            if(data.DayOfWeek == DayOfWeek.Saturday)
                return false;
            else if(data.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }

        public static bool EhDataFutura(this DateTime data)
        {
            if (data < DateTime.Now)
                return false;

            return true;
        }

        public static bool EhValidaPeloLimite(this DateTime data)
        {
            if (data.AdicionarCincoDiasUteis() >= data.Date)
                return false;

            return true;
        }

        public static DateTime AdicionarCincoDiasUteis(this DateTime data)
        {
            var diasExtrasFimDeSemana = 0;

            if (data.DayOfWeek == DayOfWeek.Saturday)
                diasExtrasFimDeSemana = 2;
            else if (data.DayOfWeek == DayOfWeek.Sunday)
                diasExtrasFimDeSemana = 1;

            return DateTime.Now.AddDays(diasExtrasFimDeSemana + 7);
        }

        public static bool EhDiaDeAltaDemanda(this DateTime data)
        {
            if (data.DayOfWeek == DayOfWeek.Thursday)
                return true;
            else if (data.DayOfWeek == DayOfWeek.Friday)
                return true;

            return false;
        }


    }
}
