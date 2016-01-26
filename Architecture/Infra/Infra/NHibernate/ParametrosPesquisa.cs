using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;

namespace Infra.NHibernate
{
    public class ParametrosPesquisa
    {
        public ParametrosPesquisa()
        {
            this.Pagina = 1;
            this.TamanhoPagina = 10;
            this.CampoOrdenar = "Id";
            this.DirecaoOrdenar = "asc";
            this.UsarPaginacao = true;
            this.UsarExclusaoLogica = true;
        }

        [ScriptIgnore]
        public int Pagina { get; set; }

        [ScriptIgnore]
        public int TamanhoPagina { get; set; }

        [ScriptIgnore]
        public int TotalRegistros { get; set; }

        [ScriptIgnore]
        public int RegistrosPular
        {
            get { return this.Pagina <= 1 ? 0 : (this.Pagina - 1) * this.TamanhoPagina; }
        }

        [ScriptIgnore]
        public bool UsarPaginacao { get; set; }

        [ScriptIgnore]
        public bool UsarExclusaoLogica { get; set; }

        [ScriptIgnore]
        public string CampoOrdenar { get; set; }

        [ScriptIgnore]
        public string DirecaoOrdenar { get; set; }

        [ScriptIgnore]
        public string Ordenacao
        {
            get { return String.Format("{0} {1}", this.CampoOrdenar, this.DirecaoOrdenar); }
        }

        [ScriptIgnore]
        [DisplayName("Listar Registros Excluídos")]
        public bool ListarExcluidos { get; set; }

        private Dictionary<string, object> _filtros;

        [ScriptIgnore]
        public Dictionary<string, object> Filtros
        {
            get { return this._filtros ?? (this._filtros = this.ObterFiltros()); }
        }

        public Dictionary<string, object> ObterFiltros()
        {
            var dicionario = new Dictionary<string, object>();

            foreach (var propertyInfo in this.GetType().GetProperties())
            {
                string nomePropriedade = propertyInfo.Name;

                if (nomePropriedade.EndsWith("Id"))
                {
                    nomePropriedade = nomePropriedade.Substring(0, nomePropriedade.Length - 2);
                    if (dicionario.ContainsKey(nomePropriedade))
                        dicionario.Remove(nomePropriedade);
                }


                if (!DeveConsiderarFiltro(nomePropriedade)) continue;

                object valorPropriedade = propertyInfo.GetValue(this, null);

                if (valorPropriedade != null && !dicionario.ContainsKey(nomePropriedade))
                    dicionario.Add(nomePropriedade, valorPropriedade);
            }

            return dicionario;
        }

        public void AdicionarFiltro(string nome, object valor)
        {
            if (valor != null)
            {
                if((valor is string) && (string.IsNullOrWhiteSpace((string)valor)))
                    return;

                this.Filtros.Add(nome, valor);
            }
        }

        private static bool DeveConsiderarFiltro(string nomePropriedade)
        {
            var propriedadesDesconsiderar = new[]
                                            {
                                                "Pagina",
                                                "TamanhoPagina",
                                                "RegistrosPular",
                                                "UsarPaginacao",
                                                "UsarExclusaoLogica",
                                                "TotalRegistros",
                                                "CampoOrdenar",
                                                "DirecaoOrdenar",
                                                "Ordenacao",
                                                "Filtros",
                                                "ListarExcluidos"
                                            };

            return !propriedadesDesconsiderar.Contains(nomePropriedade);
        }
    }
}
