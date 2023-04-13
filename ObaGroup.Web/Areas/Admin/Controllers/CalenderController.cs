using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;



namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(EventViewModel eventViewModel)
    {
        if (!ModelState.IsValid)
        {
            return Redirect("index");
        }

    
        if (eventViewModel.id == 0)
        {
            eventViewModel.UserId=_unitOfWork.ApplicationUser.GetFirstOrDefault(u=>u.Email==User.Identity.Name).Id;
            _unitOfWork.eventViewModel.Add(eventViewModel);
        }
        else
        {
            _unitOfWork.eventViewModel.Update(eventViewModel);
        }
    
        _unitOfWork.Save();
        if (eventViewModel.id == 0)
        {
            TempData["success"] = "Document created successfully";
        }
        else
        {
            TempData["success"] = "Document Updated successfully";
        }

     
        _unitOfWork.Save();
        return Redirect("index");
    }

    [HttpGet]
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
        
        return Json(events.ToArray() );
        
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
}
