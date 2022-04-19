using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Repositories
{
    public interface IContextoRepository
    {
        Guid ObterIdOficinaAutenticada();
    }
}
