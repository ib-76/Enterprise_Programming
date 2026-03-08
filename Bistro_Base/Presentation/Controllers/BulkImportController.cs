using Common.Interfaces;
using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Factories;
using System.IO.Compression;

namespace Presentation.Controllers
{
    public class BulkImportController : Controller
    {
        private ItemsMemoryRepository _memoryRepo;
        private ItemsDbRepository _dbRepo;

        // 
        public BulkImportController(
        [FromKeyedServices("db")] IItemsRepository ordersDbRepository,
        [FromKeyedServices("cache")] IItemsRepository ordersCacheRepository)
        {
            _memoryRepo = (ItemsMemoryRepository)ordersCacheRepository;
            _dbRepo = (ItemsDbRepository)ordersDbRepository;
        }


        [HttpGet]
        public IActionResult Import()
        {
            var items = _memoryRepo.Get();
            return View(items);
        }

        public IActionResult Index() => View();

        
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string json;
            using (var reader = new StreamReader(file.OpenReadStream()))
                json = reader.ReadToEnd();

            var items = ImportItemFactory.Create(json);
            _memoryRepo.Save(items);

            return View(items); // return model
        }



        [HttpPost]
        public IActionResult DownloadZip(string zipName, [FromServices] IWebHostEnvironment env)
        {
            var items = _memoryRepo.Get();
            if (items == null || !items.Any())
                return BadRequest("No items available");

            if (string.IsNullOrWhiteSpace(zipName))
                zipName = "images";

            var defaultImagePath = Path.Combine(env.WebRootPath, "Images", "default.jpg");
            if (!System.IO.File.Exists(defaultImagePath))
                return BadRequest("Default image not found.");

            using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in items)
                {
                    var entryPath = $"item-{item.BistrobaseId}/default.jpg";
                    var entry = archive.CreateEntry(entryPath);

                    using var entryStream = entry.Open();
                    using var fileStream = new FileStream(defaultImagePath, FileMode.Open, FileAccess.Read);

                    fileStream.CopyTo(entryStream);
                }
            }

            memoryStream.Position = 0;

            return File(memoryStream.ToArray(), "application/zip", $"{zipName}.zip");
        }



        [HttpPost]
        public IActionResult UploadZip(
            IFormFile zipfile,[FromServices] IWebHostEnvironment host)
        {
            if (zipfile == null || zipfile.Length == 0) return BadRequest("No file uploaded");

            using var archive = new ZipArchive(zipfile.OpenReadStream());

            foreach (var entry in archive.Entries)
            {
                if (string.IsNullOrEmpty(entry.Name)) continue;

                var parts = entry.FullName.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                var folder = parts[parts.Length - 2]; // item-M-2001
                var fileName = entry.Name;
                var bistroId = folder.Substring(5);   // M-2001 or R-1001

                string? destPath = bistroId.StartsWith("M")
                    ? Path.Combine(host.WebRootPath, "Images", "menuitems", fileName)
                    : bistroId.StartsWith("R")
                        ? Path.Combine(host.WebRootPath, "Images", "restaurants", fileName)
                        : null;

                if (destPath != null)
                {
                    using var fileStream = new FileStream(destPath, FileMode.Create);
                    entry.Open().CopyTo(fileStream);

                    SetImagePath(bistroId, fileName, _memoryRepo);
                }
            }

           _dbRepo.Save(_memoryRepo.Get());
            return RedirectToAction("Import");
        }

        private void SetImagePath(string bistroId, string fileName,
            [FromKeyedServices("cache")] IItemsRepository memoryRepo // method injection
            )
        {
            var items = _memoryRepo.Get();

            if (bistroId.StartsWith("M"))
            {
                var menuItem = items.OfType<MenuItem>()
                                    .FirstOrDefault(x => x.BistrobaseId == bistroId);
                if (menuItem != null)
                    menuItem.ImagePath = $"wwwroot/Images/menuitems/{fileName}";
            }
            else if (bistroId.StartsWith("R"))
            {
                var restaurant = items.OfType<Restaurant>()
                                      .FirstOrDefault(x => x.BistrobaseId == bistroId);
                if (restaurant != null)
                    restaurant.ImagePath = $"wwwroot/Images/restaurants/{fileName}";
            }
        }
    }
}