﻿using System.Reflection;

namespace LogicClasses
{
    public class Insertion
    {
        public static void InsertionUserPrompt()
        {
            if (Tables.UserTables.Count == 0)
            {
                Console.WriteLine("\nThere are no tables to Insert into them yet!");
                return;
            }

            UserPrompt.ShowExistingTables();

            //---------------------------------------

            string? input;
            Type? table = default;
            bool continueInsertion = true;

            do
            {
                Console.WriteLine("Enter The Table Name That You Want To Insert Into it");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || !Tables.UserTables.ContainsValue(input.ToLower()));

            table = Tables.UserTables.FirstOrDefault(KeyValue
                => KeyValue.Value == input.ToLower()).Key;


            Insertion.InsertDataIntoTable(table);

            while (continueInsertion)
            {
                char yn;
                do
                {
                    Console.WriteLine("Do You Want to Insert Again (Y/N)");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input) || !char.TryParse(input.ToLower(), out yn) ||
                    (yn != 'y' && yn != 'n'));

                continueInsertion = yn == 'y' ? true : false;

                if (continueInsertion)
                    Insertion.InsertDataIntoTable(table);
            }
        }

        private static void InsertDataIntoTable(Type type)
        {
            try
            {
                // Create an instance of the dynamic type
                var instance = Activator.CreateInstance(type);

                PropertyInfo[] props = type.GetProperties();
                List<string[]> propsData = new List<string[]>();

                string[] propsDataArr = new string[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    PropertyInfo prop = props[i];
                    prop.SetValue(instance, InsertUserPrompt(type, prop, i));
                    propsDataArr[i] = prop?.GetValue(instance)?.ToString() ?? "";

                    using (StreamWriter sw = new StreamWriter($@".\Created Tables\{type.Name.ToLower()}.txt", true))
                    {
                        if (i == props.Length - 1)
                        {
                            sw.WriteLine($"{prop.GetValue(instance)}");
                            break;
                        }
                        sw.Write($"{prop.GetValue(instance)},");
                    }
                }

                propsData.Add(propsDataArr);
                Tables.AddTableData(type, propsData);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Can't find this file");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static object InsertUserPrompt(Type type, PropertyInfo prop, int colIndexInDataArr)
        {
            string? input;

            while (true)
            {
                do
                {
                    Console.Write($"{prop.Name} = ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input) || ConvertToPropertyType(input, prop.PropertyType) == null);

                if (CheckDuplicateID(type, prop, input, colIndexInDataArr))
                    continue;
                break;

            }
            return ConvertToPropertyType(input, prop.PropertyType);
        }

        private static bool CheckDuplicateID(Type type, PropertyInfo prop, string input, int colIndexInDataArr)
        {
            if (prop.Name.ToLower() == "id" || prop.Name.ToLower().Contains("_id") ||
                            prop.Name.ToLower().Contains("id_") || prop.Name.ToLower().Contains("-id") ||
                            prop.Name.ToLower().Contains("id-"))
            {
                var r = Tables.TablesData.Where(table => table.Table == type)
                              .Select(obj => obj.Data.Find(arr => arr[colIndexInDataArr] == input))
                              .ToList();

                if (!r.Contains(null) && r.Count > 0)
                {
                    Console.WriteLine("Invalid Duplicate ID");
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public static object ConvertToPropertyType(string input, Type targetType)
        {
            try
            {
                return Convert.ChangeType(input, targetType);
            }
            catch
            {
                Console.WriteLine("Invalid Value");
                return /*targetType.IsValueType ? Activator.CreateInstance(targetType) :*/ null;
            }
        }
    }
}
