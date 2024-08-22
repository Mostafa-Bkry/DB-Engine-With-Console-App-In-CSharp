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

            #region Prompting User to choose between processes
            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                string? input;
                Console.WriteLine("Choose One Process by its number:");
                Console.WriteLine("1- Create Tables");
                Console.WriteLine("2- Insert Into Tables");
                Console.WriteLine("3- Select from Tables");
                Console.WriteLine("4- Delete from Tables");
                Console.WriteLine("5- Update on Tables");
                Console.WriteLine("6- Truncate Tables");
                Console.WriteLine("0- Exit");

                input = Console.ReadLine();

                while (true)
                {
                    if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int choice) && choice >= 0 && choice <= 6)
                    {
                        switch (choice)
                        {
                            case 0:
                                exit = true;
                                break;
                            //--------------------------------
                            case 1:
                                #region Creation Process
                                Creation.CreationUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine();
                                #endregion
                                break;
                            //--------------------------------
                            case 2:
                                #region Insertion Process
                                Insertion.InsertionUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine();
                                break;
                            #endregion
                            //--------------------------------
                            case 3:
                                #region Selection Process
                                Selection.SelectUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine();
                                #endregion
                                break;
                            //--------------------------------
                            case 4:
                                #region Deletion Process
                                Deletion.DeleteUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine();
                                #endregion
                                break;
                            //--------------------------------
                            case 5:
                                #region Updation Process
                                Updation.UpdateUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine();
                                #endregion
                                break;
                            //--------------------------------
                            case 6:
                                #region Truncation Process
                                Truncation.TruncateUserPrompt();
                                Console.WriteLine("---------------------------------------------------\n");
                                Console.WriteLine("Press Enter To Continue");
                                Console.ReadLine(); 
                                #endregion
                                break;
                            default:
                                break;
                        }

                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        input = Console.ReadLine();
                        continue;
                    }
                }
            }
            #endregion
        }
    }
}
