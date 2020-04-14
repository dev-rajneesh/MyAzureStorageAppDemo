using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureStorageDemo.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureStorageDemo.Controllers
{
    public class AzureStorageBlobController : Controller
    {
        BlobOperations blobOperations;
        TableOperations tableOperations;
        //public IConfiguration Configuration { get; }

        public AzureStorageBlobController()
        {
            blobOperations = new BlobOperations();
            tableOperations = new TableOperations();
        }

        // GET: AzureStorageBlob
        public async Task<ActionResult> Index()
        {
            var list = new List<ImageEntity>();
            return View(list);
        }

        // GET: AzureStorageBlob/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AzureStorageBlob/Create
        public IActionResult Create()
        {
            var profileId = new Random().Next(); //Generate the Profile Id Randomly
            var imageName = "IMG" + profileId.ToString();

            var model = new ImageEntity(profileId, imageName);

            model.ImageId = new Random().Next();
            model.ImageName = "";
            model.ImageDescription = "";
            model.ImageURL = "";

            return View(model);
        }

        // POST: AzureStorageBlob/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(ImageEntity obj, IFormFile profileFile)
        public async Task<ActionResult> Create(ImageEntity obj)
        {
            CloudBlockBlob profileBlob = null;

            #region Upload File In Blob Storage

            //Step 1: Uploaded File in BLob Storage
            if (obj.ImageFile != null && obj.ImageFile.Length != 0)
            {
                profileBlob = await blobOperations.UploadBlob(obj.ImageFile);
                obj.ImageURL = profileBlob.Uri.ToString();
            }

            #endregion

            #region Save Information in Table Storage
            //Step 2: Save the Infromation in the Table Storage

            //Get the Original File Size
            //obj. = User.Identity.Name; // The Login Email
            obj.RowKey = obj.ImageName;
            obj.PartitionKey = Guid.NewGuid().ToString();
            //Save the File in the Table
            tableOperations.CreateEntity(obj);
            //Ends Here
            #endregion



            return RedirectToAction("Index");
        }

        // GET: AzureStorageBlob/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AzureStorageBlob/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AzureStorageBlob/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AzureStorageBlob/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}