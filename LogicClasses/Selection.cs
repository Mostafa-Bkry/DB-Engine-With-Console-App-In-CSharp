namespace LogicClasses
{
    public class Selection
    {
        public static void SelectUserPrompt()
        {
            if(Tables.UserTables.Count == 0)
            {
                Console.WriteLine("There are no tables to select from them yet!");
                return;
            }

            string? input;
            string tableName = "";

            do
            {
                Console.WriteLine("Enter Table Name that you want to select from it");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !Tables.UserTables.ContainsValue(input.ToLower()));

            tableName = input;

            do
            {
                Console.WriteLine("How many records do you want to select (All, One)");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || (input.ToLower() != "all" && input.ToLower() != "one"));


            if (input.ToLower() == "all")
            {
                SelectAll(tableName);
            }
            else
            {
                #region Getting Column Name From User to Select with it
                bool check = true;
                string columnName = "";

                while (check)
                {
                    Console.WriteLine("Enter column name to search with it");
                    input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input))
                    {
                        var r = Tables.UserTables.Keys
                                         .Where(t => t.Name.ToLower() == tableName.ToLower())
                                         .Select(table => table.GetProperties()
                                                               .FirstOrDefault(p => p.Name.ToLower() == input.ToLower()))
                                         .ToList();

                        if (r != null)
                        {
                            columnName = r[0].Name;
                            check = false;
                        }
                        else
                            Console.WriteLine("Invalid Column Name");
                    }
                }
                #endregion

                #region Getting the value that the user search for and select result
                string? valueInput = "";

                do
                {
                    Console.Write($"Select from {tableName} Where {columnName} = ");
                    valueInput = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(valueInput));

                SelectMatching(tableName, valueInput);
                #endregion
            }
        }

        #region Select All
        private static void SelectAll(string tableName)
        {
            Console.WriteLine($"------{tableName}------");

            foreach (OneTableData item in Tables.TablesData.Where(item => item.Table.Name == tableName))
            {
                foreach (string[] arr in item.Data)
                {
                    for (int i = 0; i < arr?.Length; i++)
                    {
                        if (i == arr.Length - 1)
                        {
                            Console.WriteLine($"{item.Table.GetProperties()[i].Name} = {arr[i]}");
                            break;
                        }
                        Console.Write($"{item.Table.GetProperties()[i].Name} = {arr[i]}, ");
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Select by specific value
        private static void SelectMatching(string tableName, string valueInput)
        {
            Console.WriteLine($"------{tableName}------");

            foreach (OneTableData item in Tables.TablesData.Where(item => item.Table.Name == tableName))
            {
                foreach (string[] arr in item.Data)
                {
                    for (int i = 0; i < arr?.Length; i++)
                    {
                        if (arr.Contains(valueInput))
                        {
                            if (i == arr.Length - 1)
                            {
                                Console.WriteLine($"{item.Table.GetProperties()[i].Name} = {arr[i]}");
                                break;
                            }
                            Console.Write($"{item.Table.GetProperties()[i].Name} = {arr[i]}, ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
