using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S2022A6LLC.Models
{
    public class TrackEditViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Clip")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase AudioUpload { get; set; }
    }

    public class TrackEditFormViewModel : TrackEditViewModel
    {
        public string Name { get; set; }

        [Required]
        public HttpPostedFileBase AudioUpload { get; set; }

    }
}