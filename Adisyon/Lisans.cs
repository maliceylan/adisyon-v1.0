using Microsoft.Win32;
using System;
using System.Management;
using System.Windows.Forms;
namespace Adisyon
{
    public class Lisans
    {
        protected static string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        protected static string Parca1Karsilik = "WGHBJ1EQI4L3M86Y9VPZRSNT7XD5OAU2K0CF";
        protected static string Parca2Karsilik = "4HGY6BRNT53LPAX8SIV9WMECFZOUD071JKQ2";
        protected static string Parca3Karsilik = "FV4HQL567ZUXBJ29EWK1OMP3AS0RNDTGYCI8";
        protected static string Parca4Karsilik = "Q2NCPEFXJA75MD60H41GTBR9ULVIO8KS3ZWY";
        protected static string Parca5Karsilik = "04O37EXL6UVBQAS5JGTMFIRKNHY89W2PZ1CD";
        protected static string PIDHASH = "M42JOU6X0KFZN83H719SRVTDGCWYIQ5PABLEFV4HQL567ZUXBJ29EWK1OMP3AS0RNDTGYCI8";
        protected static string[] Karsiliklar = new string[6];


        protected static int LisansKarakterUzunlugu = 25;
        protected static char[] StringChars = new char[LisansKarakterUzunlugu];

        protected internal static string YeniKey()
        {
            Random rasRandom = new Random();
            string key = "";
            for (int i = 0; i < StringChars.Length; i++)
            {
                StringChars[i] = Chars[rasRandom.Next(Chars.Length)];
            }
            key = new string(StringChars);
            return key;
        }

        protected internal static void KeyDogrula(string key, string lisans)
        {
            string cozulenKey = KeyCoz(key);
            if (lisans == cozulenKey)
            {
                RegKayitYap(key, cozulenKey, PIDSakla(GetPID()));
            }
            else
            {
                MessageBox.Show("Lisans Kodu Yanlış!");
            }
        }

        protected internal static string KeyCoz(string key)
        {
            Karsiliklar[0] = Parca1Karsilik;
            Karsiliklar[1] = Parca2Karsilik;
            Karsiliklar[2] = Parca3Karsilik;
            Karsiliklar[3] = Parca4Karsilik;
            Karsiliklar[4] = Parca5Karsilik;
            Karsiliklar[5] = PIDHASH;
            string[] BolunenKey = key.Split('-');
            string cozulenkey = "";
            for (int i = 0; i < 5; i++)
            {
                cozulenkey += ParcaCoz(BolunenKey[i].ToCharArray(), i);
                cozulenkey += "-";
            }

            cozulenkey = cozulenkey.Remove(cozulenkey.LastIndexOf('-'), 1);
            return cozulenkey;

            // MessageBox.Show(cozulenkey.Remove(cozulenkey.LastIndexOf('-'),1));

        }

        protected internal static string PIDSakla(string PID)
        {
            string PIDCODED = "";
            PIDCODED += ParcaCoz(PID.ToCharArray(), 5);
            return PIDCODED;
        }

        private static string ParcaCoz(char[] gelenparca, int no)
        {
            string cozulen = "";
            for (int i = 0; i < gelenparca.Length; i++)
            {
                cozulen += Karsiliklar[no][Chars.IndexOf(gelenparca[i])];
            }
            return cozulen;
        }

        protected static void RegKayitYap(string key, string license, string processorID)
        {
            RegistryKey yol = Registry.CurrentUser.OpenSubKey(@"Software\Genesis\Adisyon\LICENSE", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (yol == null)
            {
                Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree).CreateSubKey("Genesis").CreateSubKey("Adisyon").CreateSubKey("LICENSE");
                yol = Registry.CurrentUser.OpenSubKey(@"Software\Genesis\Adisyon\LICENSE", RegistryKeyPermissionCheck.ReadWriteSubTree);
                yol.SetValue("KEY", key);
                yol.SetValue("LIC", license);
                yol.SetValue("PID", processorID);
                Registry.CurrentUser.Flush();
            }

            else if (yol.GetValue("KEY") == null || yol.GetValue("LIC") == null || yol.GetValue("PID") == null)
            {
                yol.SetValue("KEY", key);
                yol.SetValue("LIC", license);
                yol.SetValue("PID", processorID);
                Registry.CurrentUser.Flush();
            }
            else
            {
                yol.DeleteValue("KEY");
                yol.DeleteValue("LIC");
                yol.DeleteValue("PID");
                yol.SetValue("KEY", key);
                yol.SetValue("LIC", license);
                yol.SetValue("PID", processorID);
                Registry.CurrentUser.Flush();
            }
        }

        protected internal static bool LICKontrol()
        {
            bool isLicensed = false;
            //MessageBox.Show(GetPID()+"\n"+PIDSakla(GetPID()));
            RegistryKey yol = Registry.CurrentUser.OpenSubKey(@"Software\Genesis\Adisyon\LICENSE", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (yol == null || yol.GetValue("KEY") == null || yol.GetValue("LIC") == null || yol.GetValue("PID") == null)
            {
                isLicensed = false;
            }
            else if (yol.GetValue("LIC").ToString() != KeyCoz(yol.GetValue("KEY").ToString())) //LIC EŞİTMİ ÇÖZÜLEN VE PID HASH DOGRU MU 
            {

                isLicensed = false;

            }
            else if (yol.GetValue("PID").ToString() != PIDSakla(GetPID()))
            {
                isLicensed = false;
            }
            else
            {
                isLicensed = true;
            }
            return isLicensed;
        }

        protected internal static string GetPID()
        {
            string PID = string.Empty;
            ManagementClass managementClass = new ManagementClass("win32_processor");
            ManagementObjectCollection mocManagementObjectCollection = managementClass.GetInstances();
            foreach (ManagementObject mo in mocManagementObjectCollection)
            {
                if (PID == "")
                {
                    PID = mo.Properties["ProcessorID"].Value.ToString();
                }
            }
            return PID;
        }
    }


}
