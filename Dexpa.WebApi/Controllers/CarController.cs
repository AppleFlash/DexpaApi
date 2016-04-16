using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.DTO;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;
using Ninject.Web.Common;

namespace Dexpa.WebApi.Controllers
{
    public class CarController : ApiControllerBase
    {
        private ICarService mCarService;

        public CarController(ICarService carService)
        {
            mCarService = carService;
        }

        public IHttpActionResult GetCar(long id)
        {
            var car = mCarService.GetCar(id);
            if (car != null)
            {
                return Ok(ObjectMapper.Instance.Map<Car, CarDTO>(car));
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        public IEnumerable<CarDTO> GetAllCars(bool unassigned = false, long? includeDriverCar = null) // включая машину прикрепленную к водителю с указанным id
        {
            return ObjectMapper.Instance.Map<List<Car>, List<CarDTO>>(mCarService.GetCars(unassigned, includeDriverCar));  
        }
        
        [ValidateModel]
        public IHttpActionResult Post(CarDTO carDTO)
        {
            var car = new Car();
            ObjectMapper.Instance.Map(carDTO, car);
            var addCar = mCarService.AddCar(car);
            if (addCar != null)
            {
                return Ok(ObjectMapper.Instance.Map<Car, CarDTO>(addCar));
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, CarDTO carDTO)
        {
            if (carDTO.Description == null) carDTO.Description = "";

            var existsCar = mCarService.GetCar(id);
            if (existsCar == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
            var carModel = ObjectMapper.Instance.Map(carDTO, existsCar);
            var updatedCar = mCarService.UpdateCar(carModel);
            if (updatedCar != null)
            {
                return Ok(ObjectMapper.Instance.Map<Car, CarDTO>(updatedCar));   
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        public void Delete(int id)
        {
            mCarService.DeleteCar(id);
        }

        [Route("api/cars/brands")]
        [HttpGet]
        public List<string> GetCarBrands()
        {
            return mCarService.GetCarBrands();
        }

        [Route("api/cars/models")]
        [HttpGet]
        public List<string> GetCarModels()
        {
            return mCarService.GetCarModels();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mCarService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
