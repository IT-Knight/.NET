using PrettyPrinter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Email
{

    class EmailTest { 

        [EmailAddress]
        public string Email  { get; set; }
    }

    public class Program
    {

        static void Main(string[] args)
        {
            bool check1, check2, check3, check4;
            
            foreach (string input in email_array)
            {
                Console.WriteLine("#############################################");

                check1 = EmailIsValid(input);
                check2 = IsValidEmailByRegexp1(input);
                check3 = IsValidEmailByRegexp2(input);
                check4 = EmailIsValidByAnnotaions(input);

                Console.WriteLine($"Email: {input}");
                
                Console.WriteLine("Email is valid by .Net: " + check1);

                Console.WriteLine("Email is valid by .Net Annotations: " + check4);

                Console.WriteLine("Email is valid by RegExp1: " + check2);

                Console.WriteLine("Email is valid by RegExp2ByRFC: " + check3);

                Console.WriteLine("------------------------------------------------------\n");
            }

            Console.WriteLine(
                "====================================================================================\n" +
                " Conclusions - System.Net.Mail.MailAddress is a good native Email-validator\n" +
                "               Or, a RegExp2 is a risky best practice without a TryCatch needed\n" +
                "               - RegExp1 is bad, but still adviced, on stackoverflow, at example\n" +
                "               - [EmailAdress] annotation validation is bad and strange\n" +
                "====================================================================================");

        }

        public static bool EmailIsValid(string email_string)
        {
            try
            {
                MailAddress email = new System.Net.Mail.MailAddress(email_string);
                return true;
            }
            catch (Exception ex)  // FormatExeption 
            {
                Console.WriteLine("BAD EMAIL");
                //Console.WriteLine(ex);  
                return false;
            }

        }

        public static bool IsValidEmailByRegexp1(string emailString)
        {
            var regexp_pattern1 = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            bool isEmail = Regex.IsMatch(emailString, regexp_pattern1, RegexOptions.IgnoreCase);

            return isEmail;
        }

        public static bool IsValidEmailByRegexp2(string emailString)
        {
            var regexp_pattern2 = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                    + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                    + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            ;
            bool isEmail2 = Regex.IsMatch(emailString, regexp_pattern2, RegexOptions.IgnoreCase);
            return isEmail2;
        }

        public static string[] email_array = new string[] {
            "Abc\\@def@example.com",
            "Fred\\ Bloggs@example.com",
            "Joe.\\Blow@example.com",
            "\"Abc@def\"@example.com",
            "\"Fred Bloggs\"@example.com",
            "customer/department=shipping@example.com",
            "$A12345@example.com",
            "!def!xyz%abc@example.com",
            "_somename@example.com",
            "nayoubbnkamal1@refk.site"
            };


        
        

        public static bool EmailIsValidByAnnotaions(string email)  // wrong results by DataAnotations
        {
            try
            {
                EmailTest email_object = new EmailTest();

                TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(
                    email_object.GetType()), 
                    email_object.GetType());
                
                
                email_object.Email = email;

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(email_object, null, null);
                context.MemberName = "Email";
                bool IsValid1 = Validator.TryValidateProperty(email_object.Email, context, validationResults);
                return IsValid1;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }



}
}
