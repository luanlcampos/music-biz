using S2022A6LLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Controllers
{
    [Authorize]
    public class TracksController : Controller
    {
        private Manager m = new Manager();
        // GET: Tracks
        public ActionResult Index()
        {
            var tracks = m.TrackGetAllWithDetail();
            return View(tracks);
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            var track = m.TrackGetByIdWithDetail(id.GetValueOrDefault());

            if (track == null)
            {
                return HttpNotFound();
            }

            return View(track);
        }

        // GET: Tracks/Edit/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id)
        {
            // find the track
            var track = m.TrackGetByIdWithDetail(id.GetValueOrDefault());

            if (track == null)
            {
                return HttpNotFound();
            }

            var form = m.mapper.Map<TrackWithDetailViewModel, TrackEditFormViewModel>(track);
            return View(form);
        }

        // POST: Tracks/Edit/5
        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int id, TrackEditViewModel editedTrack)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = editedTrack.Id });
            }

            // check if the the ids are the same
            if (id  != editedTrack.Id)
            {
                return RedirectToAction("Index");
            }

            var obj = m.TrackEdit(editedTrack);
            
            if (editedTrack == null)
            {
                return RedirectToAction("Edit", new { id = editedTrack.Id });
            }

            return RedirectToAction("Details", new { id = editedTrack.Id });
        }

        // GET: Tracks/Delete/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.TrackGetByIdWithDetail(id.GetValueOrDefault());

            if (itemToDelete == null)
            {
                // Don't leak info about the delete attempt
                // Simply redirect
                return RedirectToAction("index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: Tracks/Delete/5
        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id, TrackWithDetailViewModel track)
        {
            var result = m.TrackDelete(id.GetValueOrDefault());

            // "result" will be true or false
            // We probably won't do much with the result, because 
            // we don't want to leak info about the delete attempt

            // In the end, we should just redirect to the list view
            return RedirectToAction("Index");
        }

        [Route("clip/{id}")]
        //[Authorize(Roles = "Staff")]
        public ActionResult SampleDetails(int? id)
        {
            var audio = m.TrackAudioGetById(id.GetValueOrDefault());
            if (audio == null || audio.AudioType == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(audio.Audio, audio.AudioType);
            }
        }

    }
}
