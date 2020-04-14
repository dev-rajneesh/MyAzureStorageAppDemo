using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureStorageDemo.Models
{
    public class ImageEntity : TableEntity
    {
        public ImageEntity(int profId, string imageName)
        {
            this.RowKey = profId.ToString();
            this.PartitionKey = imageName;
        }

        public ImageEntity()
        {

        }

        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string ImageURL { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
