using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S2022A6LLC.Models
{
    public class ArtistBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        // Get from Apple iTunes Preview, Amazon, or Wikipedia
        [Required, StringLength(512)]
        [Display(Name = "Artist photo")]
        public string UrlArtist { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        // For an individual, a birth name
        [StringLength(100)]
        [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }
        
        [Display(Name = "Birth date, or start date")]
        [DataType(DataType.Date)]
        public DateTime BirthOrStartDate { get; set; }

        [Required]
        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }
    }

    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {

        [DataType(DataType.MultilineText)]
        [Display(Name = "Artist portrayil")]
        [StringLength(10000)]
        public string Portrayal { get; set; }

    }

    public class ArtistWithMediaInfo : ArtistWithDetailViewModel
    {
        public ArtistWithMediaInfo()
        {
            ArtistMediaItems = new List<ArtistMediaItemBaseViewModel>();
        }
        public IEnumerable<ArtistMediaItemBaseViewModel> ArtistMediaItems { get; set; }
    }
}

