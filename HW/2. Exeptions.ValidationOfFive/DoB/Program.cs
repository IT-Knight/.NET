using System;

namespace DoB
{
    class Program
    {
        static void Main(string[] args)
        {

            string input = null;  //Console.ReadLine();

            string DoB = "09/06/1975";

            try
            {
                DateTime ValidDate = DateTime.Parse(input ?? DoB);

                int age = DateTime.Now.Year - ValidDate.Year;

                if (age < 0 || age > 120 )
                {
                    throw new Exception($"{input ?? DoB} is not a valid DateOfBirth");
                }
                Console.WriteLine($"{ValidDate.Date:MM/dd/yyyy} is a valid Date of birth");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
    }
}
