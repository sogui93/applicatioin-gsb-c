using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace gsbfrais.model
{
    class Database
    {
        private static MySqlConnection cn;
        private string server;
        private string database;
        private string user;
        private string password;


        /// <summary>
        /// connection à la base de donne 
        /// </summary>
        public Database()
        {
            server = "localhost";
            database = "gsb_frais";
            user = "root";
            password = "";
            string connectionString = "SERVER=" + server + ";" + "DATABASE = " + database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
            cn = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// acceder à la base de donnee 
        /// </summary>
        /// <returns>Retourne vrai en cas de connection ouverte, faux en cas d'échec</returns>
        public bool OpenConnection()
        {
            try
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        // Cannot connect to server
                        break;
                    case 1045:
                        // Invalid username / password
                        break;
                }

                return false;
            }
        }
        public String Tostring()
        {

            server = "localhost";
            database = "gsb_frais";
            user = "root";
            password = "";
            string connectionString = "SERVER=" + server + ";" + "DATABASE = " + database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";";
            return connectionString;
        }

        /// <summary>
        /// Fermeture de connection
        /// </summary>
        /// <returns>Vrai en cas de succès. Faux en cas d'erreur</returns>
        private bool closeConnection()
        {
            try
            {
                cn.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public MySqlConnection getCn()
        {
            return cn;
        }

        /// <summary>
        /// recuperer les information d'un utilisateur
        /// </summary>
        /// <param name="login"></param>
        public Boolean getinfo(String login, string mdp)
        {
            String s = "SELECT visiteur.id AS id, visiteur.nom AS nom, visiteur.prenom AS prenom, visiteur.mdp AS mdp FROM visiteur WHERE visiteur.login = @login  ";
            Boolean p = false;


            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@login", MySqlDbType.String).Value = @login;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    if (row["mdp"].ToString().Equals(mdp.ToString()))
                    {
                        p = true;
                    }

                }
            }
            return p;

        }

        /// <summary>
        /// recuperer les information d'un utilisateur
        /// </summary>
        /// <param name="login"></param>
        public String getinfomation(String login, string mdp)
        {
            String s = "SELECT visiteur.id AS id, visiteur.nom AS nom, visiteur.prenom AS prenom, visiteur.mdp AS mdp FROM visiteur WHERE visiteur.login = @login  ";
            string info = "";

            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@login", MySqlDbType.String).Value = @login;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    if (row["mdp"].ToString().Equals(mdp.ToString()))
                    {
                        info = row["nom"].ToString() + " " + row["prenom"].ToString();
                    }

                }
            }
            return info;

        }

        public void getLesFraisForfait(String idvisiteur, String mois, ListView l)
        {




            if (OpenConnection())
            {
                l.Items.Clear();
                string s = "SELECT fraisforfait.id as idfrais ,";
                s += "lignefraisforfait.quantite as quantite ";
                s += "FROM lignefraisforfait ";
                s += "INNER JOIN fraisforfait ";
                s += "ON fraisforfait.id = lignefraisforfait.idfraisforfait ";
                s += "WHERE lignefraisforfait.idvisiteur = @idvisiteur ";
                s += "AND lignefraisforfait.mois = @mois ";
                s += "ORDER BY lignefraisforfait.idfraisforfait";
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;
                using (MySqlDataReader lire = cmd.ExecuteReader())
                {
                    string km = "";
                    string nui = "";
                    string rep = "";
                    string etp = "";
                    while (lire.Read())
                    {
                       
                        // les colonnes a remplir 

                        if (lire["idfrais"].ToString() == "KM")
                        {
                            km = lire["quantite"].ToString();
                        }
                        if (lire["idfrais"].ToString() == "REP")
                        {
                            nui = lire["quantite"].ToString();
                        }
                        if (lire["idfrais"].ToString() == "NUI")
                        {
                            rep = lire["quantite"].ToString();
                        }
                        if (lire["idfrais"].ToString() == "ETP")
                        {
                            etp = lire["quantite"].ToString();
                        }

                    }
                    l.Items.Add(new ListViewItem(new[] { km, nui, rep, etp }));
                }
            }
        }

        /// <summary>
        ///reourn les champs des lignes de frais hors forfait sous la forme d'un tableau 
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <param name="mois"></param>
        /// <returns></returns>
        public void getLesFraisHorsForfait(String idvisiteur, String mois,ListView l)
        {
            if (OpenConnection())
            {
                l.Items.Clear();
                string s = "SELECT * FROM lignefraishorsforfait WHERE lignefraishorsforfait.idvisiteur = @idVisiteur AND lignefraishorsforfait.mois = @mois";
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;
                using (MySqlDataReader lire = cmd.ExecuteReader())
                {
                    while (lire.Read())
                    {
                        // les colonnes a remplir 
                        string id = lire["id"].ToString();
                        string dates = lire["date"].ToString();
                        string libelle = lire["libelle"].ToString();
                        string Montant = lire["montant"].ToString();
                        l.Items.Add(new ListViewItem(new[] { id, dates, libelle, Montant }));

                    }
                }
            }


        }

        /// <summary>
        /// Retourne sous forme d'un tableau  toutes les lignes de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <param name="mois"></param>
        /// <returns></returns>
        public MySqlDataReader getLesFraisForfait(String idVisiteur, DateTime mois)
        {
            string s = "SELECT fraisforfait.id as idfrais, ";
            s += "fraisforfait.libelle as libelle, ";
            s += "lignefraisforfait.quantite as quantite ";
            s += "FROM lignefraisforfait ";
            s += "INNER JOIN fraisforfait ";
            s += "ON fraisforfait.id = lignefraisforfait.idfraisforfait ";
            s += "WHERE lignefraisforfait.idvisiteur = @idVisiteur ";
            s += "AND lignefraisforfait.mois = @mois ";
            s += "ORDER BY lignefraisforfait.idfraisforfait";
            MySqlCommand cmd = new MySqlCommand(s, cn);
            cmd.Parameters.AddWithValue("@idVisiteur ", idVisiteur);
            cmd.Parameters.AddWithValue("@mois ", mois);
            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void getLesMoisDisponibles(ComboBox B, String idvisiteur)
        {
           B.Items.Clear();
            string s = "SELECT fichefrais.mois AS mois FROM fichefrais ";
            s += "WHERE fichefrais.idvisiteur = @idvisiteur ";
            s += "ORDER BY fichefrais.mois desc";
            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;

                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string rowz = row.ItemArray[0].ToString();
                    B.Items.Add(rowz);
                }
            }



        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void getjustificatif(String idvisiteur, String mois, TextBox a)
        {


            string s = "SELECT fichefrais.nbjustificatifs FROM fichefrais ";
            s += "WHERE fichefrais.idvisiteur = @idvisiteur ";
            s += "fichefrais.mois = @mois ";
            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;


                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    a.Text = row.ItemArray[0].ToString();
                }

            }
        }
                /// <summary>
                /// Retourne le nom des visiteur ainsi que le prenom
                /// </summary>
                /// <returns></returns>

     public void getNomDesVisiteur(ComboBox B)
        {
            string s = "SELECT visiteur.id AS id, visiteur.nom AS nom, ";
            s += "visiteur.prenom AS prenom ";
            s += "FROM visiteur ";
            MySqlDataAdapter da = new MySqlDataAdapter(s, Tostring());
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                string rowz = row.ItemArray[0].ToString();


                B.Items.Add(rowz);
            }
        }





        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void getLesFraisForfait(String idvisiteur, String mois, TextBox a, TextBox b, TextBox c, TextBox d)
        {


            string s = "SELECT fraisforfait.id as idfrais ,";
            s += "lignefraisforfait.quantite as quantite ";
            s += "FROM lignefraisforfait ";
            s += "INNER JOIN fraisforfait ";
            s += "ON fraisforfait.id = lignefraisforfait.idfraisforfait ";
            s += "WHERE lignefraisforfait.idvisiteur = @idvisiteur ";
            s += "AND lignefraisforfait.mois = @mois ";
            s += "ORDER BY lignefraisforfait.idfraisforfait";
            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;


                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {



                 
                    if (row["idfrais"].ToString() == "KM")
                    {
                        a.Text = row["quantite"].ToString();
                    }
                    if (row["idfrais"].ToString() == "REP")
                    {
                        b.Text = row["quantite"].ToString();
                    }
                    if (row["idfrais"].ToString() == "NUI")
                    {
                        c.Text = row["quantite"].ToString();
                    }
                    if (row["idfrais"].ToString() == "ETP")
                    {
                        d.Text = row["quantite"].ToString();
                    }

                }
            }



        }



        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majETP(String idvisiteur, String mois, TextBox a)
        {
            String p = "ETP";
            string s = "UPDATE lignefraisforfait ";
            s += "SET lignefraisforfait.quantite = @a";
            s += " WHERE lignefraisforfait.idvisiteur = @idvisiteur";
            s += " AND lignefraisforfait.mois = @mois ";
            s += " AND lignefraisforfait.idfraisforfait = '" + p + "'";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@a", a.Text.ToString());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majKM(String idvisiteur, String mois, TextBox a)
        {
            String p = "KM";
            string s = "UPDATE lignefraisforfait ";
            s += "SET lignefraisforfait.quantite = @a";
            s += " WHERE lignefraisforfait.idvisiteur = @idvisiteur";
            s += " AND lignefraisforfait.mois = @mois ";
            s += " AND lignefraisforfait.idfraisforfait = '" + p + "'";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@a", a.Text.ToString());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majNUI(String idvisiteur, String mois, TextBox a)
        {
            String p = "NUI";
            string s = "UPDATE lignefraisforfait ";
            s += "SET lignefraisforfait.quantite = @a";
            s += " WHERE lignefraisforfait.idvisiteur = @idvisiteur";
            s += " AND lignefraisforfait.mois = @mois ";
            s += " AND lignefraisforfait.idfraisforfait = '" + p + "'";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@a", a.Text.ToString());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majREP(String idvisiteur, String mois, TextBox a)
        {
            String p = "REP";
            string s = "UPDATE lignefraisforfait ";
            s += "SET lignefraisforfait.quantite = @a";
            s += " WHERE lignefraisforfait.idvisiteur = @idvisiteur";
            s += " AND lignefraisforfait.mois = @mois ";
            s += " AND lignefraisforfait.idfraisforfait = '" + p + "'";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@a", a.Text.ToString());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }

        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majustificatif(String idvisiteur, String mois, TextBox a)
        {
            string s = "UPDATE fichefrais";
            s += "SET nbjustificatifs = @a ";
            s += "WHERE fichefrais.idvisiteur = @idvisiteur";
            s += "AND fichefrais.mois = @mois";
         
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@a", a.Text.ToString());
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }
        /// <summary>
        /// Retourne les mois pour lesquel un visiteur a une fiche de frais
        /// </summary>
        /// <param name="idVisiteur"></param>
        /// <returns></returns>
        public void majhorforfait(String idvisiteur, String mois, String libelle,String date ,String montant,String id)
        {
            string s = "UPDATE lignefraishorsforfait ";
            s += "SET lignefraishorsforfait.libelle = @libelle,";
            s += "lignefraishorsforfait.montant = @montant,";
            s += "lignefraishorsforfait.date = @date";
            s += " WHERE lignefraishorsforfait.idvisiteur = @idvisiteur";
            s += " AND lignefraishorsforfait.mois = @mois ";
            s += " AND lignefraishorsforfait.id = @id ";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@libelle",libelle );
                cmd.Parameters.AddWithValue("@montant", montant);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@id", id);


                if (cmd.ExecuteNonQuery() == 1)
                {

                    MessageBox.Show("DATA UPDATED");
                }
                else
                {
                    MessageBox.Show("Data NOT UPDATED");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();
        }

    }
}

