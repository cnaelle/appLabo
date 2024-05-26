using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace ProjetLabo
{
    class classeBD
    {
        static string connString = "Server=127.0.0.1;Database=projetlabo;Uid=root;Password=; SslMode=none";
        static MySqlConnection conn = new MySqlConnection(connString);

        public static DateTime getDateCourante()
        {
            DateTime thisDay = DateTime.Today;
            return thisDay;
        }
        //Fonction pour vérifier si un utilisateur est dans la base de données
        public static int verifierLoginMdp(string unLogin, string unMdp)
        {
            conn.Open();
            int idSelected = 0;
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT id_personnel FROM personnel WHERE login=@login AND mot_de_passe=@mot_de_passe";
            cmd.Parameters.AddWithValue("@login", unLogin);
            cmd.Parameters.AddWithValue("@mot_de_passe", unMdp);
            idSelected = Convert.ToInt16(cmd.ExecuteScalar());
            conn.Close();
            return idSelected;
        }

        //Fonction pour vérifier si l'utilisateur est bien un technicien
        public static Boolean estTechnicien(int idPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM technicien WHERE id_personnel=@id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", idPersonnel);
            int technicien = Convert.ToInt16(cmd.ExecuteScalar());
            conn.Close();
            if(technicien == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Fonction pour vérifier si l'utilisateur est un responsable ou non
        public static Boolean estResponsable(int idPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT est_responsable FROM personnel WHERE id_personnel=@id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", idPersonnel);
            int responsable = Convert.ToInt16(cmd.ExecuteScalar());
            conn.Close();
            if(responsable == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Fonction statistiques incidents
        public static double getStatsIncidents(string parametre)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            MySqlCommand cmd2 = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM demande_intervention WHERE etat_prise_en_charge=@parametre";
            cmd.Parameters.AddWithValue("@parametre", parametre);
            cmd2.CommandText = "SELECT COUNT(*) FROM demande_intervention";
            double total = Convert.ToDouble(cmd2.ExecuteScalar());
            double pourcentageCalcule = Convert.ToDouble(cmd.ExecuteScalar()) * 100 / total;
            conn.Close();
            return pourcentageCalcule;
        }
        //Fonction total incident
        public static double totalIncident(string parametre)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM demande_intervention WHERE etat_prise_en_charge=@parametre";
            cmd.Parameters.AddWithValue("@parametre", parametre);
            double total = Convert.ToDouble(cmd.ExecuteScalar());
            conn.Close();
            return total;

        }

        //Fonction qui permet de modifier un membre du personnel
        public static void updatePersonnel(int unId, string unLogin, string unMotDePasse)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE personnel SET login=@login,mot_de_passe=@mot_de_passe WHERE id_personnel=@id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", unId);
            cmd.Parameters.AddWithValue("@login", unLogin);
            cmd.Parameters.AddWithValue("@mot_de_passe", unMotDePasse);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de modifier un technicien
        public static void updateTechnicien(int unId, string uneFormation, string unNiveauIntervention)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE technicien SET id_personnel=@id_personnel,formation=@formation, niveau_intervention=@niveauInterv  WHERE id_personnel=@id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", unId);
            cmd.Parameters.AddWithValue("@formation", uneFormation);
            cmd.Parameters.AddWithValue("@niveauInterv", unNiveauIntervention);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de prendre en charge un incident
        public static void updateIncident(string unetatPriseEnCharge, int unIdDemandeInterv )
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Demande_intervention SET etat_prise_en_charge=@unetatPriseEnCharge WHERE id=@unIdDemandeInterv";
            cmd.Parameters.AddWithValue("@unetatPriseEnCharge", unetatPriseEnCharge);
            cmd.Parameters.AddWithValue("@unIdDemandeInterv",unIdDemandeInterv);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une demande d'intervention à un personnel
        public static void lierDemandePersonnel(int unIdDemandeInterv, int unIdPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO fait (id_personnel, id) VALUES (@unIdPersonnel, @unIdDemandeInterv)";
            cmd.Parameters.AddWithValue("@unIdPersonnel", unIdPersonnel);
            cmd.Parameters.AddWithValue("@unIdDemandeInterv", unIdDemandeInterv);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une phase de travail à un incident
        public static void lierPhaseTravailIncident(int unIdDemandeInterv,int unIdPhaseTravail)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO demande (id, id_1) VALUES (@unIdPhaseTravail, @unIdDemandeInterv)";
            cmd.Parameters.AddWithValue("@unIdPhaseTravail", unIdPhaseTravail);
            cmd.Parameters.AddWithValue("@unIdDemandeInterv", unIdDemandeInterv);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier un materiel a un personnel
        public static void lierMaterielPersonnel(int unIdMateriel, int unIdPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO est_affecte_a (id_materiel, id_personnel) VALUES (@unIdMateriel, @unIdPersonnel)";
            cmd.Parameters.AddWithValue("@unIdMateriel", unIdMateriel);
            cmd.Parameters.AddWithValue("@unIdPersonnel", unIdPersonnel);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet d'ajouter un membre du personnel
        public static void ajoutPhaseTravail(PhaseTravail unePhaseTravail)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO phase_travail VAlUES (@id,@date_phase,@heure_debut,@heure_fin,@travail_realise)";
            cmd.Parameters.AddWithValue("@id", unePhaseTravail.getId());
            cmd.Parameters.AddWithValue("@date_phase", unePhaseTravail.getDate());
            cmd.Parameters.AddWithValue("@heure_debut", unePhaseTravail.getHeureDebut());
            cmd.Parameters.AddWithValue("@heure_fin", unePhaseTravail.getHeureFin());
            cmd.Parameters.AddWithValue("@travail_realise", unePhaseTravail.getTravailFait());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet d'ajouter un membre du personnel
        public static void ajoutPersonel(Personnel unpersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO personnel VAlUES (@id_personnel,@nom_complet,@matricule,@date_embauche,@login,@mot_de_passe,@est_responsable)";
            cmd.Parameters.AddWithValue("@id_personnel", unpersonnel.getid_perso());
            cmd.Parameters.AddWithValue("@nom_complet", unpersonnel.getNom());
            cmd.Parameters.AddWithValue("@matricule", unpersonnel.getnomatricule());
            cmd.Parameters.AddWithValue("@date_embauche", unpersonnel.getdatedembauche());
            cmd.Parameters.AddWithValue("@login", unpersonnel.getloging());
            cmd.Parameters.AddWithValue("@mot_de_passe", unpersonnel.getmotdepasse());
            cmd.Parameters.AddWithValue("@est_responsable", unpersonnel.getEstResponsable());
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        //Fonction qui permet de supprimer un personnel
        public static void supprimerUtilisateur(Personnel unPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Personnel WHERE id_personnel = @id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", unPersonnel.getid_perso());
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        //Fonction qui permet d'ajouter un matériel
        public static void ajoutMateriel(Materiel unmateriel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO materiel VAlUES (@id_materiel,@processeur,@memoire,@disque,@date_achat,@garantie)";
            cmd.Parameters.AddWithValue("@id_materiel", unmateriel.getId_materiel());
            cmd.Parameters.AddWithValue("@processeur", unmateriel.getProcesseur());
            cmd.Parameters.AddWithValue("@memoire", unmateriel.getMemoire());
            cmd.Parameters.AddWithValue("@disque", unmateriel.getDisque());
            cmd.Parameters.AddWithValue("@date_achat", unmateriel.getDateAchat());
            cmd.Parameters.AddWithValue("@garantie", unmateriel.getGarantie());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de consulter les matériels
        public static List<Materiel> consulterMateriel()
        {
            List<Materiel> lesMateriels = new List<Materiel>();
            Materiel unMateriel;
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM materiel JOIN est_affecte_a ON est_affecte_a.id_materiel=materiel.id_materiel";
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())

            {
                unMateriel = new Materiel(Convert.ToInt16(dataReader["id_materiel"]), Convert.ToString(dataReader["processeur"]), Convert.ToString(dataReader["memoire"]), Convert.ToString(dataReader["disque"]), Convert.ToDateTime(dataReader["date_achat"]), Convert.ToString(dataReader["garantie"]), Convert.ToInt16(dataReader["id_personnel"]));
                lesMateriels.Add(unMateriel);
            }
            conn.Close();
            return lesMateriels;
        }

        //Fonction qui supprime un matériel à partir de son id
        public static void supprimerMateriel(Materiel unMateriel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM materiel WHERE id_materiel = @id_materiel";
            cmd.Parameters.AddWithValue("@id_materiel", unMateriel.getId_materiel());
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        //Fonction qui ajoute un technicien
        public static void ajouterTechnicien(Technicien unTechnicien, int unIdPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO technicien VAlUES (@id_personnel, @niveau_intervention,@formation)";
            cmd.Parameters.AddWithValue("@id_personnel", unIdPersonnel);
            cmd.Parameters.AddWithValue("@niveau_intervention", unTechnicien.getniveauIntervention());
            cmd.Parameters.AddWithValue("@formation", unTechnicien.getFormation());
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        //Fonction qui ajoute une compétence
        public static void ajouterCompetence(Competence uneCompetence)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO competence VALUES (@id_competence,@description_competence)";
            cmd.Parameters.AddWithValue("@id_competence", uneCompetence.getId());
            cmd.Parameters.AddWithValue("@description_competence", uneCompetence.getDescription());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui ajoute une région
        public static void ajouterRegion(Region uneRegion)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO region VALUES (@id,@nom_region)";
            cmd.Parameters.AddWithValue("@id", uneRegion.getId());
            cmd.Parameters.AddWithValue("@nom_region", uneRegion.getNom());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une compétence à un technicien
        public static void lierCompetenceTechnicien(int unIdPersonnel, int unIdCompetence)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO a (id_personnel, id_competence) VALUES(@unIdPersonnel, @unIdCompetence)";
            cmd.Parameters.AddWithValue("@unIdPersonnel", unIdPersonnel);
            cmd.Parameters.AddWithValue("@unIdCompetence", unIdCompetence);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une compétence à un technicien
        public static void lierRegionPersonnel(int unIdPersonnel, int unIdRegion)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO passe (id_personnel, id) VALUES(@unIdPersonnel, @unIdRegion)";
            cmd.Parameters.AddWithValue("@unIdPersonnel", unIdPersonnel);
            cmd.Parameters.AddWithValue("@unIdRegion", unIdRegion);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une demande d'intervention a un technicien
        public static void lierDemandeTechnicien(int unIdDemande, int unIdTechnicien)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO intervient (id_personnel, id) VALUES(@unIdTechnicien, @unIdDemande)";
            cmd.Parameters.AddWithValue("@unIdTechnicien", unIdTechnicien);
            cmd.Parameters.AddWithValue("@unIdDemande", unIdDemande);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier une demande d'intervention a un materiel
        public static void lierDemandeMateriel(int unIdDemande, int unIdMateriel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO concerne (id, id_materiel) VALUES(@unIdDemande, @unIdMateriel)";
            cmd.Parameters.AddWithValue("@unIdMateriel", unIdMateriel);
            cmd.Parameters.AddWithValue("@unIdDemande", unIdDemande);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui permet de lier un logiciel a un materiel
        public static void lierLogicielMateriel(int unIdLogiciel, int unIdMateriel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO possède (id_materiel, id_logiciel) VALUES(@unIdMateriel, @unIdLogiciel)";
            cmd.Parameters.AddWithValue("@unIdMateriel", unIdMateriel);
            cmd.Parameters.AddWithValue("@unIdLogiciel", unIdLogiciel);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui ajoute un logiciel
        public static void ajouterLogiciel(Logiciel unLogiciel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO logiciel VALUES (@id_logiciel,@nom_logiciel,@date_installation)";
            cmd.Parameters.AddWithValue("@id_logiciel", unLogiciel.getId());
            cmd.Parameters.AddWithValue("@nom_logiciel", unLogiciel.getNom());
            cmd.Parameters.AddWithValue("@date_installation", unLogiciel.getDateInstallation());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui ajoute un incident
        public static void ajouterincident(DemandeIntervention uneDemandeIntervention)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO demande_intervention VALUES (@id,@objet,@etat_prise_en_charge,@type_prise_en_charge,@niveau_urgence,@date_demande)";
            cmd.Parameters.AddWithValue("@id", uneDemandeIntervention.getId());
            cmd.Parameters.AddWithValue("@objet", uneDemandeIntervention.getobjet());
            cmd.Parameters.AddWithValue("@etat_prise_en_charge", uneDemandeIntervention.getEtat());
            cmd.Parameters.AddWithValue("@type_prise_en_charge", uneDemandeIntervention.GetType());
            cmd.Parameters.AddWithValue("@niveau_urgence", uneDemandeIntervention.getNiveauUrgence());
            cmd.Parameters.AddWithValue("@date_demande", uneDemandeIntervention.getDate());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Fonction qui suprime un technicien
        public static void supprimerTechnicien(Technicien unIdPersonnel)
        {
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM technicien WHERE id_personnel = @id_personnel";
            cmd.Parameters.AddWithValue("@id_personnel", unIdPersonnel.getid_perso());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

       
        //Fonction qui permet consulter des incidents
        public static List<DemandeIntervention> consulterincident()
        {
            List<DemandeIntervention> lesDemandesInterventions = new List<DemandeIntervention>();
            DemandeIntervention uneDemandeIntervention;
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM demande_intervention JOIN concerne ON concerne.id=demande_intervention.id JOIN fait ON fait.id=demande_intervention.id";
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                uneDemandeIntervention = new DemandeIntervention(Convert.ToInt16(dataReader["id"]), Convert.ToString(dataReader["objet"]), Convert.ToString(dataReader["etat_prise_en_charge"]), Convert.ToString(dataReader["type_prise_en_charge"]), Convert.ToString(dataReader["niveau_urgence"]), Convert.ToDateTime(dataReader["date_demande"]), Convert.ToInt16(dataReader["id_personnel"]), Convert.ToInt16(dataReader["id_materiel"]));
                lesDemandesInterventions.Add(uneDemandeIntervention);
            }
            conn.Close();
            return lesDemandesInterventions;

        }
        //Fonction qui permet de consulter les personnels
        public static List<Personnel> consulterPersonnel()
        {
        List<Personnel> lesPersonnels = new List<Personnel>();
        Personnel unPersonnel;
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM personnel";
             MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                unPersonnel = new Personnel(Convert.ToInt16(dataReader["id_personnel"]), Convert.ToString(dataReader["nom_complet"]), Convert.ToString(dataReader["matricule"]), Convert.ToDateTime(dataReader["date_embauche"]), Convert.ToString(dataReader["login"]), Convert.ToString(dataReader["mot_de_passe"]), Convert.ToInt16(dataReader["est_responsable"]));
                lesPersonnels.Add(unPersonnel);
            }
            conn.Close();
            return lesPersonnels;


        }

        //Fonction qui permet de consulter les techniciens
        public static List<Technicien> consulterTechnicien()
        {
            List<Technicien> lesTechniciens = new List<Technicien>();
            Technicien unTechnicien;
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM technicien LEFT JOIN personnel ON technicien.id_personnel=personnel.id_personnel";
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                unTechnicien = new Technicien(Convert.ToInt16(dataReader["id_personnel"]), Convert.ToString(dataReader["nom_complet"]), Convert.ToString(dataReader["matricule"]), Convert.ToDateTime(dataReader["date_embauche"]), Convert.ToString(dataReader["login"]), Convert.ToString(dataReader["mot_de_passe"]), Convert.ToString(dataReader["niveau_intervention"]), Convert.ToString(dataReader["formation"]), Convert.ToInt16(dataReader["est_responsable"]));
                lesTechniciens.Add(unTechnicien);
            }
            conn.Close();
            return lesTechniciens;
        }

        //Fonction qui permet de consulter les phases de travail
        public static List<PhaseTravail> consulterPhasesTravail()
        {
            List<PhaseTravail> lesPhases = new List<PhaseTravail>();
            PhaseTravail unePhase;
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM phase_travail JOIN demande ON phase_travail.id=demande.id";
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                unePhase = new PhaseTravail(Convert.ToInt16(dataReader["id"]), Convert.ToDateTime(dataReader["date_phase"]), Convert.ToString(dataReader["heure_debut"]), Convert.ToString(dataReader["heure_fin"]), Convert.ToString(dataReader["travail_realise"]), Convert.ToInt16(dataReader["id_1"]));
                lesPhases.Add(unePhase);
            }
            conn.Close();
            return lesPhases;
        }

        //Fonction qui permet de consulter les logiciels
        public static List<Logiciel> consulterLogiciels()
        {
            List<Logiciel> lesLogiciels = new List<Logiciel>();
            Logiciel unLogiciel;
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM logiciel JOIN possède ON logiciel.id_logiciel=possède.id_logiciel";
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                unLogiciel = new Logiciel(Convert.ToInt16(dataReader["id_logiciel"]), Convert.ToString(dataReader["nom_logiciel"]), Convert.ToDateTime(dataReader["date_installation"]), Convert.ToInt16(dataReader["id_materiel"]));
                lesLogiciels.Add(unLogiciel);
            }
            conn.Close();
            return lesLogiciels;
        }

    }
}
