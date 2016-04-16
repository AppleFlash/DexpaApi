using System.Configuration;
using System.Net;
using System.Text;
using System.Web.Http.Results;
using Backload.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Backload;
using System.Threading.Tasks;
using Backload.Eventing.Args;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Infrastructure.Services;
using Microsoft.Ajax.Utilities;
using NLog;

namespace Dexpa.WebApi.Controllers
{
    public class ContentController : ApiControllerBase
    {
        protected Logger mLogger = LogManager.GetCurrentClassLogger();

        private IContentService mContentService;

        public ContentController(IContentService contentService)
        {
            mContentService = contentService;
        }

        private static readonly List<string> TypesList = new List<string>(new string[]
        {
            "image/jpeg",
            "image/tiff",   
            "image/png",    
            "image/bmp"
        });


        [Route("api/Content/")]
        public async Task<ActionResult> FileHandler()
        {
            try
            {
                var request = new HttpRequestWrapper(HttpContext.Current.Request);
                FileUploadHandler handler = new FileUploadHandler(request, null);
                handler.IncomingRequestStartedAsync += handler_IncomingRequestStarted;
                handler.StoreFileRequestStartedAsync += handler_StoreFileRequestStartedAsync;
                handler.StoreFileRequestFinishedAsync += handler_StoreFileRequestFinishedAsync;

                var task = handler.HandleRequestAsync();
                var jsonResult = (JsonResult)await task;

                mLogger.Info(jsonResult);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        async Task handler_IncomingRequestStarted(object sender, IncomingRequestEventArgs e)
        {
            var values = e.Param.BackloadValues;
            ;
        }

        async Task handler_StoreFileRequestStartedAsync(object sender, StoreFileRequestEventArgs e)
        {
            if (TypesList.All(d => d != e.Param.FileStatusItem.ContentType))
            {
                e.Context.PipelineControl.ExecutePipeline = false;
            }
        }

        async Task handler_StoreFileRequestFinishedAsync(object sender, StoreFileRequestEventArgs e)
        {
            try
            {
                mLogger.Info(e.Param.FileStatusItem.UploadContext);
                var fileInfo = new UploadFileInfo();

                fileInfo.FileDirectory = e.Param.FileStatusItem.StorageInfo.FileDirectory;
                fileInfo.FileName = e.Param.FileStatusItem.FileName;
                fileInfo.ThumbnailPath = e.Param.FileStatusItem.StorageInfo.ThumbnailPath;
                fileInfo.FileContext = e.Param.FileStatusItem.UploadContext;

                e.Param.FileStatusItem.Message = mContentService.Add(fileInfo);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        private string GetPath(string filePath, string subDir)
        {
            filePath = Path.Combine(filePath, subDir);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            return filePath;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mContentService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
