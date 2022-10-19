using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Models
{
    public class AlbumAddFormViewModel
    {
        public AlbumAddFormViewModel()
        {
            ReleaseDate = DateTime.Today;
        }
        [Required]
        [StringLength(100)]
        [Display(Name = "Album name")]
        public string Name { get; set; }

        //[StringLength(100)]
        //public string Coordinator { get; set; }

        
        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [StringLength(500)]
        [Display(Name = "URL to album image (cover art)")]
        public string UrlAlbum { get; set; }

        [StringLength(100)]
        [Display(Name = "Album's Primary Genre")]
        public SelectList GenreList { get; set; }

        [StringLength(10000)]
        [Display(Name = "Album background")]
        [DataType(DataType.MultilineText)]
        public string Background { get; set; }

        public string ArtistName { get; set; }
    }

    public class AlbumAddViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Coordinator { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }


        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; }

        [StringLength(500)]
        public string UrlAlbum { get; set; }

        public string ArtistName { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Background { get; set; }
    }
}