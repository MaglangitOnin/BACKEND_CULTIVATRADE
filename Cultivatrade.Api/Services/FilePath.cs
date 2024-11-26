using Cultivatrade.Api.DatabaseConnection;
using Microsoft.EntityFrameworkCore;

namespace Cultivatrade.Api.Services
{
    public class FilePath
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FilePath(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string UserImage(string fileName)
        {
            return $"{_webHostEnvironment.WebRootPath}/Images/Profiles/{fileName}";
        }

        public string ProductImage(string fileName)
        {
            return $"{_webHostEnvironment.WebRootPath}/Images/Products/{fileName}";
        }
        
        public string BusinessPermitImage(string fileName)
        {
            return $"{_webHostEnvironment.WebRootPath}/Images/BusinessPermits/{fileName}";
        }
        
        public string SanitaryImage(string fileName)
        {
            return $"{_webHostEnvironment.WebRootPath}/Images/SanitaryPermits/{fileName}";
        }
    }
}
