using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalc
{
    class Calc
    {

        public double Result { set; get; } = 0;
        public Stack<KeyValuePair<Actions?, double>> History = new Stack<KeyValuePair<Actions?, double>>();
        public enum Actions
        {
            Exit = '0',
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Modulo,
            Exponentiation,
            Root,
            Undo,
            Clear,
            History = 'h'
        };
        public static Dictionary<Actions?, string> ActionDialogs = new Dictionary<Actions?, string>();
        public Actions? Action { set; get; } = null;

        public Calc()
        {
            fillActionDialogs();
            Result = GetNumberDialog("Type the first number: ", "Only number type is expecting...");
        }

        private void fillActionDialogs()    
        {
            ActionDialogs.Add(Actions.Addition, "Type the number to add: ");
            ActionDialogs.Add(Actions.Subtraction, "Type the number to subtract: ");
            ActionDialogs.Add(Actions.Multiplication, "Type the second factor: ");
            ActionDialogs.Add(Actions.Division, "Type the divider: ");
            ActionDialogs.Add(Actions.Modulo, "Type the divider: ");
            ActionDialogs.Add(Actions.Exponentiation, "Type the exponent: ");
            ActionDialogs.Add(Actions.Root, "Type the degree of root: ");
        }

        public double GetNumberDialog(string mess, string warnMess )
        {
            bool doneAttempt = false;
            string input;
            double buff;

            do
            {
                if (doneAttempt)
                {
                    Console.Clear();
                    Console.WriteLine(warnMess);
                }
                Console.Write(mess);
                input = Console.ReadLine();
                doneAttempt = true;
            } while (!Double.TryParse(input, out buff));
            return buff;
        }

        public Actions MenuDialog(string dialogText)
        {
            Actions[] items = (Actions[])Enum.GetValues(typeof(Actions));
            string itemName;
            char inputKey;

            do
            {
                this.ClearWindow();
                Console.WriteLine(dialogText);
                foreach (Actions item in items)
                {
                    itemName = item.ToString();
                    Console.WriteLine(" {0}{1}",
                        (char)item,
                        itemName.PadLeft(20, '.'));
                }
                inputKey = Console.ReadKey().KeyChar;
                //Int32.TryParse(inputKey.ToString(), out inputNumber);
                
            } while ( !Enum.IsDefined(typeof(Actions), (int)inputKey) && !Enum.IsDefined(typeof(Actions), inputKey+32));

            return (Actions)inputKey;
        }

        public bool  Addition(double num)
        {
            Result += num;
            return true;
        }

        public bool Subtraction(double num)
        {
            Result -= num;
            return true;
        }

        public bool Multiplication(double num)
        {
            Result *= num;
            return true;
        }

        public bool Division(double num)
        {
            if( num == 0 )
            {
                Console.WriteLine("You can't divide by zero!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }
            Result /= num;
            return true;
        }

        public bool Modulo(double num)
        {
            if (num == 0)
            {
                Console.WriteLine("You can't divide by zero!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }
            Result %= num;
            return true;
        }

        public bool Exponentiation(double num)
        {
            Result = Math.Pow(Result, num);
            return true;
        }

        public bool Root(double num)
        {
            if (num == 0)
            {
                Console.WriteLine("There's no zero degree element!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return false;
            }
            Result = Math.Pow(Result, 1/num);
            return true;
        }

        public void Clear()
        {
            Result = 0;
            this.History.Clear();
        }

        public void ClearWindow()
        {
            Console.Clear();
            Console.WriteLine("Result: {0}", this.Result);
            Console.WriteLine("".PadRight(' ', '-'));
        }

        public void StoreResult(double val)
        {
            KeyValuePair<Actions?, double> kVp = new KeyValuePair<Actions?, double>(this.Action, val);
            this.History.Push(kVp);
        }

        public void Undo()
        {
            KeyValuePair<Actions?, double> lastAction = this.History.Pop();
            this.Result = lastAction.Value;
        }

    }
}
