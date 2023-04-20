using Microsoft.AspNetCore.Mvc;

namespace ObaGroupModel;

public class ConfirmEmail
{
    [TempData]
    public string StatusMessage { get; set; }
}