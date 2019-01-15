using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    public class Base64ForUrlEncryptor : IStringEncryptor
    {
        private readonly Encoding _encoder = new UTF8Encoding();

        #region IStringEncryptor Members

        public string Encrypt(string data)
        {
            byte[] buff = _encoder.GetBytes(data);
            return HttpServerUtility.UrlTokenEncode(buff);
        }

        public string Decrypt(string data)
        {
            byte[] buff = HttpServerUtility.UrlTokenDecode(data);
            return _encoder.GetString(buff);
        }

        #endregion
    }
}