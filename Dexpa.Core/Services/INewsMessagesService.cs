using System;
using System.Collections.Generic;
using Dexpa.Core.Model;

namespace Dexpa.Core.Services
{
    public interface INewsMessagesService : IDisposable
    {
        NewsMessage GetNewsMessage(long mesId);

        NewsMessage AddNewsMessage(NewsMessage newsMessage);

        void DeleteNewsMessage(long mesId);

        NewsMessage UpdateNewsMessage(NewsMessage newsMessage);

        List<NewsMessage> GetLastNewsMessages();
    }
}