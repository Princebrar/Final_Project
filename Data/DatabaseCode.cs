    //Group Number - 12
    //Group members name - Princejot Singh, Preetinder Singh, Jashanpreet Singh Sran and Rudra Kainth
    //Submitted to - Prof. Harjinder Kaur
    //Project Title - Employee Registration App
    //Date of Submittion - 10th of December, 2023

using System;
using System.Collections.Generic;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace Final_Project.Data
{
	public class DatabaseCode
	{
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DatabaseCode()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "ConnectCsharpToMysql";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert(string f_name,string l_name,DateOnly? date,string Phone,string Gendr,string job,string address)
        {
            string query = $"INSERT INTO Project (First_Name, Last_Name, Date_of_Birth, Phone_Number, Gender, Job_Title, Address) VALUES('{f_name}', '{l_name}', '{date}', '{Phone}', '{Gendr}', '{job}', '{address}')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(int id, string f_name, string l_name, DateOnly? date, string Phone, string address)
        {
            try
            {
                string query = $"UPDATE Project SET First_Name='{f_name}', Last_Name='{l_name}', Date_of_Birth='{date}', Phone_Number='{Phone}', Address='{address}' WHERE EmplyeeID='{id}'";

                //Open connection
                if (this.OpenConnection() == true)
                {
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error in Database Code in Update is: "+ex);
            }
        }

        //Delete statement
        public void Delete(int id)
        {
            string query = $"DELETE FROM Project WHERE EmplyeeID='{id}'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<Employee> Select()
        {
            try
            {
                string query = "SELECT * FROM Project";
                //Create a list to store the result
                List<Employee> list = new List<Employee>();

                //Open connection
                if (this.OpenConnection() == true)
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        Employee data = new Employee
                        {
                            employeeID = Convert.ToInt32(dataReader["EmplyeeID"]),
                            first_name = Convert.ToString(dataReader["First_Name"]),
                            last_name = Convert.ToString(dataReader["Last_Name"]),
                            dob = DateOnly.FromDateTime(Convert.ToDateTime(dataReader["Date_of_Birth"])),
                            phone = Convert.ToString(dataReader["Phone_Number"]),
                            gender = Convert.ToString(dataReader["Gender"]),
                            job = Convert.ToString(dataReader["Job_Title"]),
                            address = Convert.ToString(dataReader["Address"]),
                        };

                        list.Add(data);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    this.CloseConnection();

                    //return list to be displayed
                    return list;
                }
                else
                {
                    return list;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Problem in Database code is: "+ ex );
                List < Employee > list = new List<Employee>();
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            string query = "SELECT Count(*) FROM Project";
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\MySqlBackup" + year + "-" + month + "-" + day +
            "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error , unable to backup!");
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}",
                    uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error , unable to Restore!");
            }
        }
    }
}

