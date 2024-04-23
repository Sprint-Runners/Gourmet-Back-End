using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Helpers
{
    public class HashHelper
    {
        public HashHelper()
        {

        }
        public string HashString(string input)
        {
            // Convert the string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (SHA1 sha1Hash = SHA1.Create())
            {
                // Compute the hash of the input bytes
                byte[] hashBytes = sha1Hash.ComputeHash(inputBytes);

                // Convert the byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Convert each byte to its hexadecimal representation
                }
                return builder.ToString();
            }
        }
    }
}
