using Org.BouncyCastle.Utilities.Encoders;
using System.Security.Cryptography;
using System.Text;

namespace Zaly.Models {
    public class Hasher {
        private const int _keySize = 64;
        private const int _numberOfIterations = 400000;
        private HashAlgorithmName _algorithm = new("SHA512");
        public Hasher() { }
        public string HashPassword(string password, out string salt) {
            byte[] saltArray = RandomNumberGenerator.GetBytes(_keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltArray, _numberOfIterations, _algorithm, _keySize);
            salt = Convert.ToHexString(saltArray);
            return Convert.ToHexString(hash);
        }

        public bool VerifyPassword(string password, string hash, string salt) {
            var saltArray = DecodeSalt(salt);

            var newHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltArray, _numberOfIterations, _algorithm, _keySize);

            return CryptographicOperations.FixedTimeEquals(newHash, Convert.FromHexString(hash));
        }

        private byte[] DecodeSalt(string salt) {
            return Enumerable.Range(0, salt.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(salt.Substring(x, 2), 16))
                     .ToArray();
        }
        public string HashCode(int Id) {
            const string salt = "1BDD79E43A748308A9CFEC6F8F9751C372CEF9AF10AE927B36F49AFCF580438A7537A5F76F52573DA4836C6CCDF323CCD48495945B8D3F768C95428D8EE53488";
            const int size = 16;
            byte[] codeSalt = DecodeSalt(salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(Id.ToString()), codeSalt, _numberOfIterations, _algorithm, size);
            return Convert.ToHexString(hash);
        }
    }
}
