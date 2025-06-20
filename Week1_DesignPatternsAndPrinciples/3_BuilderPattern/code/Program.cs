using System;

namespace BuilderPatternExample
{
    /// <summary>
    /// Step 2: Define a Product Class - Computer
    /// Represents the complex object to be constructed.
    /// Its properties are set via the Builder.
    /// </summary>
    public class Computer
    {
        // Core components
        public string CPU { get; private set; }
        public int RAM_GB { get; private set; }
        public string Storage { get; private set; } // e.g., "512GB SSD", "1TB HDD"

        // Optional components
        public string GPU { get; private set; } = "Integrated"; // Default value
        public string OperatingSystem { get; private set; } = "None"; // Default value
        public string Monitor { get; private set; } = "None"; // Default value
        public bool HasWebcam { get; private set; }
        public bool HasKeyboard { get; private set; }
        public bool HasMouse { get; private set; }

        /// <summary>
        /// Step 4: Implement the Builder Pattern - Private Constructor
        /// The constructor is private to force object creation through the ComputerBuilder.
        /// It takes the ComputerBuilder instance to populate its properties.
        /// </summary>
        /// <param name="builder">The ComputerBuilder instance used to construct this Computer.</param>
        private Computer(ComputerBuilder builder)
        {
            CPU = builder.cpu;
            RAM_GB = builder.ramGb;
            Storage = builder.storage;
            GPU = builder.gpu;
            OperatingSystem = builder.operatingSystem;
            Monitor = builder.monitor;
            HasWebcam = builder.hasWebcam;
            HasKeyboard = builder.hasKeyboard;
            HasMouse = builder.hasMouse;
        }

        /// <summary>
        /// Provides a clean string representation of the Computer's configuration.
        /// </summary>
        /// <returns>A formatted string detailing the computer's specifications.</returns>
        public override string ToString()
        {
            var components = new System.Text.StringBuilder();
            components.AppendLine("--- Computer Configuration ---");
            components.AppendLine($"CPU: {CPU}");
            components.AppendLine($"RAM: {RAM_GB} GB");
            components.AppendLine($"Storage: {Storage}");
            components.AppendLine($"GPU: {GPU}");
            components.AppendLine($"OS: {OperatingSystem}");
            components.AppendLine($"Monitor: {Monitor}");
            components.AppendLine($"Webcam: {(HasWebcam ? "Yes" : "No")}");
            components.AppendLine($"Keyboard: {(HasKeyboard ? "Yes" : "No")}");
            components.AppendLine($"Mouse: {(HasMouse ? "Yes" : "No")}");
            components.AppendLine("----------------------------");
            return components.ToString();
        }

        /// <summary>
        /// Step 3: Implement the Builder Class - Nested Static Builder
        /// This static nested class is responsible for building a Computer object step-by-step.
        /// It provides fluent methods (chaining) for setting various attributes.
        /// </summary>
        public class ComputerBuilder
        {
            // Internal fields to hold the configuration values
            // These will be used by the Computer's private constructor.
            internal string cpu;
            internal int ramGb;
            internal string storage;
            internal string gpu = "Integrated"; // Default value
            internal string operatingSystem = "None"; // Default value
            internal string monitor = "None"; // Default value
            internal bool hasWebcam = false;
            internal bool hasKeyboard = false;
            internal bool hasMouse = false;

            /// <summary>
            /// Constructor for the builder.
            /// It's common to require essential components here.
            /// </summary>
            /// <param name="cpu">The CPU for the computer.</param>
            /// <param name="ramGb">The RAM in GB for the computer.</param>
            /// <param name="storage">The storage type and size (e.g., "512GB SSD").</param>
            public ComputerBuilder(string cpu, int ramGb, string storage)
            {
                this.cpu = cpu;
                this.ramGb = ramGb;
                this.storage = storage;
            }

            // Fluent methods for setting optional attributes.
            // Each method returns 'this' (the builder itself) to allow method chaining.

            public ComputerBuilder WithGpu(string gpu)
            {
                this.gpu = gpu;
                return this;
            }

            public ComputerBuilder WithOperatingSystem(string os)
            {
                this.operatingSystem = os;
                return this;
            }

            public ComputerBuilder WithMonitor(string monitor)
            {
                this.monitor = monitor;
                return this;
            }

            public ComputerBuilder AddWebcam()
            {
                this.hasWebcam = true;
                return this;
            }

            public ComputerBuilder AddKeyboard()
            {
                this.hasKeyboard = true;
                return this;
            }

            public ComputerBuilder AddMouse()
            {
                this.hasMouse = true;
                return this;
            }

            /// <summary>
            /// Provides a build() method in the Builder class that returns an instance of Computer.
            /// This is the final step to create the immutable Computer object.
            /// </summary>
            /// <returns>A new instance of Computer configured by this builder.</returns>
            public Computer Build()
            {
                return new Computer(this); // Passes itself to the private constructor of Computer
            }
        }
    }

    /// <summary>
    /// Step 5: Test the Builder Implementation
    /// Main program to demonstrate the creation of different
    /// configurations of Computer using the Builder pattern.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Builder Pattern Example: Computer Assembly ---\n");

            // Example 1: Build a Basic Office Computer
            Console.WriteLine("Building a Basic Office Computer:");
            Computer officeComputer = new Computer.ComputerBuilder("Intel Core i5", 8, "256GB SSD")
                                          .WithOperatingSystem("Windows 10 Pro")
                                          .AddKeyboard()
                                          .AddMouse()
                                          .Build();
            Console.WriteLine(officeComputer);

            // Example 2: Build a High-End Gaming Computer
            Console.WriteLine("Building a High-End Gaming Computer:");
            Computer gamingComputer = new Computer.ComputerBuilder("AMD Ryzen 9", 32, "1TB NVMe SSD")
                                          .WithGpu("NVIDIA GeForce RTX 4080")
                                          .WithOperatingSystem("Windows 11 Home")
                                          .WithMonitor("27-inch 144Hz 1440p")
                                          .AddWebcam()
                                          .AddKeyboard()
                                          .AddMouse()
                                          .Build();
            Console.WriteLine(gamingComputer);

            // Example 3: Build a Compact Workstation (Minimal configuration)
            Console.WriteLine("Building a Compact Workstation:");
            Computer workstation = new Computer.ComputerBuilder("Intel Core i7", 16, "512GB SSD")
                                        .WithOperatingSystem("Ubuntu Linux")
                                        .Build(); // Notice fewer optional components added
            Console.WriteLine(workstation);

            // Example 4: A Computer with only essential components
            Console.WriteLine("Building a Barebones Computer:");
            Computer barebonesComputer = new Computer.ComputerBuilder("AMD Athlon", 4, "128GB SSD").Build();
            Console.WriteLine(barebonesComputer);

            Console.WriteLine("\n--- Builder Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
