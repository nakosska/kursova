using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using kursova.Model;

namespace kursova.ViewModel
{
    internal class AddAuthorVM : BaseVM
    {
        internal class WinAddAuthor : BaseVM
        {
            private Author newAuthor = new();

            public Author NewAuthor
            {
                get => newAuthor;
                set
                {
                    newAuthor = value;
                    Signal();
                }
            }

            public Command InsertAuthor { get; set; }
            public WinAddAuthor()
            {
                InsertAuthor = new Command(() =>
                {
                    AuthorDB.GetDb().Insert(NewAuthor);
                    close?.Invoke();
                },
                    () =>
                    !string.IsNullOrEmpty(newAuthor.LastName) &&
                    !string.IsNullOrEmpty(newAuthor.FirstName));
            }
            Action close;
            internal void SetClose(Action close)
            {
                this.close = close;
            }
        }
    }
}
