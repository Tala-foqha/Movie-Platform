using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.BLL.Services
{
    public  interface IFileService
    {
        public Task<string?> UploadAsync(IFormFile file);
        void Delete(string fileName);
    
}
}
