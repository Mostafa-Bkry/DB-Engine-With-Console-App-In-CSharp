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


            #region Insertion Process
            string? input;
            Type? table = default;

            do
            {
                Console.WriteLine("Enter The Table Name That You Want To Insert Into it");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !Tables.UserTables.ContainsValue(input));
            #endregion

            table = Tables.UserTables.FirstOrDefault(KeyValue
                => KeyValue.Value == input).Key;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(table);
            //foreach (KeyValuePair<Type, string> keyValue in Tables.UserTables)
            //{
            //    table = keyValue.Value == input ? keyValue.Key : keyValue.Key.GetType();
            //}

            //Insertion.InsertDataIntoTable(table);

            //--------------------------------

            #region MyRegion
            //string fileContent = File.ReadAllText(@$".\{tableName}.txt");

            //string[] lines = fileContent.Split(Environment.NewLine/*, StringSplitOptions.RemoveEmptyEntries*/);

            //// the third line contains columns values
            //string[] values = lines[2].Split(',');

            //// Create an instance of the dynamic type
            //var instance = Activator.CreateInstance(table);

            //// Set property values
            //PropertyInfo nameProperty = table.GetProperty($"{columns[1]}");
            //nameProperty.SetValue(instance, values[1]);

            //// Get property values
            //string nameValue = (string)nameProperty.GetValue(instance);
            //Console.WriteLine($"Name: {nameValue}"); 
            #endregion
        }
    }
}
