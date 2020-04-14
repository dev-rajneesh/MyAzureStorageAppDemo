using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace AzureStorageDemo.Models
{
    public interface ITableOperations
    {
        void CreateEntity(ImageEntity entity);
        Task<List<ImageEntity>> GetEntities(string filter);
        Task<ImageEntity> GetEntity(string partitionKey, string rowKey);
    }

    public class TableOperations : ITableOperations
    {
        //Represent the Cloud Storage Account, this will be instantiated
        //based on the appsettings
        CloudStorageAccount storageAccount;

        //The Table Service Client object used to
        //perform operations on the Table
        CloudTableClient tableClient;

        /// <summary>
        /// COnstructor to Create Storage Account and the Table
        /// </summary>
        public TableOperations()
        {
            //Get the Storage Account from the conenction string
            storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=myazureappstorage;AccountKey=2/UzyftxfS7FOCjroHI2eDtCfutWurz0lRA08jpkJepZ+LSr7/dY4XijAzpxPiWxxwNz/fGStGyAq3+uwMw4TQ==;EndpointSuffix=core.windows.net");

            //Create a Table Client Object
            tableClient = storageAccount.CreateCloudTableClient();

            //Create Table if it does not exist
            CloudTable table = tableClient.GetTableReference("ImageEntityTable1");
            table.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Method to Create Entity
        /// </summary>
        /// <param name="entity"></param>
        public void CreateEntity(ImageEntity entity)
        {
            CloudTable table = tableClient.GetTableReference("ImageEntityTable1");

            //Create a TableOperation object used to insert Entity into Table
            TableOperation insertOperation = TableOperation.Insert(entity);

            //Execute an Insert Operation
            table.ExecuteAsync(insertOperation);
        }

        /// <summary>
        /// Method to retrieve entities based on the PartitionKey
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<ImageEntity>> GetEntities(string filter)
        {
            List<ImageEntity> Images = new List<ImageEntity>();
            CloudTable table = tableClient.GetTableReference("ImageEntityTable1");

            TableQuery<ImageEntity> query = new TableQuery<ImageEntity>()
            .Where(TableQuery.GenerateFilterCondition("ImageName", QueryComparisons.Equal, filter));
            //TableQuery<ImageEntity> query = new TableQuery<ImageEntity>();


            //foreach (var item in await table.ExecuteAsync(query))
            //{
            //    Images.Add(new ImageEntity()
            //    {
            //        ImageId = item.ImageId,
            //        ImageName = item.ImageName,
            //        ImageDescription = item.ImageDescription
            //    });
            //}

            return Images;
        }

        /// <summary>
        /// Method to get specific entity based on the Row Key and the Partition key
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<ImageEntity> GetEntity(string partitionKey, string rowKey)
        {
            ImageEntity entity = null;

            CloudTable table = tableClient.GetTableReference("ImageEntityTable1");

            TableOperation tableOperation = TableOperation.Retrieve<ImageEntity>(partitionKey, rowKey);
            //entity = await (table.ExecuteAsync(tableOperation).Result as ImageEntity);

            return entity;
        }

    }
}
