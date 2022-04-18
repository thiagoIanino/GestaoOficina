using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GestaoOficina.Domain.Entities
{
    public class Oficina
    {
        public Oficina()
        {

        }
        public Oficina(string nome, int carga)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Carga = carga;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Carga { get; set; }
    }
}
