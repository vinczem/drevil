using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMClock
{
    internal class Idopontok
    {
        public int kezd_ora { get; set; }
        public int kezd_perc { get; set; }

        public int vege_ora { get; set; }
        public int vege_perc { get; set; }

        public Idopontok(string sor)
        {
            string[] tomb = sor.Split(';');

            string[] ktmb = tomb[0].Split(':');
            kezd_ora = int.Parse(ktmb[0]);
            kezd_perc = int.Parse(ktmb[1]);

            string[] vtmb = tomb[1].Split(':');
            vege_ora = int.Parse(vtmb[0]);
            vege_perc = int.Parse(vtmb[1]);
        }
    }
}
