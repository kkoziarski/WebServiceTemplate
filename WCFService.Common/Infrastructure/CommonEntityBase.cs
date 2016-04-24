namespace WCFService.Common.Infrastructure
{
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class CommonEntityBase
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}