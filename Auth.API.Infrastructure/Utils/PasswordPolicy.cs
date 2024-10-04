using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Utils
{
    public static class PasswordPolicy
    {
        private static int _minLength = 6;
        private static int _amountCapitalized = 1;
        private static int _amountNumbers = 1;
        private static int _amountSpecChars = 1;

        public static bool IsPasswordValid(string password, out string message)
        {
            message = string.Empty;

            if (password.Length < PasswordPolicy._minLength)
            {
                message = $"Password needs to be at least {PasswordPolicy._minLength} characters long";
                return false;
            }

            if (!PasswordPolicy.CapitalLettersMatch(password))
            {
                message = $"Password needs at least {PasswordPolicy._amountCapitalized} capitalized character";
                return false;
            }

            if (!PasswordPolicy.NumberMatch(password))
            {
                message = $"Password needs at least {PasswordPolicy._amountNumbers} number";
                return false;
            }

            if (!PasswordPolicy.SpecCharsMatch(password))
            {
                message = $"Password needs at least {PasswordPolicy._amountSpecChars} special character";
                return false;
            }

            return true;
        }

        private static bool SpecCharsMatch(string password)
        {
            string pattern = $@"^.*([\W_].*){{{PasswordPolicy._amountSpecChars}}}.*$";
            return Regex.IsMatch(password, pattern);
        }

        private static bool NumberMatch(string password)
        {
            string pattern = $@"^.*(\d.*){{{PasswordPolicy._amountNumbers}}}.*$";
            return Regex.IsMatch(password, pattern);
        }

        private static bool CapitalLettersMatch(string password)
        {
            string pattern = $@"^.*([A-Z].*){{{PasswordPolicy._amountCapitalized}}}.*$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
