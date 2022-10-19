using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S2022A6LLC.Models
{
    public class TrackBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Composer Names(s)")]
        public string Composers { get; set; }

        [Display(Name = "Track Genre")]
        public string Genre { get; set; }

        public int AlbumId { get; set; }
    }

    public class TrackWithDetailViewModel : TrackBaseViewModel
    {
        [Display(Name = "Albums")]
        public IEnumerable<AlbumBaseViewModel> Albums { get; set; }

        [StringLength(200)]
        public string AudioType { get; set; }
        public byte[] Audio { get; set; }
    }

    public class TrackAudioBaseViewModel
    {
        public int Id { get; set; }
        public string AudioType { get; set; }
        public byte[] Audio { get; set; }
    }
}