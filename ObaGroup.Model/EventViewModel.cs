using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel;

public class EventViewModel
{
        [Key]
        public int id { get; set; }
        
        [ValidateNever]
        public String UserId { get; set; }
        
        public String title { get; set; }

        [Required]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime start { get; set; }

        [ValidateNever]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime end {get; set; }

        [ValidateNever]
        public Boolean allDay { get; set; }
}