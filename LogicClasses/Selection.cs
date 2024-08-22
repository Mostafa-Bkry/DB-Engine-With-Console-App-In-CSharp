using System.Reflection;

namespace LogicClasses
{
    public class Selection
    {
        public static void SelectUserPrompt()
        {
            if (Tables.UserTables.Count == 0)
            {
                Console.WriteLine("\nThere are no tables to select from them yet!");
                return;
            }

            UserPrompt.ShowExistingTables();

            //-------------------------------------

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
                #region Showing Table Columns To user
                //table columns
                var tableColumns = Tables.UserTables.Keys
                                         .Where(t => t.Name.ToLower() == tableName.ToLower())
                                         .Select(table => table.GetProperties())
                                         .ToList();

                string[]? colNames = default;

                if (tableColumns != null && tableColumns.Count > 0)
                {
                    colNames = new string[tableColumns[0].Length];

                    Console.WriteLine($"\n------{tableName} Columns------");
                    for (int i = 0; i < tableColumns?.Count; i++)
                    {
                       PropertyInfo[]? column = tableColumns[i];

                        for (int j = 0; j < column?.Length; j++)
                        {
                            colNames[j] = column[j].Name.ToLower();

                            if (j == column.Length - 1)
                            {
                                Console.WriteLine($"{column[j].Name}");
                                break;
                            }
                            Console.Write($"{column[j].Name} / ");
                        }
                    }
                    Console.WriteLine();
                }
                #endregion

                #region Getting Column Name From User to Select with it
                bool check = true;
                string columnName = "";
                int colIndex = 0;

                while (check)
                {
                    Console.WriteLine("Enter column name to search with it");
                    input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input))
                    {
                        //var r = Tables.UserTables.Keys
                        //                 .Where(t => t.Name.ToLower() == tableName.ToLower())
                        //                 .Select(table => table.GetProperties()
                        //                                       .FirstOrDefault(p => p.Name.ToLower() == input.ToLower()))
                        //                 .ToList();

                        string? r = colNames?.FirstOrDefault(n => n == input.ToLower());

                        if (r != null)
                        {
                            columnName = r;
                            colIndex = Array.IndexOf(colNames ?? [], r);
                            check = false;
                        }
                        else
                            Console.WriteLine("Invalid Column Name");
                    }
                }
                #endregion

                #region Getting the value that the user search for and select result With data Validation
                string? valueInput = "";

                while(true)
                {
                    do
                    {
                        Console.Write($"Select from {tableName} Where {columnName} = ");
                        valueInput = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(valueInput));

                    var dataChecker = Tables.UserTables.Keys
                                             .Where(t => t.Name.ToLower() == tableName.ToLower())
                                             .Select(table => table.GetProperties())
                                             .Select(props => props[colIndex].PropertyType)
                                             .FirstOrDefault();

                    if(dataChecker != null && Insertion.ConvertToPropertyType(valueInput, dataChecker) != null)
                    {
                        check = false;
                        break;
                    }
                }


                SelectMatching(tableName, valueInput, colIndex);
                #endregion
            }
        }

        #region Select All
        private static void SelectAll(string tableName)
        {
            Console.WriteLine($"\n------{tableName}------");

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
        private static void SelectMatching(string tableName, string valueInput, int columnIndex)
        {
            Console.WriteLine($"\n------{tableName}------");

            foreach (OneTableData item in Tables.TablesData.Where(item => item.Table.Name == tableName))
            {
                foreach (string[] arr in item.Data)
                {
                    for (int i = 0; i < arr?.Length; i++)
                    {
                        if (arr[columnIndex] == valueInput)
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
