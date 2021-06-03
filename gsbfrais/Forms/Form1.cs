using gsbfrais.Forms;
using gsbfrais.model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gsbfrais
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    { 
        /// <summary>
        /// page de connexion
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {


            Database db = new Database();
           
            if(db.getinfo(textBox1.Text.ToString(), textBox2.Text.ToString()))
            {
                Form_Dashboard fd = new Form_Dashboard();
                
                this.Hide();
                fd.setlabel(db.getinfomation(textBox1.Text.ToString(), textBox2.Text.ToString()));
                fd.ShowDialog();


            }
            else
            {
                label5.Visible = true;
            }
         
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Database cn = new Database();

            if(cn.OpenConnection())
            {
                MessageBox.Show("vous etes  connecter à la base de donner "+cn.OpenConnection());

            }
            else
            {
                MessageBox.Show("vous etes pas connecter à la base de donner "+cn.OpenConnection());
            }


        }
    }
}

