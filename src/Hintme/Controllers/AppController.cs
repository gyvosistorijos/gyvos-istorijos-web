using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hintme.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Hintme.Controllers
{
    public class AppController : Controller
    {
        private readonly IHintRepository _repository;
        private IConfigurationRoot _config;

        public AppController(IHintRepository repository, IConfigurationRoot config)
        {
            _config = config;
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(HintDto hint)
        {
            string url = "";

            if (hint.Picture != null)
            {
                url  = await SavePictureAsync(hint.Picture);               
            }

            _repository.SaveHint(new Hint
            {
                Header = hint.Header,
                Text = hint.Text,
                Longitude = hint.Longitude,
                Latitude = hint.Latitude,
                Url = url
            });

            return RedirectToAction("Stories", new { id = hint.Header.Replace(' ','.')});
        }

        private async Task<string> SavePictureAsync(IFormFile picture)
        {
            string fileName = Guid.NewGuid() + "." + picture.FileName.Split('.').Last();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_config["AzureBlobStorage:ConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_config["AzureBlobStorage:Container"]);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(picture.OpenReadStream());

            return _config["AzureBlobStorage:Url"] + _config["AzureBlobStorage:Container"] + "/"+ fileName;
        }


        public ActionResult Stories(string id)
        {
            id = id.Replace('.', ' ');
            var stories =
               _repository.GetHints().Where(x => (x.Header ?? "").ToLowerInvariant() == id.ToLowerInvariant());

            return View(stories);
        }
    }
}
