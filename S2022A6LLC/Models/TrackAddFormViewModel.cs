using S2022A6LLC.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Models
{
    public class TrackAddFormViewModel
    {
        [StringLength(100)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Composer Names (comma-separated)")]
        public string Composers { get; set; }

        [Display(Name = "Track Genre")]
        public SelectList GenreList { get; set; }

        [StringLength(100)]
        [Display(Name = "Album name")]
        public string AlbumName { get; set; }

        public int AlbumId { get; set; }

        [Required]
        [Display(Name = "Audio clip")]
        [DataType(DataType.Upload)]
        public string AudioUpload { get; set; }
    }

    public class TrackAddViewModel
    {
        [StringLength(100)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Composer Names (comma-separated)")]
        public string Composers { get; set; }

        [Display(Name = "Track Genre")]
        public string Genre { get; set; }

        public int AlbumId { get; set; }
        public string AlbumName { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }
    }
}