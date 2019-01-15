using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    public class SimplerAES : IStringEncryptor
    {
        #region Properties

        private static ICryptoTransform _encryptor;
        private static ICryptoTransform _decryptor;

        private static Encoding Encoder
        {
            get { return new UTF8Encoding(); }
        }

        private static byte[] Key
        {
            get
            {
                var key = new byte[] { 123, 217, 19, 11, 24, 26, 85, 45, 114, 184, 27, 162, 37, 112, 222, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
                try
                {
                    key = Encoder.GetBytes(ConfigHelper.GetAppSetting("UrlEncryptionKey"));
                }
                catch (Exception)
                {
                    //throw;
                }
                return key;
            }
        }

        private static byte[] Vector
        {
            get
            {
                var vector = new byte[] { 146, 64, 191, 111, 23, 3, 113, 119, 231, 121, 221, 112, 79, 32, 114, 156 };
                try
                {
                    vector = Encoder.GetBytes(ConfigHelper.GetAppSetting("UrlEncryptionVector"));

                    if (vector.Length < 16)
                    {
                        var vectorLst = new List<byte>(vector);
                        while (vectorLst.Count < 16)
                            vectorLst.AddRange(vector);

                        vector = vectorLst.Take(16).ToArray();
                    }
                }
                catch (Exception)
                {
                    //throw;
                }

                return vector;
            }
        }

        private static ICryptoTransform Encryptor
        {
            get
            {
                if (_encryptor == null)
                {
                    var rm = new RijndaelManaged();
                    _encryptor = rm.CreateEncryptor(Key, Vector);
                }

                return _encryptor;
            }
        }

        private static ICryptoTransform Decryptor
        {
            get
            {
                if (_decryptor == null)
                {
                    var rm = new RijndaelManaged();
                    _decryptor = rm.CreateDecryptor(Key, Vector);
                }

                return _decryptor;
            }
        }

        #endregion

        #region Methods

        private byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, Encryptor);
        }

        private byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, Decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        #endregion

        #region IStringEncryptor Members

        public string Encrypt(string unencrypted)
        {
            byte[] buff = Encrypt(Encoder.GetBytes(unencrypted));
            return HttpServerUtility.UrlTokenEncode(buff);
        }

        public string Decrypt(string encrypted)
        {
            byte[] buff = Decrypt(HttpServerUtility.UrlTokenDecode(encrypted));
            return Encoder.GetString(buff);
        }

        #endregion
    }
}