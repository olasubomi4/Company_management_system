using System.ComponentModel.DataAnnotations;

namespace ObaGroupModel.Calendar;

public class GetAnEventParam
{
    [Required] public string eventId { get; set; }
}