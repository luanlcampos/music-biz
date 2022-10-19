using S2022A6LLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Controllers
{
    [Authorize]
    public class ArtistsController : Controller
    {
        private Manager m = new Manager();
        // GET: Artists
        public ActionResult Index()
        {
            var artists = m.ArtistGetAll();
            return View(artists);
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            var artist = m.ArtistGetByIdWithMediaInfo(id.GetValueOrDefault());
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            // create a new artist add form object
            var form = new ArtistAddFormViewModel();
            // Configure the SelectList for the item-selection element on the HTML Form
            form.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");

            return View(form);
        }

        // POST: Artists/Create
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Executive")]
        public ActionResult Create(ArtistAddViewModel newArtist)
        {
            // check if model is valid
            if (!ModelState.IsValid)
            {
                return View(newArtist);
            }

            // process the input
            var addedArtist = m.ArtistAdd(newArtist);

            if (addedArtist == null)
            {
                return View();
            }

            return RedirectToAction("Details", new { id = addedArtist.Id });
        }

        // GET: Artists/{id}/AddAlbum
        [Authorize(Roles = "Coordinator"), Route("artist/{id}/addalbum")]
        public ActionResult AddAlbum(int? id)
        {
            // find the artist
            var artist = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

            if (artist == null)
            {
                return HttpNotFound();
            }

            // create a form for album
            var form = new AlbumAddFormViewModel();

            // add artist info to the form
            form.ArtistName = artist.Name;

            // add genre to the form
            form.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");

            return View(form);
        }

        // POST: artist/{id}/addalbum
        [Authorize(Roles = "Coordinator"), Route("artist/{id}/addalbum")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddAlbum(AlbumAddViewModel newAlbum)
        {
            if (!ModelState.IsValid)
            {
                return View(newAlbum);
            }

            var addedAlbum = m.AlbumAdd(newAlbum);

            if (addedAlbum == null)
            {
                return View();
            }

            return RedirectToAction("Details", "Albums", new { id = addedAlbum.Id });
        }


        // Method uses attribute routing
        // GET: Artist/5/AddArtistMediaItem
        [Authorize(Roles = "Executive"), Route("Artist/{id}/AddArtistMediaItem")]
        [ValidateInput(false)]
        public ActionResult AddArtistMediaItem(int? id)
        {
            // Attempt to get the matching object
            var o = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create a form
                var form = new ArtistMediaItemAddFormViewModel();
                // Configure its property values
                form.ArtistId = o.Id;
                form.ArtistInfo = o.Name;

                // Pass the object to the view
                return View(form);
            }
        }

        // POST: Artist/5/AddArtistMediaItem
        [Authorize(Roles = "Executive"), Route("Artist/{id}/AddArtistMediaItem")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArtistMediaItem(int? id, ArtistMediaItemAddViewModel newItem)
        {
            // Validate the input
            // Two conditions must be checked
            if (!ModelState.IsValid && id.GetValueOrDefault() == newItem.ArtistId)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ArtistAddArtistMediaItem(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", new { id = addedItem.Id });
            }
        }

    }

}
