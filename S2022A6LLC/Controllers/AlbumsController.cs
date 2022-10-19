using S2022A6LLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Controllers
{
    public class AlbumsController : Controller
    {
        private Manager m = new Manager();
        // GET: Albums
        public ActionResult Index()
        {
            var albums = m.AlbumGetAll();
            return View(albums);
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            var album = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());

            if (album == null)
            {
                return HttpNotFound();
            }
            
            return View(album);
        }

        // Add album is accessible from within the Artist Detail page
        // Path: /Artist/{id}/AddAlbum

        // GET: album/{id}/addtrack
        [Authorize(Roles = "Clerk"), Route("Album/{id}/AddTrack")]
        public ActionResult AddTrack(int? id)
        {
            // find the associated album
            var album = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());

            if (album == null)
            {
                return HttpNotFound();
            }

            // create a form for track
            var form = new TrackAddFormViewModel();

            // populate album id and name in the track
            form.AlbumId = album.Id;
            form.AlbumName = album.Name;

            // configure the genre list
            form.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");

            return View(form);
        }

        // POST: album/{id}/addtrack
        [Authorize(Roles = "Clerk"), Route("Album/{id}/AddTrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAddViewModel newTrack)
        {
            if (!ModelState.IsValid)
            {
                return View(newTrack);
            }

            // add track
            var addedTrack = m.TrackAdd(newTrack);

            if (addedTrack == null)
            {
                return View(newTrack);
            }

            return RedirectToAction("Details", "Tracks", new { id = addedTrack.Id });
        }
    }
}
