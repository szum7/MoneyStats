using System;

namespace MoneyStats.ExampleData
{
    class Program
    {
        static void Main(string[] args)
        {
            var global = new Global(CustomData.UpdateWorkflowTest);

#if true
            global.ReadRowCounts(); // READ
#endif

#if false
            global.DeleteAllFromDatabase(); // DELETE
            global.InsertAllExamples(); // INSERT
            global.ReadRowCounts(); // READ
            global.DropAllTables(); // DROP!
#endif

            Console.WriteLine("Program ended.");
        }
    }
}
