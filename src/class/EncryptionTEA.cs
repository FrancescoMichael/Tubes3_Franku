using System.Text;

namespace FrankuGUI{
    class EncryptionTEA
    {
        private static readonly uint Delta = 0x9e3779b9;    // 2654435769 or 0x9E3779B9 is chosen to be ⌊2^(32)⁄𝜙⌋
        private static readonly int Rounds = 32;            // jumlah putaran
 
        // enkripsi data
        public static byte[] Encrypt(byte[] data, byte[] key)
        { 
            // Memisahkan data menjadi dua bagian 32-bit
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);

            // Memisahkan kunci menjadi empat bagian 32-bit
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            uint sum = 0;
            for (int i = 0; i < Rounds; i++)
            {
                // Menambahkan nilai sum dengan delta
                sum += Delta;

                // operasi pada v0 dan v1 dengan kombinasi << >> dan ^
                v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
            }

            // Menggabungkan v0 dan v1 menjadi satu array byte
            byte[] encrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, encrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, encrypted, 4, 4);

            // hasil enkripsi
            return encrypted;
        }

        // dekripsi data
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            // Memisahkan data menjadi dua bagian 32-bit
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);

            // Memisahkan kunci menjadi empat bagian 32-bit
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            // Memulai sum dengan delta * rounds
            uint sum = Delta * (uint)Rounds;
            for (int i = 0; i < Rounds; i++)
            {
                // operasi pada v0 dan v1 dengan kombinasi << >> dan ^ (kebalikan dari enkripsi)
                v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
                v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);

                // Mengurangi nilai sum dengan delta
                sum -= Delta;
            }

            // Menggabungkan v0 dan v1 menjadi satu array byte
            byte[] decrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, decrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, decrypted, 4, 4);

            // hasil dekripsi
            return decrypted;
        }

        // enkripsi string
        public static string EncryptString(string plainText, string key)
        {
            // teks menjadi byte
            byte[] data = Encoding.UTF8.GetBytes(plainText);

            // padding data agar panjangnya kelipatan 8
            byte[] paddedData = Pad(data);

            // kunci menjadi byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // simpan data enkripsi
            byte[] encryptedData = new byte[paddedData.Length];

            // enkripsi setiap blok 8-byte
            for (int i = 0; i < paddedData.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(paddedData, i, block, 0, 8);
                byte[] encryptedBlock = Encrypt(block, keyBytes);
                Array.Copy(encryptedBlock, 0, encryptedData, i, 8);
            }

            // ubah enkripsi menjadi string Base64
            return Convert.ToBase64String(encryptedData);
        }

        // dekripsi string
        public static string DecryptString(string encryptedText, string key)
        {
            // teks menjadi byte
            byte[] data = Convert.FromBase64String(encryptedText);

            // kunci menjadi byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // simpan data dekripsi
            byte[] decryptedData = new byte[data.Length];

            // dekripsi setiap blok 8-byte
            for (int i = 0; i < data.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(data, i, block, 0, 8);
                byte[] decryptedBlock = Decrypt(block, keyBytes);
                Array.Copy(decryptedBlock, 0, decryptedData, i, 8);
            }

            // hapus padding
            // ubah jadi string
            return Encoding.UTF8.GetString(Unpad(decryptedData));
        }

        // padding data
        private static byte[] Pad(byte[] data)
        {
            // hitung padding yang dibutuhkan
            int paddingLength = 8 - (data.Length % 8);

            // data asli + padding
            byte[] paddedData = new byte[data.Length + paddingLength];
            Array.Copy(data, 0, paddedData, 0, data.Length);

            // tambah byte padding
            for (int i = data.Length; i < paddedData.Length; i++)
            {
                paddedData[i] = (byte)paddingLength;
            }

            // hasil padding
            return paddedData;
        }

        // hapus padding
        private static byte[] Unpad(byte[] data)
        {
            // jumlah padding
            int paddingLength = data[data.Length - 1];

            // data tanpa padding
            byte[] unpaddedData = new byte[data.Length - paddingLength];
            Array.Copy(data, 0, unpaddedData, 0, unpaddedData.Length);

            // hasil tanpa padding
            return unpaddedData;
        }
    }
}
