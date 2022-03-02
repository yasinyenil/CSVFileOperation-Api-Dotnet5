using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileApi.Operations
{
    public interface IFileOperations
    {
        public bool CheckIfCsvFile(IFormFile file);
        public Task<string> WriteFile(IFormFile file);
    }
}
