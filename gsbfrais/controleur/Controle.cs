using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace gsbfrais.model
{
    class Controle
    {
        private static Controle instance = null;

        private Comptable comptable;

        
        /// <summary>
        /// contructeur prive
        /// </summary>
        private Controle()
        {
            
        }

        /// <summary>
        ///  Création de l'instance unique de Controle
        ///@return instance
        /// </summary>
        /// <returns></returns>
        public static  Controle getInstance()
        {
            if(Controle.instance == null )
            {
                Controle.instance = new Controle();
            }
            return Controle.instance;
        }


        /// <summary>
        /// creer un profile comptable 
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// 
        public void  creerComptable(String nom ,String prenom)
        {
            comptable= new Comptable(nom, prenom);
        }

        /// <summary>
        /// recuperter le nom du comptable 
        /// </summary>
        /// <returns></returns>
        public String getNom()
        {
            return comptable.getNom();
        }



        /// <summary>
        /// recuperation du prenom compble 
        /// </summary>
        /// <returns></returns>
        public String getPrenom()
        {
           return  comptable.getPrenom();
        }


        /// <summary>
        /// recuperation du role Comptable 
        /// </summary>
        /// <returns></returns>
        public String getRole()
        {
            return comptable.getRole();
        }
    }
}
