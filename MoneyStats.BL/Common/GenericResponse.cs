using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MoneyStats.BL.Common
{
    public class SuccessResponse: GenericResponse<bool>
    {
        public SuccessResponse()
        {
            this.Message = $"[{this.GetCurrentMethod(1)}] Success.";
            this.IsError = false;
            this.Data = true;
        }
    }

    public class ErrorResponse : GenericResponse<bool>
    {
        public ErrorResponse(string message = "")
        {
            this.Message = $"[{this.GetCurrentMethod(1)}]" + message == "" ? "" : $" {message}";
            this.IsError = true;
            this.Data = false;
        }
    }

    public class GenericResponse<DataType>
    {
        public string Message { get; set; }
        public List<string> Messages { get; set; }
        public bool IsError { get; set; }

        public DataType Data { get; set; }

        public GenericResponse<DataType> Pulse()
        {
            Console.WriteLine(this.Message);

            if (this.Messages != null)
            {
                foreach (var m in this.Messages)
                {
                    Console.WriteLine(m);
                }
            }

            return this;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod(int callerOffset)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1 + callerOffset);

            return sf.GetMethod().Name;
        }
    }
}
