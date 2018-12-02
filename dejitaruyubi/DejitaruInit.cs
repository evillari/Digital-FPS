using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.IO;

namespace dejitaruyubi
{
    abstract class DejitaruInit
    {
        
        public static string DSN;
        public static string UID;
        public static string PWD;

        public static bool IsDBReady = false;
        
        //Check registry if db key exists
        public static string[] GetncheckDSNKey()
        {

            RegistryKey dejitarudsnkey = Registry.LocalMachine.OpenSubKey(@"Software\\Dejitaruyubi\\ODBC Strings\\");
            RegistryKey dsnkey;
            dsnkey = null;


            string[] keys = new string[] { };
            string[] dsn = new string[3];
            if (dejitarudsnkey != null)
            {
               
                keys = dejitarudsnkey.GetSubKeyNames();
   
                if (keys != null && keys.Length > 0)
                {
                    dsnkey = Registry.LocalMachine.OpenSubKey(@"Software\\Dejitaruyubi\\ODBC Strings\\" + keys[0]);
                    if (dsnkey != null)
                    {

                        if (dsnkey.GetValue("DSN").ToString() != null & dsnkey.GetValue("LastUser").ToString() != null && dsnkey.GetValue("hControl") != null)
                        {

                            
                            DSN = dsnkey.GetValue("DSN").ToString();
                            UID = dsnkey.GetValue("LastUser").ToString();
                            PWD = Dejitaru.DecryptStringFromBytes_Aes((byte[])dsnkey.GetValue("hControl", RegistryValueKind.Binary), Dejitaru.key, Dejitaru.iv);
        
                            dsn[0] = DSN;
                            dsn[1] = UID;
                            dsn[2] = PWD;
                        }

                    }
                } 

            }
            else
            {
                return null;
            }

            return dsn;

        }
    }
}
