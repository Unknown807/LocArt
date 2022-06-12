using System;
using System.Linq;

namespace Utilities
{
    static class Randomiser
    {
        private static Random random = new Random();
        
        /// <returns>10 random alphanumeric characters for prefixing temp and permanent image files</returns>
        public static string generateRandAlphaNumStr()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
