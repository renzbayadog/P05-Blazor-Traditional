using System.ComponentModel.DataAnnotations;
using System.Text;

namespace codegen.Helpers
{
    public class ImageDetailVM
    {
        public string UploadModuleType { get; set; } //sales, forecast, delivery, pullout, collections
        public string UploadModuleTypeId { get; set; } //SO-1, F-1, DR-1, PR-1, CO-1

        public int? DocumentypeId { get; set; }
        public string DocumentTypeName { get; set; }

        public string Base64String { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
    }
    public class FtpFileVM
    {
        public int UserId { get; set; }

        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public bool Result { get; set; }
    }
    public class FileDirectoryManager
    {
        public static bool IsFileExist(string urlFile)
        {
            if (!File.Exists(urlFile))
            {
                return false;
            }

            return true;
        }

        public static FtpFileVM GetUserProfile(string fileUrl, string filename, string publicPath)
        {
            var oFtp = new FtpFileVM();


            if (!FileDirectoryManager.IsFileExist(fileUrl))
            {
                oFtp = new FtpFileVM();
            }
            else
            {
                oFtp.FileName = $"{filename}.png";
                oFtp.FilePath = $"{publicPath}?rnd={DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
            }

            return oFtp;
        }

        public static List<string> GetAllFiles(string dirrectoryFile)
        {
            var result = new List<string>();

            try
            {
                DirectoryInfo d = new DirectoryInfo(dirrectoryFile);
                FileInfo[] files = d.GetFiles();

                foreach (FileInfo file in files)
                {
                    result.Add(file.Name);
                }

            }
            catch
            {

                //throw ex;
            }
            return result;
        }

        public static List<FtpFileVM> GetListFiles(CDNFTPConfigurationVM cDNFTPConfigurationVM, ImageDetailVM imageDetailVM)
        {
            var listFtpFileVM = new List<FtpFileVM>();

            //PRIVATE FTP folder location of documents
            var dirrectoryfilePath = $@"{cDNFTPConfigurationVM.DriveDirectory}\{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation.ToLower()}\{imageDetailVM.UploadModuleType.ToLower()}\{imageDetailVM.UploadModuleTypeId}";
            //PUBLIC folder location of documents
            var PublicFTPfilePath = $"{cDNFTPConfigurationVM.PublicHost}/{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation}/{imageDetailVM.UploadModuleType}/{imageDetailVM.UploadModuleTypeId}";

            if (imageDetailVM.UploadModuleType == "sales" || imageDetailVM.UploadModuleType == "forecast")
            {
                dirrectoryfilePath = $@"{cDNFTPConfigurationVM.DriveDirectory}\{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation.ToLower()}\{imageDetailVM.UploadModuleType}\{imageDetailVM.UploadModuleTypeId}\{imageDetailVM.DocumentypeId}";
                PublicFTPfilePath = $"{cDNFTPConfigurationVM.PublicHost}/{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation}/{imageDetailVM.UploadModuleType}/{imageDetailVM.UploadModuleTypeId}/{imageDetailVM.DocumentypeId}";
            }

            var isFolderExist = Directory.Exists(dirrectoryfilePath);

            if (isFolderExist != false)
            {
                var allDocsFiles = GetAllFiles(dirrectoryfilePath);

                foreach (var file in allDocsFiles)
                {
                    var oFtpVM = new FtpFileVM();


                    var splittedFileName = "";

                    var getExtension = file.LastIndexOf('.');
                    if (getExtension != -1)
                    {
                        var extension = file.Substring(getExtension);
                        splittedFileName = file.Replace(extension, "");
                    }

                    var convertedFileName = Convert.FromBase64String(splittedFileName);

                    var filename = Encoding.UTF8.GetString(convertedFileName);

                    oFtpVM.FileName = filename;
                    oFtpVM.FilePath = $"{PublicFTPfilePath}/{file}";

                    listFtpFileVM.Add(oFtpVM);
                }
            }

            return listFtpFileVM;
        }

        //public static bool UploadImageFileFTP(byte[] imagebytes, string imageFile, string username, string password)
        //{
        //    var result = false;

        //    try
        //    {

        //        Image image = null;
        //        //FtpWebRequest ftpWebReq = null;

        //        using (MemoryStream ms = new MemoryStream(imagebytes))
        //        {
        //            image = new Bitmap(Image.FromStream(ms));
        //        }

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            image.Save(ms, ImageFormat.Png);

        //            using (Stream writer = File.Create(imageFile))
        //            {
        //                writer.Write(ms.ToArray(), 0, ms.ToArray().Length);
        //            }

        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}

        public static FtpFileVM UploadFile(ImageDetailVM imageDetailVM, CDNFTPConfigurationVM cDNFTPConfigurationVM, int userId)
        {
            var oFtpFileVM = new FtpFileVM();

            var base64String = imageDetailVM.Base64String.Split(',');
            var imageBytes = Convert.FromBase64String(base64String[1]);

            var DirrectoryfilePath = $@"{cDNFTPConfigurationVM.DriveDirectory}\{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation}\{imageDetailVM.UploadModuleType.ToLower()}";
            var PublicFTPfilePath = $"{cDNFTPConfigurationVM.PublicHost}/{cDNFTPConfigurationVM.FTPFolderVM.DocumentLocation}/{imageDetailVM.UploadModuleType.ToLower()}";

            //create folder SALES/ FORECAST / DELIVERY / PULLOUT / COLLECTIONS
            var isFtpFolderExist = Directory.Exists(DirrectoryfilePath);
            if (isFtpFolderExist == false)
            {
                Directory.CreateDirectory(DirrectoryfilePath);
            }

            //create folder with unique id //SO-1, F01
            var privatePathFolderUniqueId = $@"{DirrectoryfilePath}\{imageDetailVM.UploadModuleTypeId}";
            var publicPathFolderUniqueId = $@"{PublicFTPfilePath}/{imageDetailVM.UploadModuleTypeId}";

            if (imageDetailVM.UploadModuleType == "sales")
            {
                privatePathFolderUniqueId = $@"{DirrectoryfilePath}\{imageDetailVM.UploadModuleTypeId}\{imageDetailVM.DocumentypeId}";
                publicPathFolderUniqueId = $@"{PublicFTPfilePath}/{imageDetailVM.UploadModuleTypeId}/{imageDetailVM.DocumentypeId}";
            }

            var isUniqueIdFolderExist = Directory.Exists(privatePathFolderUniqueId);

            if (isUniqueIdFolderExist == false)
            {
                Directory.CreateDirectory(privatePathFolderUniqueId);
            }

            DirrectoryfilePath = $@"{privatePathFolderUniqueId}";
            PublicFTPfilePath = $@"{publicPathFolderUniqueId}";

            var imgName = "";
            var extension = "";

            var a = imageDetailVM.FileName.LastIndexOf('.');

            if (a != -1)
            {
                extension = imageDetailVM.FileName.Substring(a);
                imgName = imageDetailVM.FileName.Replace(extension, "");
            }

            var byteEncoded = Encoding.UTF8.GetBytes($"{imgName}{extension}");
            var filename = Convert.ToBase64String(byteEncoded);

            var returnFileName = imageDetailVM.FileName;

            var checkingFile = true;
            var count = 0;
            while (checkingFile)
            {
                count++;
                var isFtpFileExist = FileDirectoryManager.IsFileExist($@"{DirrectoryfilePath}\{filename}{extension}");
                if (isFtpFileExist)
                {
                    byteEncoded = Encoding.UTF8.GetBytes($"{imgName}({count}){extension}");
                    filename = Convert.ToBase64String(byteEncoded);
                    returnFileName = $"{imgName}({count}).{extension}";
                }
                else
                {
                    DirrectoryfilePath = $@"{DirrectoryfilePath}\{filename}{extension}";
                    oFtpFileVM.FilePath = $"{PublicFTPfilePath}/{filename}{extension}";
                    oFtpFileVM.FileName = returnFileName;
                    checkingFile = false;
                }
            }

            // upload ftp using byte
            if (UploadFileByByte(imageBytes, DirrectoryfilePath))
            {
                oFtpFileVM.Result = true;
            }

            return oFtpFileVM;
        }

        //public static bool UploadProfile(byte[] imagebytes, string imageFile)
        //{
        //    var result = false;

        //    try
        //    {
        //        Image image = null;

        //        using (MemoryStream ms = new MemoryStream(imagebytes))
        //        {
        //            image = new Bitmap(Image.FromStream(ms));
        //        }

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            image.Save(ms, ImageFormat.Jpeg);

        //            using (Stream writer = File.Create(imageFile))
        //            {
        //                writer.Write(ms.ToArray(), 0, ms.ToArray().Length);
        //            }

        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return result;
        //}

        public static bool UploadFileByByte(byte[] byteData, string dirrectoryFilePath)
        {
            var result = false;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream writer = File.Create(dirrectoryFilePath))
                {
                    writer.Write(byteData, 0, byteData.Length);
                }

                result = true;
            }

            return result;
        }

        public static bool RemoveFile(CDNFTPConfigurationVM CDNFTPConfiguration, ImageDetailVM imageDetailVM)
        {
            var result = false;

            var imgName = "";
            var extension = "";

            var getExtension = imageDetailVM.FileName.LastIndexOf('.');

            if (getExtension != -1)
            {
                extension = imageDetailVM.FileName.Substring(getExtension);
                imgName = imageDetailVM.FileName.Replace(extension, "");
            }

            var byteEncoded = Encoding.UTF8.GetBytes($"{imgName}{extension}");
            var filename = Convert.ToBase64String(byteEncoded);

            var dirrectoryfile = $@"{CDNFTPConfiguration.DriveDirectory}\{CDNFTPConfiguration.FTPFolderVM.DocumentLocation}\{imageDetailVM.UploadModuleType}\{imageDetailVM.UploadModuleTypeId}\{imageDetailVM.DocumentypeId}\{filename}{extension}";

            if (File.Exists(dirrectoryfile))
            {
                // If file found, delete it    
                File.Delete(dirrectoryfile);

                result = true;
            }

            return result;
        }
    }
}
