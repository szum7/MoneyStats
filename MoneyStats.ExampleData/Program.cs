using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.BasicValues);

#if true
            global.InsertAllExamples(); // INSERT
            global.ReadRowCounts(); // READ
#endif

#if false
            global.DeleteAllFromDatabase(); // DELETE
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
