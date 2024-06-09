using System.Text;

namespace FrankuGUI{
    class EncryptionTEA
    {
        private static readonly uint Delta = 0x9e3779b9;
        private static readonly int Rounds = 32;

        public static byte[] Encrypt(byte[] data, byte[] key)
        { 
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            uint sum = 0;
            for (int i = 0; i < Rounds; i++)
            {
                sum += Delta;
                v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
            }

            byte[] encrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, encrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, encrypted, 4, 4);
            return encrypted;
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            uint sum = Delta * (uint)Rounds;
            for (int i = 0; i < Rounds; i++)
            {
                v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
                v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                sum -= Delta;
            }

            byte[] decrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, decrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, decrypted, 4, 4);
            return decrypted;
        }

        public static string EncryptString(string plainText, string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] paddedData = Pad(data);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] encryptedData = new byte[paddedData.Length];
            for (int i = 0; i < paddedData.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(paddedData, i, block, 0, 8);
                byte[] encryptedBlock = Encrypt(block, keyBytes);
                Array.Copy(encryptedBlock, 0, encryptedData, i, 8);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptString(string encryptedText, string key)
        {
            byte[] data = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] decryptedData = new byte[data.Length];
            for (int i = 0; i < data.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(data, i, block, 0, 8);
                byte[] decryptedBlock = Decrypt(block, keyBytes);
                Array.Copy(decryptedBlock, 0, decryptedData, i, 8);
            }

            return Encoding.UTF8.GetString(Unpad(decryptedData));
        }

        private static byte[] Pad(byte[] data)
        {
            int paddingLength = 8 - (data.Length % 8);
            byte[] paddedData = new byte[data.Length + paddingLength];
            Array.Copy(data, 0, paddedData, 0, data.Length);
            for (int i = data.Length; i < paddedData.Length; i++)
            {
                paddedData[i] = (byte)paddingLength;
            }
            return paddedData;
        }

        private static byte[] Unpad(byte[] data)
        {
            int paddingLength = data[data.Length - 1];
            byte[] unpaddedData = new byte[data.Length - paddingLength];
            Array.Copy(data, 0, unpaddedData, 0, unpaddedData.Length);
            return unpaddedData;
        }
    }
}