using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S2022A6LLC.Models
{
    public class AlbumBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display (Name = "Album Name")]
        public string Name { get; set; }

        [Display (Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        // Get from Apple iTunes Preview, Amazon, or Wikipedia
        [Required, StringLength(512)]
        [Display (Name = " Album cover art")]
        public string UrlAlbum { get; set; }

        [Required]
        [Display (Name = "Album's primary genre")]
        public string Genre { get; set; }
    }

    public class AlbumWithDetailViewModel : AlbumBaseViewModel
    {
        [DataType(DataType.MultilineText)]
        [Display(Name = "Album background")]
        [StringLength(10000)]
        public string Background { get; set; }
    }
}