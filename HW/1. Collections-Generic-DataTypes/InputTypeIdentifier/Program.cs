using System;
using System.Globalization;

namespace InputTypeIdentifier
{
    class Program
    {


        static void Main(string[] args)
        {
            string input = Console.ReadLine();

            //dynamic type;  
            // doesn't work with TryParse on (out result) to exclude if-conditions.

            GetTypeFromInput(input);
        }

        public static void GetTypeFromInput(string input)
        {
            DateTime date;  // e.g. to input: '8/8/2022 8:23'
            if (DateTime.TryParseExact(input, datetimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                Console.WriteLine(date.GetType());
                return;
            }

            int integer; // e.g. 1
            if (int.TryParse(input, out integer))
            {
                Console.WriteLine(integer.GetType());
                return;
            }

            
            double doubler; // e.g. 1.0
            if (double.TryParse(input, out doubler))
            {
                Console.WriteLine(doubler.GetType());
                return;
            }

            // else - string-type
            Console.WriteLine(input.GetType());
        }

        static string[] datetimeFormats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                   "MM/d/yyyy HH:mm:ss.ffffff" };
    }
}
