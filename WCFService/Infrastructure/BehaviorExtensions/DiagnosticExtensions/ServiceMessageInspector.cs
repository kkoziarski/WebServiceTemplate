namespace WCFService.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using System;

    using WCFService.Common.Diagnostic;
    using WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions;

    public sealed class ServiceMessageInspector : ServiceMessageInspectorBase
    {
        public ServiceMessageInspector(bool logRequest, bool logReply, bool isClientSide)
            : base(logRequest, logReply, isClientSide)
        {
        }

        protected override IWebServiceDiagnosticService InitializeDiagnosticService()
        {
            // TODO: enable service diagnostic implementation
            //IKernel kernel = new StandardKernel(new OfflineServiceModule());
            //return kernel.Get<IWebServiceDiagnosticService>();

            throw new NotImplementedException();
        }
    }
}