using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baze_LV7_predlozak
{

    // NE MIJENJAJ

    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        internal string Pwd
        {
            get
            {
                return txtBoxPwd.Text;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Close(); 
        }
    }
}
