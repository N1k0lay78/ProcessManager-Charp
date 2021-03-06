using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessManager
{
    interface IUserInterface
    {
        public void Run();
    }

    public class UI
    {
        bool Running = true;
        ProcessManager PM;

        public UI()
        {
            PM = new ProcessManager();

        }

        string[] AskComand()
        {
            Console.Write("Command> ");
            var Ans = Console.ReadLine();
            return Ans.Split();
        }

        bool IsAgree(string Text)
        {
            Console.WriteLine(Text);
            Console.WriteLine("Write y/Y to agree or something to refuse");
            Console.Write("Answer: ");
            var Ans = Console.ReadLine();
            return Ans.Trim().ToLower() == "y";
        }

        void Update(string[] Command)
        {
            switch (Command[0].ToLower())
            {
                case "quit":
                    Running = false;
                    break;
                case "help":
                    Console.WriteLine("\tQuit              - Выйти из приложения");
                    Console.WriteLine("\tGraphic           - В консоли рисуется интерфейс Q - to close");
                    Console.WriteLine("\tKill <process>    - Убить процесс                          (name/id)");
                    Console.WriteLine("\tList              - Список выполняющихся задач");
                    Console.WriteLine("\tProcess <process> - Показать процессы                      (name/id)");
                    Console.WriteLine("\tInfo <ids>        - Показать полную информацию о процессе  (ids)");
                    break;
                case "graphic":
                    var app = new GraphicConsole();
                    app.Run();
                    break;
                case "kill":
                    if (Command.Length > 1)
                    {
                        foreach (var Param in Command[1..])
                        {
                            int Id;
                            if (int.TryParse(Param, out Id))
                            {
                                if (!PM.ProcessBy(Id))
                                {
                                    Console.WriteLine("Calling a process that doesn't exist or is inaccessible");
                                }
                                else if (IsAgree("Are you sure you want to stop this process?"))
                                {
                                    if (PM.KillById(Id))
                                    {
                                        Console.WriteLine("Process killed successfully");
                                    }
                                    else
                                    {
                                        Console.WriteLine("The process escaped being killed!!!!");
                                    }
                                }
                            }
                            else
                            {
                                if (!PM.ProcessesBy(Param))
                                {
                                    Console.WriteLine("Calling a process that doesn't exist or is inaccessible");
                                }
                                else if (IsAgree("Are you sure you want to stop this process?"))
                                {
                                    if (PM.KillByName(Param))
                                    {
                                        Console.WriteLine("Process killed successfully");
                                    }
                                    else
                                    {
                                        Console.WriteLine("The process escaped being killed!!!!");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'process' (name/id)");
                    }
                    break;
                case "list":
                    PM.TrackList();
                    break;
                case "process":
                    if (Command.Length > 1)
                    {
                        foreach (var Param in Command[1..])
                        {
                            int Id;
                            if (int.TryParse(Param, out Id))
                            {
                                if (!PM.ProcessBy(Id))
                                {
                                    Console.WriteLine("Calling a process that doesn't exist or is inaccessible");
                                }
                            }
                            else
                            {
                                if (!PM.ProcessesBy(Param))
                                {
                                    Console.WriteLine("Calling a process that doesn't exist or is inaccessible");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'process' (name/id)");
                    }
                    break;
                case "info":
                    if (Command.Length > 1)
                    {
                        var Any = false;
                        foreach (var Line in Command[1..]) {
                            int Id;
                            if (int.TryParse(Line, out Id)) 
                            {
                                Any = true;
                                if (!PM.FullInfo(Id))
                                {
                                    Console.WriteLine("Calling a process that doesn't exist or is inaccessible");
                                }
                            }
                        }
                        if (!Any)
                        {
                            Console.WriteLine("must be a decimal number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'ids' (ids)");
                    }
                    break;
                default:
                    Console.WriteLine("write help to get help");
                    break;
            }
        }

        public void Run()
        {
            while (Running)
            {
                Update(AskComand());
            }
        }
    }
}
