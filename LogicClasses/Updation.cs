using System.Reflection;

namespace LogicClasses
{
    public class Updation
    {
        public static void UpdateUserPrompt()
        {
            if (Tables.UserTables.Count == 0)
            {
                Console.WriteLine("\nThere are no tables to Update them yet!");
                return;
            }

            UserPrompt.ShowExistingTables();

            //-------------------------------------

            string? input;
            string tableName = "";

            do
            {
                Console.WriteLine("Enter Table Name that you want to Update on it");
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

            #region Getting From User Column Name that he wants to Update
            bool check = true;
            string columnName = "";
            int colIndex = 0;

            while (check)
            {
                Console.WriteLine("Enter column name that you want to Update");
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

            #region Getting the value that will be the new value
            string? newValueInput = "";

            while (true)
            {
                do
                {
                    Console.WriteLine($"\nUpdate {tableName}");
                    Console.Write($"Set {columnName} = ");
                    newValueInput = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(newValueInput));

                var dataChecker = Tables.UserTables.Keys
                                         .Where(t => t.Name.ToLower() == tableName.ToLower())
                                         .Select(table => table.GetProperties())
                                         .Select(props => props[colIndex].PropertyType)
                                         .FirstOrDefault();

                if (dataChecker != null && Insertion.ConvertToPropertyType(newValueInput, dataChecker) != null)
                    break;
            }
            #endregion

            do
            {
                Console.WriteLine("\nHow many records do you want to Update (All, One)");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || (input.ToLower() != "all" && input.ToLower() != "one"));


            if (input.ToLower() == "all")
            {
                #region Checking ID Duplication
                if (columnName.ToLower() == "id" || columnName.ToLower().Contains("_id") ||
                                    columnName.ToLower().Contains("id_") || columnName.ToLower().Contains("-id") ||
                                    columnName.ToLower().Contains("id-"))
                {
                    Console.WriteLine("Can't Update Ids with same value (Invalid Duplicate IDs)");
                    return;
                }
                #endregion

                UpdateAll(tableName, newValueInput, colIndex);
            }
            else
            {
                #region Getting From User Column Name that he wants to search with it to Update
                check = true;
                string conditionColumnName = "";
                int conditionColIndex = 0;

                while (check)
                {
                    Console.WriteLine("\nEnter Where condition column name");
                    input = Console.ReadLine();

                    if (!string.IsNullOrEmpty(input))
                    {
                        string? r = colNames?.FirstOrDefault(n => n == input.ToLower());

                        if (r != null)
                        {
                            conditionColumnName = r;
                            conditionColIndex = Array.IndexOf(colNames ?? [], r);
                            check = false;
                        }
                        else
                            Console.WriteLine("Invalid Column Name");
                    }
                }
                #endregion

                #region Getting the value of column in where condition that will search by it for the right row
                string? whereValueInput = "";

                while (true)
                {
                    do
                    {
                        Console.WriteLine($"\nUpdate {tableName}");
                        Console.WriteLine($"Set {columnName} = {newValueInput}");
                        Console.Write($"Where {conditionColumnName} = ");
                        whereValueInput = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(whereValueInput));

                    var dataChecker = Tables.UserTables.Keys
                                             .Where(t => t.Name.ToLower() == tableName.ToLower())
                                             .Select(table => table.GetProperties())
                                             .Select(props => props[conditionColIndex].PropertyType)
                                             .FirstOrDefault();

                    if (dataChecker != null && Insertion.ConvertToPropertyType(whereValueInput, dataChecker) != null)
                        break;
                }
                #endregion

                #region Checking ID Duplication
                if (conditionColumnName.ToLower() == "id" || conditionColumnName.ToLower().Contains("_id") ||
                                    conditionColumnName.ToLower().Contains("id_") || conditionColumnName.ToLower().Contains("-id") ||
                                    conditionColumnName.ToLower().Contains("id-"))
                {
                    var c = Tables.TablesData
                              .Where(table => table.Table.Name.ToLower() == tableName)
                              .Select(obj => obj.Data.Find(arr => arr[conditionColIndex] == newValueInput))
                              .ToList();

                    if (!c.Contains(null) && c.Count > 0)
                    {
                        Console.WriteLine("Can't Update Ids with same value (Invalid Duplicate IDs)");
                        return;
                    }
                }
                #endregion

                UpdateMatching(tableName, whereValueInput, newValueInput, conditionColIndex, colIndex);
            }
        }

        #region Update All
        private static void UpdateAll(string tableName, string newValue, int columnIndex)
        {
            var check = Tables.TablesData?.Where(item => item.Table.Name == tableName)?.ToList();

            if (check != null && check.Count > 0)
            {
                int rowsCount = check[0]?.Data?.Count ?? 0;

                if (rowsCount > 0)
                {
                    try
                    {
                        #region Updating table at TablesData List
                        check[0].Data.ForEach(row => row[columnIndex] = newValue);
                        #endregion

                        #region Updating its File
                        try
                        {
                            string filePath = $@".\Created Tables\{tableName.ToLower()}.txt";

                            string[] lines = File.ReadAllLines(filePath);

                            // Keeping the first two lines (header)
                            string[] header = lines.Take(2).ToArray();

                            // Write the header lines back to the file
                            File.WriteAllLines(filePath, header);

                            using (StreamWriter sw = new StreamWriter(filePath, true))
                            {
                                for (int i = 2; i < lines.Length; i++)
                                {
                                    string[] row = lines[i].Split(',');
                                    row[columnIndex] = newValue;

                                    for (int j = 0; j < row?.Length; j++)
                                    {
                                        // Write the remaining valid lines back to the file
                                        if (j == row.Length - 1)
                                        {
                                            sw.WriteLine($"{row[j]}");
                                            break;
                                        }
                                        sw.Write($"{row[j]},");
                                    }
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

                        Console.WriteLine($"\nUpdated {rowsCount} records successfully");
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

        #region Update by specific value
        private static void UpdateMatching
            (string tableName, string whereValue, string newValue, int wherecolIndex, int updatedColIndex)
        {
            int updatedCount = 0;

            #region Updating at TablesData List
            foreach (OneTableData item in Tables.TablesData.Where(item => item.Table.Name == tableName))
            {
                var data = item.Data.ToList();
                updatedCount = item.Data.FindAll(arr => arr[wherecolIndex] == whereValue).Count;

                for (int i = 0; i < item.Data.Count; i++)
                {
                    if (data[i][wherecolIndex] == whereValue)
                        Tables.TablesData
                              .Where(item => item.Table.Name == tableName)
                              .Select(obj => obj.Data)
                              .ToList()[0][i][updatedColIndex] = newValue;
                }
            }
            #endregion

            #region Updating at its File
            try
            {

                string filePath = $@".\Created Tables\{tableName.ToLower()}.txt";

                string[] lines = File.ReadAllLines(filePath);

                // Keeping the first two lines (header)
                string[] header = lines.Take(2).ToArray();

                // Write the header lines back to the file
                File.WriteAllLines(filePath, header);

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    for (int i = 2; i < lines.Length; i++)
                    {
                        string[] row = lines[i].Split(',');

                        if (row[wherecolIndex] == whereValue)
                        {
                            row[updatedColIndex] = newValue;

                            for (int j = 0; j < row?.Length; j++)
                            {
                                // Write the remaining valid lines back to the file
                                if (j == row.Length - 1)
                                {
                                    sw.WriteLine($"{row[j]}");
                                    break;
                                }
                                sw.Write($"{row[j]},");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < row?.Length; j++)
                            {
                                // Write the remaining valid lines back to the file
                                if (j == row.Length - 1)
                                {
                                    sw.WriteLine($"{row[j]}");
                                    break;
                                }
                                sw.Write($"{row[j]},");
                            }
                        }
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

            Console.WriteLine($"\nUpdated {updatedCount} records successfully");
        }
        #endregion
    }
}
