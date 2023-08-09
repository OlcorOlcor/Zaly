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
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            string salt = configuration.GetConnectionString("CodeSalt")!;
            const int size = 16;
            byte[] codeSalt = DecodeSalt(salt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(Id.ToString()), codeSalt, _numberOfIterations, _algorithm, size);
            return Convert.ToHexString(hash);
        }
    }
}
