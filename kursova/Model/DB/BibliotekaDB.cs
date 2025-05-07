using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace kursova.Model
{
    internal class BibliotekaDB
    {
        DbConnection connection;

        private BibliotekaDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Biblioteka biblioteka)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `reader` Values (0, @Address, @Phone, @Email , @BibliotekaID);");

                cmd.Parameters.Add(new MySqlParameter("Address", biblioteka.Address));              
                cmd.Parameters.Add(new MySqlParameter("Phone", biblioteka.Phone));
                cmd.Parameters.Add(new MySqlParameter("Email", biblioteka.Email));
                cmd.Parameters.Add(new MySqlParameter("BibliotekaID", biblioteka.BibliotekaID));



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
                            biblioteka.ID = id;
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

        internal List<Biblioteka> SelectAll()
        {
            List<Biblioteka> bibliotekas = new List<Biblioteka>();
            if (connection == null)
                return bibliotekas;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Address`, `Phone`, `Email`, 'BibliotekaID'  from `Biblioteka` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string address = string.Empty;
                        if (!dr.IsDBNull(1))
                            address = dr.GetString(1);
                        string phone = dr.GetString(2);
                        string bibliotekaid = dr.GetString(3);
                        string email = string.Empty;
                        if (!dr.IsDBNull(4))
                            email = dr.GetString(4);




                        bibliotekas.Add(new Biblioteka
                        {
                            ID = id,
                            Address = address,
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
            return bibliotekas;
        }

        internal bool Update(Biblioteka edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {


                var mc = connection.CreateCommand($"update `biblioteka` set ` address`=@address, ` phone`=@phone, ` email`=@email, ` bibliotekaID`=@bibliotekaID,  `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("firstname", edit.Address));
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


        internal bool Remove(Biblioteka remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                ///////

                var mc = connection.CreateCommand($"delete from `biblioteka` where `id` = {remove.ID}");
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

        static BibliotekaDB db;
        public static BibliotekaDB GetDb()
        {
            if (db == null)
                db = new BibliotekaDB (DbConnection.GetDbConnection());
            return db;
        }

        internal IEnumerable<Biblioteka> SelectBy(string search)
        {
            List<Biblioteka> bibliotekas = new List<Biblioteka>();
            if (connection == null)
                return bibliotekas;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `Address`, `Phone`, `Email` " +
                    "``, `Bibliotekaid` from `biblioteka` WHERE `address` like @search  or `phone` like @search or `email` like @search");
                try
                {
                    command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string adress = string.Empty;
                        if (!dr.IsDBNull(1))
                            adress = dr.GetString(1);
                       string phone = dr.GetString(2);
                        string email = dr.GetString(3);
                        string genre = string.Empty;
                        if (!dr.IsDBNull(4))
                            genre = dr.GetString(4);
                        string bibliotekaid = dr.GetString(5);




                        bibliotekas.Add(new Biblioteka
                        {
                            ID = id,
                            Address = adress,
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
            return bibliotekas;
        }
    }
}

