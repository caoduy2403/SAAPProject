using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAAPHelper.Constant;
using SAAPHelper.Models;

namespace SAAPHelper.Helper
{
    public class MenuHelper
    {
        public static void MainMenu()
        {
            while (true)
            {
                //Console.Clear();
                Console.WriteLine("\r\n============================== MENU ==============================");
                Console.WriteLine("1. ==> Export File");
                Console.WriteLine("2. ==> Get Japanese Text From File");
                Console.WriteLine("3. ==> Convert File");
                Console.WriteLine("4. ==> Delete File");
                Console.WriteLine("9. ==> Exit");

                Console.Write("\r\nSelect an option: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine("\r\n===================================================================");
                    Console.WriteLine("Selected: 1. ==> Export File");
                    Console.WriteLine("========== Export with an option ==========");
                    Console.WriteLine("1. Before Name");
                    Console.WriteLine("2. After Name");
                    Console.WriteLine("3. Remove Comment (From After Name)");
                    Console.WriteLine("4. Translate File");
                    Console.WriteLine("All Options\r\n");

                    Console.Write("\r\nOption: ");
                    string option = Console.ReadLine();
                    Console.Write("List ID: ");
                    string listID = Console.ReadLine();

                    bool isBefore = false;
                    bool isAfter = false;
                    bool isCommented = false;
                    bool isTranslatedFile = false;

                    string[] listOption = option.Split(new Char[] {' ', ',', ';'});
                    foreach (string opt in listOption)
                    {
                        if (opt == "1")
                        {
                            isBefore = true;
                        }
                        else if (opt == "2")
                        {
                            isAfter = true;
                        }
                        else if (opt == "3")
                        {
                            isCommented = true;
                        }
                        else if (opt == "4")
                        {
                            isTranslatedFile = true;
                        }
                    }

                    ExportModel model = new ExportModel();
                    model.listID = listID;
                    model.isBefore = isBefore;
                    model.isAfter = isAfter;
                    model.isCommented = isCommented;
                    model.isTranslatedFile = isTranslatedFile;

                    JapanFileHelper.ExportFile(model);
                }
                else if (input == "2") //Get Japanese Text From File"
                {
                    Console.WriteLine("\r\n===================================================================");
                    Console.WriteLine("Selected: 1 ==> Export File");
                    Console.WriteLine("========== Translation Options ==========");
                    Console.WriteLine("1. Translation");

                    Console.Write("\r\n Option: ");
                    string option = Console.ReadLine();
                    bool isTranslated = false;

                    if (option == "1")
                    {
                        isTranslated = true;
                    }
                    JapanFileHelper.HandleJapaneseTextFile(isTranslated);
                }
                else if (input == "3") //Convert File
                {
                    Console.WriteLine("\r\n===================================================================");
                    Console.WriteLine("Selected: 3. ==> Convert File");
                    JapanFileHelper.HandleConvertFile();
                }
                else if (input == "4") //Delete File
                {
                    Console.WriteLine("\r\n==================================================");
                    Console.WriteLine("\r\nSelected: 4. ==> Delete File");
                    FolderHelper.DeleteAllFiles();
                }
                else if (input == "9")
                {
                    Console.WriteLine("Good Bye");
                    Environment.Exit(0);
                }
                //DisplayResult();
            };
        }

        public static void DisplayResult()
        {
            Console.Write("\r\nPress Enter to return to Main Menu");
            Console.ReadLine();
        }

    }
}
