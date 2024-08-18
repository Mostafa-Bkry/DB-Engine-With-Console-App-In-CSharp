namespace LogicClasses
{
    public class Tables
    {
        public static Dictionary<Type, string> UserTables { get; private set; } 
            = new Dictionary<Type, string>();

        static Tables()
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

        //checking the txt files that created recently
        private static string[]? FetchExistingTables()
        {
            try
            {
                string directoryPath = @".\Created Tables";

                // Get all .txt files in the specified directory
                string[] txtFiles = Directory.GetFiles(directoryPath, "*.txt");

                // Display the file names
                foreach (string txtFile in txtFiles)
                {
                    Console.WriteLine(txtFile);
                }

                return txtFiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return default;
        }

        //Dynamic Creation of their classes as new types
        private static void CreateExistingTables(string[] existingArr)
        {
            for(int i = 0; i < existingArr?.Length; i++)
            {
                string tableName = existingArr[i];

                DynamicTypeCreation.CreateAndSaveDynamicType(tableName,);
            }
        }


        private static List<string> GetColsNames(string tableName)
        {
            List<string> columns = new List<string>();
            string filePath = @$".\Created Tables\{tableName}";

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
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static void AddTable(Type tableDynamicClass, string tableName)
        {
            UserTables.Add(tableDynamicClass, tableName);
            Console.WriteLine($"Fetched the {tableName} table to UserTables successfully");
        }
    }
}
