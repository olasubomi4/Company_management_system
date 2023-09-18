using System.ComponentModel.DataAnnotations;

namespace ObaGroupModel.Calendar;

public class DeleteAnEventParam
{
    [Required] public string eventId { get; set; }
}