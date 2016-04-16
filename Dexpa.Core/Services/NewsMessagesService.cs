using System;
using System.Collections.Generic;
using System.Linq;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;

namespace Dexpa.Core.Services
{
    public class NewsMessagesService : INewsMessagesService
    {
        private readonly INewsMessagesRepository mNewsMessagesRepository;

        public NewsMessagesService(INewsMessagesRepository newsMessagesRepository)
        {
            mNewsMessagesRepository = newsMessagesRepository;
        }

        public NewsMessage GetNewsMessage(long mesId)
        {
            return mNewsMessagesRepository.Single(c => c.Id == mesId);
        }

        public NewsMessage AddNewsMessage(NewsMessage newsMessage)
        {
            var existingMessage = mNewsMessagesRepository.Single(c => c.Id == newsMessage.Id);
            if (existingMessage != null)
            {
                return null;
            }

            newsMessage.TimeStamp = DateTime.UtcNow;
            newsMessage = mNewsMessagesRepository.Add(newsMessage);
            mNewsMessagesRepository.Commit();

            return newsMessage;
        }

        public void DeleteNewsMessage(long mesId)
        {
            var newsMessage = mNewsMessagesRepository.Single(c => c.Id == mesId);
            if (newsMessage != null)
            {
                mNewsMessagesRepository.Delete(newsMessage);
                mNewsMessagesRepository.Commit();
            }
        }

        public NewsMessage UpdateNewsMessage(NewsMessage newsMessage)
        {
            var existsMessage = mNewsMessagesRepository.Single(c => c.Id == newsMessage.Id);
            if (existsMessage != null && existsMessage.Id != newsMessage.Id)
            {
                return null;
            }
            else
            {
                mNewsMessagesRepository.Update(newsMessage);
                mNewsMessagesRepository.Commit();
            }
            return newsMessage;
        }

        public List<NewsMessage> GetLastNewsMessages()
        {
            var yearBeforeTime = DateTime.UtcNow.AddYears(-1);
            return
                mNewsMessagesRepository.List(n => n.TimeStamp > yearBeforeTime)
                    .OrderByDescending(c => c.TimeStamp)
                    .ToList();
        }


        public void Dispose()
        {
            mNewsMessagesRepository.Dispose();
        }
    }
}
