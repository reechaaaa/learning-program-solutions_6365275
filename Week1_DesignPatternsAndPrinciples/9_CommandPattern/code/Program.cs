using System;

namespace CommandPatternExample
{
    /// <summary>
    /// Step 2: Define Command Interface
    /// This interface declares the method that all concrete command objects must implement.
    /// It encapsulates a request.
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }

    /// <summary>
    /// Step 5: Implement Receiver Class - Light
    /// The 'Receiver' knows how to perform the operations associated with the command.
    /// It's the object that actually performs the action.
    /// </summary>
    public class Light
    {
        private string _location;

        public Light(string location)
        {
            _location = location;
        }

        public void TurnOn()
        {
            Console.WriteLine($"{_location} Light is ON.");
        }

        public void TurnOff()
        {
            Console.WriteLine($"{_location} Light is OFF.");
        }
    }

    /// <summary>
    /// Step 3: Implement Concrete Commands - LightOnCommand
    /// This concrete command encapsulates the request to turn a light on.
    /// It holds a reference to the 'Receiver' (Light) and implements the Execute method.
    /// </summary>
    public class LightOnCommand : ICommand
    {
        private Light _light; // Reference to the receiver (the Light object)

        public LightOnCommand(Light light)
        {
            _light = light;
        }

        /// <summary>
        /// Executes the command by calling the receiver's TurnOn method.
        /// </summary>
        public void Execute()
        {
            _light.TurnOn();
        }
    }

    /// <summary>
    /// Step 3: Implement Concrete Commands - LightOffCommand
    /// This concrete command encapsulates the request to turn a light off.
    /// It holds a reference to the 'Receiver' (Light) and implements the Execute method.
    /// </summary>
    public class LightOffCommand : ICommand
    {
        private Light _light; // Reference to the receiver (the Light object)

        public LightOffCommand(Light light)
        {
            _light = light;
        }

        /// <summary>
        /// Executes the command by calling the receiver's TurnOff method.
        /// </summary>
        public void Execute()
        {
            _light.TurnOff();
        }
    }

    /// <summary>
    /// Step 4: Implement Invoker Class - RemoteControl
    /// The 'Invoker' sends a request to the 'Command' object.
    /// It doesn't know anything about the concrete command or the receiver.
    /// </summary>
    public class RemoteControl
    {
        private ICommand _command; // Holds a reference to the command object

        /// <summary>
        /// Sets the command that the remote control will execute.
        /// </summary>
        /// <param name="command">The command to be set.</param>
        public void SetCommand(ICommand command)
        {
            _command = command;
            Console.WriteLine($"Remote control command set to: {command.GetType().Name}");
        }

        /// <summary>
        /// Executes the currently set command. This is the 'button press' action.
        /// </summary>
        public void PressButton()
        {
            if (_command != null)
            {
                Console.WriteLine("Remote Control: Button Pressed!");
                _command.Execute(); // Delegates the execution to the command object
            }
            else
            {
                Console.WriteLine("Remote Control: No command set for the button.");
            }
        }
    }

    /// <summary>
    /// Test class to demonstrate issuing commands using the RemoteControl.
    /// This acts as the 'Client' that creates concrete command objects and sets them to the invoker.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Command Pattern Example: Home Automation System ---\n");

            // 1. Create the Receiver object(s)
            Light livingRoomLight = new Light("Living Room");
            Light kitchenLight = new Light("Kitchen");

            // 2. Create Concrete Command objects, associating them with their Receivers
            ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
            ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
            ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
            ICommand kitchenLightOff = new LightOffCommand(kitchenLight);

            // 3. Create the Invoker object
            RemoteControl remote = new RemoteControl();

            Console.WriteLine("\n--- Scenario 1: Turning Living Room Light On ---");
            remote.SetCommand(livingRoomLightOn); // Set the command
            remote.PressButton(); // Execute the command

            Console.WriteLine("\n--- Scenario 2: Turning Kitchen Light Off ---");
            remote.SetCommand(kitchenLightOff); // Change the command
            remote.PressButton(); // Execute the new command

            Console.WriteLine("\n--- Scenario 3: Turning Living Room Light Off ---");
            remote.SetCommand(livingRoomLightOff);
            remote.PressButton();

            Console.WriteLine("\n--- Scenario 4: Turning Kitchen Light On ---");
            remote.SetCommand(kitchenLightOn);
            remote.PressButton();

            Console.WriteLine("\n--- Command Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
