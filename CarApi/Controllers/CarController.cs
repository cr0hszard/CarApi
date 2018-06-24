using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CarApi.Models;
using System.Web.Http.Cors;
using System.Web;

namespace CarApi.Controllers
{
    [EnableCors(origins: "http://localhost:57356", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
        //This class is used to Get the Cars[] array or a Cars[id] element of the array


        // GET: api/Car
        public Car[] Get()
        {
            return Car.GetCars();//returns Car.Cars Array
        }

        // GET: api/Car/5
        public Car Get(int id)
        {
            Car auto = new Car();
            auto = Car.GetCars()[id - 1];
            return auto; //returns Car.Cars[id] element of the array
        }

        // PUT: api/Car/5
        [HttpPost]
        public void Post([FromBody]Car car)
        {   //We create a dummy array called temp(temporary) of length Car.Cars.Length+1(the space we need to add the new car)
            Car[] temp = new Car[Car.GetCars().Length + 1];

            //Copy the Car.Cars array into the temp (the last place in the array will be undefined)
            for (int i = 0; i < temp.Length - 1; i++)
            {
                temp[i] = Car.GetCars()[i];
            }

            //Assign the new Car we received  to the last place in the array
            temp[temp.Length - 1] = new Car(car.Brand, car.ProductionYear,temp.Length);
           
            //set the temp array as our new updated Car.Cars array
            Car.SetCars(temp);

            //return Ok();
        }

        // DELETE: api/Car/5
        public void Delete(int id)
        {

        }

    }
}

