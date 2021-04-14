using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gsbfrais.model
{
    class Comptable
    {
		private String nom;
		private String prenom;
		private String Role = "Comptable";

       
        public Comptable(String nom, String prenom)
		{
			this.nom = nom;
			this.prenom = prenom;
		}

		public String getNom()
		{
			return nom;
		}

		public String getPrenom()
		{
			return prenom;
		}


		public String getRole()
		{
			return Role;
		}

	}
}
