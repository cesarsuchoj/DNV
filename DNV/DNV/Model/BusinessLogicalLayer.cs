using System;
using System.Globalization;

namespace DNVDO.Model
{
    public class BusinessLogicalLayer
    {
        public static int IdadePai(DateTime birthDate)
        {
            int result = birthDate.Year.CompareTo(DateTime.Now.Year);
            if (result < 0)
                throw (new Exception("A data de nascimento deve ser anterior ao dia atual."));
            return result;
        }
        public static int IdadeMae(DateTime birthDate, DateTime registryDate, bool isOnDeadline)
        {
            return isOnDeadline ? birthDate.Year.CompareTo(DateTime.Now.Year) : registryDate.Year.CompareTo(DateTime.Now.Year);
        }
        public static bool DNVValidate(string dnvNumber)
        {
            long number;
            switch (dnvNumber.Length)
            {
                case 8: //BBBBBBBB
                    if (Int64.Parse(dnvNumber) < 43700001)
                        return true;
                    else
                        return false;
                case 9: //BBBBBBBB-D
                    number = Int64.Parse(dnvNumber.Substring(0, 8));
                    if (number >= 43700001 && number < 48101001)
                        return DV1(dnvNumber);
                    else
                        return false;
                default: //AA-BBBBBBBB-D
                    number = Int64.Parse(dnvNumber.Substring(2, 8));
                    if (number >= 48101001)
                        return DNVDV2(dnvNumber);
                    else
                        return DNVDV2(dnvNumber);
            }
        }
        public static bool DOValidate(string doNumber)
        {
            if (doNumber.Length <= 8)
            {
                if (Int64.Parse(doNumber) < 12075501)
                    return true;
                else
                    return false;
            }
            else
            {
                long number = Int64.Parse(doNumber.Substring(0, 8));
                if (number >= 12075501 && number < 13600000)
                    return DV1(doNumber);
                else if (number >= 13600000)
                    return DODV2(doNumber);
                else // number is < 12075501 AND has a DV
                    return false;
            }
        }
        private static bool DV1(string nineDigitNumber)
        {
            long eightDigitNumber = Int64.Parse(nineDigitNumber.Substring(0, nineDigitNumber.Length - 1), NumberStyles.Number);
            int dv = (int) eightDigitNumber % 11;
            return nineDigitNumber.EndsWith(dv == 10 ? "0" : dv.ToString());
        }
        private static bool DNVDV2(string nineDigitNumber)
        {
            int sum = 0;
            if (nineDigitNumber.Length > 9)
            {
                int z1 = Int16.Parse(nineDigitNumber.Substring(0, 1)) * 3;
                int z2 = Int16.Parse(nineDigitNumber.Substring(1, 1)) * 2;
                sum += z1 + z2;
            }
            sum += (Int16.Parse(nineDigitNumber.Substring(0, 1)) * 9) + 
                   (Int16.Parse(nineDigitNumber.Substring(1, 1)) * 8) + 
                   (Int16.Parse(nineDigitNumber.Substring(2, 1)) * 7) + 
                   (Int16.Parse(nineDigitNumber.Substring(3, 1)) * 6) + 
                   (Int16.Parse(nineDigitNumber.Substring(4, 1)) * 5) + 
                   (Int16.Parse(nineDigitNumber.Substring(5, 1)) * 4) + 
                   (Int16.Parse(nineDigitNumber.Substring(6, 1)) * 3) + 
                   (Int16.Parse(nineDigitNumber.Substring(7, 1)) * 2);
            int rest = (11 - (sum % 11));
            if (rest >= 10)
                return Int16.Parse(nineDigitNumber.Substring(nineDigitNumber.Length - 1, 1)).Equals(0);
            return Int16.Parse(nineDigitNumber.Substring(nineDigitNumber.Length - 1, 1)).Equals(rest);
        }
        private static bool DODV2(string nineDigitNumber)
        {
            int sum = 0;
            sum += (Int16.Parse(nineDigitNumber.Substring(0, 1)) * 9) + 
                   (Int16.Parse(nineDigitNumber.Substring(1, 1)) * 8) + 
                   (Int16.Parse(nineDigitNumber.Substring(2, 1)) * 7) + 
                   (Int16.Parse(nineDigitNumber.Substring(3, 1)) * 6) + 
                   (Int16.Parse(nineDigitNumber.Substring(4, 1)) * 5) + 
                   (Int16.Parse(nineDigitNumber.Substring(5, 1)) * 4) + 
                   (Int16.Parse(nineDigitNumber.Substring(6, 1)) * 3) + 
                   (Int16.Parse(nineDigitNumber.Substring(7, 1)) * 2);
            int rest = (11 - (sum % 11));
            if (rest >= 10)
                return Int16.Parse(nineDigitNumber.Substring(nineDigitNumber.Length - 1, 1)).Equals(0);
            return Int16.Parse(nineDigitNumber.Substring(nineDigitNumber.Length - 1, 1)).Equals(rest);
        }
    }

}