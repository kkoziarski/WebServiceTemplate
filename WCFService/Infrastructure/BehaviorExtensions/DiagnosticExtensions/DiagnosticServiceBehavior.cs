namespace WCFService.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions;

    public sealed class DiagnosticServiceBehavior : DiagnosticServiceBehaviorBase<ServiceMessageInspector>
    {
        public DiagnosticServiceBehavior(bool inspectRequest, bool inspectReply)
            : base(inspectRequest, inspectReply)
        {
        }

        protected override ServiceMessageInspector InspectorFactory(bool logRequest, bool logReply, bool isClientSide)
        {
            return new ServiceMessageInspector(logRequest, logReply, isClientSide);            
        }
    }
}