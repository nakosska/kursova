using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;
using System.Xml.Linq;

namespace kursova.Model
{
    internal class AuthorDB
    {
        DbConnection connection;

        private AuthorDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Author author)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `author` Values (0, @Firstname, @Lastname, @Birthday;");

               
                cmd.Parameters.Add(new MySqlParameter("Firstname", author.Firstname));
                cmd.Parameters.Add(new MySqlParameter("Lastname", author.Lastname));
                cmd.Parameters.Add(new MySqlParameter("Birthday", author.Birthday));
                


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
                            author.ID = id;
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

        internal List<Author> SelectAll()
        {
            List<Author> authors = new List<Author>();
            if (connection == null)
                return authors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `First name`, `Last name`, `Birthday` from `author` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string firstname = string.Empty;
                        if (!dr.IsDBNull(1))
                            firstname = dr.GetString(1); 
                        string lastname = string.Empty;
                        if (!dr.IsDBNull(2))
                            lastname = dr.GetString(2);
                        DateOnly birthdate = new DateOnly();
                        birthdate = dr.GetDateOnly(3);






                        authors.Add(new Author
                        {
                            ID = id,
                            Firstname = firstname,
                            Lastname = lastname,
                           Birthday = birthdate
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return authors;
        }

        internal bool Update(Author edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `author` set ` firstname`=@firstname, ` lastname`=@lastname, ` birthday`=@birthday, `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("firstname", edit.Firstname));
                mc.Parameters.Add(new MySqlParameter("lastname", edit.Lastname));
                mc.Parameters.Add(new MySqlParameter("birthday", edit.Birthday));
               

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


        internal bool Remove(Author remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                ///////

                var mc = connection.CreateCommand($"delete from `author` where `id` = {remove.ID}");
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

        static AuthorDB db;
        public static AuthorDB GetDb()
        {
            if (db == null)
                db = new AuthorDB(DbConnection.GetDbConnection());
            return db;
        }

        internal List<Author> SelectBy(string search)
        {
            List<Author> authors = new List<Author>();
            if (connection == null)
                return authors;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `firstname`, `lastname`, 'birthday' from `author` WHERE `firstname` like @search  or `lastname` like @search  or 'birthday' like @search");
                try
                {
                    command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string firstname = string.Empty;
                        if (!dr.IsDBNull(1))
                            firstname = dr.GetString(1);
                        string lastname = dr.GetString(2);   
                        string birthday = dr.GetString(3);
                        int authorid = dr.GetInt32(4);




                        authors.Add(new Author
                        {
                            ID = id,
                            Firstname = firstname,
                            Lastname = lastname,
                            Birthday = birthday

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return authors;
        }
    }
}

