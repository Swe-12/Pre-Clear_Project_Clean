using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PreClear.Api.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder);
        Task<Stream> DownloadFileAsync(string key);
        Task DeleteFileAsync(string key);
    }
}
