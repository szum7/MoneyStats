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
            //this.Messages.Add($"[{this.GetCurrentMethod(1)}] Success.");
            this.IsValid = true;
            this.Data = true;
        }
    }

    public class ErrorResponse : GenericResponse<bool>
    {
        public ErrorResponse(List<string> messages = null)
        {
            foreach (var msg in messages)
            {
                this.Messages.Add($"[{this.GetCurrentMethod(1)}]: {msg}");
            }
            this.IsValid = false;
            this.Data = false;
        }

        public ErrorResponse(string msg)
        {
            this.Messages.Add($"[{this.GetCurrentMethod(1)}]: {msg}");
            this.IsValid = false;
            this.Data = false;
        }
    }

    public class GenericResponse<DataType>
    {
        public List<string> Messages { get; set; }
        public bool IsValid { get; set; }

        public DataType Data { get; set; }

        public GenericResponse()
        {
            this.Messages = new List<string>();
        }

        public GenericResponse<DataType> Pulse()
        {
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
