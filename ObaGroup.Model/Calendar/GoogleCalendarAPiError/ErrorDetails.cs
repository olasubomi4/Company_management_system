namespace ObaGroupModel.Calendar;

public class ErrorDetails
{
    public int code { get; set; }
    public string message { get; set; }
    public Error[] errors { get; set; }
    public string status { get; set; }
}