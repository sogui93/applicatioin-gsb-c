using gsbfrais.UserControls;
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
    public partial class Form_Dashboard : Form
    {
        int panelWidth;
        bool isCollapsed;
        
        public Form_Dashboard()
        {
            InitializeComponent();
            timerTime.Start();// affiche l'heur au momment de la connexion 
            panelWidth = panelLeft.Width;
            isCollapsed = false;
            UC_Home uch = new UC_Home();
            AddControlsTopanel(uch);

        }
        public void  setlabel(string l)
        {
            name.Text = l;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// animation du menu ouverture et fermeture 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(isCollapsed)
            {
                panelLeft.Width = panelLeft.Width + 10;
                if(panelLeft.Width >=panelWidth)
                {
                    timer1.Stop();
                    isCollapsed = false;
                    this.Refresh();

                }

            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 59)
                {
                    timer1.Stop();
                    isCollapsed = true;
                    this.Refresh();

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>
        /// animation du menu pour le slide 
        /// </summary>
        /// <param name="btn"></param>
        private void moveSidePanel(Control btn)
        {
            panelSide.Top = btn.Top;
            panelSide.Height = btn.Height;
        }
        /// <summary>
        /// include un Userform 
        /// </summary>
        /// <param name="c"></param>
        private void AddControlsTopanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            panelControls.Controls.Clear();
            panelControls.Controls.Add(c);
        }
        
        private void btnHome_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnHome);
            UC_Home uch = new UC_Home();
            AddControlsTopanel(uch);
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnValider);
            UC_Valider ucv = new UC_Valider();
            AddControlsTopanel(ucv);
        }

        private void btnSuivie_Click(object sender, EventArgs e)
        {
            moveSidePanel(btnSuivie);
            UC_Paiement ucs = new UC_Paiement();
            AddControlsTopanel(ucs);
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTime.Text = dt.ToString("HH:MM:ss");
        }

        private void panelControls_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form_Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void labelTime_Click(object sender, EventArgs e)
        {

        }
    }
}
