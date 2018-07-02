using System.Collections.Generic;
using System.Web.Http;
using CarApi.Models;
using System.Web.Http.Cors;

namespace CarApi.Controllers
{
    [EnableCors(origins: "http://localhost:57356", headers: "*", methods: "*")]
    public class CarController : ApiController
    {
        //This class is used to Get the Cars[] array or a Cars[id] element of the array


        // GET: api/Car
        public List<Car> Get()
        {
            Car.updateTxtFile();
            return Car.GetCars(); ;
            //returns Car.Cars Array
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
        {   //Assign the new Car we received  to the last place in the array
            Car.UpCount();
            Car.Cars.Add(new Car(car.Brand, car.ProductionYear, Car.Cars.Count+1));
            Car.updateTxtFile();

        }

        // DELETE: api/Car/5
        public void Delete(int id)
        {

        }

    }
}

