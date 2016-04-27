namespace WCFService.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using System;

    using Ninject;

    using WCFService.Common.Diagnostic;
    using WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions;
    using WCFService.Infrastructure.DependencyResolution;

    public sealed class ServiceMessageInspector : ServiceMessageInspectorBase
    {
        public ServiceMessageInspector(bool logRequest, bool logReply, bool isClientSide)
            : base(logRequest, logReply, isClientSide)
        {
        }

        protected override IWebServiceDiagnosticService InitializeDiagnosticService()
        {
            IKernel kernel = new StandardKernel(new NinjectServiceModule());
            return kernel.Get<IWebServiceDiagnosticService>();
        }
    }
}