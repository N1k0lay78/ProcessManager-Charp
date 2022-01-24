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
        public UI()
        {
            var pm = new ProcessManager();

        }

        string[] AskComand()
        {
            Console.Write("command> ");
            var Ans = Console.ReadLine();
            return Ans.Split();
        }

        int IsIntParam(string Line)
        {
            int Res;
            int.TryParse(Line, out Res);
            return Res;
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
                    Console.WriteLine("\tKill <process>    - Убить процесс                 (name/id)");
                    Console.WriteLine("\tTaskList          - Список выполняющихся задач");
                    Console.WriteLine("\tProcess <process> - Показать процессы             (name/id)");
                    Console.WriteLine("\tInfo <id>         - Полную информацию о процессе  (id)");
                    break;
                case "kill":
                    if (Command.Length > 1)
                    {
                        var Param = String.Join(" ", Command[1..]);
                        int Id;
                        if (int.TryParse(Param, out Id))
                        {
                            Console.WriteLine($"*kill process Id = {Id}*");
                        }
                        else
                        {
                            Console.WriteLine($"*kill process Name = {Param}*");
                        }
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'process' (name/id)");
                    }
                    break;
                case "tasklist":
                    Console.WriteLine($"*taskList of processes*");
                    break;
                case "process":
                    if (Command.Length > 1)
                    {
                        var Param = String.Join(" ", Command[1..]);
                        int Id;
                        if (int.TryParse(Param, out Id))
                        {
                            Console.WriteLine($"*processes Id = {Id}*");
                        }
                        else
                        {
                            Console.WriteLine($"*processes Name = {Param}*");
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
                                Console.WriteLine($"*info about process id = {Id}*");
                            }
                        }
                        if (!Any)
                        {
                            Console.WriteLine("must be a decimal number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("missing required attribute 'id' (id)");
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
