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
            global.DropAllTables(); // DROP!
            global.InsertAllExamples(); // INSERT
            global.DeleteAllFromDatabase(); // DELETE
            global.ReadRowCounts(); // READ
            global.ReadRowCounts(); // READ
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
