using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MimeTypes;

namespace S2022A6LLC.Controllers
{
    [Authorize]
    public class ArtistMediaItemController : Controller
    {
        private Manager m = new Manager();
        
        // GET: ArtistMediaItem
        public ActionResult Index()
        {
            return View("Index", "Home");
        }

        // GET: ArtistMediaItem/5
        [Route("ArtistMediaItem/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            // Attempt to get the matching object
            var o = m.ArtistMediaItemGetById(stringId);

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Return a file content result
                // Set the Content-Type header, and return the photo bytes
                return File(o.Content, o.ContentType);
            }
        }

        // GET: ArtistMediaItem/5/Download
        // Dedicated photo DOWNLOAD method, uses attribute routing
        [Route("ArtistMediaItem/{stringId}/Download")]
        public ActionResult DetailsDownload(string stringId = "")
        {
            // Attempt to get the matching object
            var o = m.ArtistMediaItemGetById(stringId);

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Get file extension, assumes the web server is Microsoft IIS based
                // Must get the extension from the Registry (which is a key-value storage structure for configuration settings, for the Windows operating system and apps that opt to use the Registry)

                // Working variables
                // get the extension using the MymeTypeMap package 
                // More info: https://github.com/samuelneff/MimeTypeMap/blob/master/MimeTypeMap.cs
                // NOTE: An alternative solution would consist on creating a Dictionary with the tested 
                // content-types(pdf, docx, xls, etc), but the solution placed below is more broad 
                // and scalable to many types of documents
                string extension = MimeTypeMap.GetExtension(o.ContentType);

                // Create a new Content-Disposition header
                var cd = new System.Net.Mime.ContentDisposition
                {
                    // Assemble the file name + extension
                    FileName = $"{stringId}{extension}",
                    // Force the media item to be saved (not viewed)
                    Inline = false
                };
                // Add the header to the response
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(o.Content, o.ContentType);
            }
        }
    }
}
