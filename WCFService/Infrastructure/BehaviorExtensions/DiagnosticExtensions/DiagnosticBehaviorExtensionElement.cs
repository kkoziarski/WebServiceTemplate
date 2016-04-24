namespace WCFService.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions;

    public sealed class DiagnosticBehaviorExtensionElement : DiagnosticBehaviorExtensionElementBase<ServiceMessageInspector, DiagnosticServiceBehavior>
    {
        protected override DiagnosticServiceBehavior ServiceBehaviorFactory(bool inspectRequest, bool inspectReply)
        {
            return new DiagnosticServiceBehavior(inspectRequest, inspectReply);
        }
    }
}