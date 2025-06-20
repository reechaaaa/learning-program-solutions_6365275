using System;
using System.Threading; // Required for Thread.Sleep

namespace ProxyPatternExample
{
    /// <summary>
    /// Step 2: Define Subject Interface
    /// This is the common interface that both the Real Subject and the Proxy will implement.
    /// The client interacts with this interface.
    /// </summary>
    public interface IImage
    {
        void Display();
    }

    /// <summary>
    /// Step 3: Implement Real Subject Class
    /// This class represents the actual image that needs to be loaded (e.g., from a remote server).
    /// Loading is simulated with a delay to demonstrate lazy initialization.
    /// </summary>
    public class RealImage : IImage
    {
        private string _fileName;

        /// <summary>
        /// Constructor for RealImage. Simulates loading the image from a remote server.
        /// </summary>
        /// <param name="fileName">The name/path of the image file.</param>
        public RealImage(string fileName)
        {
            _fileName = fileName;
            LoadImageFromRemoteServer(); // Image loading happens in the constructor
        }

        /// <summary>
        /// Simulates the time-consuming process of loading an image.
        /// </summary>
        private void LoadImageFromRemoteServer()
        {
            Console.WriteLine($"   RealImage: Loading '{_fileName}' from remote server... (Please wait)");
            Thread.Sleep(2000); // Simulate a 2-second delay for loading
            Console.WriteLine($"   RealImage: Loaded '{_fileName}' successfully.");
        }

        /// <summary>
        /// Displays the loaded image.
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"   RealImage: Displaying '{_fileName}'.");
        }
    }

    /// <summary>
    /// Step 4: Implement Proxy Class
    /// This class acts as a proxy (placeholder) for RealImage.
    /// It implements lazy initialization (loads RealImage only when needed)
    /// and caching (avoids reloading if already loaded).
    /// </summary>
    public class ProxyImage : IImage
    {
        private string _fileName;
        private RealImage? _realImage; // Holds a reference to the RealImage (nullable initially)

        /// <summary>
        /// Constructor for ProxyImage. It only stores the file name, it does NOT load the image yet.
        /// </summary>
        /// <param name="fileName">The name/path of the image file.</param>
        public ProxyImage(string fileName)
        {
            _fileName = fileName;
            Console.WriteLine($"ProxyImage: Created proxy for '{_fileName}'. Image not loaded yet.");
        }

        /// <summary>
        /// Implements the Display method from IImage.
        /// This method controls access to the RealImage.
        /// It performs lazy loading and caching.
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"ProxyImage: Display method called for '{_fileName}'.");

            // Lazy initialization: Create RealImage only if it hasn't been created yet.
            if (_realImage == null)
            {
                Console.WriteLine($"ProxyImage: RealImage for '{_fileName}' is null. Creating it now...");
                _realImage = new RealImage(_fileName); // This triggers the actual loading
            }
            else
            {
                // Caching: If RealImage already exists, just use it.
                Console.WriteLine($"ProxyImage: RealImage for '{_fileName}' already loaded. Displaying cached version.");
            }

            // Delegate the display request to the RealImage object.
            _realImage.Display();
        }
    }

    /// <summary>
    /// Step 5: Test the Proxy Implementation
    /// The client code interacts only with the IImage interface,
    /// unaware if it's dealing with a RealImage or a ProxyImage directly.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Proxy Pattern Example: Image Viewer with Lazy Loading & Caching ---\n");

            // Create a proxy image. At this point, the RealImage is NOT loaded.
            Console.WriteLine("Client: Creating image 'photo1.jpg'...");
            IImage image1 = new ProxyImage("photo1.jpg");
            Console.WriteLine("Client: Image proxy created. Real image not loaded yet.\n");

            // First call to Display() for image1: RealImage will be loaded.
            Console.WriteLine("Client: First display call for 'photo1.jpg'...");
            image1.Display();
            Console.WriteLine("Client: First display call for 'photo1.jpg' finished.\n");

            // Second call to Display() for image1: RealImage will NOT be reloaded (cached).
            Console.WriteLine("Client: Second display call for 'photo1.jpg' (should be faster due to caching)...");
            image1.Display();
            Console.WriteLine("Client: Second display call for 'photo1.jpg' finished.\n");

            Console.WriteLine("---------------------------------------------------\n");

            // Create another proxy image. Again, RealImage is NOT loaded yet.
            Console.WriteLine("Client: Creating image 'document.png'...");
            IImage image2 = new ProxyImage("document.png");
            Console.WriteLine("Client: Image proxy created. Real image not loaded yet.\n");

            // First call to Display() for image2: RealImage will be loaded.
            Console.WriteLine("Client: Display call for 'document.png'...");
            image2.Display();
            Console.WriteLine("Client: Display call for 'document.png' finished.\n");

            Console.WriteLine("\n--- Proxy Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
