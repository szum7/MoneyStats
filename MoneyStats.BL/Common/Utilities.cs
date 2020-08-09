using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyStats.BL.Common
{
    class Utilities
    {
        /// <summary>
        /// left op right => true/false
        /// 1 < 2 => true
        /// </summary>
        /// <param name="left"></param>
        /// <param name="op"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Compare(IComparable left, string op, IComparable right)
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
    }
}
