using static LogicClasses.UserPrompt;

namespace LogicClasses
{
    public class Creation
    {
        public static void CreateTables(uint numOfTables)
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
    }
}
