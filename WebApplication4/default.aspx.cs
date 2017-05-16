using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using MySql.Data.MySqlClient;

namespace SkakkjørtTrafikkPrøve
{
    public partial class _Default : Page
    {
        // Oppretter objektet som skal inneholde brukernavn
        string brukernavn;

        // Oppretter arraylister for å ta vare på informasjon fra databasen.
        ArrayList brukere = new ArrayList();
        ArrayList arrayListBrukerId = new ArrayList();
        ArrayList arrayListForsok = new ArrayList();
        ArrayList arrayListNavn = new ArrayList();

        protected void Page_Load(object sender, EventArgs e)
        {
            // SQL- spørring som henter id, brukernavn, navn og antall forsøk fra bruker tabellen
            string sql = "SELECT id, brukernavn, navn, antallForsok FROM bruker;";
            // Connectiong string som inneholder informasjon for å koble til databasen
            MySqlConnection con = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");
            // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
            MySqlCommand cmd = new MySqlCommand(sql, con);
            // Åpner databasetilkoblingen
            con.Open();
            // Sender SQL-koden til databaseserveren og lagrer resultatene i DataReaderen (reader-objektet)
            MySqlDataReader reader = cmd.ExecuteReader();
    
            // Så lenge reader finner rader i databasen
            while (reader.Read())
            {
                // Legger brukernavnet fra databasen i brukernavn objektet
                brukernavn = (string) reader["brukernavn"];
                // Legger bruker id'en i userId objektet
                int userId = (int) reader["id"];
                // Legger forsokstallet i forsoksobjektet
                int forsok = (int) reader["antallForsok"];
                // Legger navnet i navn objektet
                string navn = (string) reader["navn"];

                // Legger informasjonen fra brukernavn objektet inn i arraylisten brukere
                brukere.Add(brukernavn);
                // Legger informasjonen fra userId objektet inn i arraylisten arrayListBrukerId
                arrayListBrukerId.Add(userId);
                // Legger informasjonen fra forsok objektet inn i arraylisten arrayListForsok
                arrayListForsok.Add(forsok);
                // Legger informasjonen fra navn objektet inn i arraylisten arrayListNavn
                arrayListNavn.Add(navn);
            }

            // Lukker reader objektet / slutter lesingen
            reader.Close();
            // Lukker databaseforbindelsen
            con.Close();
            // Skjuler feilmeldingslabelen da denne ikke skal vises med mindre det har oppstått en feil som gjøres i metodene under.
            feilmelding.Visible = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Hvis brukernavn ikke er fylt ut!
            if (txtBoxBrukernavn.Text == "")
            {
                // Gjør feilmelding labelen synlig
                feilmelding.Visible = true;
                // Setter en forklarende tekst til feilmelding
                feilmelding.Text = "Du har ikke fylt inn brukernavnet ditt!";
                // Setter en rød skriftfarge for å gjøre feilmeldingen mer synlig
                feilmelding.ForeColor = Color.Red;
            }
            // Eller hvis brukernavnet som er tastet inn finnes i arraylisten brukere
            else if (brukere.Contains(txtBoxBrukernavn.Text))
            {
                /* Henter posisjonstallet til brukeren i arraylisten brukere for å brukke samme posisjon i de andre arraylistene 
                * da disse har tilhørende informasjon om brukeren */
                int posisjonstall = brukere.IndexOf(txtBoxBrukernavn.Text);
                // Henter ID'en til brukeren og legger i idtall objektet
                int idtall = (int) arrayListBrukerId[posisjonstall];
                // Henter forsokstallet til brukeren og legger det i talletForsok objektet
                int talletForsok = (int) arrayListForsok[posisjonstall];

                // Response.Write(forsoktall); //DEBUGGING for å sjekke om forsokstallet er riktig
                try
                {
                    // Oppretter en session for id'en til brukeren som kan brukes for å identifisere brukerne
                    Session["id"] = idtall;
                    // Oppretter en session for forsokstallet som brukes for å identifisere riktig forsøk brukeren er på
                    Session["forsok"] = talletForsok + 1;
                }
                catch
                {
                    
                }

                // Hvis brukeren ikke er registrert med tidligere forsøk
                if (talletForsok == 0)
                {
                    // SQL- spørring oppdaterer brukeren sin antall forsøk og setter denne til 1 (første forsøk)
                    string sql2 = "UPDATE bruker SET antallForsok = 1 WHERE id = " + idtall + ";";
                    // Connectiong string som inneholder informasjon for å koble til databasen
                    MySqlConnection con2 = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");
                    // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                    MySqlCommand cmd2 = new MySqlCommand(sql2, con2);
                    // Åpner databasetilkoblingen
                    con2.Open();
                    // Sender SQL-koden til databaseserveren og utfører den direkte
                    cmd2.ExecuteNonQuery();
                    // Lukker databasetilkoblingen
                    con2.Close();

                    // SQL- spørring som setter inn en rad i brukersvar tabellen med verdiene som allerede er innhentet
                    string sql = "INSERT INTO brukersvar (forsok, brukerId) VALUES (1, " + idtall + ");";
                    // Connectiong string som inneholder informasjon for å koble til databasen
                    MySqlConnection con = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");
                    // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    // Åpner databasetilkoblingen
                    con.Open();
                    // Sender SQL-koden til databaseserveren og utfører den direkte
                    cmd.ExecuteNonQuery();
                    // Lukker databasetilkoblingen
                    con.Close();
                }
                // Eller hvis brukeren har gjennomført testen før
                else
                {
                    // SQL- spørring som oppdaterer brukertabellen med å legge til 1 på antall forsøk
                    string sql2 = "UPDATE bruker SET antallForsok = " + (talletForsok + 1) + " WHERE id = " + idtall + ";";
                    // Connectiong string som inneholder informasjon for å koble til databasen
                    MySqlConnection con2 = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");
                    // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                    MySqlCommand cmd2 = new MySqlCommand(sql2, con2);
                    // Åpner databasetilkoblingen
                    con2.Open();
                    // Sender SQL-koden til databaseserveren og utfører det direkte
                    cmd2.ExecuteNonQuery();
                    // Lukker databasetilkoblingen
                    con2.Close();

                    // SQL- spørring som setter inn en ny rad i brukersvar tabellen med verdiene som allerede er hentet inn
                    string sql = "INSERT INTO brukersvar (forsok, brukerId) VALUES (" + (talletForsok + 1) + ", " + idtall + ");";
                    // Connectiong string som inneholder informasjon for å koble til databasen
                    MySqlConnection con = new MySqlConnection("host=127.0.0.1;user=root;password=root;database=trafikkskole;");
                    // Kommando objekt som inneholder SQL-spørringen og tilkoblingsinformasjonen til databasen
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    // Åpner databasetilkoblingen
                    con.Open();
                    // Sender SQL-koden til databaseserveren og utfører den direkte
                    cmd.ExecuteNonQuery();
                    // Lukker databasetilkoblingen
                    con.Close();
                }
                    // Hvis brukernavnet eksisterer sendes brukeren til testens første spørsmål
                    Response.Redirect("http://localhost:51298/sporsmaal");
            }
            // Hvis brukernavnet som er skrevet inn ikke finnes!
            else
            {
                // Gjør feilmeldingslabelen synlig
                feilmelding.Visible = true;
                // Setter en passende feilmelding som text
                feilmelding.Text = "Brukernavnet som er skrevet inn finnes ikke, er du sikker på at du har skrevet riktig?!";
                // Sette skrifttypen til rød for å gjøre feilmeldingen mer synlig
                feilmelding.ForeColor = Color.Red;
            }
        }
    }
}