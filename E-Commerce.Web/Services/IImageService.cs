namespace E_Commerce.Web.Services
{
    public interface IImageService
    {
        string UploadImage(IFormFile file);
        void DeleteImage(string imageName);
    }

}
