﻿namespace Alphashop_UserWebService.Models.ResponseMsg
{
    public class ErrMsg
    {

        public ErrMsg(string message, int errcode)
        {
            this.message = message;
            this.errcode = errcode;
        }

        public string message { get; set; }
        public int errcode { get; set; }
    }
}
