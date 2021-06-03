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
    /// <summary>
    /// 
    /// </summary>
    public partial class modifier : Form
    {
        /// <summary>
        /// return id frais hors forfait
        /// </summary>
        public string id { set { textBo1.Text = value; } }
        /// <summary>
        /// return date frais hors forfait
        /// </summary>
        public string date { get { return textBo2.Text; } set { textBo2.Text = value; } }
        /// <summary>
        /// return libelle frais hors forfait
        /// </summary>
        public string libelle { get { return textBo3.Text; } set { textBo3.Text = value; } }
        /// <summary>
        /// return montant frais hors forfait
        /// </summary>
        public string montant { get { return textBo4.Text; } set { textBo4.Text = value; } }

        /// <summary>
        /// modifier les frais hors forfait
        /// </summary>
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
