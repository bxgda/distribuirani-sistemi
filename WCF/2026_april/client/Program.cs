using System;
using System.ServiceModel;
using WCF_Client.ServiceReference1;

namespace WCF_Client
{
    internal class Program
    {
        static void Main()
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            var proxy = new CalculatorClient(instanceContext);

            Console.WriteLine("Calculator - enter an operation and a number (e.g., + 5)");
            Console.WriteLine("Operations: +, -, *, /");
            Console.WriteLine("CLEAR - reset, EXIT - quit");
            Console.WriteLine("------------------------------------------");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine().Trim();

                if (input.ToUpper() == "EXIT")
                    break;

                if (input.ToUpper() == "CLEAR")
                {
                    proxy.ClearSesion();
                    Console.WriteLine("Session reset.");
                    continue;
                }

                // Parsing: the first part is the operation, the second part is the number
                string[] parts = input.Split(' ');

                if (parts.Length != 2)
                {
                    Console.WriteLine("Format: operation number (e.g., + 5)");
                    continue;
                }

                string op = parts[0];
                int num;

                if (!int.TryParse(parts[1], out num))
                {
                    Console.WriteLine("Invalid number.");
                    continue;
                }

                int result = 0;

                switch (op)
                {
                    case "+":
                        result = proxy.Add(num);
                        break;
                    case "-":
                        result = proxy.Subtract(num);
                        break;
                    case "*":
                        result = proxy.Multiply(num);
                        break;
                    case "/":
                        if (num == 0)
                        {
                            Console.WriteLine("Division by zero!");
                            continue;
                        }
                        result = proxy.Divide(num);
                        break;
                    default:
                        Console.WriteLine("Unknown operation.");
                        continue;
                }

                Console.WriteLine("Result: " + result);
            }

            proxy.Close();
            Console.WriteLine("Calculator closed.");
        }
    }

    public class CallbackHandler : ICalculatorCallback
    {
        public void OnCalculationPerformed(string expression)
        {
            Console.WriteLine(expression);
        }
    }
}
