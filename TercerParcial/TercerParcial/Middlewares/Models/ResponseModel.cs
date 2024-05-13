namespace TercerParcial.Middlewares.Models
{
    public class ResponseModel<T>
    {
        public bool Succed { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public T Data { get; set; }

        public ResponseModel()
        {

        }

        public ResponseModel(T data, string message = null)
        {
            Succed = true;
            Message = message;
            Data = data;
        }

        public ResponseModel(string message)
        {
            Succed = false;
            Message = message;
        }
    }
}
