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
        private readonly ItemsMemoryRepository _memoryRepo;
        private readonly ItemsDbRepository _dbRepo;

        // Constructor injects concrete repositories
       public BulkImportController( ItemsMemoryRepository memoryRepo, ItemsDbRepository dbRepo)
      {
          _memoryRepo = memoryRepo;
           _dbRepo = dbRepo;
       }

        [HttpGet]
        public IActionResult Import() => View();

        public IActionResult Index() => View();

        // Method-level injection of keyed service
        [HttpPost]
        public IActionResult Import(
            IFormFile file,
            [FromKeyedServices("cache")] IItemsRepository memoryRepo)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            string json;
            using (var reader = new StreamReader(file.OpenReadStream()))
                json = reader.ReadToEnd();

            var items = ImportItemFactory.Create(json);
            memoryRepo.Save(items); // use the keyed repository here

            return View();
        }

        [HttpPost]
        public IActionResult DownloadZip(
            string zipName,
            [FromKeyedServices("cache")] IItemsRepository memoryRepo)
        {
            var items = memoryRepo.Get();
            if (!items.Any()) return BadRequest("No items available");

            if (string.IsNullOrWhiteSpace(zipName)) zipName = "images";

            var tempPath = Path.Combine(Path.GetTempPath(), "itemsZip");
            if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
            Directory.CreateDirectory(tempPath);

            var defaultImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "default.jpg");

            foreach (var item in items)
            {
                var folder = Path.Combine(tempPath, $"item-{item.BistrobaseId}");
                Directory.CreateDirectory(folder);
                System.IO.File.Copy(defaultImage, Path.Combine(folder, "default.jpg"), true);
            }

            var zipPath = Path.Combine(Path.GetTempPath(), $"{zipName}.zip");
            if (System.IO.File.Exists(zipPath)) System.IO.File.Delete(zipPath);

            ZipFile.CreateFromDirectory(tempPath, zipPath);
            var bytes = System.IO.File.ReadAllBytes(zipPath);

            return File(bytes, "application/zip", $"{zipName}.zip");
        }

        [HttpPost]
        public IActionResult UploadZip(
            IFormFile zipfile,
            [FromKeyedServices("cache")] IItemsRepository memoryRepo,
            [FromKeyedServices("db")] IItemsRepository dbRepo,
            [FromServices] IWebHostEnvironment host)
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

                    SetImagePath(bistroId, fileName, memoryRepo);
                }
            }

            dbRepo.Save(memoryRepo.Get());
            return RedirectToAction("Import");
        }

        private void SetImagePath(string bistroId, string fileName,
            [FromKeyedServices("cache")] IItemsRepository memoryRepo)
        {
            var items = memoryRepo.Get();

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