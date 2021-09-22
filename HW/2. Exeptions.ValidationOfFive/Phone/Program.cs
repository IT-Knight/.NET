using System;
using System.Text.RegularExpressions;

namespace Phone
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] regexp_phone_patterns = new string[] { 
                @"^([0-9\(\)\/\+ \-]*)$",
                @"^\+? (\d[\d -. ] +)?(\([\d -. ] +\))?[\d-. ]+\d$",
                @"^([\(]{1}[0-9]{3}[\)]{1}[\.| |\-]{0,1}|^[0-9]{3}[\.|\-| ]?)?[0-9]{3}(\.|\-| )?[0-9]{4}$",
                @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$",
            };

            string[] US_phone_examples = new string[] {
                "1-234-567-8901",
                "1-234-567-8901 x1234",
                "1-234-567-8901 ext1234",
                "1 (234) 567-8901",
                "1.234.567.8901",
                "1/234/567/8901",
                "12345678901",
                "+37379845678"
            };
            string US_regexp_pattern = @"^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$";

            foreach (string phone in US_phone_examples)
            {
                try
                {
                    bool IsUsPhone = Regex.IsMatch(phone, US_regexp_pattern, RegexOptions.IgnoreCase);
                    if (!IsUsPhone)
                    {
                        throw new Exception();
                    }
                    Console.WriteLine($"{phone} is a US phone\n");
                }
                catch (Exception)
                {
                    Console.WriteLine($"{phone} is not a US phone\n");
                }
            }


        }
    }
}
