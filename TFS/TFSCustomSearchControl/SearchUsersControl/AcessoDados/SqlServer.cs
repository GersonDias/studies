using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Fcamara.PPIMultitask.TFSCustom.CustomControls.AcessoDados
{
    public class SqlServer
    {
        private string _connectionString;
        private string _table;
        private string _campoNome;
        private string _campoEmail;


        public SqlServer(string connectionString, string table, string campoNome, string campoEmail)
        {
            _connectionString   = connectionString;
            _table              = table;
            _campoNome          = campoNome;
            _campoEmail         = campoEmail;
        }

        public IEnumerable<string> RetornarNomesDeUsuario(string nome)
        {
            VerificarSeTodasAsPropriedadesForamPreenchidas();

            List<string> usuarios = new List<string>();
            SqlConnection conn = null;
            try
            {
                using (conn = new SqlConnection(_connectionString))
                {
                    var txtCommand = string.Format("Select {0}, {1} from {2} where Nome like @Nome", _campoNome, _campoEmail, _table);
                    
                    var command = new SqlCommand(txtCommand, conn);

                    command.Parameters.Add(new SqlParameter("@Nome", string.Format("%{0}%", nome)));
                    
                    conn.Open();

                    var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        usuarios.Add(string.Format("{0} ({1})", reader["Nome"], reader["Email"]));
                    }
                }
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return usuarios;
        }

        private void VerificarSeTodasAsPropriedadesForamPreenchidas()
        {
            var erros = new List<string>();
            var mensagem = "Atributo @NomeAtributo do controle deve ser preenchido";

            if (string.IsNullOrEmpty(_connectionString)) 
                erros.Add(mensagem.Replace("@NomeAtributo", "ConnectionString"));
            if (string.IsNullOrEmpty(_table)) 
                erros.Add(mensagem.Replace("@NomeAtributo", "Table"));
            if (string.IsNullOrEmpty(_campoNome)) 
                erros.Add(mensagem.Replace("@NomeAtributo", "CampoNome"));
            if (string.IsNullOrEmpty(_campoEmail)) 
                erros.Add(mensagem.Replace("@NomeAtributo", "CampoEmail"));

            if (erros.Any())
            {
                throw new ConfigurationException("Os seguintes erros foram encontrado no processamento do controle: " + Environment.NewLine + string.Join(Environment.NewLine, erros));
            }
        }
    }
}
