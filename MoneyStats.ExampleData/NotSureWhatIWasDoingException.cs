using System;

namespace MoneyStats.ExampleData
{
    public class NotSureWhatIWasDoingException : Exception
    {
        public NotSureWhatIWasDoingException()
            : base("It's okay. Program was aborted.")
        {
        }
    }
}
