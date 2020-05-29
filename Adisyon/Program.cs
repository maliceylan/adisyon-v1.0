using DevExpress.UserSkins;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Adisyon
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BonusSkins.Register();
            bool isLicensed;
            isLicensed = Lisans.LICKontrol();
            if (isLicensed)
            {
                Application.Run(new frmAdisyon());
            }
            else
            {
                //LİSANS SAYFASI AÇILIR...
                Application.Run(new LisansEkrani());
            }
        }
    }
}
