using System.Reflection;

namespace LogicClasses
{
    public class Insertion
    {
        public static void InsertDataIntoTable(Type type)
        {
            try
            {
                // Create an instance of the dynamic type
                var instance = Activator.CreateInstance(type);

                PropertyInfo[] props = type.GetProperties();

                for (int i = 0; i < props.Length; i++)
                {
                    PropertyInfo prop = props[i];
                    prop.SetValue(instance, InsertUserPrompt(prop));

                    using (StreamWriter sw = new StreamWriter($@".\Created Tables\{type.Name.ToLower()}.txt", true))
                    {
                        if(i == props.Length - 1)
                        {
                            sw.WriteLine($"{prop.GetValue(instance)}");
                            break;
                        }
                        sw.Write($"{prop.GetValue(instance)},");
                    }
                }
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

        private static object InsertUserPrompt(PropertyInfo prop)
        {
            string? input;

            do
            {
                Console.Write($"{prop.Name} = ");
                input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(input) || 
                ConvertToPropertyType(input, prop.PropertyType) == null);

            return ConvertToPropertyType(input, prop.PropertyType);
        }

        static object ConvertToPropertyType(string input, Type targetType)
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
