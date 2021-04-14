using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gsbfrais.Forms
{
    public partial class modifier : Form
    {
        public string id { set { textBo1.Text = value; } }
        public string date { get { return textBo2.Text; } set { textBo2.Text = value; } }
        public string libelle { get { return textBo3.Text; } set { textBo3.Text = value; } }
        public string montant { get { return textBo4.Text; } set { textBo4.Text = value; } }

        public modifier()
        {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;

        }
    }
}
