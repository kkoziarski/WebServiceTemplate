namespace WCFService.BusinessLogic
{
    using System.Linq;

    public class ExampleBusinessService : IExampleBusinessService
    {
        public int Add(int x)
        {
            return x + x + 1;
        }

        public string Reverse(string input)
        {
            return new string(input.Reverse().ToArray());
        }
    }
}