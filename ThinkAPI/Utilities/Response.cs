namespace ThinkusAPI.Utilities
{
    public class Response<T>
    {
        public bool status { get; set; }
        public string? msg { get; set; }
        public T? value { get; set; }
        public List<ValidationError> errors { get; set; }
    }

    public class ValidationError
    {
        public string? field { get; set; }
        public string? message { get; set; }
    }   
}
