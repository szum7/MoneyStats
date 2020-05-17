using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Repositories;
using MoneyStats.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MoneyStats.Tests
{
    public static class Extensions
    {
        /// <summary>
        /// Sets a value in an object, used to hide all the logic that goes into
        /// handling this sort of thing, so that is works elegantly in a single line.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public static void SetPropertyValueFromString(this object target, string propertyName, string propertyValue)
        {
            PropertyInfo oProp = target.GetType().GetProperty(propertyName);
            Type tProp = oProp.PropertyType;

            //Nullable properties have to be treated differently, since we 
            //  use their underlying property to set the value in the object
            if (tProp.IsGenericType
                && tProp.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //if it's null, just set the value from the reserved word null, and return
                if (propertyValue == null)
                {
                    oProp.SetValue(target, null, null);
                    return;
                }

                //Get the underlying type property instead of the nullable generic
                tProp = new NullableConverter(oProp.PropertyType).UnderlyingType;
            }

            //use the converter to get the correct value
            oProp.SetValue(target, Convert.ChangeType(propertyValue, tProp), null);
        }
    }

    [TestClass]
    public class Misc
    {
        [TestMethod]
        public void TestNotForeignKeyedEntityInlcude()
        {
            // Arrange
            var repo = new BankRowRepository();

            // Act
            var list = repo.GetWithEntities();

            // Assert
            Assert.AreEqual(1, 1);
        }

        public static bool Compare(string op, IComparable left, IComparable right)
        {
            switch (op)
            {
                case "<": return left.CompareTo(right) < 0;
                case ">": return left.CompareTo(right) > 0;
                case "<=": return left.CompareTo(right) <= 0;
                case ">=": return left.CompareTo(right) >= 0;
                case "==": return left.Equals(right);
                case "!=": return !left.Equals(right);
                default: throw new ArgumentException("Invalid comparison operator: {0}", op);
            }
        }

        [DataTestMethod]
        [DataRow(60, "<=", 50, false)]
        [DataRow(50, "<=", 50, true)]
        [DataRow(40, "<=", 50, true)]
        [DataRow(-2, "<=", 50, true)]
        [DataRow(0, "<=", 50, true)]
        [DataRow(0, "==", 50, false)]
        [DataRow(0, "!=", 50, true)]
        [DataRow(50, ">", 50, false)]
        [DataRow(51, ">", 50, true)]
        public void TestCompare(int ruleValue, string operand, int transactionValue, bool result)
        {
            // operand;Property;50;<=;
            // where Property <= 50

            var resultToTest = Misc.Compare(operand, ruleValue, transactionValue);
            Assert.AreEqual(resultToTest, result);

            if (operand != "==" && operand != "!=" && ruleValue != transactionValue)
            {
                var resultToTestNegated = Misc.Compare(operand, transactionValue, ruleValue);
                Assert.AreEqual(resultToTestNegated, !result);
            }
        }

        [TestMethod]
        public void TestRepository()
        {
            IBankRowRepository repo = new BankRowRepository();
            var bankRows = repo.Get().ToList<BankRow>();

            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestReflection()
        {
            if (typeof(IComparable).IsAssignableFrom(typeof(int)))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.Fail();
            }

            // d1.CompareTo(d2) < 0 -> d1 kisebb, r�gebbi d�tum mint d2
            // d1.CompareTo(d2) > 0 -> d1 nagyobb, k�s�bbi d�tum mint d2
            // d1.CompareTo(d2) = 0 -> d1 = d2
        }

        //[TestMethod]
        public void RemoveTestWithForeach()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var result = "124578";
            var actual = "";

            // Act
            foreach (int item in list) // System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'
            {
                if (item == 2)
                {
                    list.Remove(6);
                }
                else if (item == 3)
                {
                    list.Remove(3);
                }
                else
                {
                    result += item;
                }
            }

            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        public void RemoveTestWithWhile()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            var result = "124578";
            var actual = "";
            var i = 0;

            // Act
            while (i < list.Count)
            {
                var item = list[i];

                if (item == 2)
                {
                    list.Remove(6);
                    actual += item;
                }
                else if (item == 3)
                {
                    list.Remove(3);
                    i--;
                }
                else
                {
                    actual += item;
                }

                i++;
            }

            // Assert
            Assert.AreEqual(result, actual);
        }

        

        public class TestTransaction
        {
            public string StringProperty { get; set; }
            public int IntProperty { get; set; }
            public float? FloatNullProperty { get; set; }
            public float? NullProperty { get; set; }
        }

        [TestMethod]
        public void TestSetValueOfProperty()
        {
            // Arrange
            var dict = new Dictionary<string, object>();
            dict.Add("StringProperty", "string1");
            dict.Add("IntProperty", 7);
            dict.Add("FloatNullProperty", -1.3f);
            dict.Add("NullProperty", null);
            var instance = new TestTransaction();

            // Act
            foreach (var item in dict)
            {
                instance.SetPropertyValueFromString(item.Key, item.Value?.ToString());
            }

            // Assert
            Assert.AreEqual(1, 1);
        }
    }
}
