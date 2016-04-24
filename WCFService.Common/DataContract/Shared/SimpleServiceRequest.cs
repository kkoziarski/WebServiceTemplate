namespace WCFService.Common.DataContract.Shared
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SimpleServiceRequest : DataContractBase
    {
        [DataMember]
        public string AppVersion { get; set; }
    }
}