using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.BasicValues);

#if true
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT
#endif

#if false
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP
            global.ReadRowCounts(); // READ
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
