namespace EMS.Helpers.ErrorHelper;

public static class ErrorMessage
{
    public const string CREATE_EMPLOYEE_ERROR = "Failed to create employee";
    public const string DELETE_EMPLOYEE_ERROR = "Failed to delete employee";
    public const string UPDATE_EMPLOYEE_ERROR = "Failed to update employee";
    public const string GET_EMPLOYEE_ERROR = "An error occured while fetching employee data";

    public const string CREATE_DESIGNATION_ERROR = "Failed to create designation";
    public const string DELETE_DESIGNATION_ERROR = "Failed to delete designation";
    public const string UPDATE_DESIGNATION_ERROR = "Failed to update designation";
    public const string GET_DESIGNATION_ERROR = "An error occured while fetching designation data";

    public const string CREATE_DEPARTMENT_ERROR = "Failed to create department";
    public const string DELETE_DEPARTMENT_ERROR = "Failed to delete department";
    public const string UPDATE_DEPARTMENT_ERROR = "Failed to update department";
    public const string GET_DEPARTMENT_ERROR = "An error occured while fetching department data";

    public const string CREATE_ATTENDANCE_ERROR = "Failed to create attendance";
    public const string DELETE_ATTENDANCE_ERROR = "Failed to delete attendance";
    public const string UPDATE_ATTENDANCE_ERROR = "Failed to update attendance";
    public const string GET_ATTENDANCE_ERROR = "An error occured while fetching attendance data";

    public const string REGISTER_USER_ERROR = "Failed to register user";
    public const string DELETE_USER_ERROR = "Failed to delete user";
    public const string UPDATE_USER_ERROR = "Failed to update user";
    public const string GET_USER_ERROR = "An error occured while fetching user data";
    public const string AUTHENTICATION_USER_ERROR = "An error occured while authenticating user data";


    public const string ADD_TOKEN_ERROR = "Failed to add TOKEN";
    public const string DELETE_TOKEN_ERROR = "Failed to delete TOKEN";
    public const string UPDATE_TOKEN_ERROR = "Failed to update TOKEN";
    public const string GET_TOKEN_ERROR = "An error occured while fetching TOKEN data";


    public const string USERNAME_NOT_FOUND = "The username not found";
    public const string USERNAME_ALREADY_EXIST = "The username already exist";
    public const string EMAIL_ALREADY_EXIST = "The email already exist";


    public const string LOG_IN_ERROR = "An error occured while logging in"; 
    public const string LOG_OUT_ERROR = "An error occured while logging out"; 
    public const string INVALID_CREDENTIALS = "You have provided invalid credentials"; 
    public const string SESSION_EXPIRED = "Invalid or expired session"; 


    public const string CREATE_LOG_ERROR = "Failed to create log";
    public const string GET_LOG_ERROR = "An error occured while fetching log data";

    public const string VALIDATION_ERROR = "Validation failed. See errors for details.";

    public const string DEPARTMENT_NOT_FOUND = "The department you are trying to create is not found";

    public static string GetErrorMessage(string layerName, string methodName, string errorMessage)
    {
        return $"An error occured in {layerName} -> {methodName}.\n Error message: {errorMessage}";
    }
}