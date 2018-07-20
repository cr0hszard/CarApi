using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;

namespace CarApi.Models
{/*Database Class:
        Properties:
            -CarDBConnection: SQLiteConnection [SQLite connection used to connect to the database]
            -path : String [String containing the absolute path to the CarDB.sqlite3 database file]
    
        Constructors:
            -Database():creates a database file in the /Models folder if it doesnt exist, then it opens
                         a connection with the database and creates a table if it doesnt already exist, then closes the connection
        Methods:
            -Add(Car) : Adds the values of Car to the database
            -Size() : Returns the size (in rows) of the database
            -isPopulated() : Returns true if the Database is not empty, else returns false
            -Load() : Returns a List<Car> by taking the values out of the database 
     
     
 */
    public class Database
    {
        public SQLiteConnection CarDBConnection;

        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models\\CarDB.sqlite3");  //Find absolute path on the PC for the database file CarDB.sqlite3

       
        public Database()
        {   //if the file for the database doesnt exist, create it 
            if (!File.Exists(this.path))
            {
                SQLiteConnection.CreateFile(this.path);
            }

            using (CarDBConnection = new SQLiteConnection("Data Source=" + this.path))
            {
                CarDBConnection.Open();
              
                    //create the query String and the command to create the CarDB.sqlite3 database where we will store the Car object properties
                    String CreateTableQuery = "CREATE table IF NOT EXISTS  `Cars` (`Id`	INTEGER PRIMARY KEY AUTOINCREMENT,`Brand`	TEXT,`ProductionYear` INTEGER);";

                    SQLiteCommand Create = new SQLiteCommand(CreateTableQuery, CarDBConnection);
                    try
                    {
                        Create.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                CarDBConnection.Close();
            }
        }

        //Add(Car) method: opens a connection with the database , creates the SQL query we use to add 
        //Car object properties to  the database and executes it while catching exceptions , closes the connection
        public void Add(Car car)
        {
            using (CarDBConnection = new SQLiteConnection("Data Source=" + this.path))
            {
                CarDBConnection.Open();

                    string insertCarQuery = "INSERT INTO Cars (Brand,ProductionYear) VALUES (@Brand,@ProductionYear)";
                    SQLiteCommand InsertCarCommand = new SQLiteCommand(insertCarQuery, CarDBConnection);
                    InsertCarCommand.Parameters.AddWithValue("Brand", car.Brand);
                    InsertCarCommand.Parameters.AddWithValue("ProductionYear", car.ProductionYear);
                    try
                    {
                        InsertCarCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                CarDBConnection.Close();
            }
        }

        //Size() method: returns the size of the database
        public int Size()
        {
            int result;
            using (CarDBConnection = new SQLiteConnection("Data Source=" + this.path))
            {
                CarDBConnection.Open();

                    SQLiteCommand DatabaseSize = new SQLiteCommand("SELECT COUNT(*) from Cars", CarDBConnection);

                    try
                    {
                        result = int.Parse(DatabaseSize.ExecuteScalar().ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                CarDBConnection.Close();
            }
            return result;
        }
       
        //isPopulated() method: if the Database is populated(not empty) return true else false
        public Boolean isPopulated()
        {
            Boolean result = Size() > 0;
            return result;
        }

        //Load() method:reads the data from the database, creates a list of cars with the data and returns it 
        public List<Car> Load()
        {
            List<Car> List = new List<Car>();

            using (CarDBConnection = new SQLiteConnection("Data Source=" + this.path))
            {
                CarDBConnection.Open();

                    string stm = "SELECT * FROM Cars ";

                    using (SQLiteCommand cmd = new SQLiteCommand(stm, CarDBConnection))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                List.Add(new Car(rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(0)));//in the database(0) is the Id ,(1) is the Brand,(2) is the ProductionYear
                            }
                        }
                    }

                CarDBConnection.Close();
            }
            return List;
        }
    }
}