using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Shared;

namespace Chat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {

        private readonly IWebHostEnvironment env;

        public FileUploadController    (IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost]
        public void Post(UploadedFile uploadedFile)
        {
           
            var path = $"{env.WebRootPath}\\{uploadedFile.FileName}";
            var fs = System.IO.File.Create(path);
            fs.Write(uploadedFile.FileContent, 0,    uploadedFile.FileContent.Length);
            fs.Close();
        }
    }
}
