namespace codegen.Helpers
{
    public class CDNFTPConfigurationVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DriveDirectory { get; set; }
        public string PublicHost { get; set; }
        public FTPFolderVM FTPFolderVM { get; set; }
    }

    public class FTPFolderVM
    {
        public string DocumentLocation { get; set; }
        public string ProfilePicLocation { get; set; }
        public string SalesLocation { get; set; }
        public string ForecastLocation { get; set; }
        public string DeliveryLocation { get; set; }
        public string PulloutLocation { get; set; }
    }

    public class AppHelper
    {
        public static IConfiguration _config;
        //public static IHttpContextAccessor _httpContextAccessor;

        public AppHelper(IConfiguration config)
        {
            //_httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        
        public static CDNFTPConfigurationVM CDNFTPConfiguration
        {
            get
            {
                return new CDNFTPConfigurationVM
                {
                    Username = _config["FtpClient:UserName"],
                    Password = _config["FtpClient:Password"],
                    DriveDirectory = _config["FtpClient:DriveDirectory"],
                    PublicHost = _config["FtpClient:PublicHost"]
                };
            }
        }

        public static FTPFolderVM CDNFTPFolder
        {
            get
            {
                return new FTPFolderVM
                {
                    DocumentLocation = _config["FtpClient:OutputDirectoryDocuments"]
                };
            }
        }
    }
}
