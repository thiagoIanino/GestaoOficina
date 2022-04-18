using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Extensions
{
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
