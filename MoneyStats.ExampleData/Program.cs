using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.UpdatePageWorkflowTest);

#if true
            global.ReadRowCounts(); // READ
#endif

#if false
            global.InsertAllExamples(); // INSERT
            global.DeleteAllFromDatabase(); // DELETE
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP!
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
