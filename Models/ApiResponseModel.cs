namespace post_ang_webapi_sql.Models
{
    public class ApiResponseModel
    {
        public ApiResponseModel()
        {
            Status = "Success";
        }
        public string Status { get; set; }
        public object Data { get; set; }
    }
}