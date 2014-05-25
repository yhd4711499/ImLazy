using System;

namespace ImLazy.ControlPanel.Converters
{
    public class Status
    {
        public Status(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string Message { get; set; }
        public string DetailInfo { get; private set; }  

        public bool IsError { get; set; }
        public string PropertyName { get; private set; }    

        public bool Update(string msg, bool isError = false, string detailInfo = null)
        {
            var flag = false;
            if (Message != msg)
            {
                Message = msg;
                flag = true;
            }
            if (IsError != isError)
            {
                IsError = isError;
                flag = true;
            }
            if (DetailInfo != detailInfo)
            {
                DetailInfo = detailInfo;
                flag = true;
            }
            return flag;
        }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Message))
                return Message;
            var s = Message;
            return DetailInfo == null ? s : String.Format("{0} : {1}", s, DetailInfo);
        }
    }
}
