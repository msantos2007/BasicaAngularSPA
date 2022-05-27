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
        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> ImageUpload(List<IFormFile> files, int AlunoId)
        {
                return BadRequest("Nanan");  

                ////https://stackoverflow.com/questions/4988436/mongodb-gridfs-with-c-how-to-store-files-such-as-images
                //  var server = MongoServer.Create("mongodb://localhost:27020");
                //  var database = server.GetDatabase("tesdb");

                //  var fileName = "D:\\Untitled.png";
                //  var newFileName = "D:\\new_Untitled.png";
                //  using (var fs = new FileStream(fileName, FileMode.Open))
                //  {
                //     var gridFsInfo = database.GridFS.Upload(fs, fileName);
                //     var fileId = gridFsInfo.Id;

                //     ObjectId oid= new ObjectId(fileId);
                //     var file = database.GridFS.FindOne(Query.EQ("_id", oid));

                //     using (var stream = file.OpenRead())
                //     {
                //        var bytes = new byte[stream.Length];
                //        stream.Read(bytes, 0, (int)stream.Length);
                //        using(var newFs = new FileStream(newFileName, FileMode.Create))
                //        {
                //          newFs.Write(bytes, 0, bytes.Length);
                //        } 
                //     }
                //  }



            //Half way
            //
            // try
            // {
            //     var result = new List<FileUploadResult>();
            //     foreach (var file in files)
            //     {
            //         //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
            //         //var stream = new FileStream(path, FileMode.Create);
            //         //await file.CopyToAsync(stream);

            //         byte[] m_Bytes;
            //         await using (var memoryStream = new MemoryStream())
            //         {
            //             file.OpenReadStream().CopyTo(memoryStream);
            //             m_Bytes = memoryStream.ToArray();
            //         }

            //         //byte[] m_Bytes = await ReadToEnd(file.OpenReadStream());
            //         result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
            //     }
            //     return Ok(result);
            // }
            // catch(Exception e)
            // {
            //     var erro = e;
            //     return BadRequest(erro);            
            // }
        }
    }
}