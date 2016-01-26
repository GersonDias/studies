using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio;
using FluentNHibernate.Mapping;
using Infra.NHibernate;

namespace Repositorio.Maps
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Nome);
            Map(x => x.Idade);
            
            //SqlInsert("exec inserirPessoa @Nome=?, @Idade=?");            
        }
    }
}
