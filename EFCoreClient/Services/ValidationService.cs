using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EFCoreClient.Services
{
    public static class ValidationService
    {
        static string emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + 
                                     @"@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

        static string phoneNumberPattern = @"^[\d+]\d{7,12}[^a-z+]$";
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, emailPattern);
        }
        
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, phoneNumberPattern);
        }

        public static bool IsValidDateTimeFormat(string dateTime)
        {
            DateTime date;
            return DateTime.TryParse(dateTime,out date);
        }

        public static bool IsValidDeleveryDateTime(DateTime dateTime)
        {
            if (dateTime.CompareTo(DateTime.Today) == -1 || dateTime.CompareTo(DateTime.Today) == 0) return false;
            else return true;
        }
    }
}
