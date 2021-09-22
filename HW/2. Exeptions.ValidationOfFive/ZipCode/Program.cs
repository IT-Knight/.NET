using System;
using System.Text.RegularExpressions;

namespace ZipCode
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] zipcodes = new int[]
            {
                80170,
                30069,
                539977,
                70603,
                47197,
                88482,
                60860
            };

            foreach  (int zipcode in zipcodes)
            {
                try
                {
                    if (IsUSOrCanadianZipCode(zipcode.ToString())) {
                        Console.WriteLine($"{zipcode} - is a valid zipcode");
                    }
                    else
                    {
                        throw new Exception("Bad zipcode");
                    }
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{zipcode} - is not a valid zipcode");
                    Console.WriteLine(ex);
                }
            }

        }

        private static bool IsUSOrCanadianZipCode(string zipCode)
        {
            var validZipCode = true;

            var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
            var _caZipRegEx = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";

            if ((!Regex.Match(zipCode, _usZipRegEx).Success) && (!Regex.Match(zipCode, _caZipRegEx).Success))
            {
                validZipCode = false;
            }
            return validZipCode;
        }
    }
}

