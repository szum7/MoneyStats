using System;

namespace MoneyStats.DAL.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Rulable : Attribute
    {
    }
}
