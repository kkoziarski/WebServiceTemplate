namespace WCFService.Infrastructure.BehaviorExtensions.ElmahExtensions
{
    using System;
    using System.ServiceModel.Dispatcher;

    public class ErrorLogger : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return false;
        }

        public void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            if (error == null)
                return;
            /////In case we run outside of IIS, 
            /////make sure aspNetCompatibilityEnabled="true" in web.config under system.serviceModel/serviceHostingEnvironment
            /////to be sure that HttpContext.Current is not null
            //if (HttpContext.Current == null)
            //    return;
            //Elmah.ErrorSignal.FromCurrentContext().Raise(error);
            //Elmah.ErrorSignal.FromContext(null).Raise(error);


            //TODO: Enable when Elmah added
            //Log(error);
        }

        //public static void Log(Error error)
        //{
        //    Elmah.ErrorLog.GetDefault(null).Log(error);
        //}

        //public static void Log(Exception exception)
        //{
        //    Log(new Error(exception));
        //}

        //public static void Log(Exception exception, string message)
        //{
        //    Exception exc = new Exception(message, exception);
        //    Log(exc);
        //}
    }
}