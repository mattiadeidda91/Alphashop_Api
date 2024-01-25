using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alphashop_ArticoliWebService.Models.Responses
{
    public class ResponseMsg//<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        //public T? Data { get; set; }
        public DateTime Date { get; set; }

        public ResponseMsg(bool isSuccess,/* T? data,*/ string message, int code, DateTime? date = null) 
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            //this.Data = data; non riesco a far stampare correttamente l'oggetto nella risposta come unico JSON
            this.Code = code;
            this.Date = date ?? DateTime.Now;
        }
    }
}
