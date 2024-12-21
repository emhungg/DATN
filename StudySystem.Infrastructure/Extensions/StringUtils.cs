using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Infrastructure.Extensions
{
    public static class StringUtils
    {
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string NewUserRegisterAds()
        {
            return "Cảm ơn bạn đã đăng ký dịch vụ tin tức mới của chúng tôi";
        }

        public static string RankUser(decimal point)
        {
            if (point < 3000000)
            {
                return "T-NULL";
            }
            else if (point < 15000000)
            {
                return "T-New";
            }
            else if (point < 50000000)
            {
                return "T-Mem";
            }
            else
            {
                return "VIP";
            }
        }

        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static string ConvertToLowerCaseStart(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            char[] array = input.ToCharArray();
            array[0] = char.ToLower(array[0]);
            return new string(array);
        }

        public static string ShortenString(string input, int maxLength)
        {
            if (input.Length <= maxLength)
            {
                return input;
            }
            else
            {
                return input.Substring(0, maxLength) + "...";
            }
        }

        public static string TimeZoneUTC(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.CreateCustomTimeZone("CustomTimeZone", TimeSpan.FromHours(7), "Custom Time Zone", "Custom Time Zone")).ToString("dd/MM/yyyy HH:mm");
        }

    }
}
