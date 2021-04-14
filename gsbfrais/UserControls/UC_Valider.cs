using gsbfrais.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using gsbfrais.Forms;

namespace gsbfrais.UserControls
{
    public partial class UC_Valider : UserControl
    {

        public UC_Valider()
        {
            InitializeComponent();
            Database cn = new Database();
            cn.getNomDesVisiteur(comboBoxV);


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void UC_Valider_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
            cn.getLesMoisDisponibles(comboBoxD, comboBoxV.SelectedItem.ToString());
            comboBoxD.Visible = true;
            labeld.Visible = true;
            buttond.Visible = true;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
            cn.getLesFraisForfait(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), textBox1, textBox2, textBox3, textBox4);
            cn.getLesFraisHorsForfait(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), listView);
          




        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Database cn = new Database();
            cn.majKM(comboBoxV.SelectedItem.ToString().Substring(0,3).ToString(), comboBoxD.SelectedItem.ToString(), textBox1);
            cn.majREP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), textBox2);
            cn.majNUI(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), textBox3);
            cn.majETP(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), textBox4);


        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {



        }

    
        public void modifier()
        {
            if (listView.SelectedItems.Count> 0)
            {
                ListViewItem element = listView.SelectedItems[0];
                string id = element.SubItems[0].Text;
                string date = element.SubItems[1].Text;
                string libelle = element.SubItems[2].Text;
                string montant =  element.SubItems[3].Text;

                using (modifier m = new modifier())
                {
                    m.id = id;
                    m.date = date;
                    m.libelle = libelle;
                    m.montant = montant;

                    if (m.ShowDialog() == DialogResult.Yes)
                    {
                        Database db = new Database();
                        db.majhorforfait(comboBoxV.SelectedItem.ToString(), comboBoxD.SelectedItem.ToString(), m.libelle.ToString(), m.date.ToString(), m.montant,id);

                        element.SubItems[1].Text = m.date;
                        element.SubItems[2].Text = m.libelle;
                        element.SubItems[3].Text = m.montant;

                    }

                }

            }

            
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
             modifier();
            Console.WriteLine(listView.SelectedItems[0]);

        }
    }
}
