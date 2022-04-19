using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace GestaoOficina.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Oficina
    {
        public Oficina()
        {

        }
        public Oficina(string nome, int carga, string cnpj, string senha)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Carga = carga;
            Cnpj = cnpj;
            Senha = senha;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Carga { get; set; }
        public string Cnpj { get; set; }
        public string Senha { get; set; }
    }
}
