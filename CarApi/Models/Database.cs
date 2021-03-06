﻿using System;
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
                String CreateTableQuery = //Creating Manufacturer table
                                          "CREATE TABLE IF NOT EXISTS  `manufacturer` ("
                                                    + "`manufacturerId`	INTEGER PRIMARY KEY AUTOINCREMENT,"
                                                    + "`name`	TEXT);"
                                          //Creating Car table
                                          + "CREATE TABLE IF NOT EXISTS  `car` ("
                                                    + "`id`	INTEGER PRIMARY KEY AUTOINCREMENT,"
                                                    + "`brand`	TEXT,"
                                                    + "`productionYear` INTEGER,"
                                                    + "`carManufacturer`	INTEGER,"
                                                    + " FOREIGN KEY(carManufacturer) REFERENCES manufacturer(manufacturerId));"
                                          //Creating car_option table
                                          + "CREATE TABLE IF NOT EXISTS  `car_option` ("
                                                    + "`optionId`	INTEGER PRIMARY KEY AUTOINCREMENT,"
                                                    + "`optionName` TEXT);"
                                          //Creating car_options table
                                          + "CREATE TABLE IF NOT EXISTS  `car_options` ("
                                                    + "`carId`	INTEGER ,"
                                                    + "`carOptionId` INTEGER,"
                                                    + " FOREIGN KEY(carId) REFERENCES car(Id),"
                                                    + " FOREIGN KEY(carOptionId) REFERENCES car_option(optionId));"
                                          //Creating country table
                                          + "CREATE TABLE IF NOT EXISTS  `country` ("
                                                    + "`countryIsoId`	INTEGER PRIMARY KEY,"
                                                    + "`name`	TEXT);"
                                          //Creating car_options table
                                          + "CREATE TABLE IF NOT EXISTS  `user` ("
                                                    + "`id`  INTEGER PRIMARY KEY AUTOINCREMENT,"
                                                    + "`name`	TEXT,"
                                                    + "`dateOfBirth` INTEGER,"
                                                    +"`countryIsoId` INTEGER,"
                                                    + "FOREIGN KEY(countryIsoId) REFERENCES country(countryIsoId));"
                                          //Creating car_options table
                                          + "CREATE TABLE IF NOT EXISTS  `car_user` ("
                                                    + "`carId`	INTEGER,"//should put Primary key if i want the cars to not be owned by multiple people
                                                    + "`carUserId` INTEGER,"
                                                    + "FOREIGN KEY(carUserId) REFERENCES user(id),"
                                                    + "FOREIGN KEY(carId) REFERENCES car(id));";
                
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

                    string insertCarQuery = "INSERT INTO car (brand,productionYear) VALUES (@Brand,@ProductionYear)";
                    SQLiteCommand InsertCarCommand = new SQLiteCommand(insertCarQuery, CarDBConnection);
                    InsertCarCommand.Parameters.AddWithValue("brand", car.Brand);
                    InsertCarCommand.Parameters.AddWithValue("productionYear", car.ProductionYear);
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

                    SQLiteCommand DatabaseSizeQuery = new SQLiteCommand("SELECT COUNT(*) from car", CarDBConnection);

                    try
                    {
                        result = int.Parse(DatabaseSizeQuery.ExecuteScalar().ToString());
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

                    string LoadQuery = "SELECT * FROM car ";

                    using (SQLiteCommand cmd = new SQLiteCommand(LoadQuery, CarDBConnection))
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