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
            global.ReadRowCounts(); // READ
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT
            global.DropAllTables(); // DROP
            global.ReadRowCounts(); // READ
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
