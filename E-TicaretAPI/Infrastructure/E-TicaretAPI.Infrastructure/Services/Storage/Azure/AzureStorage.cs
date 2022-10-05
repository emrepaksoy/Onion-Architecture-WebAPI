using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_TicaretAPI.Application.Abstraction.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage :Storage, IAzureStorage
    {
        private readonly BlobServiceClient _blobServiceClient; // ilgili account a bağlanmayı sağlar.
        private  BlobContainerClient _containerClient; // account daki hedef container üzerinde dosya islemleri yapmayı sağlar.
        public AzureStorage(IConfiguration configuration)
        {

            _blobServiceClient = new(configuration["Storage:Azure"]);
            
        }

        
        public async Task DeleteAsync(string containerName, string fileName)
        {
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _containerClient.GetBlobs().Select(x => x.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _containerClient.GetBlobs().Any(x => x.Name == fileName);
           
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName); 
            await _containerClient.CreateIfNotExistsAsync();// ilgili container var mı yok mu ? yoksa oluşturur.
            await _containerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(containerName, file.FileName, HasFile);

                BlobClient blobClient = _containerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            return datas;

        }
    }
}
