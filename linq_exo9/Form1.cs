using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linq_exo9
{
    public partial class Form1 : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1.	Liste des noms des éditeurs situés à Paris triés par ordre alphabétique

            var ecr = from ec in db.EDITEUR
                      where ec.VILLEED == "paris"
                      orderby ec.NOMED descending
                      select new { ec.NOMED };
            dataGridView1.DataSource = ecr;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*2.	Liste des écrivains de (tous les champs)  langue française,
            triés par ordre alphabétique sur le nom et le prénom*/
            var ecr = db.ECRIVAIN
                .Where(er => er.LANGUECR == "français")
                .OrderByDescending(er => er.NOMECR)
                .OrderByDescending(er => er.PRENOMECR)
                .Select(a => a);
            dataGridView1.DataSource = ecr;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            //3.Liste des titres des ouvrages (NOMOUVR) ayant été édités entre (ANNEEPARU) 1986 et 1987

            var ecr = from ouv in db.OUVRAGE
                      where ouv.ANNEEPARU >= 1986 && ouv.ANNEEPARU <= 1987
                      select new { ouv.NOMOUVR };

            dataGridView1.DataSource = ecr;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //4.	Liste des éditeurs dont le n° de téléphone est inconnu

            var ecr = db.EDITEUR
                .Where(c => c.TELED == null)
                .Select(a => a);
            dataGridView1.DataSource = ecr;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //5.	Liste des auteurs (nom + prénom) dont le nom contient un ‘C’

            var ecr = from ec in db.ECRIVAIN
                      where ec.NOMECR.Contains("c")
                      select new { ec.NOMECR, ec.PRENOMECR };

            dataGridView1.DataSource = ecr;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //6.Liste des titres d’ouvrages contenant  le mot "banque" (éditer une liste triée par n° d'ouvrage croissant)

            var ouv1 = db.OUVRAGE
                .Where(ouv => ouv.NOMOUVR.Contains("banque"))
                .OrderBy(a => a.NOMOUVR)
                .Select(c => new { c.NUMOUVR, c.NOMOUVR });

            dataGridView1.DataSource = ouv1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //7.	Liste des dépôts (nom) situés à Grenoble ou à Lyon

            var dep = from dbe in db.DEPOT
                      where dbe.VILLEDEP == "Grenoble" || dbe.VILLEDEP == "Lyon"
                      select dbe;


            dataGridView1.DataSource = dep;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //15.	Nombre d'écrivains dont la langue est l’anglais ou l’allemand
            var ecr = from ecr1 in db.ECRIVAIN
                      where ecr1.LANGUECR == "Allemand" || ecr1.LANGUECR == "Anglais"
                      group ecr1 by ecr1.LANGUECR into groupecr


                      select new { groupecr.Key, NBR = (groupecr.Count()) };





            dataGridView1.DataSource = ecr;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            /*16.	Nombre total d'exemplaires
            d’ouvrages sur la « gestion de portefeuilles » (LIBRUB) stockés dans les dépôts Grenoblois.*/
            var ourg = from ec in db.STOCKER
                       join ee in db.DEPOT on ec.NUMDEP equals ee.NUMDEP
                       join ouv in db.OUVRAGE on ec.NUMOUVR equals ouv.NUMOUVR
                       join cla in db.CLASSIFICATION on ouv.NUMRUB equals cla.NUMRUB
                       where ee.VILLEDEP == "Grenoble" && cla.LIBRUB.Contains("gestion de portefeuilles")
                       group ec by ec.NUMOUVR into nbqnte
                       select new { nbqnte.Key, aasas=nbqnte.Count() };
            dataGridView1.DataSource = ourg;
    }

        private void button17_Click(object sender, EventArgs e)
        {
            //17.	Titre de l’ouvrage ayant le prix le plus élevé 
            //- faire deux requêtes. (réponse: Le Contr ôle de gestion dans la banque.)
            var ouv = from ov in db.OUVRAGE
                      join tar in db.TARIFER on ov.NUMOUVR equals tar.NUMOUVR
                      group tar by tar.PRIXVENTE into maxt
                      select new { maxt.Key, maxx = maxt.Max() };
            dataGridView1.DataSource = ouv;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //18.	Liste des écrivains avec pour chacun le nombre d’ouvrages qu’il a écrits
            var ouv = from ov in db.OUVRAGE
                      join ecr in db.ECRIRE on ov.NUMOUVR equals ecr.NUMOUVR
                      join ec in db.ECRIVAIN on ecr.NUMECR equals ec.NUMECR
                      group ov by ov.NUMOUVR into g_num
                      select new { g_num.Key, tt = (g_num.Count()) };

            dataGridView1.DataSource = ouv; 

        }

        private void button19_Click(object sender, EventArgs e)
        {
            /*19.	Liste des rubriques de classification avec, pour chacune,
            le nombre d'exemplaires en stock dans les dépôts grenoblois. (7 réponses)*/

            var ee = from ec in db.CLASSIFICATION
                     join ouv in db.OUVRAGE on ec.NUMRUB equals ouv.NUMRUB
                     join st in db.STOCKER on ouv.NUMOUVR equals st.NUMOUVR
                     join dep in db.DEPOT on st.NUMDEP equals dep.NUMDEP
                     where dep.VILLEDEP.Contains("grenoble")
                     group st by  st.QTESTOCK into som
                    
                   //  select new { som.Key,somme=som.Sum()};

            dataGridView1.DataSource = ee;
        }
    }

    }

