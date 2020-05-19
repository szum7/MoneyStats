using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.BasicValues);

#if true
#endif

#if false
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP
            global.ReadRowCounts(); // READ
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
