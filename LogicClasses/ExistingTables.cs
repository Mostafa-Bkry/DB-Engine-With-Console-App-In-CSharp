using System.Formats.Tar;

namespace LogicClasses
{
    public class ExistingTables
    {
        public static void FetchAll()
        {
            string[]? existingTables = FetchExistingTables();
            if (existingTables != null)
            {
                if (existingTables.Length > 0)
                {
                    CreateExistingTables(existingTables);
                }
            }
        }

        #region Creating Existing Tables If found any txt file exists

        //checking the txt files that created recently
        private static string[]? FetchExistingTables()
        {
            try
            {
                string directoryPath = @".\Created Tables";

                // Get all .txt files in the specified directory
                string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");
                string[] txtFilesNamesOnly = new string[txtFiles.Length];

                // Display the file names
                for (int i = 0; i < txtFiles.Length; i++)
                {
                    txtFilesNamesOnly[i] = Path.GetFileNameWithoutExtension(txtFiles[i]);
                    Console.WriteLine($"Existing Table: {txtFilesNamesOnly[i]}");
                }

                Console.WriteLine();
                return txtFilesNamesOnly;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return default;
        }


        //Dynamic Creation of their classes as new types
        private static void CreateExistingTables(string[] existingTablesArr)
        {
            for (int i = 0; i < existingTablesArr?.Length; i++)
            {
                string tableName = existingTablesArr[i];
                List<string> cols = GetColumns(tableName) ?? new List<string>();

                if (cols.Count > 0)
                {
                    List<string> columnsName = new List<string>();
                    List<Type> columnsType = new List<Type>();

                    foreach (string col in cols)
                    {
                        Type t = UserPrompt.CheckColumnDataType(col.Split(":")[0]);

                        columnsType.Add(t);
                        columnsName.Add(col.Split(":")[1]);
                    }

                    Type table =
                        DynamicTypeCreation.CreateAndSaveDynamicType(tableName, columnsName, columnsType);
                    Tables.AddTable(table, tableName);

                    Console.WriteLine();
                }
            }
        }

        private static List<string>? GetColumns(string tableName)
        {
            List<string> columns = new List<string>();
            string filePath = @$".\Created Tables\{tableName}.txt";

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the second line from the file
                    string? secondLine = File.ReadLines(filePath).Skip(1).FirstOrDefault();

                    if (!string.IsNullOrEmpty(secondLine))
                    {
                        string[] values = secondLine.Split(',');
                        foreach (string col in values)
                        {
                            columns.Add(col);
                        }

                        return columns;
                    }
                    else
                    {
                        Console.WriteLine("File does not have a second line.");
                        return null;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Can't find this file");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        #endregion
    }
}
