namespace WCFService.Common.Diagnostic
{
    using System;

    public class DiagnosticLog
    {
        public DiagnosticLog()
        {
        }
        public DiagnosticLog(
            DateTime? requestTime, 
            DateTime? responseTime, 
            int eventLevel = 10, 
            string appVersion = null, 
            string authToken = null, 
            string webMethodName = null, 
            string requestContent = null, 
            string responseContent = null, 
            string exceptionStackTrace = null, 
            string exceptionMessage = null) : this()
        {
            this.RequestTime = requestTime;
            this.ResponseTime = responseTime;
            this.EventLevel = eventLevel;
            this.AppVersion = appVersion;
            this.AuthToken = authToken;
            this.WebMethodName = webMethodName;
            this.RequestContent = requestContent;
            this.ResponseContent = responseContent;
            this.ExceptionStackTrace = exceptionStackTrace;
            this.ExceptionMessage = exceptionMessage;
        }

        public DateTime? RequestTime { get; set; }

        public DateTime? ResponseTime { get; set; }

        public int EventLevel { get; set; }

        public string AppVersion { get; set; }

        public string AuthToken { get; set; }

        public string WebMethodName { get; set; }

        public string RequestContent { get; set; }

        public string ResponseContent { get; set; }

        public string ExceptionStackTrace { get; set; }

        public string ExceptionMessage { get; set; }
    }
}