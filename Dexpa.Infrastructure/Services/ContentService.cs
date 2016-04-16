using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dexpa.Core;
using Dexpa.Core.Model;
using Dexpa.Core.Repositories;
using Dexpa.Core.Services;
using Dexpa.Infrastructure.Repositories;
using Dexpa.Infrastructure.Utils;
using System.Configuration;
using NLog;

namespace Dexpa.Infrastructure.Services
{
    public class ContentService : IContentService
    {
        private IContentRepository mContentRepository;

        private HexIdGenerator mIdGenerator;

        protected Logger mLogger = LogManager.GetCurrentClassLogger();

        private static readonly string mPath = (string)(new AppSettingsReader()).GetValue("ContentFolder", typeof(string));

        public ContentService(IContentRepository contentRepository)
        {
            mContentRepository = contentRepository;

            mIdGenerator = new HexIdGenerator();
        }

        public string Add(UploadFileInfo fileInfo)
        {
            try
            {
                var contextMass = fileInfo.FileContext.Split('@');
                DexpaContentType contentType;
                if (DexpaContentType.TryParse(contextMass[0], out contentType))
                {
                    long entityId;
                    if (long.TryParse(contextMass[1], out entityId))
                    {
                        if (contentType == DexpaContentType.DriverPhoto)
                            return AddDriverPhoto(fileInfo, entityId);
                        if (contentType == DexpaContentType.CarDamages)
                            return AddDamagesPhoto(fileInfo, entityId);

                        return AddQualityPhoto(fileInfo, entityId, contentType);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        private string AddQualityPhoto(UploadFileInfo fileInfo, long driverId, DexpaContentType type)
        {
            try
            {
                Content content = new Content();
                content.Id = mIdGenerator.Generate();

                var newWebPath = "/Content/QualityPhotos/" + content.Id + ".jpg";
                var newWebSmallPath = "/Content/QualityPhotos/_small/" + content.Id + ".jpg";
                var newWebThumbPath = "/Content/QualityPhotos/_thumb/" + content.Id + ".jpg";

                var paths = GetLocalPaths(content.Id, type);

                System.IO.File.Copy(fileInfo.FileDirectory + fileInfo.FileName, paths[0], true);
                System.IO.Directory.Delete(fileInfo.FileDirectory, true);

                PhotoCreator.CreatePhoto(paths[0], paths[1], type, false);
                PhotoCreator.CreatePhoto(paths[0], paths[2], type, true);

                content.WebUrl = newWebPath;
                content.WebUrlSmall = newWebSmallPath;
                content.WebUrlThumb = newWebThumbPath;
                content.Type = type;
                content.TimeStamp = DateTime.UtcNow;
                content.DriverId = driverId;
                var addedContent = mContentRepository.Add(content);

                mContentRepository.Commit();

                return addedContent.WebUrl;
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        private string AddDriverPhoto(UploadFileInfo fileInfo, long driverId)
        {
            try
            {
                DeleteOldDriverContent(driverId, DexpaContentType.DriverPhoto);

                Content content = new Content();
                content.Id = mIdGenerator.Generate();

                var newWebPath = "/Content/DriverPhotos/" + content.Id + ".jpg";
                var newWebSmallPath = "/Content/DriverPhotos/_small/" + content.Id + ".jpg";

                var paths = GetLocalPaths(content.Id, DexpaContentType.DriverPhoto);

                var originalPath = fileInfo.FileDirectory + fileInfo.FileName;

                PhotoCreator.CreatePhoto(originalPath, paths[0], DexpaContentType.DriverPhoto, false);
                PhotoCreator.CreatePhoto(paths[0], paths[1], DexpaContentType.DriverPhoto);

                System.IO.Directory.Delete(fileInfo.FileDirectory, true);


                content.WebUrl = newWebPath;
                content.WebUrlSmall = newWebSmallPath;
                content.DriverId = driverId;
                content.Type = DexpaContentType.DriverPhoto;
                content.TimeStamp = DateTime.UtcNow;
                var addedContent = mContentRepository.Add(content);

                mContentRepository.Commit();

                return addedContent.WebUrl;
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        private string AddDamagesPhoto(UploadFileInfo fileInfo, long repairId)
        {
            try
            {
                Content content = new Content();
                content.Id = mIdGenerator.Generate();

                var newWebPath = "/Content/DamagesPhotos/" + content.Id + ".jpg";
                var newWebSmallPath = "/Content/DamagesPhotos/_small/" + content.Id + ".jpg";

                var paths = GetLocalPaths(content.Id, DexpaContentType.CarDamages);

                System.IO.File.Copy(fileInfo.FileDirectory + fileInfo.FileName, paths[0], true);
                System.IO.Directory.Delete(fileInfo.FileDirectory, true);

                PhotoCreator.CreatePhoto(paths[0], paths[1], DexpaContentType.CarDamages);

                content.WebUrl = newWebPath;
                content.WebUrlSmall = newWebSmallPath;
                content.Type = DexpaContentType.CarDamages;
                content.TimeStamp = DateTime.UtcNow;
                content.RepairId = repairId;
                var addedContent = mContentRepository.Add(content);

                mContentRepository.Commit();

                return addedContent.WebUrl;
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                throw;
            }
        }

        public void UpdateContent(string id, string url)
        {
            var content = mContentRepository.Single(c => c.Id == id);
            if (content != null)
            {
                content.WebUrl = url;
                mContentRepository.Update(content);
            }
            throw new CoreException("Can't update content. Item is not exists.", ErrorCode.ItemNotExists);
        }

        public string GetUrl(string contentId)
        {
            var content = mContentRepository.Single(c => c.Id == contentId);
            if (content != null)
            {
                return mPath + content.WebUrl;
            }
            else
            {
                return null;
            }
        }

        public void DeleteContent(string id)
        {
            var content = mContentRepository.Single(c => c.Id == id);
            if (content != null)
            {
                var paths = GetLocalPaths(id, content.Type);
                mContentRepository.Delete(content);
                if (System.IO.File.Exists(paths[0]))
                    System.IO.File.Delete(paths[0]);
                if (System.IO.File.Exists(paths[1]))
                    System.IO.File.Delete(paths[1]);
                if (System.IO.File.Exists(paths[2]))
                    System.IO.File.Delete(paths[2]);
            }
            else
            {
                throw new CoreException("Can't delete content. Item is not exists.", ErrorCode.ItemNotExists);
            }
        }

        public IList<string> GetUrlList(IList<string> ids)
        {
            var urlList = new List<string>();

            var contents = mContentRepository.List(c => ids.Contains(c.Id));
            var contentIndex = 0;
            for (int i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                if (contents[contentIndex].Id == id)
                {
                    urlList.Add(mPath + contents[contentIndex].WebUrl);
                    contentIndex++;
                }
                else
                {
                    urlList.Add(null);
                }
            }
            return urlList;
        }

        private void DeleteOldDriverContent(long driverId, DexpaContentType? type)
        {
            IList<Content> contents;
            contents = type == null
                ? mContentRepository.List(c => c.DriverId == driverId)
                : mContentRepository.List(c => c.DriverId == driverId && c.Type == type);
            if (contents != null && contents.Count != 0)
            {
                foreach (var content in contents)
                {
                    DeleteContent(content.Id);
                }
            }
        }

        private string GetPath(string filePath, string subDir)
        {
            filePath = Path.Combine(filePath, subDir);
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            return filePath;
        }

        private List<string> GetLocalPaths(string contentId, DexpaContentType type)
        {
            var paths = new List<string>();

            string subfolder;

            switch (type)
            {
                case DexpaContentType.DriverPhoto:
                    subfolder = "DriverPhotos";
                    break;
                case DexpaContentType.CarDamages:
                    subfolder = "DamagesPhotos";
                    break;
                default:
                    subfolder = "QualityPhotos";
                    break;

            }

            var newPath = mPath;
            var newSmallPath = newPath = GetPath(newPath, subfolder);
            var newThumbPath = newSmallPath;
            newPath += "\\" + contentId + ".jpg";
            newSmallPath = GetPath(newSmallPath, "_small");
            newThumbPath = GetPath(newThumbPath, "_thumb");
            newSmallPath += "\\" + contentId + ".jpg";
            newThumbPath += "\\" + contentId + ".jpg";

            paths.Add(newPath);
            paths.Add(newSmallPath);
            paths.Add(newThumbPath);
            return paths;
        }

        public void Dispose()
        {
            mContentRepository.Dispose();
        }
    }
}
