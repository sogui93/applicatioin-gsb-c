using gsbfrais.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gsbfrais.UserControls
{
    public partial class UC_Paiement : UserControl
    {
        public UC_Paiement()
        {
            InitializeComponent();

            Database cn = new Database();
            cn.getNomDesVisiteur(comboBoxV);

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
             Database cn = new Database();
            cn.getLesMoisDisponibles(comboBoxD, comboBoxV.SelectedItem.ToString());
            comboBoxD.Visible = true;
            labeldate.Visible = true;
            buttondate.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
             
            cn.getLesFraisHorsForfaitP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), listView);
            cn.getLesFraisForfait(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), listView1);
            cn.getinfomationP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), labeletats, labelmontant);
            labelinfo.Text = comboBoxD.SelectedItem.ToString();
        }
    }
}
