using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using MimeKit;
using ObaGroupUtility;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ObaGroup.Utility;

public class EmailSender:IEmailSender
{
  private static readonly ILogger _logger = LoggerFactory.Create(builder =>
  {
    builder.AddConsole();
  }).CreateLogger("EmailSender");

  private readonly IKeyVaultManager _keyVaultManager;
  public EmailSender(IKeyVaultManager keyVaultManager)
  {
    _keyVaultManager = keyVaultManager;
  }
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      try
      {
        var body = $@"
<html xmlns=""http://www.w3.org/1999/xhtml"">
  <head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Email Template</title>
    <style>
      img {{
        border: none;
        -ms-interpolation-mode: bicubic;
        max-width: 100%;
      }}

      body {{
        background-color: #f6f6f6;
        font-family: sans-serif;
        -webkit-font-smoothing: antialiased;
        font-size: 14px;
        line-height: 1.4;
        margin: 0;
        padding: 0;
        -ms-text-size-adjust: 100%;
        -webkit-text-size-adjust: 100%;
        font-family: Verdana, Geneva, Tahoma, sans-serif;
      }}

      table {{
        border-collapse: separate;
        mso-table-lspace: 0pt;
        mso-table-rspace: 0pt;
        width: 100%;
      }}
      table td {{
        font-size: 14px;
        vertical-align: top;
        color: #777;
      }}

      .body {{
        background-color: #f6f6f6;
        width: 100%;
      }}

      .container {{
        display: block;
        margin: 0 auto !important;

        max-width: 580px;
        padding: 10px;
        width: 580px;
      }}

      .content {{
        box-sizing: border-box;
        display: block;
        margin: 0 auto;
        max-width: 580px;
        padding: 10px;
      }}

      .main {{
        background: #ffffff;
        border-radius: 3px;
        width: 100%;
      }}

      .wrapper {{
        box-sizing: border-box;
        padding: 12px;
      }}

      .content-block {{
        padding-bottom: 10px;
        padding-top: 10px;
      }}

      .footer {{
        clear: both;
        margin-top: 10px;
        text-align: center;
        width: 100%;
      }}
      .footer td,
      .footer p,
      .footer span,
      .footer a {{
        color: #999999;
        font-size: 12px;
        text-align: center;
      }}

      h1,
      h2,
      h3,
      h4 {{
        color: #333;
        font-family: sans-serif;
        font-weight: 400;
        line-height: 1.4;
        margin: 0;
        margin-bottom: 30px;
      }}

      h1 {{
        font-size: 35px;
        font-weight: 300;
        text-align: center;
        text-transform: capitalize;
      }}

      p,
      ul,
      ol {{
        font-family: Verdana, Geneva, Tahoma, sans-serif;
        font-size: 14px;
        font-weight: normal;
        margin: 0;
        margin-bottom: 15px;
      }}
      p li,
      ul li,
      ol li {{
        list-style-position: inside;
        margin-left: 5px;
      }}

      a {{
        color: #3498db;
        text-decoration: underline;
      }}

      .preheader {{
        color: transparent;
        display: none;
        height: 0;
        max-height: 0;
        max-width: 0;
        opacity: 0;
        overflow: hidden;
        mso-hide: all;
        visibility: hidden;
        width: 0;
      }}

      hr {{
        border: 0;
        border-bottom: 1px solid #f6f6f6;
        margin: 20px 0;
      }}

      /* -------------------------------------
          RESPONSIVE AND MOBILE FRIENDLY STYLES
      ------------------------------------- */
      @media only screen and (max-width: 620px) {{
        table[class=""body""] h1 {{
          font-size: 28px !important;
          margin-bottom: 10px !important;
        }}
        table[class=""body""] p,
        table[class=""body""] td,
        table[class=""body""] span,
        table[class=""body""] a {{
          font-size: 16px !important;
        }}
        table[class=""body""] .wrapper {{
          padding: 10px !important;
        }}
        table[class=""body""] .content {{
          padding: 0 !important;
        }}
        table[class=""body""] .container {{
          padding: 0 !important;
          width: 100% !important;
        }}
        table[class=""body""] .main {{
          border-left-width: 0 !important;
          border-radius: 0 !important;
          border-right-width: 0 !important;
        }}
        table[class=""body""] .btn table {{
          width: 100% !important;
        }}
        table[class=""body""] .btn a {{
          width: 100% !important;
        }}
        table[class=""body""] .img-responsive {{
          height: auto !important;
          max-width: 100% !important;
          width: auto !important;
        }}
      }}
      @media all {{
        .apple-link a {{
          color: inherit !important;
          font-family: inherit !important;
          font-size: inherit !important;
          font-weight: inherit !important;
          line-height: inherit !important;
          text-decoration: none !important;
        }}
      }}
    </style>
  </head>
  <body>
    <span class=""preheader"">OBA Group</span>
    <table
      role=""presentation""
      border=""0""
      cellpadding=""0""
      cellspacing=""0""
      class=""body""
    >
      <tr>
        <td>&nbsp;</td>
        <td class=""container"">
          <div class=""content"">
            <table role=""presentation"" class=""main"">
              <tr>
                <td class=""wrapper"">
                  <table
                    role=""presentation""
                    border=""0""
                    cellpadding=""0""
                    cellspacing=""0""
                    style=""height: 120px""
                  >
                    <tr>
                      <td>
                        <h3 style=""vertical-align: top"">
                          <img
                            src=""https://1000logos.net/wp-content/uploads/2021/11/Nike-Logo.png""
                            alt=""logo""
                            width=""200""
                            style=""vertical-align: top; width: 200px""
                          />
                        </h3>
                      </td>
                    </tr>
                  </table>
                  <table
                    role=""presentation""
                    border=""0""
                    cellpadding=""0""
                    cellspacing=""0""
                  >
                    <tr>
                      <th style=""vertical-align: center"">
                        <p
                          align=""center""
                          style=""
                            border-radius: 12px;
                            background-color: #2579a9;
                            color: #fff;
                            padding: 10px;
                            vertical-align: center;
                            font-size: 32px;
                            font-family: 'Verdana';
                          ""
                        >
                          Welcome
                        </p>
                      </th>
                    </tr>
                   
                      {htmlMessage}
                     
                    <tr>
                      <td
                        style=""color: #333; font-weight: 800; font-size: 17px""
                      >
                        Thank You
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>

            <div class=""footer"">
              <table
                role=""presentation""
                border=""0""
                cellpadding=""0""
                cellspacing=""0""
              >
                <tr>
                  <td>
                    <span style=""padding: 4px; color: #2579a9"">Tel</span
                    >{{number}}
                    <span style=""padding: 4px; color: #2579a9"">@</span>
                    oba@oba.com
                    <span style=""padding: 4px; color: #2579a9"">W</span>oba
                  </td>
                </tr>
                <tr>
                  <td class=""content-block"">
                    <span class=""apple-link"">{{address}}</span>
                    <br />
                  </td>
                </tr>
              </table>
            </div>
            <!-- END FOOTER -->
          </div>
        </td>
        <td>&nbsp;</td>
      </tr>
    </table>
  </body>
</html>

";
        var emailToSend = new MimeMessage();
        string broadCastingMail = _keyVaultManager.GetBrodCastingMail();
        string broadCastingMailPassword = _keyVaultManager.GetBrodCastingMailPassword();
        
        emailToSend.From.Add(MailboxAddress.Parse(broadCastingMail));
        emailToSend.To.Add(MailboxAddress.Parse(email));
        emailToSend.Subject = subject;
        emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


        using (var emailClient = new SmtpClient())
        {
          emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
          emailClient.Authenticate(broadCastingMail, "uufbhrpmrazrqxkn");
          emailClient.Send(emailToSend);
          emailClient.Disconnect(true);

        }

        return Task.CompletedTask;
      }
      catch (Exception e)
      {
        _logger.LogError("Could not send mail to because "+ e);
        return null;
      }
    }
}