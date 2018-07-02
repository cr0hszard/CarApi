using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;



/*Car Class:
    *   Properties:
    *       -Brand
    *       -ProductionYear
    *       -Id
    *   Static variable:
    *       -Counter (used to assign Id)
    *   Static Array:
    *       -Cars[]: has GetCars() and SetCars() methods
    *   Costructors:
    *       -Car()
    *       -Car(String Brand, int ProductionYear)
    *   Methods:
    *       -PopulateArray(int length)
    *       -GetCars()
    *       -SetCars(Car[] array)
    * 
    */
namespace CarApi.Models
{
    public class Car
    {
        private static int Counter = 0;
        public static List<Car> Cars = PopulateList(15); //static Array of cars
        public static string json;

        //The Counter is used to count the instances of objects and to set a different Id for every car in the constructor

        //Car properties----------------------------------------------------------------------------------------
        public string Brand { get; set; }
        public int ProductionYear { get; set; }
        public int Id { get; set; }

        //Cars array getter and setter ---------------------------------------------------------------------------
        public static List<Car> GetCars()
        {
            return Car.Cars;
        }

        public static void SetCars(List<Car> List)
        {
            Car.Cars = List;
        }
        //Counter getter and setter ----------------------------------------------------------------------------------

        public static void UpCount()
        {
            Car.Counter++;
        }
        //Class constructors------------------------------------------------------------------------------------------
        public Car()
        {
            this.Brand = null;

            this.ProductionYear = 0;

            Car.Counter++;
            this.Id = Car.Counter;
        }

        public Car(String brand, int productionYear)
        {
            this.Brand = brand;

            this.ProductionYear = productionYear;

            Car.Counter++;
            this.Id = Car.Counter;
        }

        public Car(String brand, int productionYear, int id)
        {
            this.Brand = brand;

            this.ProductionYear = productionYear;

            this.Id = id;
        }

        //This method is used to return a randomly generated array of cars -----------------------------------------------------------
        public static List<Car> PopulateList(int length)//length is the legth of the array to create
        {
            String[] NameArray = { "Alfa", "Peugeot", "Skoda", "BMW", "Audi", "Lamborghini", "Opel", "Seat", "Citroen", "Lada" };//Array of names

            int lowestProductionYear = 1994;//lowest year of production

            //Randoms we will use to select random numbers for years and for the index of the NameArray
            Random r1 = new Random();

            //we will create a dummy array the same size of the Cars[] array and populate the array with it 
            List<Car> List = new List<Car>(length);
            for (int i = 0; i < length; i++)
            {
                List.Add(new Car(
                    NameArray[(int)(r1.NextDouble() * 10)],
                    lowestProductionYear + (int)(r1.NextDouble() * 24)
                    ));

            }
            return List;
        }
        //Method we use to save the List to a .txt file in the CarApi/Model folder
        public static void updateTxtFile()
        {
            string _path = @"C:\Users\aless\OneDrive\Documents\GitHub\CarApi\CarApi\Models\carList.txt";
            //this clears the text file 
            Car.json = "";
            
            for (int i = 0; i < Car.Cars.Count; i++)
            {   //if its the first Car in the list open square brackets, go to new line  wtrite the first Car
                if (i == 0)
                {
                    Car.json += "[" + Environment.NewLine + JsonConvert.SerializeObject(Car.Cars[i]) + "," + Environment.NewLine;
                }
                //if its the first Car in the list close square brackets
                if (i == Car.Cars.Count-1)
                {
                    Car.json += JsonConvert.SerializeObject(Car.Cars[i]) + Environment.NewLine + "]";
                }
                //else just write the next Car add a coma(,) and go to new line 
                else
                {
                    Car.json += JsonConvert.SerializeObject(Car.Cars[i]) + "," + Environment.NewLine;
                }
            }
            //write Car.json to a . txt File in _path 
            File.WriteAllText(_path, Car.json);
        }
    }
}

