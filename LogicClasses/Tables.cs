using System.Reflection;

namespace LogicClasses
{
    public class Tables
    {
        public static Dictionary<Type, string> UserTables { get; private set; } 
            = new Dictionary<Type, string>();

        public static List<OneTableData> TablesData { get; private set; } 
            = new List<OneTableData>();


        public static void AddTable(Type tableDynamicClass, string tableName)
        {
            UserTables.Add(tableDynamicClass, tableName);
            Console.WriteLine($"Fetched the {tableName} table to UserTables successfully");
        }

        public static void AddTableData(Type table, List<string[]> tabData)
        {
            if(tabData.Count > 0)
            {
                var checkIfExist = TablesData.FirstOrDefault(t => t.Table == table);

                if(checkIfExist != null)
                {
                    foreach (string[] sArr in tabData)
                    {
                        checkIfExist.Data.Add(sArr);
                    }
                }
                else
                {
                    TablesData.Add(new OneTableData(table, tabData));
                }
            }
        }
    }
}
