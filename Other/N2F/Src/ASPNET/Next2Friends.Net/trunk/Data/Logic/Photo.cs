using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Next2Friends.Misc;
using System.Drawing.Drawing2D;
using Goheer.EXIF;

namespace Next2Friends.Data
{
    public partial class Photo
    {
        #region Main Photo Process
        /// <summary>
        /// overload using a byte array
        /// </summary>
        /// <param name="member"></param>
        /// <param name="PhotoCollectionID"></param>
        /// <param name="byteImage"></param>
        public static void ProcessMemberPhoto(Member member, int PhotoCollectionID, byte[] byteImage, DateTime TakenDT, bool SnappedFromMobile)
        {
            Image image = ByteArrayToImage(byteImage);
            ProcessMemberPhoto(member, PhotoCollectionID, image, TakenDT, SnappedFromMobile);
        }

        public static string ProcessMemberPhoto(Member member, int PhotoCollectionID, Image image, DateTime TakenDT, bool SnappedFromMobile)
        {
            string GlobalWebID = UniqueID.NewWebID();
            string FileName = GlobalWebID + @".jpg";

            Bitmap bmp = (Bitmap)image;

            try
            {
                EXIFextractor exif = new EXIFextractor(ref bmp, string.Empty);

                if (exif.DTTaken.Year != 1900)
                {
                    TakenDT = exif.DTTaken;
                }

            }
            catch { }


            Photo photo = new Photo();
            photo.Active = true;
            photo.Mobile = SnappedFromMobile;
            photo.MemberID = member.MemberID;
            photo.WebPhotoID = GlobalWebID;
            photo.PhotoCollectionID = PhotoCollectionID;
            photo.TakenDT = TakenDT;
            photo.CreatedDT = DateTime.Now;

            // create the large photo
            // just store the large image.. dont make a resource record
            System.Drawing.Image MainImage = Photo.ResizeTo800x600(image);
            string Savepath = member.NickName + @"\" + "plrge" + @"\" + FileName;
            Photo.SaveToDiskNoCompression(MainImage, Savepath);

            //create the medium
            photo.PhotoResourceFile = new ResourceFile();
            photo.PhotoResourceFile.WebResourceFileID = GlobalWebID;
            photo.PhotoResourceFile.ResourceType = (int)ResourceFileType.PhotoLarge;
            photo.PhotoResourceFile.Path = member.NickName + "/" + "pmed" + "/";
            photo.PhotoResourceFile.FileName = FileName;
            photo.PhotoResourceFile.Save();
            System.Drawing.Image MediumImage = Photo.Resize480x480(MainImage);
            Photo.SaveToDisk(MediumImage, photo.PhotoResourceFile.SavePath);

            //create the thumbnail
            photo.ThumbnailResourceFile = new ResourceFile();
            photo.ThumbnailResourceFile.WebResourceFileID = GlobalWebID;
            photo.ThumbnailResourceFile.ResourceType = (int)ResourceFileType.PhotoThumbnail;
            photo.ThumbnailResourceFile.Path = member.NickName + "/" + "pthmb" + "/";
            photo.ThumbnailResourceFile.FileName = FileName;
            photo.ThumbnailResourceFile.Save();


            System.Drawing.Image ThumbnailImage = Photo.ScaledCropTo121x91(MediumImage);


            Photo.SaveToDisk(ThumbnailImage, photo.ThumbnailResourceFile.SavePath);

            // attached the resource ids to the photos
            photo.ThumbnailResourceFileID = photo.ThumbnailResourceFile.ResourceFileID;
            photo.PhotoResourceFileID = photo.PhotoResourceFile.ResourceFileID;

            photo.Save();

            // update the number of photos
            MemberProfile memberProfile = member.MemberProfile[0];
            memberProfile.NumberOfPhotos++;
            memberProfile.Save();

            return photo.WebPhotoID;
        }

        /// <summary>
        /// Roates a Gallery image
        /// </summary>
        /// <param name="member"></param>
        /// <param name="WebPhotoID"></param>
        /// <param name="RootPath"></param>
        /// <returns></returns>
        public static bool RotateGalleryImage(Member member, string WebPhotoID, string RootPath, int Rotation)
        {
            // seems a bit overcomplicated but you cannot save to an image that is open as its locked.. so open it from a file
            // stream and then dispose the filestream
            //try
            //{
            Photo photo = Photo.GetPhotoByWebPhotoIDWithJoin(WebPhotoID);

            string FullPath = RootPath + member.NickName;
            string LargeFullPath = FullPath + @"\plrge\" + photo.PhotoResourceFile.FileName;
            string MediumFullPath = FullPath + @"\pmed\" + photo.PhotoResourceFile.FileName;
            string ThumbFullPath = FullPath + @"\pthmb\" + photo.PhotoResourceFile.FileName;

            FileStream fsLarge = new FileStream(LargeFullPath, FileMode.Open);

            Image LargeImage = Bitmap.FromStream(fsLarge);
            fsLarge.Close();
            fsLarge.Dispose();

            if (Rotation == 1)
            {
                LargeImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (Rotation == 2)
            {
                LargeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (Rotation == 3)
            {
                LargeImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }

            SaveToDiskRelativePath(LargeImage, LargeFullPath);

            //resize medium and save
            System.Drawing.Image MediumImage = Photo.Resize480x480(LargeImage);
            SaveToDiskRelativePath(MediumImage, MediumFullPath);

            //resize thumbnail and save
            System.Drawing.Image ThumbnailImage = Photo.ResizeTo124x91(MediumImage);
            SaveToDiskRelativePath(ThumbnailImage, ThumbFullPath);

            LargeImage.Dispose();

            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        #endregion

        #region NSpot Photo Process

        /// <summary>
        /// overload using a byte array
        /// </summary>
        /// <param name="member"></param>
        /// <param name="PhotoCollectionID"></param>
        /// <param name="byteImage"></param>
        public static NSpot ProcessNSpotPhoto(Member member, NSpot nSpot, byte[] byteImage)
        {
            Image image = ByteArrayToImage(byteImage);

            return ProcessNSpotPhoto(member, nSpot, image);
        }

        public static NSpot ProcessNSpotPhoto(Member member, NSpot nSpot, Image image)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            DbTransaction Transaction = null;

            try
            {
                conn.Open();
                Transaction = conn.BeginTransaction();

                string GlobalWebID = UniqueID.NewWebID();
                string FileName = GlobalWebID + @".jpg";

                // create the large photo
                // just store the large image.. dont make a resource record
                System.Drawing.Image MainImage = Photo.Resize480x480(image);
                string Savepath = member.NickName + @"\" + "nslrge" + @"\" + FileName;
                Photo.SaveToDisk(MainImage, Savepath);

                //create the medium
                ResourceFile PhotoResourceFile = new ResourceFile();
                PhotoResourceFile.CreatedDT = DateTime.Now;
                PhotoResourceFile.WebResourceFileID = GlobalWebID;
                PhotoResourceFile.ResourceType = (int)ResourceFileType.NspotPhoto;
                PhotoResourceFile.Path = member.NickName + "/" + "nsmed" + "/";
                PhotoResourceFile.FileName = FileName;
                PhotoResourceFile.Save(db);
                System.Drawing.Image MediumImage = Photo.Resize190x130(MainImage);
                Photo.SaveToDisk(MediumImage, PhotoResourceFile.SavePath);

                //create the thumbnail
                ResourceFile ThumbnailResourceFile = new ResourceFile();
                ThumbnailResourceFile.CreatedDT = DateTime.Now;
                ThumbnailResourceFile.WebResourceFileID = GlobalWebID;
                ThumbnailResourceFile.ResourceType = (int)ResourceFileType.NspotThumbnail;
                ThumbnailResourceFile.Path = member.NickName + "/" + "nsthmb" + "/";
                ThumbnailResourceFile.FileName = FileName;
                ThumbnailResourceFile.Save(db);
                System.Drawing.Image ThumbnailImage = Photo.ResizeTo124x91(MediumImage);
                Photo.SaveToDisk(ThumbnailImage, ThumbnailResourceFile.SavePath);

                // attached the resource ids to the photos
                nSpot.ThumbnailResourceFileID = ThumbnailResourceFile.ResourceFileID;
                nSpot.PhotoResourceFileID = PhotoResourceFile.ResourceFileID;

                nSpot.Save(db);

                Transaction.Commit();

            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return nSpot;
        }

        #endregion

        public static bool IsPhotoFile(byte[] PhotoBytes)
        {
            try
            {
                Image image = ByteArrayToImage(PhotoBytes);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #region AAF Photo

        /// <summary>
        /// overload using a byte array
        /// </summary>
        /// <param name="member"></param>
        /// <param name="PhotoCollectionID"></param>
        /// <param name="byteImage"></param>
        public static void ProcessAAFPhoto(Member member, AskAFriend AAF, string PhotoBase64String, int IndexOrder)
        {
            byte[] byteImage = Convert.FromBase64String(PhotoBase64String);
            Image image = ByteArrayToImage(byteImage);

            ProcessAAFPhoto(member, AAF, image, IndexOrder);
        }

        public static void ProcessAAFPhoto(Member member, AskAFriend AAF, Image image, int IndexOrder)
        {
            string GlobalWebID = UniqueID.NewWebID();
            string FileName = GlobalWebID + @".jpg";

            AskAFriendPhoto photo = new AskAFriendPhoto();
            photo.AskAFriendID = AAF.AskAFriendID;
            photo.IndexOrder = IndexOrder;

            //create the medium
            ResourceFile PhotoResourceFile = new ResourceFile();
            PhotoResourceFile.WebResourceFileID = GlobalWebID;
            PhotoResourceFile.ResourceType = (int)ResourceFileType.AAFLarge;
            PhotoResourceFile.Path = member.NickName + "/" + "aaflrge" + "/";
            PhotoResourceFile.FileName = FileName;
            PhotoResourceFile.Save();
            System.Drawing.Image MediumImage = Photo.Resize480x480(image);
            Photo.SaveToDisk(MediumImage, PhotoResourceFile.SavePath);

            photo.PhotoResourceFileID = PhotoResourceFile.ResourceFileID;

            //create the thumbnail
            ResourceFile ThumbnailPhoto = new ResourceFile();
            ThumbnailPhoto.WebResourceFileID = GlobalWebID;
            ThumbnailPhoto.ResourceType = (int)ResourceFileType.AAFThumbnail;
            ThumbnailPhoto.Path = member.NickName + "/" + "aafthmb" + "/";
            ThumbnailPhoto.FileName = FileName;
            ThumbnailPhoto.Save();
            System.Drawing.Image ThumbnailImage = Photo.ResizeTo124x91(MediumImage);
            Photo.SaveToDisk(ThumbnailImage, ThumbnailPhoto.SavePath);

            photo.Save();

            if (IndexOrder == 1)
            {
                AAF.DefaultPhotoResourceFileID = ThumbnailPhoto.ResourceFileID;
            }

            AAF.Save();
        }
        #endregion

        #region Profile Photo

        public static void ProcessProfilePhoto(Int32 memberId, String nickname, Image image)
        {
            if (String.IsNullOrEmpty(nickname))
                throw new ArgumentNullException("nickname");
            
            if (image == null)
                throw new ArgumentNullException("image");

            var webId = UniqueID.NewWebID();
            var fileName = String.Concat(webId, @".jpg");

            /* Create medium image. */
            var photoResourceFile = new ResourceFile()
            {
                WebResourceFileID = webId,
                ResourceType = (int)ResourceFileType.PhotoLarge,
                Path = String.Concat(nickname, "/pmed/"),
                FileName = fileName
            };

            photoResourceFile.Save();

            var mediumImage = Photo.Resize480x480(image);
            Photo.SaveToDisk(mediumImage, photoResourceFile.SavePath);

            /* Create thumbnail. */
            var thumbnailResourceFile = new ResourceFile()
            {
                WebResourceFileID = webId,
                ResourceType = (int)ResourceFileType.PhotoThumbnail,
                Path = String.Concat(nickname, "/pthmb/"),
                FileName = fileName
            };
            thumbnailResourceFile.Save();

            var thumbnailImage = Photo.ResizeTo102x102(mediumImage);
            Photo.SaveToDisk(thumbnailImage, thumbnailResourceFile.SavePath);

            Member.UpdateProfilePhotoResourceFileId(memberId, thumbnailResourceFile.ResourceFileID);

            mediumImage.Dispose();
            thumbnailImage.Dispose();
        }

        /// <summary>
        /// </summary>
        /// <param name="member"></param>
        /// <param name="image"></param>
        public static void ProcessProfilePhoto(Member member, Image image)
        {
            string GlobalWebID = UniqueID.NewWebID();
            string FileName = GlobalWebID + @".jpg";

            //create the medium
            ResourceFile PhotoResourceFile = new ResourceFile();
            PhotoResourceFile.WebResourceFileID = GlobalWebID;
            PhotoResourceFile.ResourceType = (int)ResourceFileType.PhotoLarge;
            PhotoResourceFile.Path = member.NickName + "/" + "pmed" + "/";
            PhotoResourceFile.FileName = FileName;
            PhotoResourceFile.Save();
            System.Drawing.Image MediumImage = Photo.Resize480x480(image);
            Photo.SaveToDisk(MediumImage, PhotoResourceFile.SavePath);

            //create the thumbnail
            ResourceFile ThumbnailResourceFile = new ResourceFile();
            ThumbnailResourceFile.WebResourceFileID = GlobalWebID;
            ThumbnailResourceFile.ResourceType = (int)ResourceFileType.PhotoThumbnail;
            ThumbnailResourceFile.Path = member.NickName + "/" + "pthmb" + "/";
            ThumbnailResourceFile.FileName = FileName;
            ThumbnailResourceFile.Save();
            System.Drawing.Image ThumbnailImage = Photo.ResizeTo102x102(MediumImage);
            Photo.SaveToDisk(ThumbnailImage, ThumbnailResourceFile.SavePath);

            member.DefaultPhoto = ThumbnailResourceFile;
            member.ProfilePhotoResourceFileID = ThumbnailResourceFile.ResourceFileID;

            member.Save();
        }

        public static void ProcessProfilePhotoWithCrop(Member member, Image Original, Image Cropped)
        {
            string GlobalWebID = UniqueID.NewWebID();
            string FileName = GlobalWebID + @".jpg";

            //create the medium
            ResourceFile PhotoResourceFile = new ResourceFile();
            PhotoResourceFile.WebResourceFileID = GlobalWebID;
            PhotoResourceFile.ResourceType = (int)ResourceFileType.PhotoLarge;
            PhotoResourceFile.Path = member.NickName + "/" + "pmed" + "/";
            PhotoResourceFile.FileName = FileName;
            PhotoResourceFile.Save();
            Photo.SaveToDisk(Original, PhotoResourceFile.SavePath);

            //create the thumbnail
            ResourceFile ThumbnailResourceFile = new ResourceFile();
            ThumbnailResourceFile.WebResourceFileID = GlobalWebID;
            ThumbnailResourceFile.ResourceType = (int)ResourceFileType.PhotoThumbnail;
            ThumbnailResourceFile.Path = member.NickName + "/" + "pthmb" + "/";
            ThumbnailResourceFile.FileName = FileName;
            ThumbnailResourceFile.Save();
            Photo.SaveToDisk(Cropped, ThumbnailResourceFile.SavePath);

            member.DefaultPhoto = ThumbnailResourceFile;
            member.ProfilePhotoResourceFileID = ThumbnailResourceFile.ResourceFileID;

            member.Save();
        }



        #endregion

        #region Save to disk
        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static void SaveToDisk(byte[] ImageFileBytes, string SavePath)
        {
            try
            {
                Image ImageFile = ByteArrayToImage(ImageFileBytes);
                SaveToDisk(ImageFile, SavePath);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static bool SaveToDiskNoCompression(Image ImageFile, string SavePath)
        {
            try
            {
                EncoderParameters EncoderParams = new EncoderParameters(2);
                EncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                EncoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, (long)24);

                ImageCodecInfo imageCodecInfo = GetEncoderInfo("image/jpeg");

                SavePath = OSRegistry.GetDiskUserDirectory() + SavePath;

                ImageFile.Save(SavePath, imageCodecInfo, EncoderParams);
            }
            catch (Exception)
            {
                throw new IOException(String.Format(Properties.Resources.IO_WriteFileErrorToSavePath, SavePath));
            }

            return true;
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static bool SaveToDisk(Image ImageFile, string SavePath)
        {
            try
            {
                EncoderParameters EncoderParams = new EncoderParameters(2);
                EncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)90);
                EncoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, (long)24);

                ImageCodecInfo imageCodecInfo = GetEncoderInfo("image/jpeg");

                SavePath = OSRegistry.GetDiskUserDirectory() + SavePath;

                ImageFile.Save(SavePath, imageCodecInfo, EncoderParams);
            }
            catch (Exception)
            {
                throw new IOException(String.Format(Properties.Resources.IO_WriteFileErrorToSavePath, SavePath));
            }

            return true;
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static bool SaveToDiskRelativePath(Image ImageFile, string SavePath)
        {
            try
            {
                EncoderParameters EncoderParams = new EncoderParameters(2);
                EncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)85);
                EncoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, (long)24);

                ImageCodecInfo imageCodecInfo = GetEncoderInfo("image/jpeg");

                ImageFile.Save(SavePath, imageCodecInfo, EncoderParams);
            }
            catch (Exception)
            {
                throw new IOException(String.Format(Properties.Resources.IO_WriteFileErrorToSavePath, SavePath));
            }

            return true;
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static bool SaveToDiskRelativePathNoCompression(Image ImageFile, string SavePath)
        {
            try
            {
                EncoderParameters EncoderParams = new EncoderParameters(2);
                EncoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                EncoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, (long)24);

                ImageCodecInfo imageCodecInfo = GetEncoderInfo("image/jpeg");

                ImageFile.Save(SavePath, imageCodecInfo, EncoderParams);
            }
            catch (Exception)
            {
                throw new IOException(String.Format(Properties.Resources.IO_WriteFileErrorToSavePath, SavePath));
            }

            return true;
        }

        #endregion

        #region resizes

        /// <summary>
        /// Creates an 800 x 600 image
        /// </summary>
        /// <returns></returns>
        public static Image ResizeTo800x600(Image ImageFile)
        {
            return ResizeImageWithRatio(ImageFile, 800, 600);
        }

        /// <summary>
        /// Creates an image max height or width 480
        /// </summary>
        /// <returns></returns>
        public static Image Resize480x480(Image ImageFile)
        {
            return ResizeImageWithRatio(ImageFile, 480, 480);
        }

        /// <summary>
        /// Creates an image max height or width 480
        /// </summary>
        /// <returns></returns>
        public static Image Resize190x130(Image ImageFile)
        {
            return ResizeImageWithRatio(ImageFile, 190, 130);
        }

        public static Image ResizeTo124x91(Image ImageFile)
        {
            return ResizeImageWithRatio(ImageFile, 124, 91);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Image ResizeTo124x91(byte[] ImageFileBytes)
        {
            try
            {
                Image ImageFile = ByteArrayToImage(ImageFileBytes);

                Image resized = ResizeImageWithRatio(ImageFile, 124, 91);

                return resized;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ResizeTo124x91(string FilePath)
        {
            string FullFilePath = OSRegistry.GetDiskUserDirectory() + FilePath;
            FileStream fs = new FileStream(FullFilePath, FileMode.Open);

            Image Image = Bitmap.FromStream(fs);
            fs.Close();
            fs.Dispose();

            Image ImageSmall = ResizeTo124x91(Image);

            SaveToDisk(ImageSmall, FilePath);

            ImageSmall.Dispose();
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static Image ResizeTo102x102(Image ImageFile)
        {
            try
            {
                Image resized;

                if (ImageFile.Size.Width == 102 && ImageFile.Size.Height == 102)
                {
                    resized = ImageFile;
                }
                else
                {
                    resized = new Bitmap(ImageFile, 102, 102);
                }

                return resized;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static Image ResizeTo640x480(Image ImageFile)
        {
            return ResizeImageWithRatio(ImageFile, 640, 480);
        }

        /// <summary>
        /// Resizes an image to 640 480
        /// </summary>
        /// <returns></returns>
        public static Image ResizeTo640x480(byte[] ImageFileBytes)
        {
            Image image = ByteArrayToImage(ImageFileBytes);

            image = ResizeImageWithRatio(image, 640, 480);

            return image;
        }

        public static Image ResizeImageWithRatio(Image ImageFile, double maxWidth, double maxHeight)
        {
            // Declare variable for the conversion
            float ratio;

            // Get height and width of current image
            int width = (int)ImageFile.Width;
            int height = (int)ImageFile.Height;

            // Ratio and conversion for new size
            if (width > maxWidth)
            {
                ratio = (float)width / (float)maxWidth;
                width = (int)(width / ratio);
                height = (int)(height / ratio);
            }

            // Ratio and conversion for new size
            if (height > maxHeight)
            {
                ratio = (float)height / (float)maxHeight;
                height = (int)(height / ratio);
                width = (int)(width / ratio);
            }

            // Create "blank" image for drawing new image
            Bitmap outImage = new Bitmap(width, height);
            Graphics outGraphics = Graphics.FromImage(outImage);
            SolidBrush sb = new SolidBrush(System.Drawing.Color.White);

            // Fill "blank" with new sized image
            outGraphics.FillRectangle(sb, 0, 0, outImage.Width, outImage.Height);
            outGraphics.DrawImage(ImageFile, 0, 0, outImage.Width, outImage.Height);
            sb.Dispose();
            outGraphics.Dispose();

            return outImage;
        }

        #endregion

        public static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static string ImageToBase64String(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return Convert.ToBase64String(ms.ToArray()); ;
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static string MemberXMLGallery(int MemberID)
        {
            Member member = new Member(MemberID);

            string ThumbPath = "user/" + member.NickName + "/" + "pthmb/";
            string LargePath = "user/" + member.NickName + "/" + "plrge/";

            List<PhotoCollection> collections = member.PhotoCollection;

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.Append(@"<gallery>");

            for (int i = 0; i < collections.Count; i++)
            {
                Photo[] photos = Photo.GetPhotoByPhotoCollectionIDWithJoin(collections[i].PhotoCollectionID);

                if (photos.Length > 0)
                {
                    object[] AlbumParameters = new object[6];
                    AlbumParameters[0] = collections[i].WebPhotoCollectionID;
                    AlbumParameters[1] = ParallelServer.Get(photos[0].ThumbnailResourceFile.FullyQualifiedURL) + photos[0].ThumbnailResourceFile.FullyQualifiedURL;
                    AlbumParameters[2] = ParallelServer.Get(LargePath) + LargePath;
                    AlbumParameters[3] = ParallelServer.Get(ThumbPath) + ThumbPath;
                    AlbumParameters[4] = collections[i].Name;
                    AlbumParameters[5] = collections[i].Description;
                    //AlbumParameters[6] = collections[i].Description;



                    //gallery title
                    sb.AppendFormat(@"  <album id=""{0}"" title=""{4}"" tn=""{1}"" lgPath=""{2}"" tnPath=""{3}"" description=""{5}"" >", AlbumParameters);

                    for (int j = 0; j < photos.Length; j++)
                    {
                        object[] PhotoParameters = new object[2];
                        PhotoParameters[0] = photos[j].PhotoResourceFile.FileName;
                        PhotoParameters[1] = string.Empty;

                        sb.AppendFormat(@"      <img src=""{0}"" caption=""{1}"" />", PhotoParameters);
                    }

                    sb.Append(@"    </album>");
                }
            }

            sb.Append(@"</gallery>");

            return sb.ToString();
        }

        public static string NSpotXMLGallery(int NSpotID)
        {
            List<PhotoCollection> collections = new List<PhotoCollection>();

            List<Photo> DynamicPhotos = Photo.GetNSpotPhotosByNSpotID(NSpotID);

            PhotoCollection NewCollection = new PhotoCollection();
            int MemberID = -1;
            int MemberPhotoCount = 0;

            for (int i = 0; i < DynamicPhotos.Count; i++)
            {
                if (DynamicPhotos[i].MemberID != MemberID)
                {
                    NewCollection = new PhotoCollection();
                    NewCollection.Name = DynamicPhotos[i].Member.NickName;
                    MemberID = DynamicPhotos[i].MemberID;
                    MemberPhotoCount = 0;
                }

                MemberPhotoCount++;

                NewCollection.Photo.Add(DynamicPhotos[i]);

                // if the next photo is a differnt member of this is the last loop then add
                if (i == (DynamicPhotos.Count - 1))
                {
                    NewCollection.Description = "Photos " + MemberPhotoCount.ToString();
                    collections.Add(NewCollection);
                }
                else if (DynamicPhotos[i + 1].MemberID != MemberID)
                {
                    NewCollection.Description = "Photos " + MemberPhotoCount.ToString();
                    collections.Add(NewCollection);
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.Append(@"<gallery>");

            for (int i = 0; i < collections.Count; i++)
            {
                List<Photo> photos = collections[i].Photo;

                if (photos.Count > 0)
                {
                    string NickName = photos[0].Member.NickName;

                    string ThumbPath = "user/" + NickName + "/" + "pthmb/";
                    string LargePath = "user/" + NickName + "/" + "plrge/";

                    object[] AlbumParameters = new object[6];
                    AlbumParameters[0] = collections[i].WebPhotoCollectionID;
                    AlbumParameters[1] = ParallelServer.Get(photos[0].ThumbnailResourceFile.FullyQualifiedURL) + photos[0].ThumbnailResourceFile.FullyQualifiedURL;
                    AlbumParameters[2] = LargePath;
                    AlbumParameters[3] = ThumbPath;
                    AlbumParameters[4] = collections[i].Name;
                    AlbumParameters[5] = collections[i].Description;
                    //AlbumParameters[6] = collections[i].Description;

                    //gallery title
                    sb.AppendFormat(@"  <album id=""{0}"" title=""{4}"" tn=""{1}"" lgPath=""{2}"" tnPath=""{3}"" description=""{5}"" >", AlbumParameters);

                    for (int j = 0; j < photos.Count; j++)
                    {
                        object[] PhotoParameters = new object[2];
                        PhotoParameters[0] = photos[j].PhotoResourceFile.FileName;
                        PhotoParameters[1] = string.Empty;

                        sb.AppendFormat(@"      <img src=""{0}"" caption=""{1}"" />", PhotoParameters);
                    }

                    sb.Append(@"    </album>");
                }
            }

            sb.Append(@"</gallery>");

            return sb.ToString();
        }

        /// <summary>
        /// Saves the photo file to disk
        /// </summary>
        /// <returns></returns>
        public static bool SaveMemberXML(string XML, Member member)
        {
            string SavePath = OSRegistry.GetDiskUserDirectory() + member.NickName + @"\" + "gallery.xml";

            try
            {
                StreamWriter writer = new StreamWriter(SavePath, false);
                writer.Write(XML);
                writer.Flush();
                writer.Close();
            }
            catch (Exception)
            {
                throw new IOException(String.Format(Properties.Resources.IO_WriteFileErrorToSavePath, SavePath));
            }

            return true;
        }



        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
        /// to contain the rotated image without cutting any corners off.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static Bitmap RotateImage(Image image, RotateDirection direction)
        {
            float angle = (direction == RotateDirection.Left) ? -90 : 90;

            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI / 2.0;

            // Why can't C# allow these to be const, or at least readonly
            // *sigh*  I'm starting to talk like Christian Graus :omg:
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;

            // Convert degrees to radians
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;

            // Ensure theta is now [0, 2pi)
            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight; // The newWidth/newHeight expressed as ints

            #region Explaination of the calculations
            /*
			 * The trig involved in calculating the new width and height
			 * is fairly simple; the hard part was remembering that when 
			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
			 * height are switched.
			 * 
			 * When you rotate a rectangle, r, the bounding box surrounding r
			 * contains for right-triangles of empty space.  Each of the 
			 * triangles hypotenuse's are a known length, either the width or
			 * the height of r.  Because we know the length of the hypotenuse
			 * and we have a known angle of rotation, we can use the trig
			 * function identities to find the length of the other two sides.
			 * 
			 * sine = opposite/hypotenuse
			 * cosine = adjacent/hypotenuse
			 * 
			 * solving for the unknown we get
			 * 
			 * opposite = sine * hypotenuse
			 * adjacent = cosine * hypotenuse
			 * 
			 * Another interesting point about these triangles is that there
			 * are only two different triangles. The proof for which is easy
			 * to see, but its been too long since I've written a proof that
			 * I can't explain it well enough to want to publish it.  
			 * 
			 * Just trust me when I say the triangles formed by the lengths 
			 * width are always the same (for a given theta) and the same 
			 * goes for the height of r.
			 * 
			 * Rather than associate the opposite/adjacent sides with the
			 * width and height of the original bitmap, I'll associate them
			 * based on their position.
			 * 
			 * adjacent/oppositeTop will refer to the triangles making up the 
			 * upper right and lower left corners
			 * 
			 * adjacent/oppositeBottom will refer to the triangles making up 
			 * the upper left and lower right corners
			 * 
			 * The names are based on the right side corners, because thats 
			 * where I did my work on paper (the right side).
			 * 
			 * Now if you draw this out, you will see that the width of the 
			 * bounding box is calculated by adding together adjacentTop and 
			 * oppositeBottom while the height is calculate by adding 
			 * together adjacentBottom and oppositeTop.
			 */
            #endregion

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                // This array will be used to pass in the three points that 
                // make up the rotated image
                Point[] points;

                /*
                 * The values of opposite/adjacentTop/Bottom are referring to 
                 * fixed locations instead of in relation to the
                 * rotating image so I need to change which values are used
                 * based on the how much the image is rotating.
                 * 
                 * For each point, one of the coordinates will always be 0, 
                 * nWidth, or nHeight.  This because the Bitmap we are drawing on
                 * is the bounding box for the rotated bitmap.  If both of the 
                 * corrdinates for any of the given points wasn't in the set above
                 * then the bitmap we are drawing on WOULDN'T be the bounding box
                 * as required.
                 */
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] { 
											 new Point( (int) oppositeBottom, 0 ), 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( 0, (int) adjacentBottom )
										 };

                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] { 
											 new Point( nWidth, (int) oppositeTop ),
											 new Point( (int) adjacentTop, nHeight ),
											 new Point( (int) oppositeBottom, 0 )						 
										 };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] { 
											 new Point( (int) adjacentTop, nHeight ), 
											 new Point( 0, (int) adjacentBottom ),
											 new Point( nWidth, (int) oppositeTop )
										 };
                }
                else
                {
                    points = new Point[] { 
											 new Point( 0, (int) adjacentBottom ), 
											 new Point( (int) oppositeBottom, 0 ),
											 new Point( (int) adjacentTop, nHeight )		
										 };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
        }

        #region Omid - Centre Crop for thumbnails

        //example Photo.ScaledCropTo121x91(SomeImage);

        public static Image ScaledCropTo121x91(Image ImageFile)
        {
            try
            {

                Image resized = Photo.ScaleCropImageFromCenter(ImageFile, 121, 91);

                return resized;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static Image CropImageFromCenter(Image ImageFile, double cropWidth, double cropHeight)
        {
            int newWidth = (int)cropWidth;
            int newHeight = (int)cropHeight;
            Bitmap outImage = new Bitmap(newWidth, newHeight);

            try
            {
                using (Graphics outGraphics = Graphics.FromImage(outImage))
                {
                    Rectangle sourceRectangle = new Rectangle(
                        (ImageFile.Width - outImage.Width) / 2,
                        (ImageFile.Height - outImage.Height) / 2,
                        outImage.Width, // width of new rectengle
                        outImage.Height); // // Height of new rectengle

                    Rectangle destRectangle = new Rectangle(0, 0, outImage.Width, outImage.Height);

                    SolidBrush sb = new SolidBrush(System.Drawing.Color.White);
                    outGraphics.FillRectangle(sb, 0, 0, outImage.Width, outImage.Height);
                    outGraphics.DrawImage(ImageFile, destRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    sb.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return outImage;
        }

        public static Image ScaleCropImageFromCenter(Image ImageFile, double cropWidth, double cropHeight)
        {
            int newWidth = (int)cropWidth;
            int newHeight = (int)cropHeight;

            Bitmap outImage = new Bitmap(newWidth, newHeight);

            float ratio = 1.0f;


            // Get height and width of current image
            int width = (int)ImageFile.Width;
            int height = (int)ImageFile.Height;

            // Ratio and conversion for new size
            if (((float)width / (float)cropWidth) <
                ((float)height / (float)cropHeight))
            {
                ratio = (float)width / (float)cropWidth;
            }
            else
            {

                ratio = (float)height / (float)cropHeight;
            }

            Rectangle sourceRectangle = new Rectangle(
                       (int)((ImageFile.Width - outImage.Width * ratio)) / 2,
                       (int)((ImageFile.Height - outImage.Height * ratio)) / 2,
                       (int)(outImage.Width * ratio), // width of new rectengle
                       (int)(outImage.Height * ratio)); // // Height of new rectengle

            Rectangle destRectangle = new Rectangle(0, 0, outImage.Width, outImage.Height);


            try
            {
                using (Graphics outGraphics = Graphics.FromImage(outImage))
                {


                    SolidBrush sb = new SolidBrush(System.Drawing.Color.White);
                    //   outGraphics.FillRectangle(sb, 0, 0, outImage.Width, outImage.Height);
                    outGraphics.DrawImage(ImageFile, destRectangle, sourceRectangle, GraphicsUnit.Pixel);

                    sb.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return outImage;
        }

        #endregion
    }

    public enum RotateDirection { Left, Right }
}