namespace WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using System.ServiceModel.Description;

    public abstract class DiagnosticServiceBehaviorBase<TServiceMessageInspector> : IEndpointBehavior where TServiceMessageInspector : ServiceMessageInspectorBase
    {
        bool logRequest;
        bool logReply;

        public DiagnosticServiceBehaviorBase(bool inspectRequest, bool inspectReply)
        {
            this.logReply = inspectReply;
            this.logRequest = inspectRequest;
        }

        protected abstract TServiceMessageInspector InspectorFactory(bool logRequest, bool logReply, bool isClientSide);

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            var inspector = this.InspectorFactory(this.logRequest, this.logReply, true);
            clientRuntime.MessageInspectors.Add(inspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            var inspector = this.InspectorFactory(this.logRequest, this.logReply, false);
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}