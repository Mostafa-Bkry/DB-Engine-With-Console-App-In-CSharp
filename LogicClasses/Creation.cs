using System.Xml.XPath;
using static LogicClasses.UserPrompt;

namespace LogicClasses
{
    public class Creation
    {
        private static void CreateTables(uint numOfTables)
        {
            for (int i = 0; i < numOfTables; i++)
            {
                List<Type> colDataType;

                string tableName = TableName();
                List<string> columns = Columns(out colDataType);

                CreateFileForTable(tableName, columns);

                Type table =
                    DynamicTypeCreation.CreateAndSaveDynamicType(tableName, columns, colDataType);
                Tables.AddTable(table, tableName);

                Console.WriteLine("\n-----------------------------------\n");
            }
        }

        public static void CreationUserPrompt()
        {
            string? numOfTablesInput;
            uint numOfTables;

            do
            {
                Console.WriteLine("How many tables do you want to create?");
                numOfTablesInput = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(numOfTablesInput) ||
                !uint.TryParse(numOfTablesInput, out numOfTables));

            Creation.CreateTables(numOfTables);

            foreach (KeyValuePair<Type, string> keyValue in Tables.UserTables)
            {
                Console.WriteLine($"Table Name: {keyValue.Value}");
                Console.WriteLine($"Table Type: {keyValue.Key}");
                foreach (var item in keyValue.Key.GetProperties())
                {
                    Console.WriteLine($"---{item}");
                }
            }
        }
    }
}
