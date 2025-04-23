using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursova
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Author { get; set; }
        public int Year_published { get; set; }
        public string Genre { get; set; }

        public int AuthorID { get; set; }

    }
}
