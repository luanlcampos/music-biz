using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2022A6LLC.Models
{
    public class ArtistAddFormViewModel
    {
        public ArtistAddFormViewModel() 
        {
            // loads the today's date to the date picker
            BirthOrStartDate = DateTime.Today;
        }

        [StringLength(100)]
        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "If applicable, artirt's birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Birth date, or start date")]
        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }

        [Display(Name = "Artit's primary genre")]
        public SelectList GenreList { get; set; }

        [StringLength(500)]
        [Display(Name = "URL to artist photo")]
        public string UrlArtist { get; set; }

        [StringLength(10000)]
        [Display(Name = "Artist portrayal")]
        [DataType(DataType.MultilineText)]
        public string Portrayal { get; set; }
    }

    public class ArtistAddViewModel
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string BirthName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }

        public string Genre { get; set; }

        [StringLength(500)]
        public string UrlArtist { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Portrayal { get; set; }

        [StringLength(200)]
        public string Executive { get; set; }
    }
}