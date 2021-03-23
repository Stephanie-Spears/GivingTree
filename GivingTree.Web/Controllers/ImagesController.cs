/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GivingTree.Data.Models;
using GivingTree.Data.Services;

namespace GivingTree.Web.Controllers
{
    public class ImagesController : Controller
    {
		private readonly GivingTreeDbContext _context;

        public ImagesController()
            : this(new GivingTreeDbContext())
        {
        }

        public ImagesController(GivingTreeDbContext context)
        {
            _context = context;
        }


        // /images/product/{TreeSKU}
        // Redirects to the main image for a product
        public ActionResult Product(string id)
        {
            var imageId =
                _context.FruitTrees
                    .Where(x => x.TreeSKU == id)
                    .SelectMany(x => x.Images.Select(img => img.Id))
                    .FirstOrDefault();

            return Image(imageId);
        }

        public ActionResult Image(long? id)
        {
            var image = _context.Images.FirstOrDefault(x => x.Id == id);

            if (!string.IsNullOrWhiteSpace(image.Url))
            {
                return RedirectPermanent(image.Url);
            }

            if (image?.Content == null)
            {
                image = GetPlaceholderImage();
            }

            return File(image.Content, image.ContentType);
        }


        internal static volatile byte[] PlaceholderImageContent;

        private Image GetPlaceholderImage()
        {
            if (PlaceholderImageContent == null)
            {
                var path = Server.MapPath("~/Content/placeholder.jpg");
                PlaceholderImageContent = System.IO.File.ReadAllBytes(path);
            }

            return new Image
            {
                Content = PlaceholderImageContent,
                ContentType = "image/jpeg"
            };
        }
    }
}*/