using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObaGroupModel.Calendar;

public class ListCalendarEventParams
{
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$",
        ErrorMessage = "The EventMaxTimeRange field must be in the format yyyy-MM-ddTHH:mm.")]
    public string EventMaxTimeRange { get; set; }

    [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$",
        ErrorMessage = "The EventMinTimeRange field must be in the format yyyy-MM-ddTHH:mm.")]
    public string EventMinTimeRange { get; set; }

    [ValidateNever] public string timeZone { get; set; }
}