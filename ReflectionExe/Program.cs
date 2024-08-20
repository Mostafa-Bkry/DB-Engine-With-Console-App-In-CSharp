using LogicClasses;
using System.Linq;

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
            #endregion

            //--------------------------------

            #region Insertion Process
            //Insertion.InsertionUserPrompt();
            #endregion

            //--------------------------------

            #region Selection Process
            Selection.SelectUserPrompt();
            #endregion

            //--------------------------------

        }
    }
}
