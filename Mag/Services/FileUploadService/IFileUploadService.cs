using Microsoft.AspNetCore.Http;

namespace Mag.Services.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file,string UserName, string folderName, string? SubFolderName=null);
    }
}
