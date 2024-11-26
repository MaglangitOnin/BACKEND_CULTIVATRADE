using Microsoft.Extensions.Configuration;

namespace FileUpload.Api.Logics
{
    public class CheckFileType
    {
        private readonly IConfiguration _configuration;

        public CheckFileType(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string? GetFileTypeDirectory(string fileExtension)
        {
            //get extensions from appsettings.json
            var audioExtensions = _configuration.GetSection("FileExtensions:Audios").Get<string[]>();
            var documentExtensions = _configuration.GetSection("FileExtensions:Documents").Get<string[]>();
            var videoExtensions = _configuration.GetSection("FileExtensions:Videos").Get<string[]>();
            var imageExtensions = _configuration.GetSection("FileExtensions:Images").Get<string[]>();

            //get folder file types from appsettings.json
            var audioFileType = _configuration.GetValue<string>("FileTypes:Audio");
            var documentFileType = _configuration.GetValue<string>("FileTypes:Document");
            var videoFileType = _configuration.GetValue<string>("FileTypes:Video");
            var imageFileType = _configuration.GetValue<string>("FileTypes:Image");

            if (audioExtensions.Contains(fileExtension))
            {
                return audioFileType;
            }
            else if (documentExtensions.Contains(fileExtension))
            {
                return documentFileType;
            }
            else if (videoExtensions.Contains(fileExtension))
            {
                return videoFileType;
            }
            else if (imageExtensions.Contains(fileExtension))
            {
                return imageFileType;
            }
            else
            {
                return null;
            }
        }

        public string? GetContentType (string fileExtension)
        {
            //get extensions from appsettings.json
            var audioExtensions = _configuration.GetSection("FileExtensions:Audios").Get<string[]>();
            var documentExtensions = _configuration.GetSection("FileExtensions:Documents").Get<string[]>();
            var videoExtensions = _configuration.GetSection("FileExtensions:Videos").Get<string[]>();
            var imageExtensions = _configuration.GetSection("FileExtensions:Images").Get<string[]>();

            var audio = _configuration.GetValue<string>("ContentTypes:Audio");
            var application = _configuration.GetValue<string>("ContentTypes:Application");
            var video = _configuration.GetValue<string>("ContentTypes:Video");
            var image = _configuration.GetValue<string>("ContentTypes:Image");
            var defaultVal = _configuration.GetValue<string>("ContentTypes:Default");

            if (audioExtensions.Contains(fileExtension))
            {
                return $"{audio}{fileExtension.Split(".")[1]}";
            }
            else if (documentExtensions.Contains(fileExtension))
            {
                return $"{application}{fileExtension.Split(".")[1]}";
            }
            else if (videoExtensions.Contains(fileExtension))
            {
                return $"{video}{fileExtension.Split(".")[1]}";
            }
            else if (imageExtensions.Contains(fileExtension))
            {
                return $"{image}{fileExtension.Split(".")[1]}";
            }
            else
            {
                return defaultVal;
            }
        }
    }
}
