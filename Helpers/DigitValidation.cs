namespace MeterReading.Helpers
{
    public static class DigitValidation
    {
        public static bool IsDigitsOnly(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            foreach (char c in str)
            {
                if(c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
