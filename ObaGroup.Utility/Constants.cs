namespace ObaGroupUtility;

public static class Constants
{
    public const string Role_Admin = "Admin";
    public const string Role_Staff = "Staff";

    public const string Create_Event_Endpoint = "/Dashboard/Calendar/Create";
    public const string List_Events_Endpoint = "/Dashboard/Calendar/ListEvents";
    public const string Get_An_Event_Endpoint = "/Dashboard/Calendar/GetAnEvent";
    public const string Delete_An_Event_Endpoint = "/Dashboard/Calendar/DeleteAnEvent";
    public const string Patch_An_event_Endpoint = "/Dashboard/Calendar/PatchAnEvent";
    public const string Get_Refresh_Token_Endpoint = "/Dashboard/Calendar/RefreshToken";

    public const string Get_A_Document_Endpoint = "/Dashboard/Document/";
    public const string Upsert_A_Document_Endpoint = "/Dashboard/Document/Upsert";
    public const string List_Documents_Endpoint = "/Dashboard/Document/GetAll/";
    public const string Delete_A_Document_File_Endpoint = "/Dashboard/Document/DeleteFile/";

    public const string Update_User_Profile_Endpoint = "/Dashboard/Profile/Update";
    public const string Change_User_profile_Password_Endpoint = "/Dashboard/Profile/ChangePassword";
    public const string Reset_Password_Endpoint = "/Dashboard/Profile/ResetPassword";
    public const string Resend_Email_Verification_Endpoint = "/Dashboard/Profile/ResendEmailVerification";
    
    public const string Create_User_EndPoint = "/Dashboard/Admin/CreateUser";
    public const string Get_All_User_Endpoint = "/Dashboard/Admin/GetAllUsers";
    public const string Delete_A_User_Endpoint = "/Dashboard/Admin/DeleteAUser";
    

    public const string Login_Endpoint = "/Login";
    public const string Logout_Endpoint = "/Logout";
    public const string Confirm_Email_Endpoint = "/ConfirmEmail";
    public const string Forgot_Password_Endpoint = "ForgotPassword";
    
    public const string Upsert_Biography_Endpoint = "/Dashboard/Biography/Upsert";
    public const string List_All_Biographies = "/Dashboard/Biography/GetAll";
    public const string Delete_A_Biography_Endpoint = "/Dashboard/Biography/Delete/";
    public const string Delete_A_Biography_Image_Endpoint = "/Dashboard/Biography/Delete/Image";
    public const string Delete_A_BioGraphy_Video_Endpoint = "/Dashboard/Biography/Delete/Video";
    public const string Get_A_Biography_Endpoint = "/Dashboard/Biography";


}