using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcamara.PPIMultitask.TFSCustom.CustomControls.Model
{
    [Serializable]
    public class UsuarioResponsavel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool EnviarEmail { get; set; }

        public UsuarioResponsavel(string nome, string email, bool enviarEmail)
        {
            Nome        = nome;
            Email       = email;
            EnviarEmail = enviarEmail;
        }

        public UsuarioResponsavel()
        {

        }
    }
}
