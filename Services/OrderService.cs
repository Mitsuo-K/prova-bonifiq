using ProvaPub.Models;
using System.Reflection.Metadata;

namespace ProvaPub.Services
{
    public class OrderService
    {
        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            Dictionary<string, Func<Task>> paymentActions = new Dictionary<string, Func<Task>>
            {
                { "pix", () => PayWithPix() },
                { "creditcard", () => PayWithCreditCard() },
                { "paypal", () => PayWithPayPal() },
            };

            if (paymentActions.TryGetValue(paymentMethod.ToLower(), out Func<Task> paymentAction))
            {
                await paymentAction.Invoke();
            }
            else
            {
                Console.WriteLine("Nenhum caso correspondente encontrado para o método de pagamento.");
            }

            return new Order
            {
                Value = paymentValue
            };
        }

        private async Task PayWithPix()
        {
            Console.WriteLine("Faz pagamento...");
        }

        private async Task PayWithCreditCard()
        {
            Console.WriteLine("Faz pagamento...");
        }

        private async Task PayWithPayPal()
        {
            Console.WriteLine("Faz pagamento...");
        }
    }
}
