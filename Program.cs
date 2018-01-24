using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalc
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "Simple Calculator";
            Console.WriteLine($"Simple calculator in C#");
            
            Calc app = new Calc();
            double buffInput,
                   buffResult;
            Type appType = app.GetType();
            MethodInfo buffMethod;
            string defaultWarnMess = "It's not number!";
            bool methodDone = false;

            do
             {

                if (app.Action == Calc.Actions.Exit)
                    continue;

                app.ClearWindow();

                if (app.Action == Calc.Actions.Clear)
                {
                    app.Clear();
                    app.Action = null;
                }
                else if (app.Action == Calc.Actions.History)
                {
                    foreach (KeyValuePair<Calc.Actions?, double> item in app.History)
                    {
                        Console.WriteLine("> {0} with before Result: {1}",
                            item.Key,
                            item.Value);
                    }
                    Console.ReadKey();
                    app.Action = null;
                }
                else if (app.Action == Calc.Actions.Undo)
                {
                    Console.Write("Are you sure to Undo action? (y/n): ");
                    char anw = Console.ReadKey().KeyChar;
                    if(anw == 'y' || anw == 'Y')
                        app.Undo();

                    app.Action = null;
                }
                else if (app.Action != null)
                {
                    buffResult = app.Result;
                    buffInput = app.GetNumberDialog(
                        Calc.ActionDialogs[app.Action], defaultWarnMess);

                    buffMethod = appType.GetMethod(app.Action.ToString());
                    methodDone = Convert.ToBoolean( 
                        buffMethod.Invoke(app, new Object[] { buffInput }) );

                    if (methodDone)
                    {
                        app.StoreResult(buffResult);
                        buffResult = 0;
                    }
                    app.Action = null;
                }
                else
                {
                    app.Action = app.MenuDialog("What do you want to perform?");
                }

            } while (app.Action != Calc.Actions.Exit);
            
        }
    }
}
