namespace BuildMaterials.Extensions
{
    public static class PassportExtensions
    {
        public static bool CheckPassportNumber(string passportNumber)
        {
            if (passportNumber.Length != 14) return false;

            int summ = 0;

            byte[] umn = { 7, 3, 1 };

            for (byte i = 0; i < 13; i++)
            {
                int num = Char.IsLetter(passportNumber[i]) ? passportNumber[i] - 55 : passportNumber[i] - 48;

                summ += num * umn[i % 3];
            }

            string sumStr = summ.ToString();
            return sumStr[sumStr.Length - 1] == passportNumber[13];
        }
    }
}
