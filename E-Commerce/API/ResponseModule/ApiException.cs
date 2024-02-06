namespace API.ResponseModule
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message=null,string detials=null) : base(statusCode,message)
        {
            Detials = detials;
        }
        public string Detials { get; set; }
    }
}
