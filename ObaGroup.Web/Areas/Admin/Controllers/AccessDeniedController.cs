using Microsoft.AspNetCore.Mvc;
using ObaGroupModel;

namespace Oba_group2.Areas.Admin.Controllers;

//[ApiController]
public class AccessDeniedController : Controller
{
    [HttpGet("/AccessDenied")]
    public IActionResult AccessDenied()
    {
        ResponseModel responseModel = new ResponseModel();
        responseModel.Message = "User does not have access to view this page";
        responseModel.StatusCode = 401;
              
        ModelState.AddModelError(string.Empty, "User does not have access to view this page");
              
        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return Unauthorized(new {responseModel, Errors =errors });
    }
}

/*[HttpGet("/AccessDenied")]
    public IActionResult Index()
    {
        return Unauthorized("you dont have access");
    }
}
*/