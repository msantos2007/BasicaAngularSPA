using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webapibasica.Controllers
{
    public class FileUploadResult
    {
        public long Length { get; set; }
        public string Name { get; set; } = "";       
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UploadFileController : ControllerBase
    {
        public UploadFileController()
        {
            
        }

        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> ImageUpload(List<IFormFile> files)
        {
            IActionResult response = BadRequest("Erro não descrito");
            try
            {
                var result = new List<FileUploadResult>();
                foreach (var file in files)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                response = Ok(result);
            }
            catch(Exception e)
            {
                var erro = e;
                response = BadRequest(erro);
            }

 
            return response;
        }
    }
}