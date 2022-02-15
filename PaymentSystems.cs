using System;
using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    class Program
    {
        static void Main(string[] args)
        {
            //Выведите платёжные ссылки для трёх разных систем платежа: 
            //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш ID заказа}
            //order.system2.ru/pay?hash={MD5 хеш ID заказа + сумма заказа}
            //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш сумма заказа + ID заказа + секретный ключ от системы}

            var order = new Order(11, 12000);

            var system1 = new System1();
            Console.WriteLine(system1.GetPayingLink(order));

            var system2 = new System2();
            Console.WriteLine(system2.GetPayingLink(order));

            var system3 = new System3("SecretKey");
            Console.WriteLine(system3.GetPayingLink(order));
        }

        public class Order
        {
            public readonly int Id;
            public readonly int Amount;

            public Order(int id, int amount) => (Id, Amount) = (id, amount);
        }

        public interface IPaymentSystem
        {
            string GetPayingLink(Order order);
        }

        public class System1 : IPaymentSystem
        {
            public string GetPayingLink(Order order)
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString()));

                return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={Convert.ToBase64String(hash)}";
            }
        }

        public class System2 : IPaymentSystem
        {
            public string GetPayingLink(Order order)
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString() + order.Amount.ToString()));

                return $"order.system2.ru/pay?hash={Convert.ToBase64String(hash)}";
            }
        }

        public class System3 : IPaymentSystem
        {
            private readonly string _secretKey;

            public System3(string secretKey) => (_secretKey) = (secretKey);

            public string GetPayingLink(Order order)
            {
                if (order == null)
                    throw new ArgumentNullException(nameof(order));

                SHA1 sha1 = SHA1.Create();
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(order.Amount.ToString() + order.Id + _secretKey));

                return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={Convert.ToBase64String(hash)}";
            }
        }
    }
}