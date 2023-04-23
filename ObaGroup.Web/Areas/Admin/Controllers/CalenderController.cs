using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupUtility;


namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
public class CalenderController: Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private  ApplicationUser applicationUser;

    public CalenderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public ActionResult Index()
    {
        EventViewModel eventViewModel = new EventViewModel();
        var name = User.Identity.Name;
     
        return View(eventViewModel);
    }
    
   
    public IActionResult Upsert(int? id)
    {
        EventViewModel eventViewModel = new EventViewModel();
       
        if (id == null || id == 0)
        {
            return View(eventViewModel);
        }
        else
        {
            eventViewModel = _unitOfWork.eventViewModel.GetFirstOrDefault(u => u.id == id);
            return View(eventViewModel);
        }
    }
    
    [HttpGet]
    [Route("Admin/Dashboard/Calendar/Upsert")]
    public IActionResult GetById(int? id)
    {
         EventViewModel calendarEvents = _unitOfWork.eventViewModel.GetFirstOrDefault(x=> x.id==id);
        return Ok(calendarEvents);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Admin/Dashboard/Calendar/Upsert")]
    public IActionResult Upsert([FromForm] EventViewModel eventViewModel)
    {
        ResponseModel responseModel = new ResponseModel();
        string message;
        int statusCode;
        Boolean isCreate = false;
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }
        
        eventViewModel.UserId=_unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Email==User.Identity.Name).Id;

        if (eventViewModel.id == 0)
        {
            _unitOfWork.eventViewModel.Add(eventViewModel);
            isCreate = true;
        }
        else
        {
            _unitOfWork.eventViewModel.Update(eventViewModel);
        }

        if (isCreate)
        {
            message = "Calendar event created successfully";
        }
        else
        {
            message = "Calendar event updated successfully";
        }
        _unitOfWork.Save();
 

        responseModel.Message = message;
        responseModel.StatusCode = 200;
        return Ok(new {responseModel});
    }
    
      
    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("Admin/Dashboard/Calendar/Delete/")]
    public IActionResult Delete(int? id)
    {
        ResponseModel responseModel = new ResponseModel();
        var obj = _unitOfWork.eventViewModel.GetFirstOrDefault(x=>x.id==id);
        
        if (obj == null)
        {
            responseModel.Message = "Calendar Event does not exist";
            responseModel.StatusCode = 404;
            ModelState.AddModelError(string.Empty, "Calendar event does not exist");
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return NotFound(new {responseModel, Errors =errors2 });
        }

        _unitOfWork.eventViewModel.Remove(obj);
        _unitOfWork.Save();
        responseModel.Message = "Calendar Event deleted successfully";
        responseModel.StatusCode = 200;
        return Ok(responseModel);
    }

    [HttpGet]
    [Route("Admin/Dashboard/Calendar/GetEvents")]
    public IActionResult GetEvents()
    {
        var viewModel = new EventViewModel();
        var events = new List<CalendarEvents>();
        var start =  DateTime.Today;
        var  end = DateTime.Today;
        string currentUser=_unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Email==User.Identity.Name).Id;
        IEnumerable<EventViewModel> eventViewModelEnumerable = _unitOfWork.eventViewModel.GetAll(u=>u.UserId==currentUser);
			
        foreach (EventViewModel calendarEvents in eventViewModelEnumerable)
        {
            events.Add(new CalendarEvents() 
            { 
                                id =calendarEvents.id,
                                title = calendarEvents.title,
                               start = calendarEvents.start.ToString("yyyy-MM-ddTHH:mm"),
                               end =  getEndDateAndTimeStringValue(calendarEvents),
                               allDay = calendarEvents.allDay
            });
            
        }
        
        return Ok(events);
    }


    private string getEndDateAndTimeStringValue(EventViewModel calendarEvents)
    {
        string endValue = null;
        if(calendarEvents.end==null)
        {
            endValue = null;
        }
        else
        {
            if (calendarEvents.end <= calendarEvents.start)
            {
                endValue = null;
            }
            else
            {
                endValue = calendarEvents.end.ToString();
            }
        }

        return endValue;
    }

    private Boolean checkIfEventOccursAlldayAndAsEndDate(EventViewModel eventViewModel)
    {
        return true;
    }
}
