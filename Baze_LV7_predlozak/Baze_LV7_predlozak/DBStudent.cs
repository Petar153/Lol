using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Baze_LV7_predlozak
{
    internal class DBStudent
    {
        //objekat koji cuva zaporku u sifriranom obliku
        private System.Security.SecureString Pwd;

        internal DBStudent(string pwd)
        {
            this.Pwd = ConvertToSecureString(pwd);
        }


        //konvertiraj obican niz znakova u sigurni
        private System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new System.Security.SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        //konvertiraj sigurni niz znakova u obican
        private string ConvertToString(System.Security.SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(value);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        //konstruraj konencijski string 
        internal SqlConnection GetConnection()
        {
            SqlConnection conn = null;

            //procitaj konekcijski string iz konfiguracije
            string cs = ConfigurationManager.AppSettings["ConnectionString"];
            
            //unesi zaporku
            cs = cs.Replace("*****", ConvertToString(this.Pwd));

            //kreiraj konecijski objekat
            conn = new SqlConnection(cs);

            return conn; 
        }

    }
}
