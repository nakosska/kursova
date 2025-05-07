using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.Windows;

namespace kursova.Model
{
    internal class BookDistributionDB
    {
        DbConnection connection;

        private BookDistributionDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(BookDistribution bookDistribution)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `reader` Values (0, @Date_of_issue, @Return_date, @Reader_ID, @Book_ID , @BibliotekarID);");

                cmd.Parameters.Add(new MySqlParameter("Date of issue", bookDistribution.Date_of_issue));
                cmd.Parameters.Add(new MySqlParameter("Return_date", bookDistribution.Return_date));
                cmd.Parameters.Add(new MySqlParameter("Reader_ID", bookDistribution.Reader_ID));
                cmd.Parameters.Add(new MySqlParameter("Book_ID", bookDistribution.Book_ID));
                cmd.Parameters.Add(new MySqlParameter("BibliotekarID", bookDistribution.BibliotekarID));



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
                            bookDistribution.ID = id;
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

        internal List<BookDistributionDB> SelectAll()
        {
            List<BookDistributionDB> bookDistributions = new List<BookDistributionDB>();
            if (connection == null)
                return bookDistributions;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Date_of_issue`, `Return_date`, `Reader_ID`, `Book_ID`, 'BibliotekaID', 'BibliotekarID'  from `BookDistributionDB` ");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string date_of_issue = string.Empty;
                        if (!dr.IsDBNull(1))
                            date_of_issue = dr.GetString(1);
                        string return_date = dr.GetString(2);
                        int reader_ID = dr.GetInt32(3);
                        int book_ID = dr.GetInt32(4);
                        int bibliotekarID = dr.GetInt32(5);
                        
                        




                        bookDistributions.Add(new BookDistribution
                        {
                            ID = id,
                            Date_of_issue = date_of_issue,
                            Return_date = return_date,
                            Reader_ID = reader_ID,
                            Book_ID = book_ID,
                            BibliotekarID = bibliotekarID,

                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return bookDistributions;
        }

        internal bool Update(BookDistribution edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {


                var mc = connection.CreateCommand($"update `bookDistributions` set ` date_of_issue`=@date_of_issue, ` return_date`=@return_date, ` reader_ID`=@reader_ID, ` book_ID`=@book_ID, ` bibliotekarID`=@bibliotekarID,  `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("Date_of_issue", edit.Date_of_issue));
                mc.Parameters.Add(new MySqlParameter("Return_date", edit.Return_date));
                mc.Parameters.Add(new MySqlParameter("Reader_ID", edit.Reader_ID));
                mc.Parameters.Add(new MySqlParameter("Book_ID", edit.Book_ID));
                mc.Parameters.Add(new MySqlParameter("BibliotekarID", edit.BibliotekarID));






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


        internal bool Remove(BookDistributionDB remove)
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

        static BookDistributionDB db;

        public object ID { get; private set; }

        public static BookDistributionDB GetDb()
        {
            if (db == null)
                db = new BookDistributionDB(DbConnection.GetDbConnection());
            return db;
        }

        internal IEnumerable<BookDistribution> SelectBy(string search)
        {
            List<BookDistribution> bookDistributions = new List<BookDistribution>();
            if (connection == null)
                return bookDistributions;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, ` Date_of_issue `, `Return_date`, `Reader_ID`, `Book_ID` " +
                    "``, `Bibliotekarid` from `bookDistribution` WHERE `date_of_issue` like @search  or `return_date` like @search  or `reader_ID` like @search or `book_ID` like @search");
                try
                {
                    command.Parameters.Add(new MySqlParameter("search", "%" + search + "%"));
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string date_of_issue = string.Empty;
                        if (!dr.IsDBNull(1))
                            date_of_issue = dr.GetString(1);
                        DateTime return_date = dr.GetDateTime(2);
                        int reader_ID = dr.GetInt32(3);
                        int book_ID = dr.GetStr(4);
                        int bibliotekarid = dr.GetInt32(5);




                        bookDistributions.Add(new BookDistribution
                        {
                            ID = id,
                            Date_of_issue = date_of_issue,
                            Return_date = return_date,
                            Reader_ID = reader_ID,
                            Book_ID = book_ID
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
    }
}

