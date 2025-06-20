using System;
using System.Collections.Generic; // Required for List<T>

namespace ObserverPatternExample
{
    /// <summary>
    /// Step 4: Define Observer Interface
    /// This interface defines the contract for all observer objects.
    /// Observers will implement this to receive updates from the subject.
    /// </summary>
    public interface IObserver
    {
        void Update(string stockName, decimal price);
    }

    /// <summary>
    /// Step 2: Define Subject Interface
    /// This interface defines the contract for the subject (the stock market).
    /// It includes methods for attaching (registering), detaching (deregistering),
    /// and notifying observers.
    /// </summary>
    public interface IStock
    {
        void RegisterObserver(IObserver observer);
        void DeregisterObserver(IObserver observer);
        void NotifyObservers();
    }

    /// <summary>
    /// Step 3: Implement Concrete Subject - StockMarket
    /// This class represents the concrete subject in our pattern.
    /// It maintains the stock price and a list of registered observers.
    /// When the stock price changes, it notifies all subscribed observers.
    /// </summary>
    public class StockMarket : IStock
    {
        private List<IObserver> _observers = new List<IObserver>();
        private string _stockName;
        private decimal _currentPrice;

        /// <summary>
        /// Constructor for the StockMarket.
        /// </summary>
        /// <param name="stockName">The name of the stock being monitored.</param>
        /// <param name="initialPrice">The initial price of the stock.</param>
        public StockMarket(string stockName, decimal initialPrice)
        {
            _stockName = stockName;
            _currentPrice = initialPrice;
            Console.WriteLine($"StockMarket created for '{_stockName}' with initial price: {initialPrice:C}");
        }

        /// <summary>
        /// Property to get and set the current stock price.
        /// Setting the price will trigger notification to all observers.
        /// </summary>
        public decimal CurrentPrice
        {
            get { return _currentPrice; }
            set
            {
                if (_currentPrice != value) // Only notify if the price actually changed
                {
                    _currentPrice = value;
                    Console.WriteLine($"\n--- Stock Price for {_stockName} changed to {_currentPrice:C} ---");
                    NotifyObservers(); // Notify all registered observers
                }
            }
        }

        /// <summary>
        /// Registers a new observer to receive updates.
        /// </summary>
        /// <param name="observer">The observer to register.</param>
        public void RegisterObserver(IObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
                Console.WriteLine($"+ Observer registered: {observer.GetType().Name}");
            }
        }

        /// <summary>
        /// Deregisters an observer, stopping it from receiving future updates.
        /// </summary>
        /// <param name="observer">The observer to deregister.</param>
        public void DeregisterObserver(IObserver observer)
        {
            if (_observers.Remove(observer))
            {
                Console.WriteLine($"- Observer deregistered: {observer.GetType().Name}");
            }
        }

        /// <summary>
        /// Notifies all currently registered observers about the latest stock price.
        /// </summary>
        public void NotifyObservers()
        {
            Console.WriteLine($"Notifying {_observers.Count} observers for {_stockName}...");
            foreach (var observer in _observers)
            {
                observer.Update(_stockName, _currentPrice);
            }
        }
    }

    /// <summary>
    /// Step 5: Implement Concrete Observers - MobileApp
    /// This observer represents a mobile application that displays stock updates.
    /// </summary>
    public class MobileApp : IObserver
    {
        private string _appName;

        public MobileApp(string appName)
        {
            _appName = appName;
        }

        public void Update(string stockName, decimal price)
        {
            Console.WriteLine($"  [{_appName} Mobile App]: {stockName} updated to {price:C}. Check your alerts!");
        }
    }

    /// <summary>
    /// Step 5: Implement Concrete Observers - WebApp
    /// This observer represents a web application that displays stock updates.
    /// </summary>
    public class WebApp : IObserver
    {
        private string _appType;

        public WebApp(string appType)
        {
            _appType = appType;
        }

        public void Update(string stockName, decimal price)
        {
            Console.WriteLine($"  [{_appType} Web App]: {stockName} current price is {price:C}. Refresh your dashboard!");
        }
    }

    /// <summary>
    /// Test class to demonstrate the registration and notification of observers.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Observer Pattern Example: Stock Market Monitoring ---\n");

            // 1. Create the Subject (Stock Market for Google)
            StockMarket googleStock = new StockMarket("GOOG", 150.00m);

            // 2. Create Concrete Observers
            MobileApp myPhoneApp = new MobileApp("MyPhoneStockTracker");
            WebApp dashboardWeb = new WebApp("FinancialDashboard");
            MobileApp partnersPhoneApp = new MobileApp("PartnerProApp");

            // 3. Register Observers with the Subject
            Console.WriteLine("\n--- Registering Observers ---");
            googleStock.RegisterObserver(myPhoneApp);
            googleStock.RegisterObserver(dashboardWeb);
            googleStock.RegisterObserver(partnersPhoneApp);

            // 4. Simulate Stock Price Changes (which trigger notifications)
            Console.WriteLine("\n--- Simulating Stock Price Changes ---");

            googleStock.CurrentPrice = 152.50m; // Price increase
            googleStock.CurrentPrice = 151.80m; // Price decrease
            googleStock.CurrentPrice = 151.80m; // No change, no notification
            googleStock.CurrentPrice = 155.00m; // Another increase

            // 5. Deregister an Observer
            Console.WriteLine("\n--- Deregistering an Observer ---");
            googleStock.DeregisterObserver(dashboardWeb);

            // 6. Simulate another Stock Price Change after deregistration
            Console.WriteLine("\n--- Simulating Price Change after Deregistration ---");
            googleStock.CurrentPrice = 154.25m; // Only remaining observers should be notified

            Console.WriteLine("\n--- Observer Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
