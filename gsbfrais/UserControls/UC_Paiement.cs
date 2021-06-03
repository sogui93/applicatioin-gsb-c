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
    /// <summary>
    /// 
    /// </summary>
    public partial class UC_Paiement : UserControl
    {
        /// <summary>
        /// suivie des paiement frais 
        /// </summary>
        public UC_Paiement()
        {
            InitializeComponent();

            Database cn = new Database();
            cn.getNomDesVisiteurP(comboBoxV,"VA");

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBoxV.SelectedItem != null)
            {


                Database cn = new Database();
                cn.getLesMoisDisponiblesP(comboBoxD, comboBoxV.SelectedItem.ToString());
                comboBoxD.Visible = true;
                labeldate.Visible = true;
                buttondate.Visible = true;
                label1.Text = cn.getname(comboBoxV.SelectedItem.ToString());
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBoxD.SelectedItem != null)
            {
                Database cn = new Database();

                cn.getLesFraisHorsForfaitP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), listView);
                cn.getLesFraisForfait(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), listView1);
                cn.getinfomationP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), labeletats, labelmontant);
                labelinfo.Text = comboBoxD.SelectedItem.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
            cn.majEtat(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), "MP");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
            if(cn.VerifEtat(comboBoxV.SelectedItem.ToString(),comboBoxD.SelectedItem.ToString()) == "MP")
            {
                cn.majEtat(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), "RB");
            }
            else
            {
                MessageBox.Show("Avant de mettre la fiche en remboursement, la mettre en mise en paiement svp");
            }




        }
    }
}
