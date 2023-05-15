namespace NSE.WebApp.MVC.Models
{
    public class ResponseResultViewModel
    {
        public string Title { get; set; }
        public int Status { get; set; }

        public ResponseErrorMessagesViewModel Errors { get; set; }
    }
}
