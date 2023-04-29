using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel.Calendar;

public class PatchEvent
{
    [Required]
    public string EventId { get; set; }
 
    [ValidateNever]
    public String Summary { get; set; }
    
    [ValidateNever]
    public String Location { get; set; }
    
    [ValidateNever]
    public String Description { get; set; }
    

    [ValidateNever]
  //  [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$", 
    //    ErrorMessage = "The StartDateTime field must be in the format yyyy-MM-ddTHH:mm.")]
    public string StartDateTime { get; set; }
    
    [ValidateNever]
    public string StartDateTimeZone { get; set; }
    
    [ValidateNever]
   //[RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$", 
        //   ErrorMessage = "The EndDateTime field must be in the format yyyy-MM-ddTHH:mm. ")]
    public string EndDateTime { get; set; }
    
    [ValidateNever]
    public string EndDateTimeZone { get; set; }
    
    [ValidateNever]
    public List<string>? AttendeeEmailList { get; set; }
    
    [ValidateNever]
    public int EmailReminderTime { get; set; }
    [ValidateNever]
    public int PopUpReminderTime { get; set; }
    
}