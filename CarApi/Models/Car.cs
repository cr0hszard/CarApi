using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        private static Car[] Cars = PopulateArray(15); //static Array of cars

        //The Counter is used to count the instances of objects and to set a different Id for every car in the constructor
        public static int Counter = 0;

        //Car properties---------------------------------
        public string Brand { get; set; }
        public int ProductionYear { get; set; }
        public int Id { get; set; }

        //Cars array getter and setter --------------------
        public static Car[] GetCars()
        {
            return Cars;
        }

        public static void SetCars(Car[] Array)
        {
            Car.Cars = Array;
        }

        //Class constructors------------------------------
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
        public Car(String brand, int productionYear,int id)
        {
            this.Brand = brand;

            this.ProductionYear = productionYear;

           
            this.Id = id;
        }

        //This method is used to return a randomly generated array of cars ------------------------------------------
        public static Car[] PopulateArray(int length)//length is the legth of the array to create
        {
            String[] NameArray = { "Alfa", "Peugeot", "Skoda", "BMW", "Audi", "Lamborghini", "Opel", "Seat", "Citroen", "Lada" };//Array of names

            int lowestProductionYear = 1994;//lowest year of production

            //Randoms we will use to select random numbers for years and for the index of the NameArray
            Random r1 = new Random();

            //we will create a dummy array the same size of the Cars[] array and populate the array with it 
            Car[] Array = new Car[length];
            for (int i = 0; i < length; i++)
            {
                Array[i] = new Car(
                    NameArray[(int)(r1.NextDouble() * 10)],
                    lowestProductionYear + (int)(r1.NextDouble() * 24)
                    );
            }
            return Array;
        }
    }
}

