using System;
using Microsoft.AspNetCore.Mvc;

namespace movie_api.Utils
{
    public sealed class ResponseBuilder
    {
        public static string LOGIN_FAILED = "error login has occurred";

        public static string LOGIN_OK = "Login successfull";

        public static string ACCOUNT_ACTIVATION_FAILED = "error while activating account";

        public static string ACCOUNT_ACTIVATION_OK = "Activate account successfull";

        public static string REGISTER_OK = "Register successfull please check your email";

        public static string REGISTER_FAILED = "error register has occurred";

        public static string GET_OK = "success get data";

        public static string GET_FAILED = "error while getting data";

        public static string CREATE_OK = "success create data";

        public static string CREATE_FAILED = "error while creating data";

        public static string UPDATE_OK = "success update data";

        public static string UPDATE_FAILED = "error while updating data";

        public static string DELETE_OK = "success delete data";

        public static string DELETE_FAILED = "error while deleting data";

        public static string NOT_FOUND = "data not found";

        public static Object SuccessResponse(string message, dynamic data = null)
        {
            return new JsonResult(new
            {
                success = true,
                message = message,
                data = data,
            }).Value;
        }

        public static Object FailedResponse(string message, dynamic errorData, int statusCode)
        {
            return new JsonResult(new
            {
                success = false,
                error_code = statusCode,
                errors = errorData,
                message = message,
            }).Value;
        }
    }


}