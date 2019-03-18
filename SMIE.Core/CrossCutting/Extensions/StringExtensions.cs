using System;
using System.Security.Cryptography;
using System.Text;

namespace SMIE.Core.CrossCutting.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateSha256Hash(this string value)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(value));
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public static string Format(this string message, params object[] parameters)
        {
            message = message ?? string.Empty;
            return parameters.Length > 0 ? string.Format(message, parameters) : message;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }
}
