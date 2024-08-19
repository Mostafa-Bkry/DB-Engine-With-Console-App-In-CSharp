namespace LogicClasses
{
    public class Tables : Creation
    {
        public static Dictionary<Type, string> UserTables { get; private set; } 
            = new Dictionary<Type, string>();


        public static void AddTable(Type tableDynamicClass, string tableName)
        {
            UserTables.Add(tableDynamicClass, tableName);
            Console.WriteLine($"Fetched the {tableName} table to UserTables successfully");
        }
    }
}
