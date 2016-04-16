using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.WebAPI.Filters;
using Dexpa.WebApi.Utils;
using Microsoft.AspNet.Identity;

namespace Dexpa.WebApi.Controllers
{
    public class CarEventController : ApiControllerBase
    {
        private ICarEventService mCarEventService;

        public CarEventController(ICarEventService carEventService)
        {
            mCarEventService = carEventService;
        }

        public IHttpActionResult GetCarEvent(long id)
        {
            var carEvent = mCarEventService.GetCarEvent(id);
            if (carEvent != null)
            {
                var rDto = ObjectMapper.Instance.Map<CarEvent, CarEventDTO>(carEvent);
                return Ok(rDto);
            }
            return StatusCode(HttpStatusCode.BadRequest);
        }

        [ValidateModel]
        public IHttpActionResult Post(CarEventDTO carEvent)
        {
            var carEventModel = ObjectMapper.Instance.Map<CarEventDTO, CarEvent>(carEvent);

            var userManager = Startup.UserManagerFactory();
            var user = userManager.Users.SingleOrDefault(u => u.UserName == carEvent.ImplementedByLogin);

            if (user == null)
                return StatusCode(HttpStatusCode.BadRequest);

            carEventModel.ImplementedById = user.Id;
            
            var addedCarEvent = mCarEventService.AddCarEvent(carEventModel);
            return Ok(ObjectMapper.Instance.Map<CarEvent, CarEventDTO>(addedCarEvent));
        }

        [ValidateModel]
        public IHttpActionResult Put(long id, CarEventDTO carEvent)
        {
            var existsCarEvent = mCarEventService.GetCarEvent(id);
            if (existsCarEvent == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            var userManager = Startup.UserManagerFactory();
            var user = userManager.Users.SingleOrDefault(u => u.UserName == carEvent.ImplementedByLogin);
            if (user == null)
                return StatusCode(HttpStatusCode.BadRequest);

            var carEventModel = ObjectMapper.Instance.Map(carEvent, existsCarEvent);

            carEventModel.ImplementedById = user.Id;

            var updatedCarEvent = mCarEventService.UpdateCarEvent(carEventModel);
            return Ok(ObjectMapper.Instance.Map<CarEvent, CarEventDTO>(updatedCarEvent));
        }

        public void Delete(int id)
        {
            mCarEventService.DeleteCarEvent(id);
        }
    }
}