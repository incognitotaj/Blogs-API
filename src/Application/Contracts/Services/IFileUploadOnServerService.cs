using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Services;

public interface IFileUploadOnServerService
{
    Task<bool> UploadFile(IFormFile file, string baseUrl, string directoryName, string fileName, string fileExtension);
}
