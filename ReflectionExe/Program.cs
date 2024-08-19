using LogicClasses;
using System.Linq;

namespace ReflectionExe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Fetching Existing Tables On the App Starting
            //Fetching already existing tables
            ExistingTables.FetchAll();
            Console.WriteLine("\n-----------------------\n");
            #endregion

            //--------------------------------

            #region Creation Process
            //string? numOfTablesInput;
            //uint numOfTables;

            //do
            //{
            //    Console.WriteLine("How many tables do you want to create?");
            //    numOfTablesInput = Console.ReadLine();
            //}
            //while (string.IsNullOrEmpty(numOfTablesInput) ||
            //    !uint.TryParse(numOfTablesInput, out numOfTables));

            //Creation.CreateTables(numOfTables);

            //foreach (KeyValuePair<Type, string> keyValue in Tables.UserTables)
            //{
            //    Console.WriteLine($"Table Name: {keyValue.Value}");
            //    Console.WriteLine($"Table Type: {keyValue.Key}");
            //    foreach (var item in keyValue.Key.GetProperties())
            //    {
            //        Console.WriteLine($"---{item}");
            //    }
            //}
            #endregion

            //--------------------------------

            #region Insertion Process
            string? input;
            Type? table = default;

            do
            {
                Console.WriteLine("Enter The Table Name That You Want To Insert Into it");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !Tables.UserTables.ContainsValue(input.ToLower()));

            table = Tables.UserTables.FirstOrDefault(KeyValue
                => KeyValue.Value == input.ToLower()).Key;


            Insertion.InsertDataIntoTable(table);
            #endregion

        }
    }
}
