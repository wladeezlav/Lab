using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.Common;


namespace Zalik_Mamontov
{
    class DBworker
    {
        public static string conn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Учёба\C#\Zalik\Zalik_Mamontov\Zalik_Mamontov\Zalik.mdf;Integrated Security=True;Connect Timeout=30";

        public SqlConnection connection = new SqlConnection(DBworker.conn);
       
        DataSet ds = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();

        public string[] Tables { get; private set; } = new string[] { "Actor", "Film", "Acting" };


        public bool Add(int tableNum, string row)
        {
            
                connection.Open();
                
                adapter.SelectCommand = new SqlCommand(GetSQL(tableNum), connection);
                adapter.Fill(ds);

                DataRow newRow;

                switch (tableNum)
                {
                    case 0:
                        {
                            newRow = AddActor(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            newRow = AddFilm(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    default:
                        {
                            connection.Close();
                            return false;

                        }
                }


                ds.Tables[0].Rows.Add(newRow);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                connection.Close();
            
            return true;
        }

        private DataRow AddActor(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');
            string Full_name;
            DateTime DOB;


            if (rowData.Length < 2)
            {
                return null;
            }

            Full_name = rowData[0].ToString();
            if(!DateTime.TryParse(rowData[1], out DOB))
            {
                Console.WriteLine("Incorrect date.");
                return null;
            }

            newRow["Full_name"] = Full_name;
            newRow["DOB"] = DOB;
            adapter.Update(ds);

            return newRow;

        }

        private DataRow AddFilm(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');
            string title;
            DateTime date;
            string country;

            if (rowData.Length < 3)
            {
                return null;
            }

            title = rowData[0].ToString();
            if (!DateTime.TryParse(rowData[1], out date))
            {
                Console.WriteLine("Incorrect date.");
                return null;
            }
            country = rowData[2].ToString();          

            newRow["Title"] = title;
            newRow["Date"] = date;
            newRow["Country"] = country;
            
            return newRow;
        }

        public void Show(int tableNum)
        {
            const int size = -30;

            //using (SqlConnection connection = conn())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(GetSQL(tableNum), connection);
                SqlDataReader reader = command.ExecuteReader();


                Console.Write("No.\t");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader.GetName(i),size}");
                }
                Console.WriteLine();

                if (reader.HasRows)
                {

                    int index = 0;
                    while (reader.Read())
                    {
                        index++;
                        Console.Write(index + "\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetValue(i),size}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Table is empty");
                }
                connection.Close();
            }
        }

        public bool Delete(int tableNum, int rowIndex)
        {
            //using (SqlConnection connection = conn())
            {
                connection.Open();

                adapter.SelectCommand = new SqlCommand(GetSQL(tableNum), connection);
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count <= rowIndex)
                {
                    connection.Close();

                    if (dt.Rows.Count == 0)
                    {
                        return true;
                    }

                    return false;
                }
                
                dt.Rows[rowIndex].Delete();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                connection.Close();
            }


            return true;
        }

        public bool Edit(int tableNum, int rowIndex, string row)
        {
            //using (SqlConnection connection = conn)
            {
                connection.Open();

                adapter.SelectCommand = new SqlCommand(GetSQL(tableNum), connection);
                adapter.Fill(ds);

                if(rowIndex >= ds.Tables[0].Rows.Count)
                {
                    connection.Close();

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }
                
                DataRow editRow = ds.Tables[0].Rows[rowIndex];

                switch (tableNum)
                {
                    case 0:
                        {
                            
                            if (AddActor(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (AddFilm(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    default:
                        {
                            connection.Close();
                            return false;

                        }
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
                connection.Close();
            }
            return true;
        }

        private string GetSQL(int tableNum)
        {
            if (tableNum < 0 || tableNum >= Tables.Length)
            {
                throw new Exception("Number of table is not correct");
            }
            return "Select * From " + Tables[tableNum];
        }

        private string GetQuery1()
        {
            return "Select * From Film Where Date >= '01.01.2017'";
        }

        private string GetQuery2()
        {
            string film;
            Console.WriteLine("Введите название фильма из таблицы фильмов");
            film = Console.ReadLine();
            return "Select * From Actor Where id = (SELECT actor_id FROM Acting WHERE film_id = (SELECT id FROM Film WHERE Title = '" + film + "'))";
        }

        private string GetQuery3()
        {
            Console.WriteLine("Введите минимальное количество фильмов, в которых сыграл актер/актриса");
            int number = Convert.ToInt32(Console.ReadLine());
            return "SELECT Acting.actor_id AS Id, Actor.Full_name, Actor.DOB, COUNT(*) AS Roles FROM dbo.Acting JOIN Actor ON(Acting.actor_id = Actor.Id) GROUP BY Full_name, Actor.DOB, actor_id HAVING COUNT(*) >= " + number;
        }
        private string GetQuery4_1(int number)
        {
            return "DELETE FROM Acting WHERE film_id IN (SELECT id FROM Film WHERE Date < (DATEADD(YEAR, " + number + ", GETDATE())))";
        }
        private string GetQuery4_2(int number)
        {
            return "DELETE FROM Film WHERE Date<(DATEADD(YEAR, " + number + ", GETDATE()));";
        }
        /*  */

        public void Queries()
        {
            connection.Open();

            const int size = -30;

            Console.WriteLine("1. Найти все фильмы, вышедшие на экран в прошлом и текущем году.");
            Console.WriteLine("2. Вывести информацию об актерах, снимавшихся в заданном фильме.");
            Console.WriteLine("3. Вывести информацию об актерах, снимавшихся как минимум в N фильмах.");
            Console.WriteLine("4. Удалить все фильмы, дата выхода которых была более заданного числа лет назад.");
            int key = Convert.ToInt32(Console.ReadLine()) - 1;

            switch (key)
            {
                case 0:
                    {
                        SqlCommand command = new SqlCommand(GetQuery1(), connection);
                        SqlDataReader reader = command.ExecuteReader();

                        Console.Write("No.\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i),size}");
                        }
                        Console.WriteLine();

                        if (reader.HasRows)
                        {

                            int index = 0;
                            while (reader.Read())
                            {
                                index++;
                                Console.Write(index + "\t");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write($"{reader.GetValue(i),size}");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Query result is empty");
                        }
                        connection.Close();
                        break;
                    }
                case 1:
                    {
                        SqlCommand command = new SqlCommand(GetQuery2(), connection);
                        SqlDataReader reader = command.ExecuteReader();

                        Console.Write("No.\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i),size}");
                        }
                        Console.WriteLine();

                        if (reader.HasRows)
                        {

                            int index = 0;
                            while (reader.Read())
                            {
                                index++;
                                Console.Write(index + "\t");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write($"{reader.GetValue(i),size}");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Query result is empty");
                        }
                        connection.Close();
                        break;
                    }
                case 2:
                    {
                        SqlCommand command = new SqlCommand(GetQuery3(), connection);
                        SqlDataReader reader = command.ExecuteReader();

                        Console.Write("No.\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader.GetName(i),size}");
                        }
                        Console.WriteLine();

                        if (reader.HasRows)
                        {

                            int index = 0;
                            while (reader.Read())
                            {
                                index++;
                                Console.Write(index + "\t");
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write($"{reader.GetValue(i),size}");
                                }
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Query result is empty");
                        }
                        Console.ReadLine();
                        connection.Close();
                        break;
                    }
                case 3:
                    {
                            Console.WriteLine("Введите количество лет, фильмы старше которого нужно удалить");
                            int number = -Convert.ToInt32(Console.ReadLine());

                            SqlDataAdapter adapter = new SqlDataAdapter(GetQuery4_1(number), connection);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
                            adapter = new SqlDataAdapter(GetQuery4_2(number), connection);
                            ds = new DataSet();
                            adapter.Fill(ds);
                            connection.Close();

                            Console.WriteLine("Completed");
                            break;
                        
                    }
                default:
                    {
                        connection.Close();
                        return;

                    }
            }
        }
    }
}
