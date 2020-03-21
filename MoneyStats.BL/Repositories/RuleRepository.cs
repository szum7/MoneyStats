using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MoneyStats.DAL.Common;

namespace MoneyStats.BL.Repositories
{
    /// <summary>
    /// This is not yet useful as operands are created on client side and stored as string.
    /// Using this enum would mean extra conversions.
    /// </summary>
    public enum OperandEnum
    {
        instanceIsLesser = -1,
        equal = 0,
        instanceIsGreater = 1
    }

    public class RuleRepository
    {
        public RuleRepository()
        {

        }

        void Test()
        {
            var rulableProperties = (from property in typeof(Transaction).GetProperties()
                                     where property.CustomAttributes.Any(customAttribute => customAttribute.AttributeType == typeof(Rulable))
                                     select property).ToList();

            foreach (PropertyInfo property in rulableProperties)
            {
                var actualPropertyName = "AccountDate";
                var actualPropertyValue = DateTime.Now;
                var actualOperand = -1;

                var actualInstance = new Transaction();

                if (property.Name == "AccountDate")
                {
                    if (typeof(IComparable).IsAssignableFrom(property.PropertyType))
                    {
                        if ((property.GetValue(actualInstance) as IComparable).CompareTo(actualPropertyValue) == actualOperand)
                        {
                            // => Megfelel a szabálynak
                        }
                    }
                }
            }
        }
    }
}
