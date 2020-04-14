using System;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AzureStorageDemo.Models
{
    public class BlobOperations
    {
        private static CloudBlobContainer profileBlobContainer;

        /// <summary>
        /// Initialize BLOB and Queue Here
        /// </summary>
        public BlobOperations()
        {
            //var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AppDbConnection"));
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=myazureappstorage;AccountKey=2/UzyftxfS7FOCjroHI2eDtCfutWurz0lRA08jpkJepZ+LSr7/dY4XijAzpxPiWxxwNz/fGStGyAq3+uwMw4TQ==;EndpointSuffix=core.windows.net");

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get the blob container reference.
            profileBlobContainer = blobClient.GetContainerReference("imagesfromapp1");
            //Create Blob Container if not exist
            profileBlobContainer.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Method to Upload the BLOB
        /// </summary>
        /// <param name="profileFile"></param>
        /// <returns></returns>
        public async Task<CloudBlockBlob> UploadBlob(IFormFile profileFile)
        {
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(profileFile.FileName);

            // GET a blob reference.
            CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);

            // Uploading a local file and Create the blob.
            using (var fs = profileFile.OpenReadStream())
            {
                await profileBlob.UploadFromStreamAsync(fs);
            }
            return profileBlob;
        }
    }
}
