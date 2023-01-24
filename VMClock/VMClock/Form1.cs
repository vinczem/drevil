using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace VMClock
{
    public partial class Form1 : Form
    {
        List<Idopontok> tanorak = new List<Idopontok>();
        string aktualisOraNeve = ""; // milyen óra van éppen? (orarend.csv-ben szereplő óra neve)
        List<string> aktualisOraTanuloi = new List<string>(); // az éppen zajló óra tanulói (az orarend.csv-ben szereplő csoport nevével megegyező .csv tartalma)
        int osszesSzivatasSzama = -1; // összesen ennyiszer fog sorsolni
        int eddigiSzivatasokSzama; // az adott órán eddig ennyiszer sorsolt
        long kovetkezoSzivatasIdeje; // itt lesz a következő név kisorsolva
        public static string szivatottTanuloNeve = "";
        public static bool szivatasVan = false;
        private string oraNevenekLekerdezese()
        {
            int aktualisOraSzama = ora_szunet(); // hanyadik óra van
            string aktualisOraNeve = "";

            int maiNapSzama = (int)DateTime.Now.DayOfWeek;

            foreach (var sor in File.ReadAllLines("orarend.csv"))
            {
                if (!sor.StartsWith("#") && !sor.StartsWith(" "))
                {
                    string[] tomb = sor.Split(';');

                    if (int.Parse(tomb[0]) == maiNapSzama)
                    {
                        try
                        {
                            aktualisOraNeve = tomb[aktualisOraSzama];
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Hiba: Az órarendben nincs erre az időponthoz óra rendelve!");
                        }
                    }
                }
            }

            return aktualisOraNeve == "" ? "-" : aktualisOraNeve;
        }

        private void idopontokFeltoltese()
        {
            foreach (var sor in File.ReadAllLines("orak_kezdete_vege.csv").Skip(1))
            {
                Idopontok i = new Idopontok(sor);
                tanorak.Add(i);
            }
        }

        private void pozicio()
        {
            Screen rightmost = Screen.AllScreens[0];
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.Right > rightmost.WorkingArea.Right)
                    rightmost = screen;
            }

            this.Left = rightmost.WorkingArea.Right - this.Width;
            this.Top = rightmost.WorkingArea.Bottom - this.Height;
            this.Left = (rightmost.WorkingArea.Right / 2) - (this.Width / 2);
            this.Top = 0;
        }

        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.DimGray;
            this.TransparencyKey = Color.DimGray;

            pozicio();
            idopontokFeltoltese();
        }

        private int ora_szunet()
        {
            int retVal = -1;

            DateTime today = DateTime.Today;

            DateTime now = DateTime.Now;
            long nowUnixtime = atvalt(now);

            for (int i = 0; i < tanorak.Count; i++)
            {
                // A tanórák kezdete
                DateTime kezdete = DateTime.Today.AddHours(tanorak[i].kezd_ora).AddMinutes(tanorak[i].kezd_perc);
                DateTime vege = DateTime.Today.AddHours(tanorak[i].vege_ora).AddMinutes(tanorak[i].vege_perc);

                long kezdeteUnixtime = atvalt(kezdete);
                long vegeUnixtime = atvalt(vege);

                if (nowUnixtime >= kezdeteUnixtime && nowUnixtime <= vegeUnixtime)
                {
                    // tart az óra
                    retVal = i + 1;
                }
            }

            return retVal;
        }

        private long atvalt(DateTime atvaltando)
        {
            return ((DateTimeOffset)atvaltando).ToUnixTimeSeconds();
        }

        private void oraTanuloinakLekerdezese()
        {
            try
            {
                foreach (var tanulo in File.ReadAllLines(aktualisOraNeve + ".csv"))
                {
                    aktualisOraTanuloi.Add(tanulo);
                    Console.WriteLine("      - Tanuló: {0}", tanulo);
                };
            }
            catch (Exception)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int ora = ora_szunet();

            if (ora != -1)
            {
                DateTime oraKezdete = DateTime.Today.AddHours(tanorak[ora - 1].kezd_ora).AddMinutes(tanorak[ora - 1].kezd_perc);
                long oraKezdeteUnixtime = atvalt(oraKezdete);

                DateTime hatravan = UnixTimeStampToDateTime(hatralevoMasodpercekSzama());
                DateTime now = DateTime.Now;
                long nowUnixtime = atvalt(now);

                // ha elkezdődik az óra, lekérdezzük, hogy milyen óra van
                if ((hatralevoMasodpercekSzama() == 45 * 60 || aktualisOraNeve == "") && osszesSzivatasSzama == -1)
                {
                    aktualisOraNeve = oraNevenekLekerdezese();
                    Console.WriteLine("[{0}] Info: Most {1} óra van.", DateTime.Now, aktualisOraNeve);

                    // betöltjük az óra tanulóit
                    oraTanuloinakLekerdezese();

                    // töröljük, hogy eddig hányszor volt szívatás
                    eddigiSzivatasokSzama = 0;

                    // szívatások beállítása
                    szivatasSorsolas();
                }

                // 5 perccel az óra vége előtt narancssárgára váltunk
                if (hatralevoMasodpercekSzama() <= 300 && hatralevoMasodpercekSzama() > 180)
                {
                    label1.Text = DateTime.Now.ToString("HH:mm:ss") + " (" + ora + ". óra)";
                    label1.ForeColor = Color.Orange;
                }
                // utolsó 3 percben piros lesz a szám és visszaszámlálunk
                else if (hatralevoMasodpercekSzama() <= 180 && hatralevoMasodpercekSzama() != -1)
                {
                    label1.Text = hatravan.ToString("mm:ss") + " (" + ora + ". óra)";
                    label1.ForeColor = Color.Red;
                }
                // előtte pedig fehér
                else
                {
                    label1.Text = DateTime.Now.ToString("HH:mm:ss") + " (" + ora + ". óra)";
                    label1.ForeColor = Color.White;
                }
            }
            // a szünetben zöld
            else
            {
                label1.Text = DateTime.Now.ToString("HH:mm:ss") + " (SZÜNET)";
                osszesSzivatasSzama = -1;
                label1.ForeColor = Color.Green;
            }

            if (szivatasVan)
            {
                label1.Text = "Dr. Evil sorsolt!";
                label1.ForeColor = Color.Red;
            }
        }

        private void szivatasSorsolas()
        {
            // hány alkalommal lesz szívatás
            Random random = new Random();
            // osszesSzivatasSzama = random.Next(0, 5);
            osszesSzivatasSzama = 10;
            Console.WriteLine("[{0}] Info: Ezen az órán {1} alkalommal fogunk sorsolni.", DateTime.Now, osszesSzivatasSzama);

            // ha lesz az órán szívatás, akkor generálunk véletlenszámokat
            if (osszesSzivatasSzama > 0) {
                // egy óra 45 * 60 = 2700 mp
                int oszto = 2700 / osszesSzivatasSzama;
                Console.WriteLine("  Osztó: {0}", oszto);

                // 
                for (int i = 0; i < osszesSzivatasSzama; i++)
                {
                    int tol = i * oszto;
                    int ig = (i + 1) * oszto - 1;
                    int veletlenSzam = random.Next(tol, ig) + 1;
                    Console.WriteLine("  {0}. szívatás lehetséges ideje: {1} - {2} másodpercben. A sorsolt idő: {3}", i + 1, tol, ig, veletlenSzam);

                    // beállítjuk a timert
                    timerBeallitasa(veletlenSzam);
                }
            }
        }

        private void timerBeallitasa(int szam)
        {
            System.Timers.Timer newTimer = new System.Timers.Timer();
            newTimer.Elapsed += new ElapsedEventHandler(szivatasTimerTick);
            newTimer.Interval = szam * 1000;
            newTimer.Enabled = true;
            Console.WriteLine("A timer elindult.");
        }

        private void szivatasTimerTick(object sender, EventArgs e)
        {
            System.Timers.Timer tickeloTimer = (System.Timers.Timer)sender;
            Console.WriteLine("Szívatás!");

            // leállítjuk a tickelő timert
            tickeloTimer.Stop();

            // ha van az órához tanuló rendelve, sorsolunk, és megjelenítjük a szívatás panelt
            if (aktualisOraTanuloi.Count > 0)
            {
                Console.WriteLine("  Vannak az órához tanulók rendelve, sorsolunk.");
                szivatottTanuloNeve = szivatottTanuloSorsolasa();

                // szívatás jelző beállítása
                szivatasVan = true;

                szivatasForm tmp = new szivatasForm();
                tmp.ShowDialog();
            }
            else
            {
                Console.WriteLine("  Nincs az órához tanuló rendelve, nem tudunk sorsolni.");
            }
        }

        private string szivatottTanuloSorsolasa()
        {
            if (aktualisOraTanuloi.Count > 0)
            {
                Random random = new Random();
                int szam = random.Next(0, aktualisOraTanuloi.Count);

                Console.WriteLine("  A következő tanulót választottam: {0}", aktualisOraTanuloi[szam]);
                return aktualisOraTanuloi[szam];
            }
            else
            {
                return "";
            }

        }

        private long hatralevoMasodpercekSzama()
        {
            int folyamatbanLevoOraSzama = ora_szunet();

            if (folyamatbanLevoOraSzama != -1)
            {
                DateTime vege = DateTime.Today.AddHours(tanorak[folyamatbanLevoOraSzama - 1].vege_ora).AddMinutes(tanorak[folyamatbanLevoOraSzama - 1].vege_perc);
                long vegeUnixtime = atvalt(vege);

                DateTime now = DateTime.Now;
                long nowUnixtime = atvalt(now);

                return vegeUnixtime - nowUnixtime;
            }
            else
            {
                return -1;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
        }
    }
}
