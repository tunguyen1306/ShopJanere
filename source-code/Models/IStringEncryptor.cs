using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public interface IStringEncryptor
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}
