namespace WCFService.Common.Diagnostic
{
    public interface IWebServiceDiagnosticService
    {
        void LogRequestResponse(DiagnosticLog diagnosticLog);
    }
}