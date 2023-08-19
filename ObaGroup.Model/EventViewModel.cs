using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ObaGroupModel.Validation;

namespace ObaGroupModel;

public class EventViewModel
{
    [Key] public int id { get; set; }

    [ValidateNever] public string UserId { get; set; }

    public string title { get; set; }

    [Required]
    [BindProperty]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime start { get; set; }


    [BindProperty]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    [OneOfEndDateAndAllDay("end", "allDay")]
    public DateTime? end { get; set; }


    public bool? allDay { get; set; }
}