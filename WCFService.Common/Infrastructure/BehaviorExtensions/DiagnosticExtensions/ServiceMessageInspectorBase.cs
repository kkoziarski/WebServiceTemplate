namespace WCFService.Common.Infrastructure.BehaviorExtensions.DiagnosticExtensions
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;

    using WCFService.Common.Diagnostic;

    public abstract class ServiceMessageInspectorBase : IClientMessageInspector, IDispatchMessageInspector
    {
        private readonly bool logRequest;
        private readonly bool logReply;
        private bool isClientSide;

        [ThreadStatic]
        private bool isRequest;

        private string strRequest;
        private string strResponse;
        private string appVersion;
        private string authToken;

        private string webMethodName;

        private DateTime requestTime;
        private DateTime responseTime;

        private IWebServiceDiagnosticService diagnosticService;

        protected ServiceMessageInspectorBase(bool logRequest, bool logReply, bool isClientSide)
        {
            this.logReply = logReply;
            this.logRequest = logRequest;
            this.isClientSide = isClientSide;
            this.ResetFields();
        }

        protected abstract IWebServiceDiagnosticService InitializeDiagnosticService();

        private IWebServiceDiagnosticService GetDiagnosticService()
        {
            if (this.diagnosticService == null)
            {
                this.diagnosticService = this.InitializeDiagnosticService();
            }

            return this.diagnosticService;
        }

        #region IDispatchMessageInspector Members

        object IDispatchMessageInspector.AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //return null;
            this.ResetFields();

            Uri requestUri = request.Headers.To;
            if (this.logRequest)
            {
                this.requestTime = DateTime.UtcNow;

                try
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        HttpRequestMessageProperty httpReq = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

                        //sw.WriteLine("{0} {1}", httpReq.Method, requestUri);
                        //foreach (var header in httpReq.Headers.AllKeys)
                        //{
                        //    sw.WriteLine("{0}: {1}", header, httpReq.Headers[header]);
                        //}
                        //sw.WriteLine("{0}", requestUri.PathAndQuery);
                        this.webMethodName = requestUri.PathAndQuery;
                        if (!request.IsEmpty)
                        {
                            //sw.WriteLine();
                            sw.WriteLine(this.MessageToString(ref request, true));
                        }

                        var s = sw.ToString();
                        this.strRequest = s;

                        System.Diagnostics.Debug.WriteLine("-------AfterReceiveRequest - Message Body: " + s);
                    }
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
                //InspectMessageBody(ref request, true);
            }
            return requestUri;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref Message reply, object correlationState)
        {
            //return;
            if (this.logReply)
            {
                this.responseTime = DateTime.UtcNow;
                try
                {
                    using (StringWriter sw = new StringWriter())
                    {
                        //sw.WriteLine("Response to request to {0}:", (Uri)correlationState);
                        HttpResponseMessageProperty httpResp = (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];
                        //sw.WriteLine("{0} {1}", (int)httpResp.StatusCode, httpResp.StatusCode);

                        if (!reply.IsEmpty)
                        {
                            //sw.WriteLine();
                            sw.WriteLine(this.MessageToString(ref reply, false));
                        }

                        this.responseTime = DateTime.UtcNow;
                        var s = sw.ToString();
                        this.strResponse = s;

                        this.LogRequestResponse((int)httpResp.StatusCode);

                        System.Diagnostics.Debug.WriteLine("------BeforeSendReply - Message Body: " + s);
                    }
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
                //InspectMessageBody(ref request, true);
            }
        }

        #endregion

        #region IClientMessageInspector Members

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (this.logReply)
            {
                //InspectMessageBody(ref reply, false);
            }
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (this.logRequest)
            {
                //InspectMessageBody(ref request, true);
            }
            return null;
        }
        #endregion


        private WebContentFormat GetMessageContentFormat(Message message)
        {
            WebContentFormat format = WebContentFormat.Default;
            if (message.Properties.ContainsKey(WebBodyFormatMessageProperty.Name))
            {
                WebBodyFormatMessageProperty bodyFormat;
                bodyFormat = (WebBodyFormatMessageProperty)message.Properties[WebBodyFormatMessageProperty.Name];
                format = bodyFormat.Format;
            }

            return format;
        }

        private string MessageToString(ref Message message, bool isRequest)
        {
            WebContentFormat messageFormat = this.GetMessageContentFormat(message);
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = null;
            switch (messageFormat)
            {
                case WebContentFormat.Default:
                case WebContentFormat.Xml:
                    writer = XmlDictionaryWriter.CreateTextWriter(ms);
                    break;
                case WebContentFormat.Json:
                    writer = JsonReaderWriterFactory.CreateJsonWriter(ms);
                    break;
                case WebContentFormat.Raw:
                    return CreateRawResponse(ref message);
            }

            message.WriteMessage(writer);
            writer.Flush();
            string messageBody = Encoding.UTF8.GetString(ms.ToArray());

            // Here would be a good place to change the message body, if so desired.

            // now that the message was read, it needs to be recreated.
            ms.Position = 0;

            // if the message body was modified, needs to reencode it, as show below
            // ms = new MemoryStream(Encoding.UTF8.GetBytes(messageBody));

            XmlDictionaryReader reader;
            XmlDictionaryReader xreader;
            //parse request - find field and its value
            if (messageFormat == WebContentFormat.Json)
            {
                xreader = JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
            }
            else
            {
                xreader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
            }

            this.ParseRequest(xreader);
            ms.Position = 0;

            if (messageFormat == WebContentFormat.Json)
            {
                reader = JsonReaderWriterFactory.CreateJsonReader(ms, XmlDictionaryReaderQuotas.Max);
            }
            else
            {
                reader = XmlDictionaryReader.CreateTextReader(ms, XmlDictionaryReaderQuotas.Max);
            }

            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            newMessage.Headers.CopyHeadersFrom(message.Headers);
            message = newMessage;

            return messageBody;
        }

        private static string CreateRawResponse(ref Message message)
        {
            object response;
            if (message.Properties.TryGetValue(HttpResponseMessageProperty.Name, out response))
            {
                HttpResponseMessageProperty httpResponse = (HttpResponseMessageProperty)response;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    return "[Raw Data] | " + httpResponse.StatusCode;
                }
            }

            // special case for raw, easier implemented separately
            //return this.ReadRawBody(ref message);
            return "[Raw Data]";
        }

        private string ReadRawBody(ref Message message)
        {
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            bodyReader.ReadStartElement("Binary");
            byte[] bodyBytes = bodyReader.ReadContentAsBase64();
            string messageBody = Encoding.UTF8.GetString(bodyBytes);

            // Now to recreate the message
            MemoryStream ms = new MemoryStream();
            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter(ms);
            writer.WriteStartElement("Binary");
            writer.WriteBase64(bodyBytes, 0, bodyBytes.Length);
            writer.WriteEndElement();
            writer.Flush();
            ms.Position = 0;
            XmlDictionaryReader reader = XmlDictionaryReader.CreateBinaryReader(ms, XmlDictionaryReaderQuotas.Max);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, message.Version);
            newMessage.Properties.CopyProperties(message.Properties);
            message = newMessage;

            return messageBody;
        }

        private void ParseRequest(XmlDictionaryReader reader)
        {
            if (reader == null || reader.EOF)
            {
                return;
            }
            string lastElement = null;
            string elementValue = null;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        lastElement = reader.LocalName;
                        break;
                    case XmlNodeType.Text:

                        if (!String.IsNullOrEmpty(lastElement))
                        {
                            elementValue = reader.Value;
                            switch (lastElement.ToUpper())
                            {
                                case "APPVERSION":
                                    this.appVersion = elementValue;
                                    break;
                                case "AUTHTOKEN":
                                case "USERTOKEN":
                                    this.authToken = elementValue;
                                    break;
                            }

                        }
                        break;
                }
            }
        }

        private void ResetFields()
        {
            this.strRequest = null;
            this.strResponse = null;
            this.appVersion = null;
            this.authToken = null;
            this.webMethodName = null;
            this.requestTime = DateTime.UtcNow;
            this.responseTime = DateTime.UtcNow;
        }

        private void LogRequestResponse(int code)
        {
            this.GetDiagnosticService().LogRequestResponse(new DiagnosticLog(requestTime: this.requestTime, responseTime: this.responseTime, eventLevel: code, appVersion: this.appVersion, authToken: this.authToken, webMethodName: this.webMethodName, requestContent: this.strRequest, responseContent: this.strResponse, exceptionStackTrace: null, exceptionMessage: null));
        }

        private void LogError(Exception exception)
        {
            try
            {
                this.GetDiagnosticService().LogRequestResponse(new DiagnosticLog(requestTime: this.requestTime, responseTime: this.responseTime, eventLevel: 3000, appVersion: this.appVersion, authToken: this.authToken, webMethodName: this.webMethodName, requestContent: this.strRequest, responseContent: this.strResponse, exceptionStackTrace: exception == null ? null : exception.StackTrace, exceptionMessage: exception == null ? null : exception.Message));
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        #region Unused code


        void InspectMessageBody_msdn(ref Message message, bool isRequest)
        {
            if (!message.IsFault)
            {
                XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
                XmlReader bodyReader = message.GetReaderAtBodyContents().ReadSubtree();
                XmlReaderSettings wrapperSettings = new XmlReaderSettings();
                wrapperSettings.CloseInput = true;
                wrapperSettings.Schemas = null;
                wrapperSettings.ValidationFlags = XmlSchemaValidationFlags.None;
                wrapperSettings.ValidationType = ValidationType.None;
                XmlReader wrappedReader = XmlReader.Create(bodyReader, wrapperSettings);

                // pull body into a memory backed writer to validate
                this.isRequest = isRequest;
                MemoryStream memStream = new MemoryStream();
                XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateBinaryWriter(memStream);
                xdw.WriteNode(wrappedReader, false);
                xdw.Flush(); memStream.Position = 0;
                XmlDictionaryReader xdr = XmlDictionaryReader.CreateBinaryReader(memStream, quotas);

                // reconstruct the message with the validated body
                Message replacedMessage = Message.CreateMessage(message.Version, null, xdr);
                replacedMessage.Headers.CopyHeadersFrom(message.Headers);
                replacedMessage.Properties.CopyProperties(message.Properties);
                message = replacedMessage;

                // string content = xdr.ReadOuterXml();
            }
        }

        void InspectMessageBody_tests(ref System.ServiceModel.Channels.Message message, bool isRequest)
        {
            //if (!message.IsFault)
            //{
            //    using (StringWriter stream = new StringWriter())
            //    {
            //        using (XmlWriter writer = XmlWriter.Create(stream))
            //        {
            //            MessageBuffer buffer = message.CreateBufferedCopy(Int32.MaxValue);
            //            buffer.CreateMessage().WriteMessage(writer);
            //            writer.Flush();
            //            var s = stream.ToString();

            //            System.Diagnostics.Debug.WriteLine("-------Message Body: " + s);
            //        }
            //    }
            //}

            //if (!message.IsFault)
            //{
            //    string s = "";
            //    MessageBuffer buffer = message.CreateBufferedCopy(Int32.MaxValue);
            //    //buffer.CreateMessage().WriteMessage(writer);
            //    using (var reader = buffer.CreateMessage().GetReaderAtBodyContents())
            //    {
            //        if (reader.Read())
            //        {
            //            s = new string(Encoding.ASCII.GetChars(reader.ReadContentAsBase64()));
            //            System.Diagnostics.Debug.WriteLine("-------Message Body: " + s);
            //        }
            //    }
            //}


            //MessageBuffer buffer = message.CreateBufferedCopy(Int32.MaxValue);
            //message = buffer.CreateMessage();
            ////Console.WriteLine("Received:\n{0}", buffer.CreateMessage().ToString());
            //var s = buffer.CreateMessage().ToString();
        }


        #endregion
    }
}