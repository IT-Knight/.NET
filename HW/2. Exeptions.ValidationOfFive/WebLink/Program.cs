using System;

namespace WebLink
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] urls_array = new string[]
            {
                "http://example.com/",
                "http://www.example.com/",
                "https://www.example.com/",
                "https://example.com/",
                "https:///www.example.com/aftermath.php",
                "https://example.net/bone?base=birth#arch"
            };

            foreach (string item in urls_array)
            {
                try
                {
                    Console.WriteLine(new Uri(item).AbsoluteUri + " - is a valid url");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{item} - is not a valid url\n");
                    Console.WriteLine(ex);
                    Console.WriteLine();
                }

            }
            
        }
    }
}
