using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S2022A6LLC.Models
{
    public class ArtistMediaItemAddFormViewModel
    {
        public int ArtistId { get; set; }

        [Display(Name = "Artist information")]
        public string ArtistInfo { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Descriptive caption")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Media Item")]
        [DataType(DataType.Upload)]
        public string MediaItemUpload { get; set; }
    }

    public class ArtistMediaItemAddViewModel
    {
        public int ArtistId { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        public HttpPostedFileBase MediaItemUpload { get; set; }

    }
    public class ArtistMediaItemBaseViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Added on date/time")]
        public DateTime Timestamp { get; set; }

        // For the generated identifier
        [Required, StringLength(100)]
        [Display(Name = "Unique identifier")]
        public string StringId { get; set; }

        // Brief descriptive caption
        [Required, StringLength(100)]
        [Display(Name = "Descriptive caption")]
        public string Caption { get; set; }

        [StringLength(200)]
        public string ContentType { get; set; }
    }

    public class ArtistMediaItemContentViewModel
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }

}