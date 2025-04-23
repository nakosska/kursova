using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace kursova
{
    public class Author
    {
        public int ID { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateOnly Birthday { get; set; }
        public string FIO { get => $"{Lastname} {Firstname}"; }

        public List<Book> Book { get; set; } = new();

    }
}
