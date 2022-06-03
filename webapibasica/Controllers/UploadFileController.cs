using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

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
            //return BadRequest("Nanan");  


            ////Using new driver
            ////http://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database;
            GridFSBucket bucket;
            database = client.GetDatabase("tesdb");
            bucket = new GridFSBucket(database);

            //Upload
            byte[] m_Bytes;
            await using (var memoryStream = new MemoryStream())
            {
                files[0].OpenReadStream().CopyTo(memoryStream);
                m_Bytes = memoryStream.ToArray();
            }

            var id = await bucket.UploadFromBytesAsync(files[0].FileName, m_Bytes);

            ////Download
            //var bytes = await bucket.DownloadAsBytesAsync(id);

            return Ok(id);


            // Using legacy
            ////https://stackoverflow.com/questions/4988436/mongodb-gridfs-with-c-how-to-store-files-such-as-images


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

        [HttpGet]
        [Route("ImageList")]
        public async Task<IActionResult> ImageList()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database;
            IMongoCollection<BsonDocument> collection;
            database = client.GetDatabase("tesdb");
            collection = database.GetCollection<BsonDocument>("fs.files");

            var resp = await collection.Find(_ => true).ToListAsync();

            return Ok(resp);
        }

    }
}