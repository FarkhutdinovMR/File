using System;
using System.Linq;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderForm = new OrderForm();
            var paymentHandler = new PaymentHandler();
            PaymentSystem[] paymentSystems = { new PaymentSystem(new QIWI(), nameof(QIWI)), new PaymentSystem(new WebMoney(), nameof(WebMoney)), new PaymentSystem(new Card(), nameof(Card)) };

            var systemId = orderForm.ShowForm(paymentSystems);

            PaymentSystem paymentSystem = paymentSystems.FirstOrDefault(payment => payment.SystemId == systemId);

            if (paymentSystem == null)
                throw new ArgumentNullException();

            paymentSystem.Transition();
            paymentHandler.ShowPaymentResult(paymentSystem);
        }
    }

    public class OrderForm
    {
        public string ShowForm(PaymentSystem[] paymentSystems)
        {
            Console.Write("Мы принимаем: ");
            foreach (PaymentSystem paymentSystem in paymentSystems)
                Console.Write(paymentSystem.SystemId + " ");

            Console.WriteLine();

            // Симуляция веб интерфейса.
            Console.WriteLine("Какой системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(PaymentSystem paymentSystem)
        {
            Console.WriteLine($"Вы оплатили с помощью {paymentSystem.SystemId}");

            paymentSystem.Pay();

            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public interface IPaymentSystem
    {
        void Transition();
        void Pay();
    }

    public class PaymentSystem
    {
        private readonly IPaymentSystem _paymentSystem;

        public PaymentSystem(IPaymentSystem paymentSystem, string systemId)
        {
            _paymentSystem = paymentSystem;
            SystemId = systemId;
        }

        public string SystemId { get; private set; }

        public void Transition()
        {
            _paymentSystem.Transition();
        }

        public void Pay()
        {
            _paymentSystem.Pay();
        }
    }

    public class QIWI : IPaymentSystem
    {
        public void Transition()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }

        public void Pay()
        {
            Console.WriteLine("Проверка платежа через QIWI...");
        }
    }

    public class WebMoney : IPaymentSystem
    {
        public void Transition()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }

        public void Pay()
        {
            Console.WriteLine("Проверка платежа через WebMoney...");
        }
    }

    public class Card : IPaymentSystem
    {
        public void Transition()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }

        public void Pay()
        {
            Console.WriteLine("Проверка платежа через Card...");
        }
    }
}