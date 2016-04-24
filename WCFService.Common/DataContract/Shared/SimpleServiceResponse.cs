namespace WCFService.Common.DataContract.Shared
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimpleServiceResponse : DataContractBase
    {
        public SimpleServiceResponse()
        {
            this.ResponseStatus = true;
        }

        [DataMember]
        public string ResponseDetails { get; set; }

        [DataMember(Name = "Status")]
        public bool ResponseStatus { get; set; }

        public void SetErrorMessage(string message)
        {
            this.ResponseDetails = message;
            this.ResponseStatus = false;
        }

        public void SetErrorMessage(Exception exception)
        {
            this.SetErrorMessage(exception.Message);
        }
    }
}