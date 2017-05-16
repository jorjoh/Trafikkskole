using System;
using System.Collections;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;


namespace SkakkjørtTrafikkPrøve
{
    public partial class resultater : System.Web.UI.Page
    {
        // Oppretter objektene som brukes i de forskjellige metodene i applikasjonen
        int forsokstall;
        int antallRiktige;
        int brukerId;
        string spm;
        string fasit;

        // Oppretter arraylistene som brukes i de forskjellige metodene i applikasjonen
        ArrayList brukerSvar = new ArrayList();
        ArrayList fasitSvarArrayList = new ArrayList();
        ArrayList arrayListSpmId = new ArrayList();
        ArrayList arrayListFeiLSvar = new ArrayList();

        // Oppretter connectionstring til databasen som kan brukes i alle metodene
        MySqlConnection con = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Henter forsokstallet fra session
                forsokstall = (int) Session["forsok"];
                // Henter bruker id'en fra session
                brukerId = (int) Session["id"];
            }
            catch
            {
                
            }

            // Gjør bilde synlig (illustrasjon av resultatet)
            Image1.Visible = true;
            
            // Metoden som henter opp svarene til brukeren
            GetBrukerSvar();
            // Metoden som henter svarene fra fasiten
            GetFasitSvar();

            //Response.Write(brukerSvar.Count.ToString()); //DEBUGGING for å sjekke om alle svarene blir lagt inn i arraylista
           
            // For loop for å sjekke om brukersvarene stemmer med fasit svarene.
            for (int r = 0; r < brukerSvar.Count; r++)
            {
                // Hvis fasitsvaret stemmer med brukersvaret
                if (fasitSvarArrayList[r].ToString() == brukerSvar[r].ToString())
                {
                    // Legg til 1 på antallRiktige
                    antallRiktige++;
                }
                // Hvis fasitsvaret ikke stemmer med brukersvaret
                else
                {
                    // Legg spørsmålsid'en til i arraylisten arrayListSpmId (tar r + 1 for å få riktig id i forhold til databasen da arraylistene teller fra 0 og databasen fra 1)
                    arrayListSpmId.Add((r + 1));
                }
            }

            // Metoden som setter antall riktige svar
            SetAntallRiktig();

            // Hvis antallRiktige er større enn 17 er testen bestått
            if (antallRiktige > 17)
            {
                // Gir bilde URL til en tommel opp-smiley
                Image1.ImageUrl = "http://images.clipartpanda.com/smiley-face-clip-art-thumbs-up-clipart-two-thumbs-up-happy-smiley-emoticon-512x512-eec6.png";
                // Setter en gratulasjonstekst i statusMsg labelen med hvor mange riktige man hadde av 20 spm.
                statusMsg.Text = "Gratulerer! Du bestod teoritesten og hadde " + antallRiktige + " av 20  spørsmål riktig!" + "<br>" + "Du er nå klar for å gjennomføre teoriprøven hos Statens Vegvesen!";
            }
            // Hvis antallRiktige er under 17 er testen ikke bestått
            else
            {
                // Gir bilde URL til et beklager bilde
                Image1.ImageUrl = "http://fristrup.com/oversigt/Beklager.JPG";
                // Setter en beklagelsestekst med en oppfordring om å øve mer og hvor mange riktige man hadde av 20 spm.
                statusMsg.Text = "Beklager, men du må nok øve mer! Du hadde " + antallRiktige + " av 20 spørsmål riktig!";
            }

            // Metoden som henter antall feil og lister disse opp i gridview
            GetAntallFeil();
        }

        private void GetBrukerSvar()
        {
            // SQL- spørring som henter alle brukersvarene på det forsøket brukeren er på
            string sql = "SELECT * FROM brukersvar WHERE brukerId = " + brukerId + " AND forsok = " + forsokstall + ";";
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd = new MySqlCommand(sql, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
            MySqlDataReader reader = cmd.ExecuteReader();

            // Så lenge reader finner rader i databasen
            while (reader.Read())
            {
                // Loop igjennom alle svarene
                for (int r = 1; r < 21; r++)
                {
                    // Legger til brukersvarene i arraylisten brukerSvar
                    brukerSvar.Add(reader.GetString("svar" + r));
                }
            }

            // Lukker reader objektet / slutter lesingen
            reader.Close();
            // Lukker databasetilkoblingen
            con.Close();
        }

        private void GetFasitSvar()
        {
            // SQL- spørring som henter alle svarene fra fasit tabellen sortert etter ID
            string sql = "SELECT svar FROM fasit ORDER BY id";
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd = new MySqlCommand(sql, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
            MySqlDataReader leser = cmd.ExecuteReader();

            // Så lenge reader finner rader i databasen
            while (leser.Read())
            {
                // Legg til fasitsvaret i arraylisten fasitSvarArrayList
                fasitSvarArrayList.Add(leser.GetString("svar"));
            }

            // Lukker leser objektet / slutter lesingen
            leser.Close();
            // Lukker databasetilkoblingen
            con.Close();
        }

        private void SetAntallRiktig()
        {
            // Setter antall feil til 20 (antall spm) minus antallRiktige
            int antallFeil = 20 - antallRiktige;
            // SQL- spørring som setter inn resultatet til brukeren inn i resultater tabellen med hvilket forsøk brukeren brukte
            string sql = "INSERT INTO resultater (forsok, brukerId, antallRiktige, antallFeil) VALUES (" + forsokstall + ", " + brukerId + ", " + antallRiktige + ", " + antallFeil + ");";
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd = new MySqlCommand(sql, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og utfører den direkte
            cmd.ExecuteNonQuery();
            // Lukker databasetilkoblingen
            con.Close();
        }

        private void GetAntallFeil()
        {
            // Løper igjennom alle verdiene i arrayListSpmId (arraylisten med ID'en til de spørsmålene som er besvart feil)
            foreach (int o in arrayListSpmId)
            {
                // Oppretter en SQL setning for å hente ut spørsmålet som er besvart feil og riktige svaret til spørsmålet fra fasiten
                string sqlFasit = "SELECT sporsmaal.id, sporsmaal.spm, fasit.svar FROM sporsmaal JOIN fasit ON sporsmaal.id = fasit.id WHERE sporsmaal.id =" + o + ";";
                // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                MySqlCommand cmdFasit = new MySqlCommand(sqlFasit, con);
                // Åpner databasetilkoblingen
                con.Open();
                // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
                MySqlDataReader reader = cmdFasit.ExecuteReader();
                
                // Så lenge reader objektet finner rader i databasen
                while (reader.Read())
                {
                    // Gir spm objektet spørsmålet som er besvart feil
                    spm = (string) reader["spm"];
                    // Gir fasit objektet fasiten til svaret som er besvart feil
                    fasit = (string) reader["svar"];
                    //Response.Write(sqlFasit); //DEBUGGING for å sjekke om SQL-koden funker
                    // Legger spm og fasit til i todimensjonal arraylist (spm, fasit)
                    arrayListFeiLSvar.Add(new ListItem(spm, fasit)); 
                }

                /* DEBUGGING for å sjekke om ID'en til alle spørsmålene 
                 * som er besvart feil blir lagt i arraylisten arrayListSpmId */

                //Response.Write(o + ", "); 

                // Lukker databasetilkoblingen
                con.Close();
                // Lukker reader objektet / slutter lesingen
                reader.Close();
            }

            // Setter kilden til gridview1 til arraylisten som inneholder spørsmålene som er besvart feil og fasiten til de spm'ene
            GridView1.DataSource = arrayListFeiLSvar;
            // Binder gridview1
            GridView1.DataBind();
        }
    }
}