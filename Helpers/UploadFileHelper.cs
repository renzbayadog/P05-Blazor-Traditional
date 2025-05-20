namespace codegen.Helpers
{
    public class UploadFileHelper
    {
        public static FtpFileVM UploadFile(string model)
        {
            var oFtpFile = new FtpFileVM();
            if (!string.IsNullOrEmpty(model))
            {
                var ImageByte = Convert.FromBase64String(model.Split(',')[1]);
                var filename = Cryptologic.GetMd5Hash($"img_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}");

                var CDNFTPConfiguration = AppHelper.CDNFTPConfiguration;
                CDNFTPConfiguration.FTPFolderVM = AppHelper.CDNFTPFolder;

                var oFileUrl = $@"{CDNFTPConfiguration.DriveDirectory}\{CDNFTPConfiguration.FTPFolderVM.DocumentLocation}\{filename}.png";
                var oFilePath = $"{CDNFTPConfiguration.PublicHost}/{CDNFTPConfiguration.FTPFolderVM.DocumentLocation}/{filename}.png";

                var uploadResult = FileDirectoryManager.UploadFileByByte(ImageByte, oFileUrl);

                if (uploadResult == true)
                {
                    oFtpFile.FileName = filename;
                    oFtpFile.FilePath = $"{oFilePath}?rnd={DateTime.Now.ToString("yyyyMMddHHmmssfff")}";
                }
                else
                {
                    oFtpFile = null;
                }
            }

            return oFtpFile;
        }
    }
}
