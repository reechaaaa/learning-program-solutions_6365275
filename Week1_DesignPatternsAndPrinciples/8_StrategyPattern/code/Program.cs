using System;

namespace StrategyPatternExample
{
    /// <summary>
    /// Step 2: Define Strategy Interface
    /// This interface declares the common operation for all concrete strategies.
    /// In our case, it's the 'pay' method.
    /// </summary>
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    /// <summary>
    /// Step 3: Implement Concrete Strategies
    /// Implements the IPaymentStrategy for Credit Card payments.
    /// </summary>
    public class CreditCardPayment : IPaymentStrategy
    {
        private string _cardNumber;
        private string _cardHolderName;
        private string _cvv;
        private string _expirationDate;

        public CreditCardPayment(string cardNumber, string cardHolderName, string cvv, string expirationDate)
        {
            _cardNumber = cardNumber;
            _cardHolderName = cardHolderName;
            _cvv = cvv;
            _expirationDate = expirationDate;
        }

        /// <summary>
        /// Implements the payment logic for Credit Card.
        /// </summary>
        /// <param name="amount">The amount to pay.</param>
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Processing Credit Card payment of {amount:C}...");
            Console.WriteLine($"Card Number: {_cardNumber}");
            Console.WriteLine($"Card Holder: {_cardHolderName}");
            // In a real scenario, you'd integrate with a payment gateway.
            Console.WriteLine("Credit Card payment processed successfully!");
        }
    }

    /// <summary>
    /// Step 3: Implement Concrete Strategies
    /// Implements the IPaymentStrategy for PayPal payments.
    /// </summary>
    public class PayPalPayment : IPaymentStrategy
    {
        private string _email;
        private string _password; // For demonstration; in real app, never store plain password.

        public PayPalPayment(string email, string password)
        {
            _email = email;
            _password = password;
        }

        /// <summary>
        /// Implements the payment logic for PayPal.
        /// </summary>
        /// <param name="amount">The amount to pay.</param>
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount:C}...");
            Console.WriteLine($"PayPal Email: {_email}");
            // In a real scenario, you'd integrate with PayPal API.
            Console.WriteLine("PayPal payment processed successfully!");
        }
    }

    /// <summary>
    /// Step 4: Implement Context Class
    /// This class holds a reference to a concrete strategy object
    /// and delegates the execution of the payment to the selected strategy.
    /// </summary>
    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        /// <summary>
        /// Sets the payment strategy to be used. This allows changing the strategy at runtime.
        /// </summary>
        /// <param name="strategy">The payment strategy to set.</param>
        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
            Console.WriteLine($"\nPayment method set to: {_paymentStrategy.GetType().Name}");
        }

        /// <summary>
        /// Executes the payment using the currently set strategy.
        /// </summary>
        /// <param name="amount">The amount to pay.</param>
        public void ExecutePayment(decimal amount)
        {
            if (_paymentStrategy == null)
            {
                Console.WriteLine("Error: No payment strategy has been set.");
                return;
            }
            Console.WriteLine($"Attempting to pay {amount:C}...");
            _paymentStrategy.Pay(amount); // Delegates the call to the selected strategy
        }
    }

    /// <summary>
    /// Step 5: Test the Strategy Implementation
    /// The client code that selects and uses different payment strategies
    /// through the PaymentContext.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Strategy Pattern Example: Payment System ---\n");

            // Create the context
            PaymentContext paymentProcessor = new PaymentContext();

            // Scenario 1: Pay using Credit Card strategy
            Console.WriteLine("Client wants to pay using Credit Card:");
            IPaymentStrategy creditCard = new CreditCardPayment("1234-5678-9012-3456", "John Doe", "123", "12/25");
            paymentProcessor.SetPaymentStrategy(creditCard);
            paymentProcessor.ExecutePayment(50.00m);

            Console.WriteLine("\n----------------------------------\n");

            // Scenario 2: Pay using PayPal strategy
            Console.WriteLine("Client wants to pay using PayPal:");
            IPaymentStrategy payPal = new PayPalPayment("john.doe@example.com", "mysecurepassword"); // In real app, don't pass raw passwords
            paymentProcessor.SetPaymentStrategy(payPal);
            paymentProcessor.ExecutePayment(75.50m);

            Console.WriteLine("\n----------------------------------\n");

            // Scenario 3: Change strategy again to Credit Card for another payment
            Console.WriteLine("Client wants to pay again using Credit Card:");
            // Re-use existing credit card object or create a new one
            paymentProcessor.SetPaymentStrategy(creditCard);
            paymentProcessor.ExecutePayment(25.00m);

            Console.WriteLine("\n--- Strategy Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
