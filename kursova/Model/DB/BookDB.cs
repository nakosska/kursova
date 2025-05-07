using System.Data.Common;
using System.Text;
using System.Windows;
using MySqlConnector;
using static System.Reflection.Metadata.BlobBuilder;

namespace kursova.Model
{
    internal class BookDB
    {
        DbConnection connection;

        private BookDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Book book)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `book` Values (0, @Title, @Author, @Year_published, @Genre , @AuthorID);");

                cmd.Parameters.Add(new MySqlParameter("title", book.Title));
                cmd.Parameters.Add(new MySqlParameter("author_id", book.Author));
                cmd.Parameters.Add(new MySqlParameter("year_published", book.Year_published));
                cmd.Parameters.Add(new MySqlParameter("genre", book.Genre));
                cmd.Parameters.Add(new MySqlParameter("AuthorID", book.AuthorID));



                try
                {
                    // вставка заиписи
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd = connection.CreateCommand("select LAST_INSERT_ID();");
                        // получение id последней вставленнойц записи
                        int id = (int)(ulong)cmd.ExecuteScalar();
                        if (id > 0)
                        {
                            book.ID = id;
                            result = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Book> SelectAll()
        {
            List<Book> book = new List<Book>();
            if (connection == null)
                return book;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Title`, `Author`, `Year_published`, `Genre`, 'AuthorID'  from `book` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString(1);
                        int author = dr.GetInt32(2); 
                        int year_published = dr.GetInt32(3);
                        int authorid = dr.GetInt32(4);
                        string genre = string.Empty;
                        if (!dr.IsDBNull(4))
                            genre = dr.GetString(5);
                       
                        
                       

                        book.Add(new Book
                        {
                            ID = id,
                            Title = title,
                            Author = author,
                            Year_published = year_published,
                            Genre = genre,
                            AuthorID = authorid,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return book;
        }

        internal bool Update(Book edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `book` set ` title`=@title, ` author`=@author, ` year_published`=@year_published, ` genre`=@genre, 'AuthorID'=@authorid where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("Author", edit.Author));
                mc.Parameters.Add(new MySqlParameter("Year_published", edit.Year_published));
                mc.Parameters.Add(new MySqlParameter("Genre", edit.Genre));
                mc.Parameters.Add(new MySqlParameter("AuthorID", edit.AuthorID));


                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Book remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                ///////

                var mc = connection.CreateCommand($"delete from `book` where `id` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("точно точно удалить??");
                }
            }
            connection.CloseConnection();
            return result;
        }

        static BookDB db;
        public static BookDB GetDb()
        {
            if (db == null)
                db = new BookDB(DbConnection.GetDbConnection());
            return db;
        }

        internal IEnumerable<Book> SelectBy(string search)
        {
            List<Book> book = new List<Book>();
            if (connection == null)
                return book;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `title`, `author`, `year_published`, " +
                    "`genre`, `authorid` from `book` WHERE `title` like @search  or `genre` like @search  or `year_published` like @search");
                try
                {
                    command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string title = string.Empty;
                        if (!dr.IsDBNull(1))
                            title = dr.GetString(1);
                        int author = dr.GetInt32(2);
                        int year_published = dr.GetInt32(3);
                        string genre = string.Empty;
                        if (!dr.IsDBNull(4))
                            genre = dr.GetString(4);
                        int authorid = dr.GetInt32(5);


                       

                        book.Add(new Book
                        {
                            ID = id,
                            Title = title,
                            Author = author,
                            Year_published = year_published,
                            Genre = genre,
                            AuthorID = authorid,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return book;
        }
    }
}