using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Controls;
using System.Web.Script.Serialization;
using Fcamara.PPIMultitask.TFSCustom.CustomControls.Model;
using System.Collections.Specialized;
using System.Text;

namespace Fcamara.PPIMultitask.TFSCustom.CustomControls
{
    public partial class SearchControl : UserControl, IWorkItemControl, IWorkItemToolTip
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void AlertaErro(string mensagem)
        {
            MessageBox.Show(mensagem, "TFS Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cboDuplicados.Items.Clear();

                if (string.IsNullOrEmpty(txtNome.Text))
                {
                    AlertaErro("Digite o nome de usuário a ser buscado na base de dados");
                    return;
                }

                AcessoDados.SqlServer dados = new AcessoDados.SqlServer(_properties["ConnectionString"], _properties["Table"], _properties["CampoNome"], _properties["CampoEmail"]);

                var usuarios = dados.RetornarNomesDeUsuario(txtNome.Text);

                switch (usuarios.Count())
                {
                    case 0:
                        AlertaErro("Nenhum usuário encontrado contendo " + txtNome.Text + " no nome");
                        break;
                    case 1:
                        lstUsuarios.Items.Add(usuarios.First(), true);
                        break;
                    default:
                        cboDuplicados.Text = "Selecione o usuário correto";
                        usuarios.ToList().ForEach(x => cboDuplicados.Items.Add(x));
                        break;
                }
            }
            catch (Exception err)
            {
                AlertaErro(err.Message + " " + err.StackTrace);
            }
        }

        public event EventHandler AfterUpdateDatasource;

        public event EventHandler BeforeUpdateDatasource;

        private void UpdateData(bool workItemChange = false)
        {
            try
            {
                if (!workItemChange)
                {
                    string jsonUsuariosResponsaveis = GetValueFromListUsuarios();
                    _workItem.Fields[_workItemFieldName].Value = jsonUsuariosResponsaveis;
                }
                else
                {
                    //Preencher o controle lstUsuario
                    string fieldValue = _workItem.Fields[_workItemFieldName].Value.ToString();

                    SetValuesInListUsuarios(fieldValue);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message + " " + err.StackTrace);
            }
        }

        private void SetValuesInListUsuarios(string fieldValue)
        {
            
            if (string.IsNullOrEmpty(fieldValue))
                return;

            var serializer = new JavaScriptSerializer();

            var listaUsuarios = serializer.Deserialize<List<UsuarioResponsavel>>(fieldValue);
            lstUsuarios.Items.Clear();
            foreach (var usuarioResponsavel in listaUsuarios)
            {
                lstUsuarios.Items.Add(string.Format("{0} ({1})", usuarioResponsavel.Nome, usuarioResponsavel.Email), usuarioResponsavel.EnviarEmail);
            }
        }

        public string GetValueFromListUsuarios()
        {
            var usuariosResponsaveis = new List<UsuarioResponsavel>();

            foreach (var item in lstUsuarios.Items)
            {
                string strItem = item.ToString();
                string nome = strItem.Substring(0, strItem.IndexOf('(')).Trim();
                string email = strItem.Substring(strItem.IndexOf('(')).Replace("(", "").Replace(")", "").Trim();
                
                usuariosResponsaveis.Add(new UsuarioResponsavel(nome, email, lstUsuarios.CheckedItems.Contains(item)));  
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(usuariosResponsaveis); 
            return json;
        }

        public void Clear()
        {
        }

        public void FlushToDatasource()
        {
            UpdateData();
        }

        public void InvalidateDatasource()
        {
            UpdateData(true);
        }

        private StringDictionary _properties;

        StringDictionary IWorkItemControl.Properties
        {
            get
            {
                return _properties;
            }
            set
            {
                _properties = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public void SetSite(IServiceProvider serviceProvider)
        {
        }

        private WorkItem _workItem;

        public object WorkItemDatasource
        {
            get
            {
                return _workItem;
            }
            set
            {
                _workItem = value as WorkItem;
            }
        }

        private string _workItemFieldName;
        public string WorkItemFieldName
        {
            get
            {
                return _workItemFieldName;
            }
            set
            {
                _workItemFieldName = value;
            }
        }

        public Label Label
        {
            get;
            set;
        }

        public ToolTip ToolTip
        {
            get;
            set;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (cboDuplicados.SelectedIndex >= 0)
            {
                if (lstUsuarios.Items.Contains(cboDuplicados.Items[cboDuplicados.SelectedIndex].ToString()))
                {
                    AlertaErro("Usuário já foi inserido");
                    return;
                }

                lstUsuarios.Items.Add(cboDuplicados.Items[cboDuplicados.SelectedIndex].ToString(), true);
            }
            else
            {
                AlertaErro("Selecione um usuário no combo box para ser inserido");
            }

            GetValueFromListUsuarios();
        }
    }
}
