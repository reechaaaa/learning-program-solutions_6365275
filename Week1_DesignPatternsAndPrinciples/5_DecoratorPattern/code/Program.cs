using System;

namespace DecoratorPatternExample
{
    /// <summary>
    /// Step 2: Define Component Interface
    /// This is the common interface for all notification types, both base and decorated.
    /// Clients will interact with this interface.
    /// </summary>
    public interface INotifier
    {
        void Send(string message);
    }

    /// <summary>
    /// Step 3: Implement Concrete Component
    /// This is the basic, concrete implementation of the Notifier.
    /// It sends notifications via Email.
    /// </summary>
    public class EmailNotifier : INotifier
    {
        public void Send(string message)
        {
            Console.WriteLine($"Sending Email Notification: {message}");
        }
    }

    /// <summary>
    /// Step 4: Implement Decorator Classes - Abstract Decorator
    /// This abstract class serves as the base for all concrete decorators.
    /// It implements the INotifier interface and holds a reference to another INotifier object.
    /// Its Send method typically calls the wrapped notifier's Send method.
    /// </summary>
    public abstract class NotifierDecorator : INotifier
    {
        protected INotifier _wrappedNotifier; // The notifier being decorated

        /// <summary>
        /// Constructor takes the notifier object to be decorated.
        /// </summary>
        /// <param name="notifier">The INotifier instance to wrap.</param>
        public NotifierDecorator(INotifier notifier)
        {
            _wrappedNotifier = notifier;
        }

        /// <summary>
        /// The default Send implementation simply delegates to the wrapped notifier.
        /// Concrete decorators will override this to add their own logic before/after delegation.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public virtual void Send(string message)
        {
            _wrappedNotifier.Send(message);
        }
    }

    /// <summary>
    /// Step 4: Implement Decorator Classes - Concrete Decorator (SMS)
    /// This concrete decorator adds SMS notification functionality.
    /// It extends NotifierDecorator and adds its own Send logic.
    /// </summary>
    public class SMSNotifierDecorator : NotifierDecorator
    {
        public SMSNotifierDecorator(INotifier notifier) : base(notifier) { }

        /// <summary>
        /// Overrides the Send method to first send via the wrapped notifier,
        /// and then add SMS specific notification.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public override void Send(string message)
        {
            base.Send(message); // Send using the wrapped notifier (e.g., Email)
            Console.WriteLine($"Sending SMS Notification: {message}"); // Add SMS functionality
        }
    }

    /// <summary>
    /// Step 4: Implement Decorator Classes - Concrete Decorator (Slack)
    /// This concrete decorator adds Slack notification functionality.
    /// It extends NotifierDecorator and adds its own Send logic.
    /// </summary>
    public class SlackNotifierDecorator : NotifierDecorator
    {
        public SlackNotifierDecorator(INotifier notifier) : base(notifier) { }

        /// <summary>
        /// Overrides the Send method to first send via the wrapped notifier,
        /// and then add Slack specific notification.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public override void Send(string message)
        {
            base.Send(message); // Send using the wrapped notifier (e.g., Email or Email+SMS)
            Console.WriteLine($"Sending Slack Notification: {message}"); // Add Slack functionality
        }
    }

    /// <summary>
    /// Step 5: Test the Decorator Implementation
    /// The client code demonstrates how to combine different decorators
    /// to achieve various notification sending capabilities.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Decorator Pattern Example: Notification System ---\n");

            // Scenario 1: Basic Email Notification
            Console.WriteLine("Scenario 1: Basic Email Notification");
            INotifier emailNotifier = new EmailNotifier();
            emailNotifier.Send("Your order has been placed!");
            Console.WriteLine("\n----------------------------------\n");

            // Scenario 2: Email + SMS Notification
            Console.WriteLine("Scenario 2: Email + SMS Notification");
            // Start with EmailNotifier, then decorate it with SMS
            INotifier emailAndSmsNotifier = new SMSNotifierDecorator(new EmailNotifier());
            emailAndSmsNotifier.Send("Your package has shipped!");
            Console.WriteLine("\n----------------------------------\n");

            // Scenario 3: Email + SMS + Slack Notification
            Console.WriteLine("Scenario 3: Email + SMS + Slack Notification");
            // Start with EmailNotifier, then decorate with SMS, then decorate with Slack
            INotifier allChannelNotifier = new SlackNotifierDecorator(
                                                new SMSNotifierDecorator(
                                                    new EmailNotifier()
                                                )
                                            );
            allChannelNotifier.Send("New critical alert!");
            Console.WriteLine("\n----------------------------------\n");


            // Scenario 4: SMS-only (assuming SMS is a core component now, or you want to start with it)
            // Note: If you want a truly SMS-only, you'd need an SMSEmailNotifier core component.
            // For this example, we show how you'd add Email on top of an SMS-first system if SMS was the base.
            // For true "SMS-only", SMSNotifier would be the INotifier concrete component.
            Console.WriteLine("Scenario 4: Starting with SMS functionality, then adding Email (conceptual reverse)");
            // Let's assume for this scenario, SMSNotifier is the base component for some reason.
            // (Typically, Email is the base, and SMS/Slack are added. This shows flexibility.)
            INotifier smsBaseNotifier = new SMSNotifierWrapper(); // A simple wrapper to make it act like a base component
            smsBaseNotifier.Send("Reminder: Meeting at 10 AM.");
            Console.WriteLine("\n----------------------------------\n");

            Console.WriteLine("--- Decorator Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }

        // A simple wrapper to show how you might treat SMS as a base component if needed.
        // In a real scenario, SMSNotifier might be the Concrete Component instead of EmailNotifier.
        public class SMSWrapper : INotifier
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending SMS (base) Notification: {message}");
            }
        }
        // Renamed wrapper class to avoid confusion.
        // This is purely for demonstration of flexibility if SMS were a "base" channel.
        public class SMSNotifierWrapper : INotifier
        {
            public void Send(string message)
            {
                Console.WriteLine($"Sending SMS (base) Notification: {message}");
            }
        }
    }
}
