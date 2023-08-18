using System.Text.Json;
using System.Web;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using ObaGoupDataAccess.Repository.IRepository;
using ObaGroupModel;
using ObaGroupModel.Calendar;
using ObaGoupDataAccess;
using ObaGroupUtility;



namespace Oba_group2.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.Role_Admin+","+Constants.Role_Staff)]
public class CalenderController: Controller
{

    private readonly string currentDirectory = Directory.GetCurrentDirectory();
    private readonly IUnitOfWork _unitOfWork;
    private  ApplicationUser applicationUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CalenderController> _logger;
    private readonly IKeyVaultManager _keyVaultManager;
    private readonly IOAuthTokenProperties _oAuthTokenProperties;
    private readonly IOauth _oAuth;

    public CalenderController(IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor,ILogger<CalenderController> logger,IKeyVaultManager keyVaultManager,IOAuthTokenProperties oAuthTokenProperties,IOauth oauth)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _keyVaultManager = keyVaultManager;
        _oAuth = oauth;
        _oAuthTokenProperties = oAuthTokenProperties;

    }

    [HttpGet(Constants.Get_All_Events)]
    public IActionResult Get()
    {
       
        return File("~/dashboard/calendar/index.html", "text/html");
    }
    [HttpGet(Constants.Create_Event_Endpoint)]
    public IActionResult uploadToGoogle(string? error)
    {
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
        return File("~/dashboard/calendar/create/index.html", "text/html");
    }
    
    [HttpPost]
    [Route(Constants.Create_Event_Endpoint)]
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




        var googleCalendarApiKey = _keyVaultManager.GetGoogleCalendarApiKey();
        var sendUpdates = "all";
        
        if (!_oAuth.RefreshTokens())
        {
            return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");

        
        }
        var accessToken = _oAuthTokenProperties.GetAccessToken(); 
        if (accessToken == null)
        {
            return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");


        }

        string model = newEvent.ToJson().ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder(Constants.Google_Calendar_BaseURL+"/primary/events");
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");
      
        var body = model;
     
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = googleCalendarApiKey;
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
    [Route(Constants.List_Events_Endpoint)]
    public async Task<IActionResult> ListEvents(ListCalendarEventParams calendarEventParams)
    {
        var calendarId = "primary";
        var googleCalendarApiKey = _keyVaultManager.GetGoogleCalendarApiKey();
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

        if (!_oAuth.RefreshTokens())
        {
            //return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
            return Ok($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }

        var token = _oAuthTokenProperties.GetAccessToken();

        if (string.IsNullOrWhiteSpace(token))
        {
            return Ok($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
            //return  Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }
        
        var accessToken = token;
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder(Constants.Google_Calendar_BaseURL+"/"+calendarId+"/events?");
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = googleCalendarApiKey;
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
    [Route(Constants.Get_An_Event_Endpoint)]
    public async Task<IActionResult> GetAnEvent(GetAnEventParam getAnEventParam)
    {
        var googleCalendarApiKey = _keyVaultManager.GetGoogleCalendarApiKey();
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

        if (!_oAuth.RefreshTokens())
        {
            return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }
        var token = _oAuthTokenProperties.GetAccessToken(); //JObject.Parse((System.IO.File.ReadAllText(tokenFile)));

        if (string.IsNullOrWhiteSpace(token))
        {
            Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Callback_Endpoint}");
        }
        
 
        var accessToken = token;//tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder(Constants.Google_Calendar_BaseURL+"/"+calendarId+"/events/"+getAnEventParam.eventId);
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = googleCalendarApiKey;
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
    [Route(Constants.Delete_An_Event_Endpoint)]
    public async Task<IActionResult> DeleteAnEvent( DeleteAnEventParam deleteAnEventParam)
    {
        ResponseModel responseModel = new ResponseModel();
        var googleCalendarApiKey = _keyVaultManager.GetGoogleCalendarApiKey();
        
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
 


        if (!_oAuth.RefreshTokens())
        {
            return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }
        
        var token = _oAuthTokenProperties.GetAccessToken();//JObject.Parse((System.IO.File.ReadAllText(tokenFile)));


        if (string.IsNullOrWhiteSpace(token))
        {
            Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }

        var accessToken = token;//tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder(Constants.Google_Calendar_BaseURL+"/"+calendarId+"/events/"+deleteAnEventParam.eventId);
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + accessToken);
        headers.Add("Accept", "application/json");

        var query = HttpUtility.ParseQueryString(builder.Query);
        query["key"] = googleCalendarApiKey;
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
            responseModel.Message = "Event deleted successfully";
            responseModel.StatusCode = 200;
            return Ok(responseModel);
        }
        
        GoogleCalendarApiError googleCalendarApiError =  response.Content.ReadFromJsonAsync<GoogleCalendarApiError>().Result;
        responseModel.Message = googleCalendarApiError.error.message;
        responseModel.StatusCode = googleCalendarApiError.error.code;
        return BadRequest(new {responseModel, Errors =googleCalendarApiError});
    }

    [HttpPatch]
    [Route(Constants.Patch_An_event_Endpoint)]
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
                start.DateTime = DateTime.Parse(patchEvent.StartDateTime);
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
                end.DateTime = DateTime.Parse(patchEvent.EndDateTime);
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
        var googleCalendarApiKey = _keyVaultManager.GetGoogleCalendarApiKey();

        
        if (!_oAuth.RefreshTokens())
        {
            return Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }
        var token = _oAuthTokenProperties.GetAccessToken();//JObject.Parse((System.IO.File.ReadAllText(tokenFile)));
      
        if (string.IsNullOrWhiteSpace(token))
        {
            Redirect($"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Authorization_Endpoint}");
        }

        var accessToken = token;//tokens["access_token"].ToString();
        
        HttpClient client = new HttpClient();
        var builder = new UriBuilder(Constants.Google_Calendar_BaseURL+"/"+calendarId+"/events/"+patchEvent.EventId);
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
        query["key"] = googleCalendarApiKey;
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

        var patchEndpoint = $"{Constants.Google_Calendar_BaseURL}/primary/events/{patchEvent.EventId}?key={googleCalendarApiKey}";

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


    [Route(Constants.Google_Calendar_Authorization_Endpoint)]
    public ActionResult OauthRedirect()
    {
        Console.WriteLine(currentDirectory);
        var googleCalenderGrantPermissionClientId = _keyVaultManager.GetGoogleCalenderGrantPermissionClientId();

        var referringUrl = HttpContext.Request.Headers["Referer"];

        string loginHint = User.Identity.Name;

        var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                          "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                          "access_type=offline&" +
                          "include_granted_scopes=true&" +
                          "login_hint="+loginHint+"&"+
                          "response_type=code&" +
                          "state=referringUrl&" +
                          "redirect_uri="+$"{Request.Scheme}://{Request.Host}{Constants.Google_Calendar_Callback_Endpoint}&" +
                          "client_id=" +googleCalenderGrantPermissionClientId;

        return Redirect(redirectUrl);
    }
}
