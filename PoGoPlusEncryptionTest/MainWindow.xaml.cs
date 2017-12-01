using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoGoPlusEncryptionTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            String sIn = tbChallange.Text;
            byte[] first = HexStringToByteArray(sIn.Substring(0, sIn.Length / 2)); //Should get the first 16 bytes
            byte[] sec = HexStringToByteArray(sIn.Substring(sIn.Length / 2, sIn.Length / 2)); //Should get the next 16 bytes
            byte[] EnC = EncryptAES(first, HexStringToByteArray("bda885742bc53918793ade3fa7b6cf3b"), true); // Encrypt(first);
            byte[] result = XOR(EnC, sec);
            tbResult.Text = ByteArrayToHexString(result).ToLower();
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public byte[] XOR(byte[] buffer1, byte[] buffer2)
        {
            for (int i = 0; i < buffer1.Length; i++)
                buffer1[i] ^= buffer2[i];
            return buffer1;
        }

        public static byte[] EncryptAES(byte[] message, byte[] key, bool doEncryption)
        {
            try
            {
                Aes aes = new AesManaged();
                aes.Key = key;
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                ICryptoTransform cipher;

                if (doEncryption == true)
                    cipher = aes.CreateEncryptor();
                else
                    cipher = aes.CreateDecryptor();

                return cipher.TransformFinalBlock(message, 0, message.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
