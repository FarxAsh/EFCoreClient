using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EFCoreClient.Services
{
    
    public  class DiscountService
    {
        float discoutRate = 0.8f;
        
        Dictionary<string, string> Holidays = new Dictionary<string, string>
        {
            {"Новый год", "01.01"},
            {"День всех влюбленных","14.02" },
            {"Международный женский день", "08.02"},
            {"День победы", "09.05" }
        };
        

        public float GetDiscountRate()
        {
            foreach(var date in Holidays)
            {
                if (DateTime.Today.ToString("dd.MM", CultureInfo.InvariantCulture) == date.Value) return discoutRate;
               
            }
            return 1;
        }


    }
}
