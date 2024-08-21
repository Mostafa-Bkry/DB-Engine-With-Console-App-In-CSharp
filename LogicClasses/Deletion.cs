using System.Reflection;

namespace LogicClasses
{
    public class Deletion
    {
        public static void DeleteUserPrompt()
        {
            if (Tables.UserTables.Count == 0)
            {
                Console.WriteLine("There are no tables to Delete from them yet!");
                return;
            }

            UserPrompt.ShowExistingTables();

            //-------------------------------------

            string? input;
            string tableName = "";

            do
            {
                Console.WriteLine("Enter Table Name that you want to Delete from it");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !Tables.UserTables.ContainsValue(input.ToLower()));

            tableName = input;

            //Checking if the table has no data
            if ((Tables.TablesData?
                       .FirstOrDefault(t => t.Table.Name.ToLower() == tableName.ToLower())?
                       .Data.Count ?? 0) < 1)
            {
                Console.WriteLine($"\nThe {tableName} table already has no records");
                return;
            }

            do
            {
                Console.WriteLine("How many records do you want to delete (All, One)");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || (input.ToLower() != "all" && input.ToLower() != "one"));


            if (input.ToLower() == "all")
            {
                DeleteAll(tableName);
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

                #region Getting Column Name From User to Delete with it
                bool check = true;
                string columnName = "";
                int colIndex = 0;

                while (check)
                {
                    Console.WriteLine("Enter column name to search with it");
                    input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input))
                    {
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

                #region Getting the value that the user search for and delete result
                string? valueInput = "";

                while (true)
                {
                    do
                    {
                        Console.Write($"Delete from {tableName} Where {columnName} = ");
                        valueInput = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(valueInput));

                    var dataChecker = Tables.UserTables.Keys
                                             .Where(t => t.Name.ToLower() == tableName.ToLower())
                                             .Select(table => table.GetProperties())
                                             .Select(props => props[colIndex].PropertyType)
                                             .FirstOrDefault();

                    if (dataChecker != null && Insertion.ConvertToPropertyType(valueInput, dataChecker) != null)
                    {
                        check = false;
                        break;
                    }
                }


                DeleteMatching(tableName, valueInput, colIndex);
                #endregion
            }
        }

        #region Delete All
        private static void DeleteAll(string tableName)
        {
            var check = Tables.TablesData?.Where(item => item.Table.Name == tableName)?.ToList();
            if (check != null && check.Count > 0)
            {
                int rowsCount = check[0]?.Data?.Count ?? 0;

                if (rowsCount > 0)
                {
                    try
                    {
                        #region Deleting from TablesData List
                        check[0].Data.Clear();
                        #endregion

                        #region Deleting from its File
                        string filePath = $@".\Created Tables\{tableName.ToLower()}.txt";

                        string[] lines = File.ReadAllLines(filePath);

                        // Keeping the first two lines (header)
                        string[] remainingLines = lines.Take(2).ToArray();

                        // Write the remaining lines back to the file
                        File.WriteAllLines(filePath, remainingLines);
                        #endregion

                        Console.WriteLine($"\nDeleted {rowsCount} records successfully");
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine($"Can't find {tableName}.txt");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"\nThe {tableName} table already has no records");
            }
        }
        #endregion

        #region Delete by specific value
        private static void DeleteMatching(string tableName, string valueInput, int columnIndex)
        {
            int deletedCount = 0;

            #region Deleting from TablesData List
            foreach (OneTableData item in Tables.TablesData.Where(item => item.Table.Name == tableName))
            {
                for (int i = 0; i < item.Data.Count; i++)
                {
                    string[] arr = item.Data[i];
                    deletedCount = item.Data.FindAll(arr => arr[columnIndex] == valueInput).Count;
                    item.Data.FindAll(arr => arr[columnIndex] == valueInput).Remove(arr);

                    //if (arr[columnIndex] == valueInput)
                    //{
                    //    item.Data.Remove(arr);
                    //}

                    //if (arr.Contains(valueInput))
                    //{
                    //var r = Tables.TablesData
                    //               .Where(item => item.Table.Name == tableName)
                    //               .Select(t => t.Data.FindAll(dataArr => dataArr[columnIndex] == valueInput))
                    //               .ToList();

                    //    deletedCount = r[0].Count();
                    //}
                }
            }
            #endregion

            #region Deleting from its File
            try
            {

                string filePath = $@".\Created Tables\{tableName.ToLower()}.txt";

                string[] lines = File.ReadAllLines(filePath);

                // Keeping the first two lines (header)
                string[] header = lines.Take(2).ToArray();

                // Write the header lines back to the file
                File.WriteAllLines(filePath, header);

                using(StreamWriter sw = new StreamWriter(filePath, true))
                {
                    for (int i = 2; i < lines.Length; i++)
                    {
                        string[] row = lines[i].Split(',');

                        if (row[columnIndex] == valueInput)
                            continue;

                        // Write the remaining valid lines back to the file
                        sw.WriteLine(lines[i]);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Can't find {tableName}.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            #endregion

            Console.WriteLine($"\nDeleted {deletedCount} records successfully");
        }
        #endregion
    }
}
