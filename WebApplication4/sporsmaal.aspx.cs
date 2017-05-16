using System;
using System.Collections;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace SkakkjørtTrafikkPrøve
{
    public partial class test : System.Web.UI.Page
    {
        // Oppretter objekter som brukes for å registrere svarene i databasen
        int brukerId;
        int forsokstallet;
        static int spmNr;

        // En statisk arrayliste som brukes for å holde orden på hvilke spm som er besvart
        static ArrayList numbers = new ArrayList();

        // Connectionstring for å koble til databasen
        MySqlConnection con = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Henter brukerId'en fra session
                brukerId = (int)Session["id"];
                // Henter forsokstallet fra session
                forsokstallet = (int)Session["forsok"];
            }
            catch
            {

            }

            // Hvis siden lastes uten at den responderer på en hendelse (f.eks ved button_click)
            if (!IsPostBack)
            {
                // Setter spmText til Spørsmål + spørsmålstallet man besvarer
                spmText.Text = "Spørsmål " + GetCounter();

                // Metoden som henter random nummer for å vise tilfeldig spørsmål
                HitRandomNummer();

                // Hvis arraylisten numbers inneholder spmNr (tilfeldig tall) så oppdater siden og hent nytt nummer
                if (numbers.Contains(spmNr))
                {
                    // Oppdaterer siden til den siden man er på
                    Response.Redirect(Request.RawUrl);
                }
                // Hvis nummeret ikke finnes i arraylisten
                else
                {
                    // Hent metoden som laster spørsmålet
                    sporsmaal();
                    // Hent metoden som laster svaralternativene
                    svarAlt();
                }

                //Debugging for å sjekke at nummere og id stemmer
                //Response.Write("SpmNr: " + spmNr);
                //Response.Write("GetNumber: " + GetRandomNummer());
                //Response.Write("Bruker id: " + brukerId);
                //Response.Write("forsokstall: " + forsokstallet);

            }

            //Hvis spørsmålsnummeret er mindre enn 20
            if (GetCounter() < 20)
            {
                // Endre teksten på knappen til neste spørsmål
                btnFortsett.Text = "Neste Spørsmål >>";
            }
            // Hvis spørsmålsnummeret er større en 20
            else
            {
                // Endre teksten på knappen til resultater (for å gå til resultater)
                btnFortsett.Text = "Resultater >>";
            }

            // DEBUGGING for å sjekke at id'en til spørsmålene som er besvart blir lagt i arraylisten numbers
            //Response.Write("Antall nummere i arrayliste = " + numbers.Count); 

            // DEBUGGING for å sjekke at metoden GetRandomNumber() henter tilfeldig tall
            //Label3.Visible = true;
            //Label3.Text = GetRandomNummer().ToString();

            // DEBUGGING for å sjekke innholdet i arraylista numbers
            /*foreach (int i in numbers)
            {
                Response.Write(i + ", ");
            }*/

            // Hvis et spørsmål ikke har en illustrasjon
            if (Image1.ImageUrl == "")
            {
                // Skjul bildeboksen
                Image1.Visible = false;
            }
        }


        private void sporsmaal()
        {
            // SQL- spørring som henter all informasjon fra sporsmaal tabellen hvor id er spmNr (tilfeldig nummer)
            string sql = "SELECT * FROM sporsmaal WHERE id = " + spmNr + ";";
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd = new MySqlCommand(sql, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
            MySqlDataReader reader = cmd.ExecuteReader();
    
            // Så lenge reader objektet finner rader i databasen
            while (reader.Read())
            {
                // Sett spørsmålstext labelen til spørsmålet som ligger i valgt rad i databasen
                sporsmaalText.Text = reader.GetString("spm");
            }

            // Hvis kolonnen url inneholder en url til et bilde
            if (reader["url"] != null)
            {
                // Gjør bildeboksen synlig
                Image1.Visible = true;
                // Set url'en til bildeboksen til url'en fra databasen
                Image1.ImageUrl = reader["url"].ToString();
            }

            // Lukk reader objektet / avslutter lesingen
            reader.Close();
            // Lukker databasetilkoblingen
            con.Close();
        }

        private void svarAlt()
        {
            // SQL-kode for å hente alle svaralternativene til spørsmålet som lastes inn
            string sql2 = "SELECT * FROM svaralternativer WHERE id=" + spmNr + ";";
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd2 = new MySqlCommand(sql2, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
            MySqlDataReader leser = cmd2.ExecuteReader();

            // Så lenge leser objektet finner rader i databasen
            while (leser.Read())
            {
                // Setter literal / "radiobutton1 sin tekst" til svaralternativ 1
                Radio1.Text = leser.GetString("alternativ_1");
                // Setter literal / "radiobutton2 sin tekst" til svaralternativ 2
                Radio2.Text = leser.GetString("alternativ_2");
                // Setter literal / "radiobutton3 sin tekst" til svaralternativ 3
                Radio3.Text = leser.GetString("alternativ_3");
            }

            // Lukker leser objektet / avslutter lesing
            leser.Close();
            // Lukker databasetilkoblingen
            con.Close();

            // Gjør slik at man ikke kan trykke på neste spørsmål knappen før man har avgitt svar
            btnFortsett.Enabled = false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // Hvis man har svart på mindre enn 20 spm
            if (GetCounter() < 20)
            {
                // Kjør metoden HitCounter()
                HitCounter();
                // Oppdaterer siden for å laste inn nytt spørsmål
                Response.Redirect(Request.RawUrl);
            }
            // Hvis man har svart på 20 spørsmål
            else
            {
                // Gå til resultater siden
                Response.Redirect("http://localhost:51298/resultater");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //statusMsg.Visible = true; // DEBUGGING for å sjekke om svaret blir registrert

            // Hvis ingen svaralternativer er valgt
            if (!RadioButton1.Checked && !RadioButton2.Checked && !RadioButton3.Checked)
            {
                // Gjør feilmeldingslabelen synlig
                feilmelding.Visible = true;
                // Sett skriftfargen til rød for å gjøre den mer synlig
                feilmelding.ForeColor = Color.Red;
                // Gir feilmeldingslabelen en passende tekst til at man ikke har valgt noe svar
                feilmelding.Text = "Du har ikke valgt noe alternativ!";
            }
            // Hvis et av alternativene er valgt
            else
            {
                // Hvis arraylisten numbers ikke inneholder spmNr (id'en på spørsmålet)
                if (!numbers.Contains(spmNr))
                {
                    // Legg til spmNr (id'en på spørsmålet) i arraylisten numbers
                    numbers.Add(spmNr);
                    // SQL-kode for å oppdatere brukersvar tabellen med svaret brukeren har avgitt på forsøket brukerne er på
                    string sql = "UPDATE brukersvar SET svar" + spmNr + " = '" + feilmelding.Text + "' WHERE brukerId = " + brukerId + " AND forsok = " + forsokstallet + ";";
                    //Response.Write(sql); //DEBUGGING for å sjekke SQL-koden

                    // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    // Åpner databasetilkoblingen
                    con.Open();
                    // Sender SQL-koden til databaseserveren og utfører den direkte 
                    cmd.ExecuteNonQuery();
                    // Lukker databasetilkoblingen
                    con.Close();
                }

                // Setter fortsett knappen aktiv
                btnFortsett.Enabled = true;
                // Deaktiverer svar knappen, sånn at man ikke kan avgi svar flere ganger
                btnSvar.Enabled = false;
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Hvis radiobutton1 er valgt
            if (RadioButton1.Checked)
            {
                // Gi feilmelding sin tekst verdien til radiobutton1 sitt alternativ
                feilmelding.Text = Radio1.Text;
            }
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // Hvis radiobutton2 er valgt
            if (RadioButton2.Checked)
            {
                // Gi feilmelding sin tekst verdien til radiobutton2 sitt alternativ
                feilmelding.Text = Radio2.Text;
            }
        }

        protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // Hvis radiobutton3 er valgt
            if (RadioButton3.Checked)
            {
                // Gi feilmelding sin tekst verdien til radiobutton3 sitt alternativ
                feilmelding.Text = Radio3.Text;
            }
        }

        
        // Oppretter et statisk objekt og setter verdien til 1 som brukes for å vise hvilket spørsmål man besvarer
        private static long count = 1;

        public static void HitCounter()
        {
            // Når metoden HitCounter() kalles, økes objektet count med 1
            count++;
        }
        public static long GetCounter()
        {
            // Når metoden GetCounter() kalles, returnerer den verdien til count objektet
            return count;
        }

        // Oppretter objektet randomNummer
        public static long randomNummer;

        public static void HitRandomNummer()
        {
            // Når metoden HitRandomNummer() kalles, lager metoden et random objekt
            Random rnd = new Random();
            // spmNr får verdien til randomobjektet som henter et tilfeldig tall mellom 1 og 20 (antall spm)
            spmNr = rnd.Next(1, 21);
        }

        public static long GetRandomNummer()
        {
            // Når metoden GetRandomNummer() kalles, returnerer den tilfeldig nummer fra metoden HitRandomNummer()
            return spmNr;
        }
    }
}