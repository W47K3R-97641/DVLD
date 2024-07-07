using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UI_LAYER.Global_Classes
{
    internal class clsValidation
    {
        public static bool ValidateInteger(string Number)
        {
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidateFloat(string Number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }
        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number));
        }
        static public bool ValidateName(string name)
        {
            // Define the regular expression pattern for valid names
            // The pattern allows only alphabetic characters and spaces
            // and optionally hyphens and apostrophes if you choose to include them.
            string pattern = @"^[A-Za-z\s-']{1,50}$";

            // Match the name against the regular expression
            Regex regex = new Regex(pattern);
            bool isValid = regex.IsMatch(name);

            // Return true if the name is valid according to the pattern
            return isValid;
        }
        static public bool ValidatePhoneNumber(string phoneNumber)
        {
            // Define the regular expression pattern for a valid phone number
            // This pattern allows digits, spaces, hyphens, and parentheses
            // The number must contain at least three digits
            string pattern = @"^(?=.*\d{3})[\d\s\-\(\)]*$";

            // Match the phone number against the regular expression pattern
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }
        static public bool ValidateEmail(string email)
        {
            // Define the regular expression pattern for a valid email address
            // The pattern follows the format: local part, @, domain part
            // Local part can contain letters, digits, and certain special characters
            // Domain part follows domain name rules with letters, digits, hyphens, and periods
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Match the email against the regular expression pattern
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        
        public static bool ValidateUsernameFormat(string username)
        {
            // Check if username is null or empty
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            // Check if username length is between 3 and 20 characters
            if (username.Length < 3 || username.Length > 20)
            {
                return false;
            }

            // Check if username contains only alphanumeric characters and underscores
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
            {
                return false;
            }

            // Username is valid
            return true;
        }
        public static bool ValidatePassword(string password)
        {
            // Check if the password meets the criteria
            if (string.IsNullOrWhiteSpace(password))
            {
                // Password should not be null, empty, or contain only whitespace
                return false;
            }

            // Regular expression pattern to enforce password requirements
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            // Validate password against the pattern
            if (!Regex.IsMatch(password, pattern))
            {
                // Password does not meet the requirements
                return false;
            }

            // Password meets all requirements
            return true;
        }
    }
}
