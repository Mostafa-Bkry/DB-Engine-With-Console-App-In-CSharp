namespace LogicClasses
{
    public static class UserPrompt
    {
        #region Table
        //Enter Table
        public static string TableName()
        {
            string? input = default;
            bool check = true;
            while (check)
            {
                do
                {
                    Console.WriteLine("Enter Table Name: ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input));

                if (CheckIfFileExist(input))
                {
                    Console.WriteLine($"You already has The {input} table");
                    Console.WriteLine("You can only insert, update or delete on this table");
                }
                else
                {
                    check = false;
                }
            }


            return input;
        }

        //Check If the user already created the table
        private static bool CheckIfFileExist(string fileName)
        {
            try
            {
                StreamReader sr = new StreamReader($@".\{fileName.ToLower()}.txt");
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
        #endregion

        #region Columns
        //Table's Columns
        public static List<string> Columns(out List<Type> colsDataType)
        {
            List<string> cols = new List<string>();
            colsDataType = new List<Type>();

            string? input;
            uint colsNumber;

            do
            {
                Console.WriteLine("Enter How Many Columns: ");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !uint.TryParse(input, out colsNumber));

            for (uint i = 0; i < colsNumber; i++)
            {
                do
                {
                    Console.WriteLine($"Enter The {i + 1} Column Name: ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input));
                cols.Add(input);

                do
                {
                    Console.WriteLine($"Enter The {i + 1} Column Data Type: ");
                    Console.WriteLine("Note: Enter the following types as provided exactly");
                    Console.WriteLine(" char , varchar");
                    input = Console.ReadLine();
                }                                  
                while (string.IsNullOrEmpty(input) || CheckColumnDataType(input) == null);
                colsDataType.Add(CheckColumnDataType(input));
            }

            Console.WriteLine("\n-----------------------------------");
            return cols;
        }

        private static Type? CheckColumnDataType(string typeString)
        {
            switch (typeString.ToLower())
            {
                // Exact numerics
                case "bit":
                    return typeof(bool);
                case "tinyint":
                    return typeof(byte);
                case "smallint":
                    return typeof(short);
                case "int":
                    return typeof(int);
                case "bigint":
                    return typeof(long);
                case "decimal":
                case "numeric":
                case "money":
                case "smallmoney":
                    return typeof(decimal); // Placeholder for numeric types

                // Approximate numerics
                case "float":
                    return typeof(float);
                case "real":
                    return typeof(double); // Placeholder for approximate numeric types

                // Date and time
                case "date":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                case "smalldatetime":
                case "time":
                    return typeof(DateTime);

                // Character strings
                case "char":
                case "varchar":
                case "text":
                    return typeof(string);

                // Unicode character strings
                case "nchar":
                case "nvarchar":
                    return typeof(string);

                // Binary strings
                case "binary":
                case "varbinary":
                    return typeof(byte[]);

                // Other data types (add more as needed)
                case "xml":
                    return typeof(string); // Placeholder for XML type

                default:
                    return null;
                    /*throw new ArgumentException("Invalid SQL Server data type. Please enter a valid type.")*/;
            }
        }
        #endregion

        #region File Creation
        //Create the file
        public static void CreateFileForTable(string tableName, List<Type> colDataType, List<string> columns)
        {
            //FileHeader
            using (StreamWriter sw = new StreamWriter(@$".\Created Tables\{tableName.ToLower()}.txt", false))
            {
                sw.WriteLine($"{tableName}");
                for (int i = 0; i < columns?.Count; i++)
                {
                    if (i == columns.Count - 1)
                    {
                        sw.WriteLine($"{colDataType[i]}:{columns[i]}");
                        break;
                    }
                    sw.Write($"{colDataType[i]}:{columns[i]},");
                }

                //for (int j = 0; j < columns?.Count; j++)
                //{
                //    if (j == columns.Count - 1)
                //    {
                //        sw.WriteLine($"{AddFirstRow(columns[j])}");
                //        break;
                //    }
                //    sw.Write($"{AddFirstRow(columns[j])},");
                //}
            }
        }

        //private static string? AddFirstRow(string colName)
        //{
        //    Console.Write($"{colName} = ");
        //    return Console.ReadLine();
        //} 
        #endregion
    }
}
