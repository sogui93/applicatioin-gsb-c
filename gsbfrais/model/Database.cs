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
        /// savoir si on est 
        /// </summary>
        /// <returns>Retourne vrai en cas de connection ouverte, faux en cas d'échec</returns>
        /// 
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
        /// <summary>
        /// connexion à la base de donné
        /// </summary>
        /// <returns></returns>
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
        /// connexion à l'application avec login et mdp valide
        /// </summary>
        /// <param name="login"></param>
        /// /// <param name="mdp"></param>
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
       /// connexion à l'application avec login et mdp valide 
       /// </summary>
       /// <param name="login"></param>
       /// <param name="mdp"></param>
       /// <returns></returns>
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

        /// <summary>
        /// afficher les information d'un visiteur pour x mois 
        /// pour la suivie de paiement des frais 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="l"></param>
        /// <param name="l1"></param>
        public void getinfomationP(String idvisiteur, String mois,Label l , Label l1)
        {
            String s = "SELECT fichefrais.idetat as idEtat, ";
            s += "fichefrais.datemodif as dateModif,";
            s += "fichefrais.nbjustificatifs as nbJustificatifs, ";
            s += "fichefrais.montantvalide as montantValide, ";
            s += "etat.libelle as libEtat ";
            s += "FROM fichefrais ";
            s += "INNER JOIN etat ON fichefrais.idetat = etat.id ";
            s += " WHERE fichefrais.idvisiteur =@idvisiteur ";
            s += "AND fichefrais.mois = @mois";
           
            

            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    l.Text = row["libEtat"].ToString() + " depuis le " + row["dateModif"].ToString();
                    l1.Text = row["montantValide"].ToString();

                }
            }

        }
      
        /// <summary>
        /// afficher les frais forfait par visteur pour x mois 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="l"></param>
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
        /// afficher les champs des lignes de frais hors forfait sous la forme d'un tableau 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="l"></param>
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
        /// afficher les champs des lignes de frais hors forfait sous la forme d'un tableau 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="l"></param>
        public void getLesFraisHorsForfaitP(String idvisiteur, String mois, ListView l)
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
                        string dates = lire["date"].ToString();
                        string libelle = lire["libelle"].ToString();
                        string Montant = lire["montant"].ToString();
                        l.Items.Add(new ListViewItem(new[] {dates, libelle, Montant }));

                    }
                }
            }


        }

      public String getname(String idvisiteur)
      {
            String s = "SELECT visiteur.id AS id, visiteur.nom AS nom, visiteur.prenom AS prenom, visiteur.mdp AS mdp FROM visiteur WHERE visiteur.id = @idvisiteur ";
            string  p = "";
            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {


                    p = row["nom"].ToString();
                    

                }
            }
            return p;
        }

        /// <summary>
        /// afficher les mois disponible pour x visiteur
        /// </summary>
        /// <param name="B"></param>
        /// <param name="idvisiteur"></param>

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
        /// afficher les mois disponible pour x visiteur
        /// </summary>
        /// <param name="B"></param>
        /// <param name="idvisiteur"></param>

        public void getLesMoisDisponiblesP(ComboBox B, String idvisiteur)
        {
            B.Items.Clear();
            string p = "VA";
            string m = "MP";
            string s = "SELECT distinct fichefrais.mois AS mois FROM fichefrais ";
            s += "WHERE fichefrais.idvisiteur = @idvisiteur and fichefrais.idetat ='" + m + "' or idetat ='" + p + "' " ;
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
        /// afficher le nombre de justificatif par visiteur pour x mois 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="a"></param>
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
        /// afficher les visiteur dont etat est CL pour la validation 
        /// </summary>
        /// <param name="B"></param>
         /// <param name="etat"></param>

        public void getNomDesVisiteur(ComboBox B , string etat)
        {
            string s = "SELECT DISTINCT visiteur.id AS id, visiteur.nom AS nom, ";
            s += "visiteur.prenom AS prenom ";
            s += "FROM visiteur ";
            s += "INNER JOIN fichefrais ";
            s += "ON visiteur.id=fichefrais.idvisiteur ";
            s += "Where fichefrais.idetat =@etat";

            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@etat", MySqlDbType.String).Value = @etat;

                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string rowz = row.ItemArray[0].ToString();
                    B.Items.Add(rowz);
                }

            }
            
        }

        /// <summary>
        /// verifier que etat est en mise en paiement
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <returns></returns>
        public String VerifEtat(String idvisiteur, String mois)
        {
            string p = " ";
            if (OpenConnection())
            {
                string s = " SELECT * FROM fichefrais Where fichefrais.idvisiteur=@idvisiteur and fichefrais.mois=@mois ";
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.Add("@idvisiteur", MySqlDbType.String).Value = @idvisiteur;
                cmd.Parameters.Add("@mois", MySqlDbType.String).Value = @mois;
                using (MySqlDataReader lire = cmd.ExecuteReader())
                {
                    p = " **************************************************** ";
                    while (lire.Read())
                    {
                        p = lire["idetat"].ToString();

                    }
                }
            }
            return p;
        }

        /// <summary>
        /// afficher les visteur 
        /// </summary>
        /// <param name="B"></param>
        /// <param name="etat"></param>
        public void getNomDesVisiteurP(ComboBox B, string etat)
        {
            string p = "MP";
            string s = "SELECT DISTINCT visiteur.id AS id, visiteur.nom AS nom, ";
            s += "visiteur.prenom AS prenom ";
            s += "FROM visiteur ";
            s += "INNER JOIN fichefrais ";
            s += "ON visiteur.id=fichefrais.idvisiteur ";
            s += "Where fichefrais.idetat =@etat or idetat ='" + p + "'";

            DataTable dt = new DataTable();
            using (var cmd = new MySqlCommand(s, cn))
            using (var da = new MySqlDataAdapter(cmd))
            {
                cmd.Parameters.Add("@etat", MySqlDbType.String).Value = @etat;

                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string rowz = row.ItemArray[0].ToString();
                    B.Items.Add(rowz);
                }

            }

        }

        /// <summary>
        /// modifier etat d'une fiche de frais 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="etat"></param>
        public void majEtat(String idvisiteur, String mois, String etat)
        {
            string s = "UPDATE fichefrais ";
            s += "SET fichefrais.idetat = @etat ,datemodif = now() ";
            s += " WHERE fichefrais.idvisiteur = @idvisiteur";
            s += " AND fichefrais.mois = @mois ";
            OpenConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand(s, cn);
                cmd.Parameters.AddWithValue("@idvisiteur", idvisiteur);
                cmd.Parameters.AddWithValue("@mois", mois);
                cmd.Parameters.AddWithValue("@etat", etat);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    if (etat == "MP")
                    { MessageBox.Show("La fiche a bien été mis en etat de mise en paiement"); }
                    else if(etat == "RB")
                    {
                        MessageBox.Show("La fiche est bien remboursée");
                    }
                    else if(etat == "VA")
                    { MessageBox.Show("La fiche a bien été mis en etat de mise en paiement"); }
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            closeConnection();

        }



        /// <summary>
        /// afficher les frais forfait des visiteur 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
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
        /// mettre à jour les frais forfait etape
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="a"></param>
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
        /// mettre à jour les  frais forfait kilometrique 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="a"></param>
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
         /// mettre à jour les frais fforfait nuité 
         /// </summary>
         /// <param name="idvisiteur"></param>
         /// <param name="mois"></param>
         /// <param name="a"></param>
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
       /// mettre à jour les les frais forfat repa
       /// </summary>
       /// <param name="idvisiteur"></param>
       /// <param name="mois"></param>
       /// <param name="a"></param>
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
        /// mettre à jour les justificatifs 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="a"></param>
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
        /// mettre à jouir les frais hors forfait 
        /// </summary>
        /// <param name="idvisiteur"></param>
        /// <param name="mois"></param>
        /// <param name="libelle"></param>
        /// <param name="date"></param>
        /// <param name="montant"></param>
        /// <param name="id"></param>
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

