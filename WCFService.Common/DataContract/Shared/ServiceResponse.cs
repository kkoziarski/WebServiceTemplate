namespace WCFService.Common.DataContract.Shared
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ServiceResponse<TResponseType> : SimpleServiceResponse 
    {
        public ServiceResponse()
        {
        }

        public ServiceResponse(TResponseType responseData)
        {
            this.ResponseData = responseData;
        }

        [DataMember(Name = "Data")]
        public TResponseType ResponseData { get; set; }
    }
}