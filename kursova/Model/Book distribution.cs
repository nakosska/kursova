using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursova
{
    public class BookDistribution
    {
        public int Reader_ID { get; set; }
        public int Book_ID { get; set; }
        public int BibliotekarID { get; set; }

        public DateTime Date_of_issue { get; set; }

        public DateTime Return_date { get; set; }
        public int ID { get; internal set; }
    }
}
