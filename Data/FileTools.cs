using Microsoft.IdentityModel.Tokens;
using System.Drawing.Drawing2D;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using System.IO;
using BaseballUa.Models;
using static BaseballUa.Data.Enums;

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


        public static List<IFormFile> GetValidated(List<IFormFile> files, bool isIcon = false)
        {
            List<IFormFile> result = new List<IFormFile>();
            if (files != null && files.Count > 0)
            {
                foreach (IFormFile file in files)
                {
                    if (CheckExt(file) && CheckSig(file) && CheckSize(file, isIcon))
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

        private static bool CheckSize(IFormFile file, bool isIcon = false)
        {
            if (!isIcon && file.Length >= Constants.MinImageSize && file.Length <= Constants.MaxImageSize
                || isIcon && file.Length >= Constants.MinIconSize && file.Length <= Constants.MaxIconSize)
            {
                return true;
            }

            return false;
        }

        public async static Task<bool> ResizeAndSave(IFormFile file, int albumId, string rootPath, string fileName, ImageType imageType)
        {
            double maxImageRatio;
            int bigImageHeight;
            int smallImageHeight;
            string imageBaseDir;
            string bigImageSubDir;
            string smallImageSubDir;
            switch (imageType)
            { 

                case ImageType.Flag:
                    maxImageRatio = Constants.MaxIconRatio;
                    bigImageHeight = Constants.BigIconHeight;
                    smallImageHeight = Constants.SmallIconHeight;
                    imageBaseDir = Constants.FlagBaseDir;
                    bigImageSubDir = Constants.BigFlagSubDir;
                    smallImageSubDir = Constants.SmallFlagSubDir;
                    break;
                case ImageType.Club:
                    maxImageRatio = Constants.MaxIconRatio;
                    bigImageHeight = Constants.BigIconHeight;
                    smallImageHeight = Constants.SmallIconHeight;
                    imageBaseDir = Constants.ClubBaseDir;
                    bigImageSubDir = Constants.BigClubSubDir;
                    smallImageSubDir = Constants.SmallClubSubDir;
                    break;
                case ImageType.Staff:
                    maxImageRatio = Constants.MaxAvatarRatio;
                    bigImageHeight = Constants.BigAvatarHeight;
                    smallImageHeight = Constants.SmallAvatarHeight;
                    imageBaseDir = Constants.StaffBaseDir;
                    bigImageSubDir = Constants.BigStaffSubDir;
                    smallImageSubDir = Constants.SmallStaffSubDir;
                    break;
                case ImageType.Team:
                    maxImageRatio = Constants.MaxIconRatio;
                    bigImageHeight = Constants.BigIconHeight;
                    smallImageHeight = Constants.SmallIconHeight;
                    imageBaseDir = Constants.TeamBaseDir;
                    bigImageSubDir = Constants.BigTeamSubDir;
                    smallImageSubDir = Constants.SmallTeamSubDir;
                    break;
                default:
                    maxImageRatio = Constants.MaxImageRatio;
                    bigImageHeight = Constants.BigImageHeight;
                    smallImageHeight = Constants.SmallImageHeight;
                    imageBaseDir = Constants.ImageBaseDir;
                    bigImageSubDir = Constants.BigImageSubDir;
                    smallImageSubDir = Constants.SmallImageSubDir;
                    break;
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (var image = System.Drawing.Image.FromStream(memoryStream))
                {
                    if ((image.Width / image.Height) > maxImageRatio 
                        || (image.Height / image.Width) > maxImageRatio
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
                            var imageHeight = bigImageHeight;
                            if (imageType != ImageType.Photo && image.Width > image.Height)
                            {
                                imageHeight = image.Height * bigImageHeight / image.Width;
                            }
                            bigImage = ResizeImage(image, imageHeight);
                        }
                        catch (Exception ex) 
                        {
                            return false;
                        }
                        
                        try
                        {
                            var imageHeight = smallImageHeight;
                            if (imageType != ImageType.Photo && image.Width > image.Height)
                            {
                                imageHeight = image.Height * smallImageHeight / image.Width;
                            }
                            smallImage = ResizeImage(image, imageHeight);
                        }
                        catch (Exception ex)
                        {
                            return false;
                        }

                        //var fileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
                        var bigImageDirPath = Path.Combine(imageBaseDir, bigImageSubDir, albumId == 0 ? "" : albumId.ToString());
                        var bigImageFullPath = Path.Combine(bigImageDirPath, fileName);
                        var smallImageDirPath = Path.Combine(imageBaseDir, smallImageSubDir, albumId == 0 ? "" : albumId.ToString());
                        var smallImageFullPath = Path.Combine(smallImageDirPath, fileName);

                        Encoder myEncoder = Encoder.Quality;
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

        public static void RemoveAlbumPhoto(Photo photo)
        {
            var bigImageDirPath = Path.Combine(Constants.ImageBaseDir, Constants.BigImageSubDir, photo.AlbumId.ToString(), photo.FnameBig);
            var smallImageDirPath = Path.Combine(Constants.ImageBaseDir, Constants.SmallImageSubDir, photo.AlbumId.ToString(), photo.FnameSmall);

            if (File.Exists(bigImageDirPath))
            {
                File.Delete(bigImageDirPath);
            }
            if (File.Exists(smallImageDirPath))
            {
                File.Delete(smallImageDirPath);
            }
        }

        public static void RemoveStaffAvatar(Staff staff)
        {
            if(staff != null 
                && staff.AvatarLarge != null 
                && staff.AvatarSmall != null 
                && staff.AvatarLarge != Constants.DefaultStaffBigImage
                && staff.AvatarSmall != Constants.DefaultStaffSmallImage) 
            {
                var bigImageDirPath = Path.Combine(Constants.StaffBaseDir, Constants.BigStaffSubDir, staff.AvatarLarge);
                var smallImageDirPath = Path.Combine(Constants.StaffBaseDir, Constants.SmallStaffSubDir, staff.AvatarSmall);
                if(File.Exists(bigImageDirPath)) 
                {
                    File.Delete(bigImageDirPath);
                }
                if(File.Exists(smallImageDirPath))
                {
                    File.Delete(smallImageDirPath);
                }
            }

        }

        public static void RemoveTeamLogo(Team team)
        {
            if (team != null
                && team.FnameLogoBig != null
                && team.FnameLogoSmall != null
                && team.FnameLogoSmall != Constants.DefaultTeamSmallImage
                && team.FnameLogoBig != Constants.DefaultTeamBigImage)
            {
                var bigLogo = Path.Combine(Constants.TeamBaseDir, Constants.BigTeamSubDir, team.FnameLogoBig);
                var smallLogo = Path.Combine(Constants.TeamBaseDir, Constants.SmallTeamSubDir, team.FnameLogoSmall);

                if (File.Exists(bigLogo))
                {
                    File.Delete(bigLogo);
                }
                if (File.Exists(smallLogo))
                {
                    File.Delete(smallLogo);
                }
            }
        }
    }


}
