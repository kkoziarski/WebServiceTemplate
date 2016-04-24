namespace WCFService.Common.DataContract.Shared
{
    using System.Runtime.Serialization;

    [DataContract]
    public class TokenServiceRequest : SimpleServiceRequest
    {
        [DataMember]
        public string UserToken { get; set; }

        // This value is set by CustomValidationBehavior, before web method is called (on BeforeCall)
        public int ValidatedUserId { get; set; }
    }
}