using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LondonSpider
{
    public class BlobService
    {
        private static CloudBlobContainer _container;
        public BlobService()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("BlobConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            _container = blobClient.GetContainerReference(Environment.GetEnvironmentVariable("BlobContainer"));
            
            
        }
        public async Task UploadBlobAsync(string filePath, string sourceUrl)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference($"{Guid.NewGuid()}.jpeg");
            blob.Metadata.Add("sourceUrl", sourceUrl);

            await blob.UploadFromFileAsync(filePath);

        }
    }
}
