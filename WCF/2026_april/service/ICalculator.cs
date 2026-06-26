using System;
using System.ServiceModel;

namespace WCF_Service
{
    [ServiceContract(CallbackContract = typeof(ICalculatorCallback))]
    public interface ICalculator
    {
        // trebalo bi float svuda ali briga nas
        [OperationContract]
        void ClearSesion();

        [OperationContract]
        int Add(int num);

        [OperationContract]
        int Subtract(int num);

        [OperationContract]
        int Multiply(int num);

        [OperationContract]
        int Divide(int num);
    }

    public interface ICalculatorCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnCalculationPerformed(String expresion);
    }
}
