using Microsoft.AspNetCore.Mvc;
using ObaGroupModel;
using ObaGroupUtility;

namespace Oba_group2.Areas.Admin.Controllers;

//[ApiController]
public class AccessDeniedController : Controller
{
    [HttpGet("post/{pid}")]
    public IActionResult AccessDeniedView(string pid)
    {
        return File("~/post/[pid]/index.html", "text/html");
    }

    [HttpGet(Constants.Access_Denied_Endpoint)]
    public IActionResult AccessDenied()
    {
        var responseModel = new ResponseModel();
        responseModel.Message = "User does not have access to view this page";
        responseModel.StatusCode = 401;

        ModelState.AddModelError(string.Empty, "User does not have access to view this page");

        var errors = ModelState.Values.SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return Unauthorized(new { responseModel, Errors = errors });
    }
}