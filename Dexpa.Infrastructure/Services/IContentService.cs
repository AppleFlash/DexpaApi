using System;
using System.Collections.Generic;
using Dexpa.Core;
using Dexpa.Core.Model;

namespace Dexpa.Infrastructure.Services
{
    public interface IContentService : IDisposable
    {
        string Add(UploadFileInfo fileInfo);

        void UpdateContent(string id, string url);

        string GetUrl(string contentId);

        void DeleteContent(string id);

        IList<string> GetUrlList(IList<string> ids);
    }
}