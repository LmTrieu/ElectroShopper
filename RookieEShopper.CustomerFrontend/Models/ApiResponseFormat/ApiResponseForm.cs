namespace RookieEShopper.Api.Dto
{
    public class ApiListObjectResponse<T>
    {        
        public List<T> Data { get; set; }
        public string Message { get; set; }
        public int Total { get; set; }
    }
    public class ApiSingleObjectResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
