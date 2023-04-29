using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Discovery.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NuGet.Protocol;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupModel.Calendar;
using System.Net.Http.Json;
using Google.Apis.Requests;
using ObaGroupUtility;
using RestSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
public class CalenderController: Controller
{
    class CalendarListEntryResponse {
        public Events Events { get; set; }
    }
    private readonly string currentDirectory = Directory.GetCurrentDirectory();
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


    [HttpGet("/Admin/Dashboard/Calendar/google")]
    public IActionResult uploadToGoogle(string? error)
    {
        Console.WriteLine("redirected");
        if (error != null)
        {
            ResponseModel responseModel = new ResponseModel();
            ModelState.AddModelError(string.Empty, error);
            responseModel.Message = error;
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            
            return BadRequest(new {responseModel, Errors =errors2});
        }

        return Ok();
    }
    
    [HttpPost]
    [Route("Admin/Dashboard/Calendar/google")]
    public async Task<IActionResult> uploadToGoogle([FromForm] CalendarEvent calendarEvents)
    {
        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }
        if (calendarEvents.EndDateTimeZone == null)
        {
            calendarEvents.EndDateTimeZone = "Africa/Lagos";
        }
        if (calendarEvents.StartDateTimeZone == null)
        {
            calendarEvents.StartDateTimeZone = "Africa/Lagos";
        }

        if (calendarEvents.EmailReminderTime==null)
        {
            calendarEvents.EmailReminderTime = 0;
        }
        if (calendarEvents.PopUpReminderTime==null)
        {
            calendarEvents.PopUpReminderTime = 0;
        }
        List<EventAttendee> eventAttendees = new List<EventAttendee>();
        foreach (var attendee in calendarEvents.AttendeeEmail)
        {
            eventAttendees.Add(new EventAttendee() { Email = attendee});
        }
        Event newEvent = new Event()
        {
            Summary = calendarEvents.Summary,
            Location = calendarEvents.Location,
            Description = calendarEvents.Description,
            Start = new EventDateTime()
            {
                DateTime = DateTime.Parse(calendarEvents.StartDateTime),
                TimeZone = calendarEvents.StartDateTimeZone,
            },
            End = new EventDateTime()
            {
                DateTime = DateTime.Parse(calendarEvents.EndDateTime),
                TimeZone = calendarEvents.EndDateTimeZone,
            },
            
            Attendees = eventAttendees,
            
            Reminders = new Event.RemindersData()
            {
                UseDefault = false,
                Overrides = new EventReminder[] {
                    new EventReminder() { Method = "email", Minutes = calendarEvents.EmailReminderTime*60 },
                    new EventReminder() { Method = "popup", Minutes = calendarEvents.EmailReminderTime*60},
                }
            }
        };

        var tokenFile =  currentDirectory+ "/tokens.json";
        var apiKeyFIle =  currentDirectory+"/Apikey.json";

        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
        var apiKeyJson = JObject.Parse((System.IO.File.ReadAllText(apiKeyFIle)));

        var apiKey = apiKeyJson["ApiKey"].ToString();
        var sendUpdates = "all";
     
        if (tokens == null || !tokens.ContainsKey("refresh_token"))
        {
            Redirect("https://localhost:7151/calendar");
        }
        
        OAuth.RefreshTokens();

        var accessToken = tokens["access_token"].ToString();

        string model = newEvent.ToJson().ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder("https://www.googleapis.com/calendar/v3/calendars/primary/events");
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");
      
        var body = model;
     
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = apiKey;
        query["sendUpdates"] = sendUpdates;
        builder.Query = query.ToString();

        var request = new HttpRequestMessage(HttpMethod.Post, builder.Uri)
        {
            Content = new StringContent(body)
        };

        foreach (var header in headers)
        {
            request.Headers.Add(header.Key,header.Value);
        }

        var response =  await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }
        
        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }

    
    [HttpGet]
    [Route("Admin/Dashboard/Calendar/ListEvents")]
    public async Task<IActionResult> ListEvents(ListCalendarEventParams calendarEventParams)
    {
        var calendarId = "primary";
        var tokenFile =  currentDirectory+ "/tokens.json";
        var apiKeyFIle =  currentDirectory+"/Apikey.json";
        var orderBy = "startTime";

        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }
        
        if (calendarEventParams.timeZone==null)
        {
            calendarEventParams.timeZone = "Africa/Lagos";
        }
        calendarEventParams.EventMinTimeRange = DateTime.Parse(calendarEventParams.EventMinTimeRange).ToString("yyyy-MM-ddTHH:mm:sszzz");
        calendarEventParams.EventMaxTimeRange = DateTime.Parse(calendarEventParams.EventMaxTimeRange).ToString("yyyy-MM-ddTHH:mm:sszzz");
       

        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
        var apiKeyJson = JObject.Parse((System.IO.File.ReadAllText(apiKeyFIle)));

        var apiKey = apiKeyJson["ApiKey"].ToString();
        
        if (tokens == null || !tokens.ContainsKey("refresh_token"))
        {
          return  Redirect("https://localhost:7151/calendar");
        }
        
        OAuth.RefreshTokens();
        
        var accessToken = tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder("https://www.googleapis.com/calendar/v3/calendars/"+calendarId+"/events?");
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = apiKey;
        query["timeMax"] =calendarEventParams.EventMaxTimeRange ;
        query["timeMin"] = calendarEventParams.EventMinTimeRange;
        query["timeZone"] = calendarEventParams.timeZone;
        builder.Query = query.ToString();
        
        Console.WriteLine(builder.Query);
        Console.WriteLine(builder.ToString());
        var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key,header.Value);
        }

        
        var response =  await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            
            return Ok( await response.Content.ReadFromJsonAsync<JsonElement>());
        }

        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }
    
      [HttpGet]
    [Route("Admin/Dashboard/Calendar/GetAnEvent")]
    public async Task<IActionResult> GetAnEvent(GetAnEventParam getAnEventParam)
    {
        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }
        
        var calendarId = "primary";
        var tokenFile =  currentDirectory+ "/tokens.json";
        var apiKeyFIle =  currentDirectory+"/Apikey.json";


        
        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
        var apiKeyJson = JObject.Parse((System.IO.File.ReadAllText(apiKeyFIle)));

        var apiKey = apiKeyJson["ApiKey"].ToString();
        
        if (tokens == null || !tokens.ContainsKey("refresh_token"))
        {
            Redirect("https://localhost:7151/calendar");
        }
        
        OAuth.RefreshTokens();
        
        var accessToken = tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder("https://www.googleapis.com/calendar/v3/calendars/"+calendarId+"/events/"+getAnEventParam.eventId);
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = apiKey;
        builder.Query = query.ToString();
        
        Console.WriteLine(builder.Query);
        Console.WriteLine(builder.ToString());
        var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key,header.Value);
        }

        var response =  await client.SendAsync(request);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }
        
        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }

    [HttpDelete]
    [Route("Admin/Dashboard/Calendar/DeleteAnEvent")]
    public async Task<IActionResult> DeleteAnEvent( DeleteAnEventParam deleteAnEventParam)
    {
        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            var errors2 = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors2 });
        }
        
        var calendarId = "primary";
        var sendUpdate = "all";
        var tokenFile =  currentDirectory+ "/tokens.json";
        var apiKeyFIle =  currentDirectory+"/Apikey.json";

        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
        var apiKeyJson = JObject.Parse((System.IO.File.ReadAllText(apiKeyFIle)));

        var apiKey = apiKeyJson["ApiKey"].ToString();
        if (tokens == null || !tokens.ContainsKey("refresh_token"))
        {
            Redirect("https://localhost:7151/calendar");
        }
        OAuth.RefreshTokens();
        
        var accessToken = tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder("https://www.googleapis.com/calendar/v3/calendars/"+calendarId+"/events/"+deleteAnEventParam.eventId);
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = apiKey;
        query["sendUpdates"] = sendUpdate;
        builder.Query = query.ToString();

        var request = new HttpRequestMessage(HttpMethod.Delete, builder.Uri);
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key,header.Value);
        }
        
        var response =  await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }
        
        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }

    [HttpPatch]
    [Route("Admin/Dashboard/Calendar/PatchAnEvent")]
    public async Task<IActionResult> UpdateAnEvent([FromForm] PatchEvent patchEvent)
    {
        IEnumerable<string> errors;
        ResponseModel responseModel = new ResponseModel();
        if (!ModelState.IsValid)
        {
            responseModel.Message = "Bad Request";
            responseModel.StatusCode = 400;
            errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(new {responseModel, Errors =errors });
        }
        
        OAuth.RefreshTokens();
   
        var sendUpdates = "all";
        Event patchEventbody = new Event();
        if (!string.IsNullOrWhiteSpace(patchEvent.Summary))
        {
            patchEventbody.Summary = patchEvent.Summary;
        }

        if (!string.IsNullOrWhiteSpace(patchEvent.Location))
        {
            patchEventbody.Location = patchEvent.Location;
        }

        if (!string.IsNullOrWhiteSpace(patchEvent.Description))
        {
            patchEventbody.Description = patchEvent.Description;

        }

        if (!string.IsNullOrWhiteSpace(patchEvent.StartDateTime) ||
            !string.IsNullOrWhiteSpace(patchEvent.StartDateTimeZone))
        {
            EventDateTime start = new EventDateTime();

            if (!string.IsNullOrWhiteSpace(patchEvent.StartDateTime))
            {
                start.DateTime = DateTime.Parse("2023-04-30T10:50:00+01:00");
            }

            if (!string.IsNullOrWhiteSpace(patchEvent.StartDateTimeZone))
            {
                start.TimeZone = patchEvent.StartDateTimeZone;
            }
            else
            {
                start.TimeZone = "Africa/Lagos";
            }

            patchEventbody.Start = start;
        }
        
        if (!string.IsNullOrWhiteSpace(patchEvent.EndDateTime) ||
            !string.IsNullOrWhiteSpace(patchEvent.EndDateTimeZone))
        {
            EventDateTime end = new EventDateTime();

            if (!string.IsNullOrWhiteSpace(patchEvent.EndDateTime))
            {
                end.DateTime = DateTime.Parse("2023-04-30T11:50:00+01:00");
            }

            if (!string.IsNullOrWhiteSpace(patchEvent.EndDateTimeZone))
            {
                end.TimeZone = patchEvent.EndDateTimeZone;
            }
            else
            {
                end.TimeZone = "Africa/Lagos";
            }
            patchEventbody.End = end;
        }
        
        if (patchEvent.AttendeeEmailList!=null)
        {
            List<EventAttendee> eventAttendees = new List<EventAttendee>();
            foreach (var attendee in patchEvent.AttendeeEmailList)
            {
                eventAttendees.Add(new EventAttendee() { Email = attendee});
            }

            patchEventbody.Attendees = eventAttendees;
        }

        if ((patchEvent.EmailReminderTime > 0) || (patchEvent.PopUpReminderTime > 0))
        {
            List<EventReminder> eventReminder = new List<EventReminder>();
            if (patchEvent.EmailReminderTime > 0)
            {
                eventReminder.Add(new EventReminder()
                    { Method = "email", Minutes = patchEvent.EmailReminderTime * 60 });
            }

            if (patchEvent.PopUpReminderTime > null)
            {
                eventReminder.Add(new EventReminder()
                    { Method = "popup", Minutes = patchEvent.EmailReminderTime * 60 });
            }
            patchEventbody.Reminders.Overrides = eventReminder;
        }
        var calendarId = "primary";
        var tokenFile =  currentDirectory+ "/tokens.json";
        var apiKeyFIle =  currentDirectory+"/Apikey.json";
        
        var tokens = JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
        var apiKeyJson = JObject.Parse((System.IO.File.ReadAllText(apiKeyFIle)));

        var apiKey = apiKeyJson["ApiKey"].ToString();

        if (tokens == null || !tokens.ContainsKey("refresh_token"))
        {
            Redirect("https://localhost:7151/calendar");
        }
        OAuth.RefreshTokens();
        
        var accessToken = tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder("https://www.googleapis.com/calendar/v3/calendars/"+calendarId+"/events/"+patchEvent.EventId);
        var headers = new Dictionary<string, string>();

        Console.WriteLine(patchEventbody.ToJson().ToString());
        string body = null;
        if (patchEventbody != null)
        {
             body = patchEventbody.ToJson().ToString();
        }
        
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = apiKey;
       // query["events"] = patchEvent.EventId;
        query["sendUpdates"] = sendUpdates;
        builder.Query = query.ToString();
        Console.WriteLine(builder.Uri.ToString());
        
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), builder.Uri);
      
        if (patchEventbody != null)
        {
            request.Content = new StringContent(JsonConvert.SerializeObject(patchEventbody));
        }
        
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var patchEndpoint = $"https://www.googleapis.com/calendar/v3/calendars/primary/events/{patchEvent.EventId}?key={apiKey}";

        var patchContent = new StringContent(
            patchEventbody.ToJson(),
            System.Text.Encoding.UTF8,
            "application/json");

        var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), patchEndpoint)
        {
            Content = patchContent
        };

        var response = await client.SendAsync(patchRequest);
       
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key,header.Value);
        }
        var response2 =  await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return Ok(await response.Content.ReadAsStringAsync());
        }
        
        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }


    [Route("calendar")]
    public ActionResult OauthRedirect()
    {
        Console.WriteLine(currentDirectory);
        var credentialsFile = currentDirectory+"/client_secret_87857337556-iqm8t560cfhc8ddln4mdk88ahl311na9.apps.googleusercontent.com.json";
        JObject credentials = JObject.Parse((System.IO.File.ReadAllText(credentialsFile)));
        var client_id= credentials["web"]["client_id"].ToString();
        Console.WriteLine(client_id);

        string loginHint = "olasubomiodekunle@gmail.com";

        var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                          "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                          "access_type=offline&" +
                          "include_granted_scopes=true&" +
                          "login_hint="+loginHint+"&"+
                          "response_type=code&" +
                          "state=hellothere&" +
                          "redirect_uri=https://localhost:7151/oauth/callback&" +
                          "client_id=" +client_id;
        
        return Redirect(redirectUrl);
    }

    [Route("calendard")]
    public void OauthRedirectd()
    {
        OAuth.RefreshTokens();
    }
}
