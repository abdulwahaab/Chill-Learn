using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ChillLearn
{
    public class Encryptor
    {
        static string secretKey = "012904c2cdf716c4b4f8f7e186efece9";

        public static string Encrypt(string plainText)
        {
            try
            {
                RijndaelManaged manager = GetRijndaelManaged(secretKey);
                byte[] result = manager.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, Encoding.UTF8.GetBytes(plainText).Length);
                byte[] completeBytes = new byte[result.Length + manager.IV.Length];
                System.Buffer.BlockCopy(manager.IV, 0, completeBytes, 0, manager.IV.Length);
                System.Buffer.BlockCopy(result, 0, completeBytes, manager.IV.Length, result.Length);
                return ByteArrayToString(completeBytes);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string Decrypt(string encryptedData)
        {
            try
            {
                RijndaelManaged manager = GetRijndaelManaged(secretKey);
                byte[] inputBytes = HexString2Bytes(encryptedData);
                byte[] result = manager.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                byte[] newArray = new byte[result.Length - 32];
                Array.Copy(result, 32, newArray, 0, newArray.Length);
                return Encoding.UTF8.GetString(newArray);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[32];
            //var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var secretKeyBytes = HexToByteArray(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 256,
                BlockSize = 256,
                Key = keyBytes,
                IV = keyBytes,
                FeedbackSize = 256
            };
        }

        public static byte[] HexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static byte[] HexString2Bytes(string hexString)
        {
            if (hexString == null) return null;
            int len = hexString.Length;
            if (len % 2 == 1) return null;
            int len_half = len / 2;
            byte[] bs = new byte[len_half];
            try
            {
                for (int i = 0; i != len_half; i++)
                {
                    bs[i] = (byte)Int32.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch (Exception)
            {
            }
            return bs;
        }

        public static string ByteArrayToString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}