using System.Reflection.Emit;
using System.Reflection;

namespace LogicClasses
{
    public static class DynamicTypeCreation
    {
        public static Type CreateAndSaveDynamicType(string typeName, List<string> propertyNames, List<Type> propDataType)
        {
            // Create a dynamic assembly and module
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = 
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // Create a new type (class)
            TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

            // Add properties dynamically
            for (int i = 0; i < propertyNames.Count; i++)
            {
                string propertyName = propertyNames[i];
                FieldBuilder fieldBuilder = typeBuilder.DefineField($"_{propertyName}",
                    propDataType[i], FieldAttributes.Private);
                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, 
                    PropertyAttributes.HasDefault, propDataType[i], null);

                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod($"get_{propertyName}", 
                    MethodAttributes.Public | MethodAttributes.SpecialName | 
                    MethodAttributes.HideBySig, propDataType[i], Type.EmptyTypes);
                ILGenerator getIL = getMethodBuilder.GetILGenerator();
                getIL.Emit(OpCodes.Ldarg_0);
                getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                getIL.Emit(OpCodes.Ret);

                MethodBuilder setMethodBuilder = typeBuilder.DefineMethod($"set_{propertyName}", 
                    MethodAttributes.Public | MethodAttributes.SpecialName | 
                    MethodAttributes.HideBySig, null, new[] { propDataType[i] });
                ILGenerator setIL = setMethodBuilder.GetILGenerator();
                setIL.Emit(OpCodes.Ldarg_0);
                setIL.Emit(OpCodes.Ldarg_1);
                setIL.Emit(OpCodes.Stfld, fieldBuilder);
                setIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getMethodBuilder);
                propertyBuilder.SetSetMethod(setMethodBuilder);
            }

            // Create the type
            Type dynamicType = typeBuilder.CreateType();


            Console.WriteLine($"Created type '{typeName}' with properties: {string.Join(", ", propertyNames)}");

            return dynamicType;
        }

    }
}
