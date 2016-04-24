namespace WCFService.Common.Infrastructure.BehaviorExtensions.Validation
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public abstract class CustomValidationOperationInvokerBase : IOperationInvoker, IParameterInspector
    {
        private readonly IOperationInvoker originalInvoker;
        private readonly string operationName;

        public CustomValidationOperationInvokerBase(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            this.originalInvoker = dispatchOperation.Invoker;
            this.operationName = dispatchOperation.Name;
        }
        #region IOperationBehavior Members

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            // TODO: Remove this when finished
#if DEBUG
            OperationContext operationContext = OperationContext.Current;
            ServiceSecurityContext securityContext = ServiceSecurityContext.Current;

            // VALIDATION LOGIC GOES HERE

            string user = null;
            bool isAnonymous = true;

            if (securityContext != null)
            {
                user = securityContext.PrimaryIdentity.Name;
                isAnonymous = securityContext.IsAnonymous;
            }

            Uri remoteAddress = operationContext.Channel.LocalAddress.Uri;
            string sessionId = operationContext.SessionId;
            MessageVersion messageVersion = operationContext.IncomingMessageVersion;

            Trace.WriteLine("Username: " + user);
            Trace.WriteLine("Is Anonymoys" + isAnonymous);
            Trace.WriteLine("Server address: " + remoteAddress);
            Trace.WriteLine("Session id: " + sessionId);
            Trace.WriteLine("Message version: " + messageVersion);
            Trace.WriteLine("Operation:" + this.operationName);
            Trace.WriteLine("Arguments:");
            foreach (object input in inputs)
                Trace.WriteLine(input);
#endif

            object result = this.originalInvoker.Invoke(instance, inputs, out outputs);
            return result;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            return this.originalInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult asyncResult)
        {
            object result = this.originalInvoker.InvokeEnd(instance, out outputs, asyncResult);
            return result;
        }

        public object[] AllocateInputs()
        {
            return this.originalInvoker.AllocateInputs();
        }

        public bool IsSynchronous
        {
            get { return this.originalInvoker.IsSynchronous; }
        }

        #endregion

        #region IParameterInspector Members
        public object BeforeCall(string operationName, object[] inputs)
        {
            // REQUEST VALIDATION
            this.Validate(operationName, inputs);
            return null;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }
        #endregion

        protected abstract bool Validate(string operationName, object[] inputs);
    }

    #region OperationBehavior - add as attribute on method (operation)

    /// <summary>
    /// Use this to add validation to a single method, instead of all methods in servie
    /// Example usage - add as attribute on method (operation):
    /// *[OperationContract]
    /// *[CustomValidationOperation]
    /// *string Reverse(string input);
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class CustomValidationOperationAttributeBase : Attribute, IOperationBehavior
    {
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            var invoker = this.CreateInvoker(operationDescription, dispatchOperation);
            dispatchOperation.Invoker = invoker;
            dispatchOperation.ParameterInspectors.Add(invoker);
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) {}

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation) {}

        public void Validate(OperationDescription operationDescription) {}

        protected abstract CustomValidationOperationInvokerBase CreateInvoker(OperationDescription operationDescription, DispatchOperation dispatchOperation);
    }

    #endregion

    #region ServiceBehavior - add as attribute on Service implementation class

    /// <summary>
    /// Use this to add validation to all methods (operations) in the service
    /// Example usage - add as attribute on Service implementation class:
    /// [CustomValidationServiceBehavior]
    /// public class ExampleService : IExampleService
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class CustomValidationServiceBehaviorAttributeBase : Attribute, IServiceBehavior
    {
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    IOperationBehavior behavior = this.CreateOperationBehavior();
                    operation.Behaviors.Add(behavior);
                }
            }
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

        protected abstract CustomValidationOperationBehaviorBase CreateOperationBehavior();
    }

    public abstract class CustomValidationOperationBehaviorBase : IOperationBehavior
    {
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            var invoker = this.CreateInvoker(operationDescription, dispatchOperation);
            dispatchOperation.Invoker = invoker;
            dispatchOperation.ParameterInspectors.Add(invoker);
        }

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation) { }

        public void Validate(OperationDescription operationDescription) { }

        public abstract CustomValidationOperationInvokerBase CreateInvoker(OperationDescription operationDescription, DispatchOperation dispatchOperation);
    }

    #endregion

}