using System;
using System.Collections.Generic; // Required for Dictionary<TKey, TValue>

namespace DependencyInjectionExample
{
    // A simple Customer class to represent our data model
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"Customer [ID: {Id}, Name: {Name}, Email: {Email}]";
        }
    }

    /// <summary>
    /// Step 2: Define Repository Interface - ICustomerRepository
    /// This interface defines the contract for any class that wants to act as a customer data repository.
    /// It specifies what operations can be performed (e.g., finding a customer).
    /// </summary>
    public interface ICustomerRepository
    {
        Customer? FindCustomerById(string id); // Using nullable reference type
    }

    /// <summary>
    /// Step 3: Implement Concrete Repository - CustomerRepositoryImpl
    /// This class provides a concrete implementation of the ICustomerRepository interface.
    /// It simulates data storage and retrieval (e.g., from a database or in-memory collection).
    /// </summary>
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        // Simulate a database/data store with some pre-defined customers
        private readonly Dictionary<string, Customer> _customers;

        public CustomerRepositoryImpl()
        {
            _customers = new Dictionary<string, Customer>
            {
                { "C101", new Customer("C101", "Alice Johnson", "alice@example.com") },
                { "C102", new Customer("C102", "Bob Williams", "bob@example.com") },
                { "C103", new Customer("C103", "Charlie Brown", "charlie@example.com") }
            };
            Console.WriteLine("CustomerRepositoryImpl: Initialized with sample data.");
        }

        /// <summary>
        /// Finds a customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to find.</param>
        /// <returns>The Customer object if found, otherwise null.</returns>
        public Customer? FindCustomerById(string id)
        {
            Console.WriteLine($"CustomerRepositoryImpl: Searching for customer with ID '{id}'.");
            _customers.TryGetValue(id, out var customer);
            return customer;
        }
    }

    /// <summary>
    /// Step 4: Define Service Class - CustomerService
    /// This class represents a business service that depends on a repository to perform its operations.
    /// It defines the high-level business logic (e.g., "get customer details").
    /// </summary>
    public class CustomerService
    {
        // Step 5: Implement Dependency Injection - Constructor Injection
        // The CustomerService does NOT create its own ICustomerRepository.
        // Instead, it receives one through its constructor.
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Constructor for CustomerService, demonstrating Constructor Injection.
        /// The dependency (ICustomerRepository) is "injected" when the CustomerService object is created.
        /// </summary>
        /// <param name="customerRepository">The repository instance to be used by this service.</param>
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            Console.WriteLine("CustomerService: Initialized with injected Customer Repository.");
        }

        /// <summary>
        /// Retrieves customer details by ID using the injected repository.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The Customer object if found, otherwise null.</returns>
        public Customer? GetCustomerDetails(string customerId)
        {
            Console.WriteLine($"CustomerService: Requesting customer details for ID '{customerId}'.");
            return _customerRepository.FindCustomerById(customerId);
        }
    }

    /// <summary>
    /// Step 6: Test the Dependency Injection Implementation
    /// The Main class acts as the "Composition Root" where dependencies are resolved and injected.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Dependency Injection Example: Customer Management ---\n");

            // 1. Create the dependency (Concrete Repository implementation)
            // This is where we "new up" the concrete class.
            Console.WriteLine("Client/Composition Root: Creating CustomerRepositoryImpl instance.");
            ICustomerRepository repository = new CustomerRepositoryImpl();

            // 2. Create the service, injecting the dependency into its constructor
            // The CustomerService doesn't know it's getting CustomerRepositoryImpl;
            // it only knows it's getting an ICustomerRepository.
            Console.WriteLine("\nClient/Composition Root: Creating CustomerService instance, injecting repository.");
            CustomerService customerService = new CustomerService(repository);

            // 3. Use the service to perform an operation
            Console.WriteLine("\nClient: Using CustomerService to find a customer.");
            Customer? foundCustomer1 = customerService.GetCustomerDetails("C101");
            if (foundCustomer1 != null)
            {
                Console.WriteLine($"Client: Found customer: {foundCustomer1}");
            }
            else
            {
                Console.WriteLine("Client: Customer C101 not found.");
            }

            Console.WriteLine("\n--- Attempting to find a non-existent customer ---");
            Customer? foundCustomer2 = customerService.GetCustomerDetails("C999");
            if (foundCustomer2 != null)
            {
                Console.WriteLine($"Client: Found customer: {foundCustomer2}");
            }
            else
            {
                Console.WriteLine("Client: Customer C999 not found (as expected).");
            }

            Console.WriteLine("\n--- Dependency Injection Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
