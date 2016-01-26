using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dominio
{
    public class Pessoa : Entidade
    {
        public virtual string Nome { get; set; }
        public virtual int Idade { get; set; }
    }
}
