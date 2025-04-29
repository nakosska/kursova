using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;
using System.Reflection.PortableExecutable;

namespace kursova.Model
{
    internal class ReaderDB
    {
        DbConnection connection;

        private ReaderDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Reader reader)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `reader` Values (0, @Firstname, @Lastname, @Phone, @Email , @BibliotekaID);");

                cmd.Parameters.Add(new MySqlParameter("Firstname", reader.Firstname));
                cmd.Parameters.Add(new MySqlParameter("Lastname", reader.Lastname));
                cmd.Parameters.Add(new MySqlParameter("Phone", reader.Phone));
                cmd.Parameters.Add(new MySqlParameter("Email", reader.Email));
                cmd.Parameters.Add(new MySqlParameter("BibliotekaID", reader.BibliotekaID));



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
                            reader.ID = id;
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

        internal List<Reader> SelectAll()
        {
            List<Reader> reader = new List<Reader>();
            if (connection == null)
                return reader;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Firstname`, `Lastname`, `Phone`, `Email`, 'BibliotekaID'  from `Reader` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string firstname = string.Empty;
                        if (!dr.IsDBNull(1))
                            firstname = dr.GetString(1);
                        string lastname = dr.GetString(2);
                        string phone = dr.GetString(3);
                        string bibliotekaid = dr.GetString(4);
                        string email = string.Empty;
                        if (!dr.IsDBNull(4))
                            email = dr.GetString(5);




                        reader.Add(new Reader
                        {
                            ID = id,
                            Firstname = firstname,
                            Lastname = lastname,
                            Phone = phone,
                            Email = email,
                            BibliotekaID = bibliotekaid,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return reader;
        }

        internal bool Update(Reader edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
               

                var mc = connection.CreateCommand($"update `reader` set ` firstname`=@firstname, ` lastname`=@lastname, ` phone`=@phone, ` email`=@email, ` bibliotekaID`=@bibliotekaID,  `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("firstname", edit.Firstname));
                mc.Parameters.Add(new MySqlParameter("lastname", edit.Lastname));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));
                mc.Parameters.Add(new MySqlParameter("email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("BibliotekaID", edit.BibliotekaID));






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


        internal bool Remove(Reader remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                ///////

                var mc = connection.CreateCommand($"delete from `reader` where `id` = {remove.ID}");
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

        static ReaderDB db;
        public static ReaderDB GetDb()
        {
            if (db == null)
                db = new ReaderDB(DbConnection.GetDbConnection());
            return db;
        }

        //internal IEnumerable<Reader> SelectBy(string search)
        //{
        //    List<Reader> reader = new List<Reader>();
        //    if (connection == null)
        //        return reader;

        //    if (connection.OpenConnection())
        //    {
        //        var command = connection.CreateCommand("select `id`, `Firstname`, `Lastname`, `Phone`, `Email` " +
        //            "``, `Bibliotekaid` from `reader` WHERE `firstname` like @search  or `lastname` like @search  or `phone` like @search or `email` like @search");
        //        try
        //        {
        //            command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
        //            MySqlDataReader dr = command.ExecuteReader();
        //            while (dr.Read())
        //            {
        //                int id = dr.GetInt32(0);
        //                string title = string.Empty;
        //                if (!dr.IsDBNull(1))
        //                    title = dr.GetString(1);
        //                int author = dr.GetInt32(2);
        //                int year_published = dr.GetInt32(3);
        //                string genre = string.Empty;
        //                if (!dr.IsDBNull(4))
        //                    genre = dr.GetString(4);
        //                int authorid = dr.GetInt32(5);




        //                reader.Add(new Reader
        //                {
        //                    ID = id,
        //                    Firstname = firstname,
        //                    Lastname = lastname,
        //                    Phone = phone,
        //                    Email = email,
        //                    BibliotekaID = bibliotekaid,

        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //    connection.CloseConnection();
        //    return reader;
        //}
    }
}

