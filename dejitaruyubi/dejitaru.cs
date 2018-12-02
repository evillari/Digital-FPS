using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.Data;
using System.Data.Odbc;
using System.IO;
using DPFP;
using DPFP.Capture;
using System.Reflection;
using Microsoft.Win32;
using System.Security;
using System.Security.Cryptography;

namespace dejitaruyubi
{
    abstract class Dejitaru
    {   
        public static int progress;
        public static mainform mf = new mainform();
        
        public static string database, driver, lastuser, server, control;

        public static byte[] key = Encoding.ASCII.GetBytes("5227f20c24193f7d90d5d7bbe5c99ff2");
        public static byte[] iv = Encoding.ASCII.GetBytes("90d5d7bbe5c99ff2");

        public static string[] SystemInfo()
        {
            string[] property = new string[5];
            try
            {
                property[0] = Environment.MachineName;
                property[1] = Environment.OSVersion.ToString();
                property[2] = Environment.ProcessorCount.ToString();
                property[3] = Environment.Version.ToString();
                property[4] = Environment.Is64BitOperatingSystem.ToString();
                
                
            }
            catch(Exception e)
            {

            }
           
            return property;
        }    
        
        public static string[] getDSNLists()
        {
            string[] DSN = new string[] { };
            //RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\ODBC\\ODBC.INI\\ODBC Data Sources");

            RegistryKey key;
            if (Environment.Is64BitOperatingSystem)
            {
                key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\ODBC\\ODBC.INI\\ODBC Data Sources");
            }else
            {
                key = Registry.LocalMachine.OpenSubKey("Software\\ODBC\\ODBC.INI\\ODBC Data Sources");
            }
            
    
            try
                {
                    DSN = key.GetValueNames();
                }
            catch (SecurityException e)
                {

                }
            catch (ObjectDisposedException e)
                {

                }
            catch (UnauthorizedAccessException e)
                {

                }
            catch (IOException e)
                {

                }
            finally
                {
                    if (key != null)
                        {
                            key.Close();
                        }
                }

                  
          
            return DSN;
        }

        public static string[] getODBCValue(string dsn)
        {
            RegistryKey odbckey;
            string[] values = new string[4];
            if (Environment.Is64BitOperatingSystem)
            {
                odbckey = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\ODBC\\ODBC.INI\\" + dsn);
            }
            else
            {
                odbckey = Registry.LocalMachine.OpenSubKey("Software\\ODBC\\ODBC.INI\\" + dsn);
            }

            if (odbckey != null)
            {
                try
                {
                    values[0] = odbckey.GetValue("Database").ToString();
                    values[1] = odbckey.GetValue("Driver").ToString();
                    values[2] = odbckey.GetValue("LastUser").ToString();
                    values[3] = odbckey.GetValue("Server").ToString();
                    return values;
                }
                catch (SecurityException e)
                {

                }
                catch (ObjectDisposedException e)
                {

                }
                catch (IOException e)
                {

                }
                catch (UnauthorizedAccessException e)
                {

                }
                finally
                {
                    if (odbckey != null)
                    {
                        odbckey.Close();
                    }
                }

            }

            return null;

        }

        public static void CreateODBCkey(string dsn)
        {
            
            RegistryKey odbckey;
            
            database = "";
            driver = "";
            lastuser = "";
            server = "";
            if (Environment.Is64BitOperatingSystem)
            {
                odbckey = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\ODBC\\ODBC.INI\\" + dsn);
            }
            else
            {
                odbckey = Registry.LocalMachine.OpenSubKey("Software\\ODBC\\ODBC.INI\\" + dsn);
            }



            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"Software\\Dejitaruyubi\\", true); 

            if(rkey != null)
            {
                try
                {
                    rkey.DeleteSubKeyTree("ODBC Strings");
                }
                catch(ArgumentNullException e)
                {

                }catch(ArgumentException e)
                {

                }catch(IOException e)
                {

                }catch (SecurityException e)
                {

                }catch(ObjectDisposedException e)
                {

                }catch(UnauthorizedAccessException e)
                {

                }

                
            }

            RegistryKey dejitarudsnkey = Registry.LocalMachine.CreateSubKey(@"Software\\Dejitaruyubi\\ODBC Strings\\" + dsn);
           

            if(odbckey != null){
                try
                {
                    database = odbckey.GetValue("Database").ToString();
                    driver = odbckey.GetValue("Driver").ToString();
                    lastuser = odbckey.GetValue("LastUser").ToString();
                    server = odbckey.GetValue("Server").ToString();
                }
                catch (SecurityException e)
                {

                }
                catch (ObjectDisposedException e)
                {

                }
                catch (IOException e)
                {

                }
                catch (UnauthorizedAccessException e)
                {

                }
                finally
                {
                    if(odbckey != null)
                    {
                        odbckey.Close();
                    }
                }

            }
            
            try {
                dejitarudsnkey.SetValue("DSN", dsn);
                dejitarudsnkey.SetValue("Driver", driver);
                dejitarudsnkey.SetValue("LastUser", lastuser);
                dejitarudsnkey.SetValue("Server", server);
            }
            catch(ArgumentNullException e)
            {

            }
            catch(ArgumentException e)
            {

            }
            catch(ObjectDisposedException e)
            {

            }
            catch(UnauthorizedAccessException e)
            {

            }
            catch (SecurityException e)
            {

            }
            catch(IOException e)
            {

            }
            finally
            {
                if(dejitarudsnkey != null)
                {
                    dejitarudsnkey.Close();
                }
            }

        }


        public static void SControl(string hControl,string dsn)
        {
            RegistryKey dejitarudsnkey = Registry.LocalMachine.CreateSubKey("Software\\Dejitaruyubi\\ODBC Strings\\" + dsn);
            AesManaged maes = new AesManaged();
            try
            {
                dejitarudsnkey.SetValue("hControl", EncryptStringToBytes_Aes(hControl, key, iv), RegistryValueKind.Binary);
                //dejitarudsnkey.SetValue("hControl",EncryptStringToBytes_Aes(hControl,maes.Key,maes.IV), RegistryValueKind.Binary);
            }
            catch(ArgumentNullException e)
            {

            }
            catch(ArgumentException e)
            {


            }
            catch (ObjectDisposedException e)
            {

            }
            catch(UnauthorizedAccessException e)
            {

            }
            catch(SecurityException i)
            {

            }
            catch (IOException e)
            {

            }
            finally
            {
                if (dejitarudsnkey != null)
                {
                    dejitarudsnkey.Close();
                }
            }
                

        }

        public static string RControl(string dsn)
        {
            RegistryKey dejitarudsnkey = Registry.LocalMachine.CreateSubKey("Software\\Dejitaruyubi\\ODBC Strings\\" + dsn);
            string plaintext;
            plaintext = "";
            AesManaged maes = new AesManaged();
            
            try
            {
                byte[] gControl = (byte[])dejitarudsnkey.GetValue("hControl", RegistryValueKind.Binary);
                plaintext = DecryptStringFromBytes_Aes(gControl, key, iv);
                // plaintext = DecryptStringFromBytes_Aes(gControl, maes.Key, maes.IV);
                return plaintext;
            }
            catch (ObjectDisposedException e)
            {

            }
            catch (UnauthorizedAccessException e)
            {

            }
            catch (SecurityException i)
            {

            }
            catch (IOException e)
            {

            }
            finally
            {

            }

            return plaintext;
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {  
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
          
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        private delegate void SetControlPropertyDelegate(Form form, Control control, String property, object value);
        public static void SetControlProperty(Form form, Control control, String property, object value)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(new SetControlPropertyDelegate(SetControlProperty), new object[] { form, control, property, value });
            }
            else
            {
                control.GetType().InvokeMember(property, BindingFlags.SetProperty, null, control, new object[] { value });
            }
        }

        public static void SerializetoDB(byte[] fpsample, Label lblstatus, Form form)
        {
            OdbcConnection cn;
            OdbcCommand cmd;
            
            string insert = "INSERT INTO fpbasic (firstname,lastname,nationality,age,fingerprint) VALUES (?,?,?,?,?);";
            string dsnname = "fpbasic";
            string uid = "Administrator";
            string password = "2wsx3edc";
            cn = new OdbcConnection("dsn=" + dsnname + "; UID=" + uid + "; PWD=" + password + ";");
            cn.Open();
            cmd = new OdbcCommand(insert, cn);
           // cmd.Parameters.AddWithValue("@firstname", SqlDbType.NVarChar).Value = tbfname.Text;
           // cmd.Parameters.AddWithValue("@lasttname", SqlDbType.NVarChar).Value = tblname.Text;
           // cmd.Parameters.AddWithValue("@nationality", SqlDbType.NVarChar).Value = tbnationality.Text;
           // cmd.Parameters.AddWithValue("@age", SqlDbType.Int).Value = Int32.Parse(tbage.Text);
           // cmd.Parameters.AddWithValue("@fingerprint", SqlDbType.VarBinary).Value =fpsample;


            cmd.ExecuteNonQuery();

            
            SetControlProperty(form, mf.lstatus, "Text", "Enrollment successful. One row inserted.");
            
            cn.Close();
        }

        public static void ShowFP(DPFP.Sample sample, PictureBox pb, Form form)
        {
            DPFP.Capture.SampleConversion Converter = new DPFP.Capture.SampleConversion();
            Bitmap bmp = null;

            Converter.ConvertToPicture(sample, ref bmp);


            if (bmp != null)
            {
                int w = bmp.Width;
                int h = bmp.Height;
                Color p;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        p = bmp.GetPixel(x, y);
                        int a = p.A;
                        int g = p.G;


                        int px = 0;

                        if (g > 120)
                        {
                            bmp.SetPixel(x, y, Color.FromArgb(255, px, px, px));
                        }
                        else
                        {
                            //bmp.SetPixel(x, y, Color.FromArgb(255, px, 220, px));                           
                            bmp.SetPixel(x, y, Color.FromArgb(255, 0, 174, 219));
                            // bmp.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255));
                        }

                    }
                }
            }

            SetControlProperty(form, pb,"Image", bmp);
        }

        public static bool TestDB(string dsn, string uid, string password)
        {
            OdbcConnection cn;
            OdbcCommand cmd;

            string testString = "Select 1";
            if ((dsn != null && dsn.Length > 0) && (uid != null && uid.Length > 0) && (password != null && password.Length > 0))
            {
                //password = "2wsx3edc";
                cn = new OdbcConnection("dsn=" + dsn + "; UID=" + uid + "; PWD=" + password + ";");
                cmd = new OdbcCommand(testString, cn);
                try
                {
                    cn.Open();
                    return true;
                }
                catch(Exception e)
                {
                    
                    return false;
                }
            }
            else
            {
                return false;
            }

            
        }
    }
}
