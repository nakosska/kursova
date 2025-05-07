using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursova
{
    public class Bibliotekar
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Biblioteka_ID { get; set; }
        public string FIO { get => $"{Lastname} {Firstname}"; }
    }
}
