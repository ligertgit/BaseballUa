using Microsoft.IdentityModel.Tokens;
using System.Drawing.Drawing2D;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using System.IO;

namespace BaseballUa.Data
{
    public static class FileTools
    {
        private static readonly Dictionary<string, List<byte[]>> _fileSignature = new()
            {
                { ".jpg", new List<byte[]>
                                {
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                                }
                },
                { ".jpeg", new List<byte[]>
                                {
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                                }
                },
                { ".png", new List<byte[]>
                                {
                                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                                }
                },
                { ".heic", new List<byte[]>
                                {
                                    new byte[] { 0x66, 0x74, 0x79, 0x70, 0x68 },
                                    new byte[] { 0x66, 0x74, 0x79, 0x70, 0x6D },
                                }
                },
                { ".jp2", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".j2k", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".jpf", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".jpm", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".jpg2", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".j2c", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".jpc", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".jpx", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
                { ".mj2", new List<byte[]>
                                {
                                    new byte[] { 0x00, 0x00, 0x00, 0x0C },
                                    new byte[] { 0xFF, 0x4F, 0xFF, 0x51 },
                                }
                },
            };


        public static List<IFormFile> GetValidated(List<IFormFile> files)
        {
            List<IFormFile> result = new List<IFormFile>();
            if (files != null && files.Count > 0)
            {
                foreach (IFormFile file in files)
                {
                    if (CheckExt(file) && CheckSig(file) && CheckSize(file))
                    {
                        result.Add(file);
                    }
                }
            }

            return result;
        }

        private static bool CheckExt(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            if (!String.IsNullOrEmpty(ext) && _fileSignature.ContainsKey(ext))
            {
                return true;
            }

            return false;
        }

        private static bool CheckSig(IFormFile file)
        {
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                var ext = Path.GetExtension(file.FileName);
                var sig = reader.ReadBytes(_fileSignature[ext].Max(m => m.Length));
                if (_fileSignature[ext].Any(s => sig.SequenceEqual(s)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckSize(IFormFile file)
        {
            if (file.Length >= Constants.MinImageSize && file.Length <= Constants.MaxImageSize)
            {
                return true;
            }

            return false;
        }

        public async static Task<bool> ResizeAndSave(IFormFile file, int albumId, string rootPath, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var image = System.Drawing.Image.FromStream(memoryStream))
                {
                    if ((image.Width / image.Height) > Constants.MaxImageRatio 
                        || (image.Height / image.Width) > Constants.MaxImageRatio
                        || image.Width == 0
                        || image.Height == 0
                       )
                    {
                        return false;    
                    }
                    else
                    {
                        System.Drawing.Image bigImage;
                        System.Drawing.Image smallImage;
                        try
                        {
                            bigImage = ResizeImage(image, Constants.BigImageHeight);
                        }
                        catch (Exception ex) 
                        {
                            return false;
                        }
                        
                        try
                        {
                            smallImage = ResizeImage(image, Constants.SmallImageHeight);
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }

                        //var fileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
                        var bigImageDirPath = Path.Combine(Constants.ImageBaseDir, Constants.BigImageSubDir, albumId.ToString());
                        var bigImageFullPath = Path.Combine(bigImageDirPath, fileName);
                        var smallImageDirPath = Path.Combine(Constants.ImageBaseDir, Constants.SmallImageSubDir, albumId.ToString());
                        var smallImageFullPath = Path.Combine(smallImageDirPath, fileName);

                        Encoder myEncoder = Encoder.Quality; ;
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        ImageCodecInfo codecInfo = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                        try
                        { 
                            Directory.CreateDirectory(bigImageDirPath);
                            Directory.CreateDirectory(smallImageDirPath);
                            
                            bigImage.Save(bigImageFullPath, codecInfo, myEncoderParameters);
                            smallImage.Save(smallImageFullPath, codecInfo, myEncoderParameters);
                        }
                        catch 
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static System.Drawing.Image ResizeImage(System.Drawing.Image image, int height)
        {
            int width = (int)((double)image.Width / image.Height * height);

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }


}
