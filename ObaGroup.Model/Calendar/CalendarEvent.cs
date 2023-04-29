using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel.Calendar;

public class CalendarEvent
{
    [Required]
    public String Summary { get; set; }
    [ValidateNever]
    public String Location { get; set; }
    [ValidateNever]
    public String Description { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$", 
        ErrorMessage = "The StartDateTime field must be in the format yyyy-MM-ddTHH:mm.")]
    public string StartDateTime { get; set; }
    [ValidateNever]
    public string StartDateTimeZone { get; set; }
    
  
    [Required]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$", 
        ErrorMessage = "The EndDateTime field must be in the format yyyy-MM-ddTHH:mm.")]
    public string EndDateTime { get; set; }
    
    [ValidateNever]
    public string EndDateTimeZone { get; set; }
    
    [Required]
    public List<string> AttendeeEmail { get; set; }
    
    [ValidateNever]
    public int EmailReminderTime { get; set; }
    [ValidateNever]
    public int PopUpReminderTime { get; set; }
    
}