using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dexpa.Core.Model;
using Dexpa.Core.Services;
using Dexpa.WebApi.Utils;

namespace Dexpa.WebApi.Controllers
{
    public class NewsMesagesController : ApiController
    {
        private INewsMessagesService mNewsMessagesService;

        public NewsMesagesController(INewsMessagesService newsMessagesService)
        {
            mNewsMessagesService = newsMessagesService;
        }

        public IHttpActionResult GetMessages()
        {
            var news = mNewsMessagesService.GetLastNewsMessages();
            return Ok(ObjectMapper.Instance.Map<List<NewsMessage>, List<NewsMessageDTO>>(news));
        }

        public IHttpActionResult Post(NewsMessageDTO newsMessageDTO)
        {
            var newsMessage = new NewsMessage();
            ObjectMapper.Instance.Map(newsMessageDTO, newsMessage);
            var addNewsMessage = mNewsMessagesService.AddNewsMessage(newsMessage);
            if (addNewsMessage != null)
            {
                return Ok(ObjectMapper.Instance.Map<NewsMessage, NewsMessageDTO>(addNewsMessage));
            }

            return StatusCode(HttpStatusCode.BadRequest);
        }
    }
}
