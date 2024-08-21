using LogicClasses;

namespace ReflectionExe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Fetching Existing Tables On the App Starting
            //Fetching already existing tables
            ExistingTables.FetchAll();
            Console.WriteLine("---------------------------------------------------\n");
            #endregion

            //--------------------------------

            #region Creation Process
            //Creation.CreationUserPrompt();
            //Console.WriteLine("---------------------------------------------------\n");
            #endregion

            //--------------------------------

            #region Insertion Process
            //Insertion.InsertionUserPrompt();
            //Console.WriteLine("---------------------------------------------------\n");
            #endregion

            //--------------------------------

            #region Selection Process
            //Selection.SelectUserPrompt();
            //Console.WriteLine("---------------------------------------------------\n");
            #endregion

            //--------------------------------

            #region Deletion Process
            Deletion.DeleteUserPrompt();
            Console.WriteLine("---------------------------------------------------\n");
            #endregion
        }
    }
}
