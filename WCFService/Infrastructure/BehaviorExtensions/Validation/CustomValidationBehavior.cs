namespace WCFService.Infrastructure.BehaviorExtensions.Validation
{
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using WCFService.Common.DataContract.Shared;
    using WCFService.Common.Infrastructure.BehaviorExtensions.Validation;

    public sealed class UserTokenValidationOperationInvoker : CustomValidationOperationInvokerBase
    {
        public UserTokenValidationOperationInvoker(OperationDescription operationDescription, DispatchOperation operation)
            : base(operationDescription, operation)
        {
        }

        protected override bool Validate(string operationName, object[] inputs)
        {
            // VALIDATION LOGIC GOES HERE

            if (inputs == null || inputs.Length == 0)
            {
                return true;
            }

            string value = inputs[0] as string;
            if (value == "throw")
            {
                throw new FaultException(string.Format("[{0}]: Invalid webservice request", operationName));
            }

            foreach (var input in inputs)
            {
                this.ValidateUserToken(operationName, input);
            }

            return true;
        }

        private bool ValidateUserToken(string operationName, object input)
        {
            TokenServiceRequest tokenRequest = input as TokenServiceRequest;
            if (tokenRequest == null)
            {
                // if this is not TokenServiceRequest, nothing to validate
                return true;
            }

            if (string.IsNullOrEmpty(tokenRequest.UserToken) || string.IsNullOrEmpty(tokenRequest.AppVersion))
            {
                throw new FaultException(string.Format("[{0}]: Invalid empty webservice request", operationName));
            }

            //TODO: update after ninject add
            //IKernel kernel = NinjectWebCommon.CreateKernel(); //new StandardKernel(new OfflineServiceModule());
            //var service = kernel.Get<IJobOfflineDataBusService>();
            //var validationResult = service.ValidateUserForToken(tokenRequest.UserToken);
            //if (validationResult == null)
            //{
            //    throw new FaultException("Invalid token");
            //}

            //tokenRequest.ValidatedUserId = validationResult.UserID;

            return true;
        }
    }

    #region OperationBehavior - add as attribute on method (operation)

    /// <summary>
    /// Use this to add validation to a single method, instead of all methods in servie
    /// Example usage - add as attribute on method (operation):
    /// *[OperationContract]
    /// *[CustomValidationOperation]
    /// *string Reverse(string input);
    /// </summary>
    /// [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ValidateUserTokenAttribute : CustomValidationOperationAttributeBase
    {
        protected override CustomValidationOperationInvokerBase CreateInvoker(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            return new UserTokenValidationOperationInvoker(operationDescription, dispatchOperation);
        }
    }

    #endregion

    #region ServiceBehavior - add as attribute on Service implementation class

    /// <summary>
    /// Use this to add validation to all methods (operations) in the service
    /// Example usage - add as attribute on Service implementation class:
    /// [CustomValidationServiceBehavior]
    /// public class ExampleService : IExampleService
    /// </summary>
    /// [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CustomValidationServiceBehaviorAttribute : CustomValidationServiceBehaviorAttributeBase
    {
        protected override CustomValidationOperationBehaviorBase CreateOperationBehavior()
        {
            return new CustomValidationOperationBehavior();
        }
    }

    public sealed class CustomValidationOperationBehavior : CustomValidationOperationBehaviorBase
    {
        public override CustomValidationOperationInvokerBase CreateInvoker(OperationDescription operationDescription, DispatchOperation operation)
        {
            return new UserTokenValidationOperationInvoker(operationDescription, operation);
        }
    }

    #endregion

}