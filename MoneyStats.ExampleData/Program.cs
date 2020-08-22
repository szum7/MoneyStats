using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.UpdatePageWorkflowTest);

#if true
#endif

#if false
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT
            global.ReadRowCounts(); // READ
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP!
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
