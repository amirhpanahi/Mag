using System.Globalization;

namespace Mag.Common
{
    public class Utility
    {
        #region ConvertToPersian
        public static string ConvertToPersian(DateTime InputDateTime, bool hour = true)
        {
            string DatePersian = String.Empty;
            try
            {
                var pc = new PersianCalendar();
                DatePersian = $"{pc.GetYear(InputDateTime)}/{pc.GetMonth(InputDateTime)}/{pc.GetDayOfMonth(InputDateTime)}";
                if (hour)
                {
                    DatePersian += $" {pc.GetHour(InputDateTime)}:{pc.GetMinute(InputDateTime)}:{pc.GetSecond(InputDateTime)}";
                }
            }
            catch { }
            return DatePersian;
        }
        #endregion

        #region CalculateSumOfInt
        public static long CalculateSum(string numbersString)
        {
            long sum = 0;
            if (numbersString.Length == 11 && long.TryParse(numbersString, out long number))
            {
                // تبدیل رشته به عدد و محاسبه مجموع
                sum = CalculateSumOfDigits(number);
            }
            else
            {
                Console.WriteLine("رشته ورودی معتبر نیست.");
            }
            return sum;
        }
        static long CalculateSumOfDigits(long number)
        {
            long sum = 0;
            while (number > 0)
            {
                sum += number % 10;
                number /= 10;
            }
            return sum;
        }
        #endregion

        #region CalculateSumOfString
        public static int CalculateAsciiSum(string inputString)
        {
            int sum = 0;
            foreach (char c in inputString)
            {
                sum += (int)c;
            }
            return sum;
        }
        #endregion
    }
}