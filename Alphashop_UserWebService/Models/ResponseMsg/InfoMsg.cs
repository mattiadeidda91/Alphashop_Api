namespace Alphashop_UserWebService.Models.ResponseMsg
{
    public class InfoMsg
    {
        public DateTime Data { get; set; }
        public string Message { get; set; }

        public InfoMsg(DateTime Data, String Message)
        {
            this.Data = Data;
            this.Message = Message;
        }
    }
}
