using System.IO;

namespace Mag.Services.FileUploadService
{
    public class LocalFileUploadService : IFileUploadService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public LocalFileUploadService(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UploadFileAsync(IFormFile file,string UserName,string folderName)
        {
            var fileName = Guid.NewGuid().ToString() + "$" + UserName +"$"+ file.FileName;
            var FilePath = Path.Combine(_environment.ContentRootPath, @"wwwroot\Media\"+folderName, fileName);
            var StreamFile = new FileStream(FilePath,FileMode.Create);
            await file.CopyToAsync(StreamFile);
            return fileName;
        }
    }
}
