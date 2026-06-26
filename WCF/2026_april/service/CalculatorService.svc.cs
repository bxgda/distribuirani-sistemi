using System;
using System.ServiceModel;

namespace WCF_Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class CalculatorService : ICalculator
    {
        int result = 0;
        String expresion = "0";

        ICalculatorCallback Callback
        {
            get { return OperationContext.Current.GetCallbackChannel<ICalculatorCallback>(); }
        }

        public int Add(int num)
        {
            this.result += num;
            this.expresion += "+" + num;
            Callback.OnCalculationPerformed(this.expresion);
            return this.result;
        }

        public void ClearSesion()
        {
            this.result = 0;
            this.expresion = "0";
            Callback.OnCalculationPerformed("Expresion is now empty");
        }

        public int Divide(int num)
        {
            if (num == 0)
                return this.result;

            this.result /= num;
            this.expresion += "/" + num;
            Callback.OnCalculationPerformed(this.expresion);
            return this.result;
        }

        public int Multiply(int num)
        {
            this.result *= num;
            this.expresion += "*" + num;
            Callback.OnCalculationPerformed(this.expresion);
            return this.result;
        }

        public int Subtract(int num)
        {
            this.result -= num;
            this.expresion += "-" + num;
            Callback.OnCalculationPerformed(this.expresion);
            return this.result;
        }
    }
}
