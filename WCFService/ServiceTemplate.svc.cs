namespace WCFService
{
    using System;

    using WCFService.BusinessLogic;

    public class ServiceTemplate : IServiceTemplate
    {
        private readonly IExampleBusinessService businessService;

        public ServiceTemplate(IExampleBusinessService businessService)
        {
            this.businessService = businessService;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string Reverse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            return this.businessService.Reverse(input);
        }
    }
}
