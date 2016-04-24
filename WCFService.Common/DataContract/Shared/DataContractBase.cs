namespace WCFService.Common.DataContract.Shared
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "https://webservice.afsgo.com/contracts/v1")]
    [Serializable]
    public abstract class DataContractBase
    {
    }
}