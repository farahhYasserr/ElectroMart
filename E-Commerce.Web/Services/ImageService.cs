namespace E_Commerce.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string UploadImage(IFormFile file)
        {
            string imageName = "";
            if (file != null)
            {
                var root = _environment.WebRootPath;
                var fileName = Guid.NewGuid().ToString();
                var filePath = Path.Combine(root, @"Images\Products\");
                var ext = Path.GetExtension(file.FileName);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                using (var fileStream = new FileStream(Path.Combine(filePath, fileName + ext), FileMode.CreateNew))
                {
                    file.CopyTo(fileStream);
                }

                imageName = @"Images\Products\" + fileName + ext;
            }

            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                var root = _environment.WebRootPath;
                var oldImage = Path.Combine(root, imageName);
                if (System.IO.File.Exists(oldImage))
                {
                    System.IO.File.Delete(oldImage);
                }
            }
        }
    }

}
