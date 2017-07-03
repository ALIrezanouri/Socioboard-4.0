﻿using Domain.Socioboard.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Socioboard.Helpers
{
    public static class SBHelper
    {
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }





        public static string MD5Hash(string text)
        {
            MD5 md5 = MD5.Create();

            //compute hash from the bytes of text
            byte[] result = md5.ComputeHash(System.Text.Encoding.Unicode.GetBytes(text));

            //get hash result after compute it
            //  byte[] result = md5.ComputeHash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }


        public static int GetMaxProfileCount(SBAccountType accountType)
        {
            int ret = 5;
            switch (accountType)
            {
                case SBAccountType.Free:
                    ret = 5;
                    break;
                case SBAccountType.Standard:
                    ret = 10;
                    break;

                case SBAccountType.Premium:
                    ret = 20;
                    break;
                case SBAccountType.Deluxe:
                    ret = 50;
                    break;
                case SBAccountType.Topaz:
                    ret = 100;
                    break;
                case SBAccountType.Ruby:
                    ret = 200;
                    break;
                case SBAccountType.Gold:
                    ret = 500;
                    break;
                case SBAccountType.Platinum:
                    ret = 1000;
                    break;
                default:
                    ret = 5;
                    break;
              


            }
            return ret;
        }

        public static int GetMaxGroupCount(SBAccountType accountType)
        {
            int ret = 5;
            switch (accountType)
            {
                case SBAccountType.Free:
                    ret = 1;
                    break;
                case SBAccountType.Standard:
                    ret = 1;
                    break;

                case SBAccountType.Premium:
                    ret = 1;
                    break;
                case SBAccountType.Deluxe:
                    ret = 5;
                    break;
                case SBAccountType.Topaz:
                    ret = 10;
                    break;
                case SBAccountType.Ruby:
                    ret = 15;
                    break;
                case SBAccountType.Gold:
                    ret = 20;
                    break;
                case SBAccountType.Platinum:
                    ret = 25;
                    break;
                default:
                    ret = 1;
                    break;



            }
            return ret;
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }


    }
}
