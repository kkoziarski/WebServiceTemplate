namespace WCFService.Infrastructure.DependencyResolution
{
    using Ninject.Modules;
    using Ninject.Web.Common;

    using WCFService.BusinessLogic;
    using WCFService.Common.Diagnostic;

    public class NinjectServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IWebServiceDiagnosticService>().To<WebServiceDiagnosticServiceFake>().InRequestScope();
            this.Bind<IExampleBusinessService>().To<ExampleBusinessService>();
        }
    }
}
