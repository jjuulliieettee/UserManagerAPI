using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UserManagerAPI.Services
{
    public static class PasswordService
    {
        private static readonly Random Random = new Random();

        private const int Pbkdf2IterCount = 10000;
        private const int Pbkdf2SubkeyLength = 256 / 8;
        private const int SaltSize = 128 / 8;

        public static string GetRandomString(int length = 8)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return HashPasswordInternal(password);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return VerifyHashedPasswordInternal(hashedPassword, password);
        }

        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        private static string HashPasswordInternal(string password)
        {
            var bytes = HashPasswordInternal(password, KeyDerivationPrf.HMACSHA256, Pbkdf2IterCount, SaltSize, Pbkdf2SubkeyLength);
            return Convert.ToBase64String(bytes);
        }

        private static byte[] HashPasswordInternal(
            string password,
            KeyDerivationPrf prf,
            int iterCount,
            int saltSize,
            int numBytesRequested)
        {
            var salt = new byte[saltSize];
            Rng.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

            var outputBytes = new byte[13 + salt.Length + subkey.Length];

            outputBytes[0] = 0x01;

            WriteNetworkByteOrder(outputBytes, 1, (uint)prf);

            WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);

            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);

            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);

            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

            return outputBytes;
        }

        private static bool VerifyHashedPasswordInternal(string hashedPassword, string password)
        {
            var decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            if (decodedHashedPassword.Length == 0)
            {
                return false;
            }

            try
            {
                if (decodedHashedPassword[0] != 0x01)
                {
                    return false;
                }

                var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHashedPassword, 1);

                var iterCount = (int)ReadNetworkByteOrder(decodedHashedPassword, 5);

                var saltLength = (int)ReadNetworkByteOrder(decodedHashedPassword, 9);

                if (saltLength < 128 / 8)
                {
                    return false;
                }

                var salt = new byte[saltLength];
                Buffer.BlockCopy(decodedHashedPassword, 13, salt, 0, salt.Length);

                var subkeyLength = decodedHashedPassword.Length - 13 - salt.Length;
                if (subkeyLength < 128 / 8)
                {
                    return false;
                }

                var expectedSubkey = new byte[subkeyLength];
                Buffer.BlockCopy(decodedHashedPassword, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

                var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, subkeyLength);
                return ByteArraysEqual(actualSubkey, expectedSubkey);
            }
            catch
            {
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint)buffer[offset + 0] << 24)
                   | ((uint)buffer[offset + 1] << 16)
                   | ((uint)buffer[offset + 2] << 8)
                   | buffer[offset + 3];
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)(value >> 0);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}