using System.Reflection;

namespace LogicClasses
{
    public class Truncation
    {
        public static void TruncateUserPrompt()
        {
            if (Tables.UserTables.Count == 0)
            {
                Console.WriteLine("\nThere are no tables to Truncate them yet!");
                return;
            }

            UserPrompt.ShowExistingTables();

            //-------------------------------------

            string? input;
            string tableName = "";

            do
            {
                Console.WriteLine("Enter Table Name that you want to Truncate from it");
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

            TruncateAll(tableName);
        }

        #region Truncate All
        private static void TruncateAll(string tableName)
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

                        Console.WriteLine($"\nTruncated {rowsCount} records successfully");
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
    }
}
