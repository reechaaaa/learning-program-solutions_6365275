using System;

namespace AdapterPatternExample
{
    /// <summary>
    /// Step 2: Define Target Interface
    /// This is the interface that our client code (e.g., the payment system) expects to work with.
    /// All adapters will conform to this interface.
    /// </summary>
    public interface IPaymentProcessor
    {
        void ProcessPayment(decimal amount);
    }

    // --- Step 3: Implement Adaptee Classes ---
    // These are existing third-party payment gateways with their own unique, incompatible interfaces.

    /// <summary>
    /// Adaptee 1: An older payment gateway with a specific 'MakePayment' method.
    /// This represents an existing component that we cannot modify.
    /// </summary>
    public class OldPaymentGateway
    {
        public void MakePayment(double amountInDollars)
        {
            Console.WriteLine($"[OldPaymentGateway] Processing payment of ${amountInDollars:F2} using legacy system.");
            // Simulate complex legacy payment logic
            Console.WriteLine("[OldPaymentGateway] Transaction completed successfully via old system.");
        }
    }

    /// <summary>
    /// Adaptee 2: A newer payment service with a different 'ExecuteTransaction' method
    /// and possibly expects amounts as decimals directly.
    /// This also represents an existing component with a different interface.
    /// </summary>
    public class NewPaymentService
    {
        public void ExecuteTransaction(decimal transactionAmount, string currency)
        {
            Console.WriteLine($"[NewPaymentService] Executing transaction for {transactionAmount:F2} {currency}.");
            // Simulate modern payment service logic
            Console.WriteLine("[NewPaymentService] Transaction approved by new service.");
        }
    }

    // --- Step 4: Implement the Adapter Classes ---
    // Each adapter wraps an Adaptee and makes it compatible with the Target Interface.

    /// <summary>
    /// Adapter for OldPaymentGateway.
    /// Implements IPaymentProcessor and adapts the OldPaymentGateway's interface.
    /// </summary>
    public class OldPaymentGatewayAdapter : IPaymentProcessor
    {
        private readonly OldPaymentGateway _oldGateway;

        /// <summary>
        /// Constructor takes an instance of the OldPaymentGateway to adapt.
        /// </summary>
        /// <param name="oldGateway">The OldPaymentGateway instance to wrap.</param>
        public OldPaymentGatewayAdapter(OldPaymentGateway oldGateway)
        {
            _oldGateway = oldGateway;
        }

        /// <summary>
        /// Implements the ProcessPayment method of IPaymentProcessor.
        /// Translates the call to the OldPaymentGateway's MakePayment method,
        /// handling any necessary data conversions (e.g., decimal to double).
        /// </summary>
        /// <param name="amount">The payment amount (expected by IPaymentProcessor).</param>
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine("--- Using OldPaymentGatewayAdapter ---");
            _oldGateway.MakePayment((double)amount); // Convert decimal to double as required by OldPaymentGateway
            Console.WriteLine("--- OldPaymentGatewayAdapter finished ---\n");
        }
    }

    /// <summary>
    /// Adapter for NewPaymentService.
    /// Implements IPaymentProcessor and adapts the NewPaymentService's interface.
    /// </summary>
    public class NewPaymentServiceAdapter : IPaymentProcessor
    {
        private readonly NewPaymentService _newService;

        /// <summary>
        /// Constructor takes an instance of the NewPaymentService to adapt.
        /// </summary>
        /// <param name="newService">The NewPaymentService instance to wrap.</param>
        public NewPaymentServiceAdapter(NewPaymentService newService)
        {
            _newService = newService;
        }

        /// <summary>
        /// Implements the ProcessPayment method of IPaymentProcessor.
        /// Translates the call to the NewPaymentService's ExecuteTransaction method,
        /// providing additional parameters (like currency) if needed.
        /// </summary>
        /// <param name="amount">The payment amount (expected by IPaymentProcessor).</param>
        public void ProcessPayment(decimal amount)
        {
            Console.WriteLine("--- Using NewPaymentServiceAdapter ---");
            _newService.ExecuteTransaction(amount, "USD"); // Pass amount and a default currency
            Console.WriteLine("--- NewPaymentServiceAdapter finished ---\n");
        }
    }

    /// <summary>
    /// Step 5: Test the Adapter Implementation
    /// The client code that uses the IPaymentProcessor interface without knowing
    /// the specifics of the underlying payment gateways.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Adapter Pattern Example: Payment Processing System ---\n");

            // Scenario 1: Process payment using the Old Payment Gateway via its Adapter
            Console.WriteLine("Client wants to process payment via Old Payment Gateway:");
            OldPaymentGateway oldGateway = new OldPaymentGateway();
            IPaymentProcessor oldGatewayAdapter = new OldPaymentGatewayAdapter(oldGateway);
            oldGatewayAdapter.ProcessPayment(100.50m); // Client calls standard ProcessPayment

            // Scenario 2: Process payment using the New Payment Service via its Adapter
            Console.WriteLine("Client wants to process payment via New Payment Service:");
            NewPaymentService newService = new NewPaymentService();
            IPaymentProcessor newServiceAdapter = new NewPaymentServiceAdapter(newService);
            newServiceAdapter.ProcessPayment(250.75m); // Client calls standard ProcessPayment

            // You can even put them in a list and process generically
            Console.WriteLine("Processing multiple payments generically through adapters:");
            var processors = new System.Collections.Generic.List<IPaymentProcessor>
            {
                new OldPaymentGatewayAdapter(new OldPaymentGateway()),
                new NewPaymentServiceAdapter(new NewPaymentService())
            };

            foreach (var processor in processors)
            {
                processor.ProcessPayment(50.00m);
            }

            Console.WriteLine("\n--- Adapter Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
