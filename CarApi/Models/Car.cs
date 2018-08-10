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
    *       -Cars[]: has GetCars() and SetCars(Car[] array) methods
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
        private static int Counter = 0;     //The Counter is used to count the instances of objects and to set a different Id for every car in the constructor
        public static Database CarDB = new Database();
        public static List<Car> CarsList = PopulateList(30); //static Array of cars



        //Car properties----------------------------------------------------------------------------------------
        public string Brand { get; set; }
        public int ProductionYear { get; set; }
        public int Id { get; set; }
        //Cars array getter and setter ---------------------------------------------------------------------------
        public static List<Car> GetCars()
        {
            return CarsList;
        }

        public static void SetCars(List<Car> List)
        {
            CarsList = List;
        }
        // Adds a car to the array
        public static void AddCar(Car car)
        {
            CarsList.Add(new Car(car.Brand, car.ProductionYear, CarsList.Count + 1));
        }


        //Counter getter and setter ----------------------------------------------------------------------------------
        public static void UpCount()
        {
            Counter++;
        }


        //Class constructors------------------------------------------------------------------------------------------
        public Car()
        {
            this.Brand = null;
            this.ProductionYear = 0;
            Counter++;
            this.Id = Counter;
        }

        public Car(String brand, int productionYear)
        {
            this.Brand = brand;
            this.ProductionYear = productionYear;
            Counter++;
            this.Id = Counter;
        }

        public Car(String brand, int productionYear, int id)
        {
            this.Brand = brand;
            this.ProductionYear = productionYear;
            this.Id = id;
        }


        //This method is used to return a randomly generated array of cars -----------------------------------------------------------
        public static List<Car> PopulateList(int length)     //length is the legth of the array to create
        {
            if (!CarDB.isPopulated())
            {
                String[] NameArray = { "Alfa", "Peugeot", "Skoda", "BMW", "Audi", "Lamborghini", "Opel", "Seat", "Citroen", "Lada" };//Array of names
                int lowestProductionYear = 1994;         //lowest year of production
                Random r1 = new Random();                //Randoms we will use to select random   numbers for years and for the index of the NameArray
                List<Car> List = new List<Car>(length);  //we will create a dummy array the same size of the Cars[] array and return it to populate the array with it 

                for (int i = 0; i < length; i++)
                {
                    Car x = new Car(
                        NameArray[(int)(r1.NextDouble() * 10)],
                        lowestProductionYear + (int)(r1.NextDouble() * 24)
                        );
                    List.Add(x);//Add the Car "x" to the List<Car> List that is going to get returned to the CarList
                    CarDB.Add(x);//Add the Car "x" to the CarDB database
                }
                return List;
            }
            else
            {
                return CarDB.Load();
            }
        }


        //Method we use to save the List to a .txt file in the CarApi/Model folder
        public static void UpdateTxtFile()
        {
            string _path = "";
            string json = "";

            String projectPath = AppDomain.CurrentDomain.BaseDirectory;
            String dataBasePath = "Models\\carList.txt";
            _path = Path.Combine(projectPath, dataBasePath);


            for (int i = 0; i < Car.CarsList.Count; i++)
            {
                if (i == 0)     //if its the first Car in the list open square brackets, go to new line  wtrite the first Car
                {
                    json += "[" + Environment.NewLine + JsonConvert.SerializeObject(Car.CarsList[i]) + "," + Environment.NewLine;
                }

                else if (i == Car.CarsList.Count - 1)      //if its the first Car in the list close square brackets
                {
                    json += JsonConvert.SerializeObject(Car.CarsList[i]) + Environment.NewLine + "]";
                }
                else       //else just write the next Car add a coma(,) and go to new line 
                {
                    json += JsonConvert.SerializeObject(Car.CarsList[i]) + "," + Environment.NewLine;
                }
            }
            File.WriteAllText(_path, json);      //write Car.json to a . txt File in _path 
        }

    }
}


