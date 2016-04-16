using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;
using Dexpa.Core.Model;
using NLog;

namespace Dexpa.Infrastructure.Utils
{
    public class PhotoCreator
    {
        public static void CreatePhoto(string origFileName, string previewFileName, DexpaContentType contentType, bool isThumb = true)
        {
            try
            {
                int maxSide;

                switch (contentType)
                {
                    case DexpaContentType.DriverPhoto:
                        maxSide = isThumb ? 35 : 200;
                        break;
                    case DexpaContentType.CarDamages:
                        maxSide = 200;
                        break;
                    default: // DexpaContentType.QualityPhotos
                        maxSide = isThumb ? 35 : 200;
                        break;
                }

                var origImg = new Bitmap(origFileName);
                int width = 0, heigth = 0;
                if (origImg.Width > origImg.Height)
                {
                    width = maxSide;
                    heigth = (int)(maxSide / (double)origImg.Width * origImg.Height);
                }
                else
                {
                    heigth = maxSide;
                    width = (int)(maxSide / (double)origImg.Height * origImg.Width);
                }
                var previewImg = new Bitmap(origImg, width, heigth);
                origImg.Dispose();
                previewImg.Save(previewFileName, ImageFormat.Jpeg);
                previewImg.Dispose();
            }
            catch (Exception ex)
            {
                Logger mLogger = LogManager.GetCurrentClassLogger();
                mLogger.ErrorException("Failed to save albumPreview", ex);
            }
        }
    }
}