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
    internal class BibliotekarDB
    {
        DbConnection connection;

        private BibliotekarDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Bibliotekar bibliotekar)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `bibliotekar` Values (0, @Firstname, @Lastname, @Phone, @Email, @Biblioteka_ID);");

                cmd.Parameters.Add(new MySqlParameter("Firstname", bibliotekar.Firstname));
                cmd.Parameters.Add(new MySqlParameter("Lastname", bibliotekar.Lastname));
                cmd.Parameters.Add(new MySqlParameter("Phone", bibliotekar.Phone));
                cmd.Parameters.Add(new MySqlParameter("Email", bibliotekar.Email));
                cmd.Parameters.Add(new MySqlParameter("BibliotekaID", bibliotekar.Biblioteka_ID));




                try
                {
                    
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        cmd = connection.CreateCommand("select LAST_INSERT_ID();");
                        
                        int id = (int)(ulong)cmd.ExecuteScalar();
                        if (id > 0)
                        {
                            bibliotekar.ID = id;
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

        internal List<Bibliotekar> SelectAll()
        {
            List<Bibliotekar> bibliotekars = new List<Bibliotekar>();
            if (connection == null)
                return bibliotekars;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Firstname`, `Lastname`, `Phone`, `Email`, 'Biblioteka_ID'  from `Bibliotekar` ");
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
                        string email = string.Empty;
                        if (!dr.IsDBNull(4))
                            email = dr.GetString(4);
                        string biblioteka_id = dr.GetString(5);



                        bibliotekars.Add(new Bibliotekar
                        {
                            ID = id,
                            Firstname = firstname,
                            Lastname = lastname,
                            Phone = phone,
                            Email = email,
                            Biblioteka_ID = biblioteka_id,


                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return bibliotekars;
        }

        internal bool Update(Bibliotekar edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {


                var mc = connection.CreateCommand($"update `bibliotekar` set ` firstname`=@firstname, ` lastname`=@lastname, ` phone`=@phone, ` email`=@email, ` bibliotekaID`=@bibliotekaID,  `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("firstname", edit.Firstname));
                mc.Parameters.Add(new MySqlParameter("lastname", edit.Lastname));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));
                mc.Parameters.Add(new MySqlParameter("email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("Biblioteka_ID", edit.Biblioteka_ID));







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


        internal bool Remove(Bibliotekar remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                ///////

                var mc = connection.CreateCommand($"delete from `bibliotekar` where `id` = {remove.ID}");
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

        static BibliotekarDB db;
        public static BibliotekarDB GetDb()
        {
            if (db == null)
                db = new BibliotekarDB(DbConnection.GetDbConnection());
            return db;
        }

        internal IEnumerable<Bibliotekar> SelectBy(string search)
        {
            List<Bibliotekar> bibliotekars = new List<Bibliotekar>();
            if (connection == null)
                return bibliotekars;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `Firstname`, `Lastname`, `Phone`, `Email` " +
                    "``, `Bibliotekaid` from `bibliotekar` WHERE `firstname` like @search  or `lastname` like @search  or `phone` like @search or `email` like @search");
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
                        string phone = dr.GetString(3);
                        string email = string.Empty;
                        if (!dr.IsDBNull(4))
                            email = dr.GetString(4);
                        string biblioteka_ID = dr.GetString(5);




                        bibliotekars.Add(new Bibliotekar
                        {
                            ID = id,
                            Firstname = firstname,
                            Lastname = lastname,
                            Phone = phone,
                            Email = email,
                            Biblioteka_ID = biblioteka_ID,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return bibliotekars;
        }
    }
}

