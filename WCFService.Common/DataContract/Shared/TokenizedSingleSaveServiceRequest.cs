namespace WCFService.Common.DataContract.Shared
{
    using System;
    using System.Runtime.Serialization;

    using WCFService.Common.Helpers;

    /// <summary>
    /// This class is simpler than <see cref="TokenizedSingleSaveServiceRequest"/> and doesn't contain "RemoteId"
    /// </summary>
    /// <seealso cref="TokenServiceRequest" />
    [DataContract]
    public class TokenizedSaveServiceRequest : TokenServiceRequest
    {
        //this is need to parse date time. It can't be just datetime if we want to receive date in format dd/MM/yyyy
        [DataMember(Name = "EventDate")]
        private string eventDate;
        public DateTime? EventDate { get; set; }

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            this.eventDate = this.EventDate.ToJsonString();
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            this.EventDate = this.eventDate.ToJsonDateTime();
        }
    }

    /// <summary>
    /// This class has "RemoteId" and is more specialized than <see cref="TokenizedSaveServiceRequest"/>
    /// </summary>
    /// <seealso cref="TokenizedSaveServiceRequest" />
    [DataContract]
    public class TokenizedSingleSaveServiceRequest : TokenizedSaveServiceRequest
    {
        [DataMember]
        public int? RemoteId { get; set; }
    }

    /// <summary>
    /// This class has "RemoteId" and is more specialized than <see cref="TokenizedSaveServiceRequestWithData{TRequestDataType}"/>
    /// </summary>
    /// <typeparam name="TRequestDataType">The type of the request data type.</typeparam>
    /// <seealso cref="TokenizedSingleSaveServiceRequest" />
    [DataContract]
    public class TokenizedSingleSaveServiceRequestWithData<TRequestDataType> : TokenizedSingleSaveServiceRequest
    {
        [DataMember(Name = "Data")]
        public TRequestDataType RequestData { get; set; }
    }

    /// <summary>
    /// This class is simpler than <see cref="TokenizedSingleSaveServiceRequestWithData{TRequestDataType}" and doesn't contain "RemoteId"/>
    /// </summary>
    /// <typeparam name="TRequestDataType">The type of the request data type.</typeparam>
    /// <seealso cref="TokenizedSingleSaveServiceRequest" />
    [DataContract]
    public class TokenizedSaveServiceRequestWithData<TRequestDataType> : TokenizedSaveServiceRequest
    {
        [DataMember(Name = "Data")]
        public TRequestDataType RequestData { get; set; }
    }
}