namespace FeedbackPage.Api.Models
{
    public class GenericDataResponse<T> : GenericResponse
    {
        public T Data { get; set; }
    }
}
