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

        private readonly Data.BasicaContext _context;
        private readonly Data.MongoContext _contextMongo;

        public UploadFileController(Data.BasicaContext context, Data.MongoContext contextMongo)
        {
            _context = context;
            _contextMongo = contextMongo;
        }

        [HttpPost]
        [Route("ImageUpload")]
        public async Task<IActionResult> ImageUpload(List<IFormFile> files, int AlunoId)
        {
            // //CQRS Pending
            // MongoClient client = new MongoClient("mongodb://localhost:27017");
            // IMongoDatabase database;
            // GridFSBucket bucket;
            // database = client.GetDatabase("tesdb");
            // bucket = new GridFSBucket(database);

            //Upload
            byte[] m_Bytes;
            await using (var memoryStream = new MemoryStream())
            {
                files[0].OpenReadStream().CopyTo(memoryStream);
                m_Bytes = memoryStream.ToArray();
            }

            ObjectId id = await _contextMongo._bucket.UploadFromBytesAsync(files[0].FileName, m_Bytes);

            //CQRS Pending
            _context.AlunoImagemDbSet.Where(r => r.AlunoId == AlunoId).ToList().ForEach(r => r.Ativo = false);
            _context.AlunoImagemDbSet.Add(new Entities.AlunoImagem
            {
                AlunoId = AlunoId,
                Ativo = true,
                ImagemId = id.ToString()
            });

            return Ok(id);
        }

        [HttpPost]
        [Route("ImageList")]
        public async Task<IActionResult> ImageList(List<Models.AlunoImagemViewModel> listaImagens)
        {
            // //CQRS Pending
            // //Download
            // MongoClient client = new MongoClient("mongodb://localhost:27017");
            // IMongoDatabase database;
            // IMongoCollection<BsonDocument> collection;
            // GridFSBucket bucket;
            // database = client.GetDatabase("tesdb");
            // bucket = new GridFSBucket(database);

            IMongoCollection<BsonDocument> collection;

            collection = _contextMongo._database.GetCollection<BsonDocument>("fs.files");

            var resp = await collection.Find(_ => true).ToListAsync();

            Task<byte[]> t;
            Byte[] bytes;
            foreach (var item in listaImagens)
            {
                ObjectId obj = new ObjectId(item.ImagemId);
                t = _contextMongo._bucket.DownloadAsBytesAsync(obj);
                Task.WaitAll(t);
                bytes = t.Result;

                item.ImagemByte = bytes;
            }

            return Ok(listaImagens);
        }
    }
}



// //MongoDB Standalone não suporta transação (o que é ReplicaSet?)
// ObjectId id = new ObjectId("629e5549a7046f0326f5bbb3");
// using (var session = await _mongocontext._client.StartSessionAsync())
// {
//     session.StartTransaction();
//     try
//     {
//         id = await _mongocontext._bucket.UploadFromBytesAsync(files[0].FileName, m_Bytes);

//         try
//         {
//             //CQRS Pending
//             _context.AlunoImagemDbSet.Where(r => r.AlunoId == AlunoId).ToList().ForEach(r => r.Ativo = false);
//             _context.AlunoImagemDbSet.Add(new Entities.AlunoImagem
//             {
//                 AlunoId = AlunoId,
//                 Ativo = true,
//                 ImagemId = id.ToString()
//             });

//             await session.CommitTransactionAsync();
//             _context.SaveChanges();
//         }
//         catch (Exception)
//         {
//             await session.AbortTransactionAsync();
//         }
//     }
//     catch (Exception)
//     {
//         await session.AbortTransactionAsync();
//     }

// }

// [HttpPost]
// [Route("ImageUpload")]
// public async Task<IActionResult> ImageUpload(List<IFormFile> files, int AlunoId)
// {
//     ////Using new driver
//     ////http://mongodb.github.io/mongo-csharp-driver/2.2/reference/gridfs/uploadingfiles/
//     MongoClient client = new MongoClient("mongodb://localhost:27017");
//     IMongoDatabase database;
//     GridFSBucket bucket;
//     database = client.GetDatabase("tesdb");
//     bucket = new GridFSBucket(database);

//     //Upload
//     byte[] m_Bytes;
//     await using (var memoryStream = new MemoryStream())
//     {
//         files[0].OpenReadStream().CopyTo(memoryStream);
//         m_Bytes = memoryStream.ToArray();
//     }

//     var id = await bucket.UploadFromBytesAsync(files[0].FileName, m_Bytes);

//     //CQRS Pending
//     _context.AlunoImagemDbSet.Where(r => r.AlunoId == AlunoId).ToList().ForEach(r => r.Ativo = false);
//     _context.AlunoImagemDbSet.Add(new Entities.AlunoImagem
//     {
//         AlunoId = AlunoId,
//         Ativo = true,
//         ImagemId = id
//     });
//     _context.SaveChanges();


//     return Ok(id);


//     // Using legacy
//     ////https://stackoverflow.com/questions/4988436/mongodb-gridfs-with-c-how-to-store-files-such-as-images


//     //Half way
//     //
//     // try
//     // {
//     //     var result = new List<FileUploadResult>();
//     //     foreach (var file in files)
//     //     {
//     //         //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", file.FileName);
//     //         //var stream = new FileStream(path, FileMode.Create);
//     //         //await file.CopyToAsync(stream);

//     //         byte[] m_Bytes;
//     //         await using (var memoryStream = new MemoryStream())
//     //         {
//     //             file.OpenReadStream().CopyTo(memoryStream);
//     //             m_Bytes = memoryStream.ToArray();
//     //         }

//     //         //byte[] m_Bytes = await ReadToEnd(file.OpenReadStream());
//     //         result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
//     //     }
//     //     return Ok(result);
//     // }
//     // catch(Exception e)
//     // {
//     //     var erro = e;
//     //     return BadRequest(erro);            
//     // }
// }






//     [HttpGet]
//     [Route("ImageList")]
//     public async Task<IActionResult> ImageList()
//     {
//         MongoClient client = new MongoClient("mongodb://localhost:27017");
//         IMongoDatabase database;
//         IMongoCollection<BsonDocument> collection;
//         GridFSBucket bucket;
//         database = client.GetDatabase("tesdb");
//         bucket = new GridFSBucket(database);

//         collection = database.GetCollection<BsonDocument>("fs.files");

//         var resp = await collection.Find(_ => true).ToListAsync();

//         List<byte[]> image = new List<byte[]>();

//         var t = bucket.DownloadAsBytesByNameAsync("moedor_2.png");
//         Task.WaitAll(t);
//         var bytes = t.Result;

//         image.Add(bytes);

//         t = bucket.DownloadAsBytesByNameAsync("moedor_3.png");
//         Task.WaitAll(t);
//         bytes = t.Result;

//         image.Add(bytes);

//         return Ok(image);
//     }

//     //         [HttpGet]
//     //         [Route("ImageGet")]
//     //         public async Task<IActionResult> ImageGet(int order)
//     //         {
//     //             MongoClient client = new MongoClient("mongodb://localhost:27017");
//     //             IMongoDatabase database;
//     //             IMongoCollection<BsonDocument> collection;
//     //             database = client.GetDatabase("tesdb");
//     //             collection = database.GetCollection<BsonDocument>("fs.files");

//     //             BsonDocument resp;
//     //             if (order == 1)
//     //             {
//     //                 resp = await collection.Find(_ => true).FirstOrDefaultAsync();
//     //             }
//     //             else
//     //             {
//     // resp = await collection.Find(_ => true).SortByDescending(_ => ((ObjectId)_))
//     //             }


//     //             return Ok(resp);
//     //         }
