using System;
using System.Linq;

namespace Utilities
{
    static class Randomiser
    {
        private static Random random = new Random();
        public static string generateRandAlphaNumStr()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
