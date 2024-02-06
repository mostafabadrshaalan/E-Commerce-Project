namespace API.ResponseModule
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetResponseMessageForStatusCode(statusCode);
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetResponseMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You made a bad request",
                401 => "You are not authorized",
                404 => "Resourse is not found",
                500 => "Server error",
                _ => null
            };
        }
    }
}
