using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Adisyon
{
    public partial class frmAdisyon : DevExpress.XtraEditors.XtraForm
    {

        int secilenMasaID = 1;
        string secilenGrup = "";
        int secilenMasaGrup = 0;
        public static string dbkonumu = @"D:\Adisyon.mdf";
        SimpleButton[] masaGruplar = new SimpleButton[masaGrupSayisi()];
        SimpleButton[] masalar = new SimpleButton[masaSayisi()];
        LabelControl[] tutarlar = new LabelControl[masaSayisi()];
        LabelControl[] sureler = new LabelControl[masaSayisi()];
        Panel[] paneller = new Panel[masaSayisi()];
        String[] sureYazilar = new String[masaSayisi()];
        public frmAdisyon()
        {
            dbOlustur();
            InitializeComponent();
        }

        public static void dbOlustur()
        {
            //if (!File.Exists(dbkonumu))
            //{
            //    try
            //    {
            //        //Database.
            //        string sqlConnectionString = @"Data Source=(localdb)\v11.0;Integrated Security=True";
            //        string path = Application.StartupPath + "\\script.sql";
            //        FileInfo file = new FileInfo(path);
            //        string script = file.OpenText().ReadToEnd();
            //        SqlConnection conn = new SqlConnection(sqlConnectionString);
            //        Server server = new Server(new ServerConnection(conn));
            //        server.ConnectionContext.ExecuteNonQuery(script);
                    
            //        //Masalar.
            //        for (int i = 0; i < 100; i++)
            //        {
            //            string adi = "MASA ";
            //            bool aktif = false;
            //            if (i < 9) adi += "0";
            //            if (i < 20) aktif = true;
            //            adi += (i + 1).ToString();
            //            SqlCommand komut2 = new SqlCommand();
            //            komut2.Connection = conn;
            //            komut2.CommandText = "INSERT Adisyon.dbo.Masalar SELECT @sirano,@adi,0,NULL,@aktif,0;";
            //            komut2.Parameters.AddWithValue("@sirano", i+1);
            //            komut2.Parameters.AddWithValue("@adi",adi);
            //            komut2.Parameters.AddWithValue("@aktif", aktif);
            //            komut2.ExecuteNonQuery();
            //        }

            //        //Masa Grupları.
            //        for (int i = 0; i < 11; i++)
            //        {
            //            string adi = "";
            //            if (i == 0) adi = "-";
            //            else if (i < 4) adi = "Grup " + i.ToString();
            //            SqlCommand komut2 = new SqlCommand();
            //            komut2.Connection = conn;
            //            komut2.CommandText = "INSERT Adisyon.dbo.MasaGruplar SELECT @id,@adi;";
            //            komut2.Parameters.AddWithValue("@id", i);
            //            komut2.Parameters.AddWithValue("@adi", adi);
            //            komut2.ExecuteNonQuery();
            //        }
            //        MessageBox.Show("Database oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    catch(Exception ex)
            //    {
            //        XtraMessageBox.Show("Database oluşturulamadı!\n"+ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        public static int masaSayisi()
        {
            dbOlustur();
            int sayi = 0;
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select COUNT(*) from Masalar";
                baglanti.Open();
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    sayi = dr.GetInt32(0);
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sayi;
        }

        public static int masaGrupSayisi()
        {
            dbOlustur();
            int sayi = 0;
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select COUNT(*) from MasaGruplar";
                baglanti.Open();
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    sayi = dr.GetInt32(0);
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sayi;
        }

        public void masalarTemizle()
        {
            for (int i = 0; i < paneller.Length; i++)
            {
                paneller[i].Visible = false;
            }
        }

        public void masaCek(int grup)
        {
            try
            {
                string grupSorgu = "";
                if (grup != 0) grupSorgu = " and GrupNo=@grupno";
                SqlConnection baglanti = new SqlConnection(Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select SiraNo,Adi,ToplamTutar,ISNULL(BaslangicSaat,0) from Masalar where Aktif=1" + grupSorgu;
                if (!grupSorgu.Equals("")) komut.Parameters.AddWithValue("@grupno", grup);
                baglanti.Open();
                komut.ExecuteNonQuery();
                SqlDataReader dr = komut.ExecuteReader();
                int siraNo;
                int masaBoyut = 120;
                flpnlMasalar.Controls.Clear();
                while (dr.Read())
                {
                    siraNo = dr.GetInt32(0) - 1;
                    sureYazilar[siraNo] = dr.GetDateTime(3).ToLongTimeString();

                    //lblMasaSure
                    LabelControl lblMasaSure = new LabelControl();
                    lblMasaSure.Appearance.BackColor = SystemColors.ControlDarkDark;
                    lblMasaSure.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
                    lblMasaSure.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    lblMasaSure.AutoSizeMode = LabelAutoSizeMode.None;
                    lblMasaSure.Dock = DockStyle.Bottom;
                    lblMasaSure.Name = "lblMasaSure" + (siraNo + 1).ToString();
                    lblMasaSure.Size = new Size(masaBoyut, 22);
                    lblMasaSure.Text = sureYazilar[siraNo];

                    //lblMasaTutar
                    LabelControl lblMasaTutar = new LabelControl();
                    lblMasaTutar.Appearance.BackColor = SystemColors.ControlDarkDark;
                    lblMasaTutar.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
                    lblMasaTutar.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    lblMasaTutar.AutoSizeMode = LabelAutoSizeMode.None;
                    lblMasaTutar.Dock = DockStyle.Top;
                    lblMasaTutar.Name = "lblMasaTutar" + (siraNo + 1).ToString();
                    lblMasaTutar.Size = new Size(masaBoyut, 22);
                    lblMasaTutar.Text = dr[2].ToString() + " TL";

                    //btnMasa
                    SimpleButton btnMasa = new SimpleButton();
                    btnMasa.Appearance.BackColor = SystemColors.ControlDarkDark;
                    btnMasa.Appearance.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                    btnMasa.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    btnMasa.Dock = DockStyle.Fill;
                    btnMasa.Name = "btnMasa" + (siraNo + 1).ToString();
                    btnMasa.Text = dr[1].ToString();
                    btnMasa.Click += new EventHandler(this.btnMasa_Click);

                    //pnlMasa
                    Panel pnlMasa = new Panel();
                    pnlMasa.Margin = new Padding(5);
                    pnlMasa.Name = "pnlMasa" + (siraNo + 1).ToString();
                    pnlMasa.Size = new Size(masaBoyut, masaBoyut);

                    //Panel içi doldur.
                    pnlMasa.Controls.Add(btnMasa);
                    pnlMasa.Controls.Add(lblMasaSure);
                    pnlMasa.Controls.Add(lblMasaTutar);

                    //Flow içine at.
                    flpnlMasalar.Controls.Add(pnlMasa);

                    masalar[siraNo] = btnMasa;
                    tutarlar[siraNo] = lblMasaTutar;
                    sureler[siraNo] = lblMasaSure;
                    paneller[siraNo] = pnlMasa;
                }
                baglanti.Close();
                aktifMasaSayilari();
                masaAyarla();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void masaGruplarOlustur()
        {
            pnlMasaGruplar.Controls.Clear();
            SimpleButton btnTum = new SimpleButton();
            btnTum.Appearance.BackColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(115)))), ((int)(((byte)(196)))));
            btnTum.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
            btnTum.Appearance.Options.UseBackColor = true;
            btnTum.Appearance.Options.UseFont = true;
            btnTum.Name = "btnTumMasalar";
            btnTum.Size = new Size(135, 65);
            btnTum.Text = "Tüm Masalar";
            btnTum.Click += new EventHandler(this.btnTumMasalar_Click);
            pnlMasaGruplar.Controls.Add(btnTum);

            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select * from MasaGruplar where Adi!='' and ID!=0";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                int sayi = 0;
                while (dr.Read())
                {
                    SimpleButton btnMasaGrup = new SimpleButton();
                    btnMasaGrup.Appearance.Font = new Font("Tahoma", 14.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(162)));
                    btnMasaGrup.Appearance.BackColor = Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
                    btnMasaGrup.Appearance.Options.UseBackColor = true;
                    btnMasaGrup.Appearance.Options.UseFont = true;
                    btnMasaGrup.Name = "btnMasaGrup" + dr[0].ToString();
                    btnMasaGrup.Size = new Size(135, 65);
                    btnMasaGrup.Text = dr[1].ToString() + "\n";
                    btnMasaGrup.Click += new EventHandler(this.btnMasaGrup_Click);
                    pnlMasaGruplar.Controls.Add(btnMasaGrup);
                    masaGruplar[sayi] = btnMasaGrup;
                    sayi++;
                }
                baglanti.Close();
                aktifMasaSayilari();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTumMasalar_Click(Object sender, EventArgs e)
        {
            secilenMasaGrup = 0;
            SimpleButton btn = (sender as SimpleButton);
            for (int i = 0; i < pnlMasaGruplar.Controls.Count - 1; i++)
            {
                masaGruplar[i].Appearance.BackColor = Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            }
            masaCek(secilenMasaGrup);
        }

        private void btnMasaGrup_Click(Object sender, EventArgs e)
        {
            SimpleButton btn = (sender as SimpleButton);
            if (secilenMasaGrup != 0)
            {
                for (int i = 0; i < pnlMasaGruplar.Controls.Count - 1; i++)
                {
                    masaGruplar[i].Appearance.BackColor = Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
                }
            }
            btn.Appearance.BackColor = Color.DarkOrange;
            if (secilenMasaGrup != Convert.ToInt32(btn.Name.Replace("btnMasaGrup", "")))
            {
                secilenMasaGrup = Convert.ToInt32(btn.Name.Replace("btnMasaGrup", ""));
                masaCek(secilenMasaGrup);
            }
        }

        public void aktifMasaSayilari()
        {
            int toplamGrupSayisi = pnlMasaGruplar.Controls.Count - 1;
            int toplamMasa = 0;
            int toplamAcikMasa = 0;

            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;

                baglanti.Open();

                for (int i = 0; i < toplamGrupSayisi; i++)
                {
                    int grupNo = Convert.ToInt32(pnlMasaGruplar.Controls[i + 1].Name.Replace("btnMasaGrup", ""));
                    komut.CommandText = "select GrupNo,COUNT(GrupNo) from Masalar where Aktif=1 and ISNULL(BaslangicSaat,0)!=0 and GrupNo=" + grupNo.ToString() + " group by GrupNo;";
                    SqlDataReader dr = komut.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr.GetInt32(0) == grupNo)
                        {
                            pnlMasaGruplar.Controls[i + 1].Text = pnlMasaGruplar.Controls[i + 1].Text.Split('\n')[0];
                            pnlMasaGruplar.Controls[i + 1].Text += "\n" + dr[1].ToString();
                            toplamAcikMasa += dr.GetInt32(1);
                        }
                    }
                    else
                    {
                        pnlMasaGruplar.Controls[i + 1].Text = pnlMasaGruplar.Controls[i + 1].Text.Split('\n')[0];
                        pnlMasaGruplar.Controls[i + 1].Text += "\n0";
                    }
                    dr.Close();
                }

                for (int i = 0; i < toplamGrupSayisi; i++)
                {
                    int grupNo = Convert.ToInt32(pnlMasaGruplar.Controls[i + 1].Name.Replace("btnMasaGrup", ""));
                    komut.CommandText = "select GrupNo,COUNT(GrupNo) from Masalar where Aktif=1 and GrupNo=" + grupNo.ToString() + " group by GrupNo;";
                    SqlDataReader dr = komut.ExecuteReader();

                    if (dr.Read())
                    {
                        if (dr.GetInt32(0) == grupNo)
                        {
                            pnlMasaGruplar.Controls[i + 1].Text += "/" + dr[1].ToString();
                            toplamMasa += dr.GetInt32(1);
                        }
                    }
                    else
                    {
                        pnlMasaGruplar.Controls[i + 1].Text += "/0";
                    }
                    dr.Close();
                }

                pnlMasaGruplar.Controls[0].Text = "Tüm Masalar\n" + toplamAcikMasa.ToString() + "/" + toplamMasa.ToString();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void masaOlustur()
        {
            int masaBoyut = 120;

            for (int i = 0; i < masalar.Length; i++)
            {
                //lblMasaSure
                LabelControl lblMasaSure = new LabelControl();
                lblMasaSure.Appearance.BackColor = SystemColors.ControlDarkDark;
                lblMasaSure.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
                lblMasaSure.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                lblMasaSure.AutoSizeMode = LabelAutoSizeMode.None;
                lblMasaSure.Dock = DockStyle.Bottom;
                lblMasaSure.Name = "lblMasaSure" + (i + 1).ToString();
                lblMasaSure.Size = new Size(masaBoyut, 22);
                lblMasaSure.Text = "00:00:00";

                //lblMasaTutar
                LabelControl lblMasaTutar = new LabelControl();
                lblMasaTutar.Appearance.BackColor = SystemColors.ControlDarkDark;
                lblMasaTutar.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Bold);
                lblMasaTutar.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                lblMasaTutar.AutoSizeMode = LabelAutoSizeMode.None;
                lblMasaTutar.Dock = DockStyle.Top;
                lblMasaTutar.Name = "lblMasaTutar" + (i + 1).ToString();
                lblMasaTutar.Size = new Size(masaBoyut, 22);
                lblMasaTutar.Text = "0,00 TL";

                //btnMasa
                SimpleButton btnMasa = new SimpleButton();
                btnMasa.Appearance.BackColor = SystemColors.ControlDarkDark;
                btnMasa.Appearance.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                btnMasa.Dock = DockStyle.Fill;
                btnMasa.Name = "btnMasa" + (i + 1).ToString();
                btnMasa.Click += new EventHandler(this.btnMasa_Click);

                //pnlMasa
                Panel pnlMasa = new Panel();
                pnlMasa.Margin = new Padding(5);
                pnlMasa.Name = "pnlMasa" + (i + 1).ToString();
                pnlMasa.Size = new Size(masaBoyut, masaBoyut);

                //Panel içi doldur.
                pnlMasa.Controls.Add(btnMasa);
                pnlMasa.Controls.Add(lblMasaSure);
                pnlMasa.Controls.Add(lblMasaTutar);

                //Flow içine at.
                flpnlMasalar.Controls.Add(pnlMasa);

                masalar[i] = btnMasa;
                tutarlar[i] = lblMasaTutar;
                sureler[i] = lblMasaSure;
                paneller[i] = pnlMasa;
            }
        }

        private void btnMasa_Click(Object sender, EventArgs e)
        {
            SimpleButton btn = (sender as SimpleButton);
            string masaAdi = btn.Text;
            int masaId = Convert.ToInt32(btn.Name.Replace("btnMasa", ""));
            if (lblMasaTasi.Visible == true)//masa taşı modu
            {
                masaTasi(masaId, masaAdi);
            }
            else
            {
                masaBaslat(masaId, masaAdi);
            }
        }

        public void masaBaslat(int masaId, string masaAdi)
        {
            secilenMasaID = masaId;
            gcSiparis.Text = "Sipariş Paneli: " + masaAdi;
            if (masalar[secilenMasaID - 1].Appearance.BackColor == Color.DimGray)
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Masalar set BaslangicSaat=@saat where SiraNo=@sirano";
                    komut.Parameters.AddWithValue("@saat", DateTime.Now.AddSeconds(-1));
                    komut.Parameters.AddWithValue("@sirano", masaId);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                masaCek(secilenMasaGrup);
            }
            navigationFrame.SelectedPage = sayfaSiparisler;
            tileBar.SelectedItem = siparisTileBarItem;
            siparisTabloDoldur();
        }

        public void masaTasi(int masaId, string masaAdi)
        {
            try
            {
                double toplamtutar = 0;
                DateTime baslangic = DateTime.Now;
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "update Siparis set MasaID=@yeniid where MasaID=@eskiid";
                komut.Parameters.AddWithValue("@yeniid", masaId);
                komut.Parameters.AddWithValue("@eskiid", secilenMasaID);
                baglanti.Open();
                komut.ExecuteNonQuery();

                komut.CommandText = "select ToplamTutar,BaslangicSaat from Masalar where SiraNo=@eskiid";
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    toplamtutar = dr.GetDouble(0);
                    baslangic = dr.GetDateTime(1);
                }
                dr.Close();
                komut.CommandText = "update Masalar set ToplamTutar=0,BaslangicSaat=NULL where SiraNo=@eskiid";
                komut.ExecuteNonQuery();

                komut.CommandText = "update Masalar set ToplamTutar=@tutar,BaslangicSaat=@saat where SiraNo=@yeniid";
                komut.Parameters.AddWithValue("@tutar", toplamtutar);
                komut.Parameters.AddWithValue("@saat", baslangic);
                komut.ExecuteNonQuery();
                baglanti.Close();

                //Düzeni eski hale getir.
                int ID;
                for (int i = 0; i < flpnlMasalar.Controls.Count; i++)
                {
                    ID = Convert.ToInt32(flpnlMasalar.Controls[i].Controls[0].Name.Replace("btnMasa", "")) - 1;
                    if (masalar[ID].Appearance.BackColor == Color.DarkOrange)
                    {
                        paneller[ID].Enabled = true;
                    }
                }
                paneller[secilenMasaID - 1].Enabled = true;
                tileBar.Enabled = true;
                pnlMasaGruplar.Enabled = true;
                lblMasaTasi.Visible = false;
                tmrMasaTasi.Stop();
                secilenMasaID = masaId;
                gcSiparis.Text = "Sipariş Paneli: " + masaAdi;
                masaCek(secilenMasaGrup);
                tileBar.SelectedItem = siparisTileBarItem;
                navigationFrame.SelectedPage = sayfaSiparisler;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void masaAyarla()
        {
            //for (int i = 0; i < masalar.Length; i++)
            //{
            //    if (masalar[i].Visible == true)
            //    {
            //        TimeSpan zaman;
            //        if (sureYazilar[i] != "00:00:00")
            //        {
            //            masalar[i].Appearance.BackColor = Color.DarkOrange;
            //            tutarlar[i].Appearance.BackColor = Color.DarkOrange;
            //            sureler[i].Appearance.BackColor = Color.DarkOrange;
            //            zaman = (DateTime.Now - Convert.ToDateTime(sureYazilar[i]));
            //            if (zaman.ToString().Substring(0, 1) == "-") zaman = (DateTime.Now.AddDays(1) - Convert.ToDateTime(sureYazilar[i]));
            //            sureYazilar[i] = zaman.ToString().Substring(0, 8);
            //            sureler[i].Text = sureYazilar[i];
            //        }
            //        else
            //        {
            //            masalar[i].Appearance.BackColor = Color.DimGray;
            //            tutarlar[i].Appearance.BackColor = Color.DimGray;
            //            sureler[i].Appearance.BackColor = Color.DimGray;
            //        }
            //    }
            //}

            int ID;
            TimeSpan zaman;
            for (int i = 0; i < flpnlMasalar.Controls.Count; i++)
            {
                ID = Convert.ToInt32(flpnlMasalar.Controls[i].Controls[0].Name.Replace("btnMasa", "")) - 1;

                if (sureYazilar[ID] != "00:00:00")
                {
                    masalar[ID].Appearance.BackColor = Color.DarkOrange;
                    tutarlar[ID].Appearance.BackColor = Color.DarkOrange;
                    sureler[ID].Appearance.BackColor = Color.DarkOrange;
                    zaman = (DateTime.Now - Convert.ToDateTime(sureYazilar[ID]));
                    if (zaman.ToString().Substring(0, 1) == "-") zaman = (DateTime.Now.AddDays(1) - Convert.ToDateTime(sureYazilar[ID]));
                    sureYazilar[ID] = zaman.ToString().Substring(0, 8);
                    sureler[ID].Text = sureYazilar[ID];
                }
                else
                {
                    masalar[ID].Appearance.BackColor = Color.DimGray;
                    tutarlar[ID].Appearance.BackColor = Color.DimGray;
                    sureler[ID].Appearance.BackColor = Color.DimGray;
                }
            }
        }

        public void masaZamanArtır()
        {
            int ID;
            for (int i = 0; i < flpnlMasalar.Controls.Count; i++)
            {
                ID = Convert.ToInt32(flpnlMasalar.Controls[i].Controls[0].Name.Replace("btnMasa", "")) - 1;

                if (sureYazilar[ID] != "00:00:00")
                {
                    sureYazilar[ID] = Convert.ToDateTime(sureYazilar[ID]).AddSeconds(1).ToLongTimeString();
                    sureler[ID].Text = sureYazilar[ID];
                    lblSiparisMasaSure.Text = "Aktif Süre: " + sureYazilar[secilenMasaID - 1];
                }
            }
        }

        public void menuTabloCek()
        {
            menuGrupDoldur();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if (rbUrun.Checked) komut.CommandText = "select Menu.SiraNo[No],Menu.Adi[Ürün Adı],Gruplar.Adi[Grubu],Menu.Fiyat from Menu left join Gruplar on Menu.Grubu=Gruplar.ID order by Menu.SiraNo";
                else if (rbGrup.Checked) komut.CommandText = "select G.ID[No],G.Adi[Grup Adı],B.Adi[Mutfak/Bar] from Gruplar as G left join Barlar as B on G.BarNo=B.ID order by G.ID";
                else komut.CommandText = "select ID[No],Adi[Mutfak/Bar Adı] from Barlar order by ID";
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblMenu.DataSource = dt;
                baglanti.Close();
                tblMenu.AutoResizeColumns();
                tblMenu.Columns[0].Width = 30;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void masaGrupDoldur()
        {
            cmbMasaGrubu.Properties.Items.Clear();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select Adi from MasaGruplar where Adi!=''";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    cmbMasaGrubu.Properties.Items.Add(dr[0].ToString());
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void masaTabloCek()
        {
            masaGrupDoldur();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if (rbMasa.Checked) komut.CommandText = "select Masalar.SiraNo[Masa No],MasaGruplar.Adi[Grubu],Masalar.Adi[Masa Adı],Masalar.Aktif from Masalar inner join MasaGruplar on Masalar.GrupNo=MasaGruplar.ID";
                else komut.CommandText = "select ID[No],Adi[Grup Adı] from MasaGruplar where ID!=0";
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblMasalar.DataSource = dt;
                baglanti.Close();
                tblMasalar.AutoResizeColumns();
                tblMasalar.Columns[0].Width = 60;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            pnlMasaBoyut.Visible = false;
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
            if (navigationFrame.SelectedPage == sayfaSiparisler)
            {
                btnGeri.PerformClick();
                menuGrupOlustur();
            }
            else if (navigationFrame.SelectedPage == sayfaMasalar)
            {
                masaCek(secilenMasaGrup);
                pnlMasaBoyut.Visible = true;
            }
            else if (navigationFrame.SelectedPage == sayfaKasa)
            {
                rbGunluk.Checked = false;
                dtGunluk.EditValue = DateTime.Today;
                btnFisIcerikGeriDon.PerformClick();
                rbGunluk.Checked = true;
                txtKasaAramaMetin.Text = null;

                btnKasaNumaraUret.PerformClick();
                cmbKasaIslemTur.SelectedIndex = 0;
            }
        }

        private void employeesLabelControl_Click(object sender, EventArgs e)
        {
            tileBar.SelectedItem = siparisTileBarItem;
            navigationFrame.SelectedPageIndex = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            masaGruplarOlustur();
            //masaOlustur();
            masaCek(0);
            tmrMasa.Start();
            menuGrupOlustur();
            tmrOtoGuncelle.Start();
            ztbMasalar.Value = Properties.Settings.Default.masaBoyut;
        }



        private void btnIskonto_Click(object sender, EventArgs e)
        {
            gcSiparis.Enabled = false;
            gcUrunGruplari.Enabled = false;
            tileBar.Enabled = false;
            panelAc(ApnlIskonto,this);
            txtPNLIskontoToplamTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(lblTutar.Text.Replace(" TL","").Replace("TUTAR: ","")));
            txtPNLIskontoToplamTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoToplamTutar.Text));
            txtPNLIskontoToplamAlis.Text = "0,00";
            txtPNLIskontoToplamAlis.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoToplamAlis.Text));
            txtPNLIskontoMiktar.Text = "20";
            rbPNLIskontoPara.Checked = true;
            cmbPNLIskontoOdemeSekli.SelectedIndex = 0;
            iskontoHesap();
        }

        private void btnPNLIskontoIptal_Click(object sender, EventArgs e)
        {
            ApnlIskonto.Visible = false;
            gcSiparis.Enabled = true;
            gcUrunGruplari.Enabled = true;
            tileBar.Enabled = true;
        }

        private void btnPNLIskontoOnayla_Click(object sender, EventArgs e)
        {
            string parayuzde = "";
            if (rbPNLIskontoYuzde.Checked == true) parayuzde = "%";
            else parayuzde = "TL";
            lblTutar.Text="TUTAR: "+txtPNLIskontoYeniTutar.Text+" TL";
            btnPNLIskontoIptal.PerformClick();
            hesapAl(cmbPNLIskontoOdemeSekli.SelectedItem.ToString(),"İskonto Uygulandı("+txtPNLIskontoMiktar.Text+parayuzde+")");
        }

        void iskontoHesap()
        {
            try
            {
                double yenitutar = 0;
                if (rbPNLIskontoPara.Checked == true) yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text) - Convert.ToDouble(txtPNLIskontoMiktar.Text);
                else yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text) - (Convert.ToDouble(txtPNLIskontoToplamTutar.Text) * Convert.ToDouble(txtPNLIskontoMiktar.Text)) / 100;
                txtPNLIskontoYeniTutar.Text = yenitutar.ToString();
                txtPNLIskontoYeniTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoYeniTutar.Text));
            }
            catch
            {
                double yenitutar = 0;
                if (rbPNLIskontoPara.Checked == true) yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text);
                else yenitutar = Convert.ToDouble(txtPNLIskontoToplamTutar.Text);
                txtPNLIskontoYeniTutar.Text = yenitutar.ToString();
                txtPNLIskontoYeniTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtPNLIskontoYeniTutar.Text));
            }
        }

        private void rbPNLIskontoYuzde_CheckedChanged(object sender, EventArgs e)
        {
            iskontoHesap();
        }

        private void txtPNLIskontoMiktar_TextChanged(object sender, EventArgs e)
        {
            iskontoHesap();
        }

        void panelAc(Panel panel,Control control)
        {
            
            panel.Location = new Point(
            this.ClientSize.Width / 2 - panel.Size.Width / 2,
            this.ClientSize.Height / 2 - panel.Size.Height / 2);
            panel.Anchor = AnchorStyles.None;
            control.Controls.Add(panel);
            panel.Visible = true;
            panel.BringToFront();
        }
        public void menuGrupOlustur()
        {
            pnlMenuGrup.Controls.Clear();
            //Tüm ürünler butonunu ekle
            SimpleButton btnTum = new SimpleButton();
            btnTum.Dock = DockStyle.Fill;
            btnTum.MinimumSize = new Size(136, 30);
            btnTum.Name = "btnGrup_TumUrunler";
            btnTum.AutoSize = true;
            btnTum.TabIndex = 9;
            btnTum.Text = "Tüm Ürünler";
            btnTum.Appearance.BackColor = Color.DarkOrange;
            btnTum.Font = new Font("Tahoma", 12, FontStyle.Bold);
            btnTum.Click += new EventHandler(this.btnTumUrunler_Click);
            pnlMenuGrup.Controls.Add(btnTum, 0, 0);
            pnlMenuGrup.SetColumnSpan(btnTum, 2);
            List<String> gruplar = new List<String>();
            int grupSayisi = 0;
            //Veri tabanından bilgileri listeye al
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select * from Gruplar where Adi!=''";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[1].ToString() != "")
                    {
                        gruplar.Add(dr[1].ToString());
                        grupSayisi++;
                    }
                }
                baglanti.Close();

            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (grupSayisi % 2 == 0) pnlMenuGrup.RowCount = grupSayisi / 2;
            else pnlMenuGrup.RowCount = ((grupSayisi / 2) + 1);

            for (int i = 0; i < grupSayisi; i++)
            {
                SimpleButton btn = new SimpleButton();
                btn.Dock = DockStyle.Fill;
                btn.MinimumSize = new Size(136, 30);
                btn.Name = "btnGrup_" + gruplar[i].ToString();
                btn.AutoSize = true;
                btn.TabIndex = 9;
                btn.Text = gruplar[i];
                btn.Font = new Font("Tahoma", 12, FontStyle.Bold);
                btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                btn.Click += new EventHandler(this.btnGrup_Click);
                pnlMenuGrup.Controls.Add(btn, i % 2, (i / 2) + 1);
            }
        }

        private void btnGrup_Click(Object sender, EventArgs e)
        {
            SimpleButton btn = (sender as SimpleButton);
            String[] btnadi = btn.Name.Split('_');
            string grupAdi = btnadi[1];
            pnlMenuGrup.SendToBack();
            btnGeri.Visible = true;
            secilenGrup = grupAdi;
            menuGrupAc(grupAdi);
        }



        private void btnTumUrunler_Click(Object sender, EventArgs e)
        {
            SimpleButton btn = (sender as SimpleButton);
            String[] btnadi = btn.Name.Split('_');
            string grupAdi = btnadi[1];
            pnlMenuGrup.SendToBack();
            btnGeri.Visible = true;
            menuGrupAc(grupAdi);
        }

        public void menuGrupAc(string grup)
        {
            ztbUrunler.Visible = true;
            pnlMenuUrunler.Controls.Clear();
            int urunSayisi = 0;
            List<String> urunler = new List<String>();
            //Veri tabanından bilgileri listeye al
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if (grup != "TumUrunler")
                {
                    gcUrunGruplari.Text = grup;
                    komut.CommandText = "select Menu.Adi,Menu.SiraNo,Gruplar.Adi from Menu inner join Gruplar on Menu.Grubu=Gruplar.ID where Gruplar.Adi=@grup and Menu.Adi!=''";
                    komut.Parameters.AddWithValue("@grup", grup);
                }
                else
                {
                    gcUrunGruplari.Text = "Tüm Ürünler";
                    komut.CommandText = "select Menu.Adi,Menu.SiraNo,Gruplar.Adi from Menu inner join Gruplar on Menu.Grubu=Gruplar.ID";
                }
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[0].ToString() != "")
                    {
                        urunler.Add(dr[0].ToString() + "?" + dr[1].ToString() + "?" + dr[2].ToString());
                        urunSayisi++;
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < urunSayisi; i++)
            {
                String[] adkod = urunler[i].Split('?');
                SimpleButton btn = new SimpleButton();
                if (i % 2 == 0) btn.Appearance.BackColor = System.Drawing.Color.DarkOrange;
                else
                {
                    btn.Appearance.BackColor = System.Drawing.Color.Bisque;
                }

                int boyut = 136;
                float font = 9.75F;
                if (ztbUrunler.Value == 1)
                {
                    boyut = 207;
                    font = 11.25F;
                }
                else if (ztbUrunler.Value == 2)
                {
                    boyut = 420;
                    font = 12.75F;
                }

                btn.Appearance.Font = new Font("Tahoma", font, FontStyle.Bold);//9.75-11.25-12.75
                btn.Appearance.ForeColor = Color.Black;
                btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                btn.Location = new System.Drawing.Point(3, 3);
                btn.Name = "btnMenuUrun_" + (adkod[1]).ToString();
                btn.Size = new System.Drawing.Size(boyut, 65);//136-207-420
                btn.TabIndex = 0;
                if (grup != "TumUrunler") btn.Text = adkod[0];
                else btn.Text = adkod[2] + "\n" + adkod[0];
                btn.Click += new EventHandler(this.btnUrun_Click);
                pnlMenuUrunler.Controls.Add(btn);
            }
            pnlMenuUrunler.BringToFront();
            pnlMenuUrunler.HorizontalScroll.Maximum = 0;
            pnlMenuUrunler.VerticalScroll.Maximum = 0;
            pnlMenuUrunler.VerticalScroll.Visible = false;
            pnlMenuUrunler.HorizontalScroll.Visible = false;
            pnlMenuUrunler.AutoScroll = true;
        }
        private void btnUrun_Click(Object sender, EventArgs e)
        {
            SimpleButton btn = (sender as SimpleButton);
            String[] kod = btn.Name.Split('_');

            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select Menu.SiraNo,Menu.Adi,Gruplar.Adi,Menu.Fiyat from Menu inner join Gruplar on Menu.Grubu=Gruplar.ID where Menu.SiraNo=@no";
                komut.Parameters.AddWithValue("@no", Convert.ToInt32(kod[1]));
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                int sirano = 1;
                if (dr.Read())
                {
                    if (tlSiparis.Nodes.Count > 0) sirano = Convert.ToInt32(tlSiparis.Nodes[tlSiparis.Nodes.Count - 1].GetValue("SiraNo")) + 1;
                    TreeListNode tnode = tlSiparis.AppendNode(null, null);
                    tnode.SetValue("SiraNo", sirano);
                    tnode.SetValue("Grubu", dr[2].ToString());
                    tnode.SetValue("UrunAdi", dr[1].ToString());
                    tnode.SetValue("Durum", "Yeni Siparis");
                    tnode.SetValue("Fiyat", dr[3].ToString() + " TL");
                }
                dr.Close();
                siraTutarHesap();

                komut.CommandText = "INSERT Siparis SELECT @masaid,@sirano,@kod,'Yeni Siparis','';";
                komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                komut.Parameters.AddWithValue("@sirano", sirano);
                komut.Parameters.AddWithValue("@kod", kod[1]);
                komut.ExecuteNonQuery();

                komut.CommandText = "update Masalar set ToplamTutar=@tutar where SiraNo=@masaid";
                komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim()));
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tlSiparis.FocusedNode = tlSiparis.Nodes.LastNode;
        }

        private void btnNotEkle_Click(object sender, EventArgs e)
        {
            if (tlSiparis.Nodes.Count > 0)
            {
                int urunID;
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "select UrunKodu from Siparis where MasaID=@masaid and SiraNo=@sirano";
                    komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                    komut.Parameters.AddWithValue("@sirano", tlSiparis.FocusedNode.GetValue("SiraNo"));
                    baglanti.Open();
                    SqlDataReader dr = komut.ExecuteReader();
                    if (dr.Read())
                    {
                        urunID = dr.GetInt32(0);
                    }
                    else
                    {
                        urunID = 0;
                    }
                    baglanti.Close();
                    notAc(urunID);
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void notAc(int urunID)
        {
            ztbUrunler.Visible = true;
            pnlMenuUrunler.Controls.Clear();
            btnGeri.Visible = true;
            int notSayisi = 0;
            List<String> notlar = new List<String>();
            //Veri tabanından bilgileri listeye al
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                gcUrunGruplari.Text = "Ürün Notları";
                komut.CommandText = "select ID,SiparisNotu from Notlar where UrunID=@urunid";
                komut.Parameters.AddWithValue("@urunid",urunID);
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    if (dr[1].ToString() != "")
                    {
                        notlar.Add(dr[0].ToString() + "?" + dr[1].ToString());
                        notSayisi++;
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            for (int i = 0; i < notSayisi; i++)
            {
                String[] adkod = notlar[i].Split('?');
                SimpleButton btn = new SimpleButton();
                if (i % 2 == 0) btn.Appearance.BackColor = System.Drawing.Color.LightCoral;
                else
                {
                    btn.Appearance.BackColor = System.Drawing.Color.IndianRed;
                }

                int boyut = 136;
                float font = 9.75F;
                if (ztbUrunler.Value == 1)
                {
                    boyut = 207;
                    font = 11.25F;
                }
                else if (ztbUrunler.Value == 2)
                {
                    boyut = 420;
                    font = 12.75F;
                }

                btn.Appearance.Font = new Font("Tahoma", font, FontStyle.Bold);//9.75-11.25-12.75
                btn.Appearance.ForeColor = Color.Black;
                btn.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                btn.Location = new System.Drawing.Point(3, 3);
                btn.Name = "btnMenuNot_" + (adkod[0]).ToString();
                btn.Size = new System.Drawing.Size(boyut, 65);//136-207-420
                btn.TabIndex = 0;
                btn.Text = adkod[1];
                btn.Click += new EventHandler(this.btnNot_Click);
                pnlMenuUrunler.Controls.Add(btn);
            }
            pnlMenuUrunler.BringToFront();
            pnlMenuUrunler.HorizontalScroll.Maximum = 0;
            pnlMenuUrunler.VerticalScroll.Maximum = 0;
            pnlMenuUrunler.VerticalScroll.Visible = false;
            pnlMenuUrunler.HorizontalScroll.Visible = false;
            pnlMenuUrunler.AutoScroll = true;

        }

        private void btnNot_Click(Object sender, EventArgs e)
        {
            if (tlSiparis.FocusedNode.ParentNode == null)
            {
                SimpleButton btn = (sender as SimpleButton);
                String[] kod = btn.Name.Split('_');

                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "select SiparisNotu,Fiyat from Notlar where ID=@id";
                    komut.Parameters.AddWithValue("@id", Convert.ToInt32(kod[1]));
                    baglanti.Open();
                    SqlDataReader dr = komut.ExecuteReader();
                    int sirano = 1;
                    if (dr.Read())
                    {
                        if (tlSiparis.FocusedNode.Nodes.Count > 0) sirano = Convert.ToInt32(tlSiparis.FocusedNode.Nodes[tlSiparis.FocusedNode.Nodes.Count - 1].GetValue("SiraNo")) + 1;
                        TreeListNode tnode = tlSiparis.AppendNode(null, tlSiparis.FocusedNode);
                        tnode.SetValue("SiraNo", sirano);
                        tnode.SetValue("Grubu", "Not:");
                        tnode.SetValue("UrunAdi", dr[0].ToString());
                        tnode.SetValue("Durum", "");
                        tnode.SetValue("Fiyat", dr[1].ToString() + " TL");
                    }
                    dr.Close();
                    siraTutarHesap();
                    tlSiparis.ExpandAll();
                    komut.CommandText = "INSERT SiparisNotlari SELECT @masaid,@sirano,@parentid,@id;";
                    komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                    komut.Parameters.AddWithValue("@sirano", sirano);
                    komut.Parameters.AddWithValue("@parentid", tlSiparis.FocusedNode.GetValue("SiraNo"));
                    komut.ExecuteNonQuery();

                    komut.CommandText = "update Masalar set ToplamTutar=@tutar where SiraNo=@masaid";
                    komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim()));
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void siraTutarHesap()
        {
            double tutar = 0;
            for (int i = 0; i < tlSiparis.Nodes.Count; i++)
            {
                tlSiparis.Nodes[i].SetValue("SiraNoGorunen", (i + 1));
                tutar += Convert.ToDouble(tlSiparis.Nodes[i].GetValue("Fiyat").ToString().Replace(" TL", ""));
                if (tlSiparis.Nodes[i].HasChildren)
                {
                    for (int j = 0; j < tlSiparis.Nodes[i].Nodes.Count; j++)
                    {
                        tlSiparis.Nodes[i].Nodes[j].SetValue("SiraNoGorunen", (j + 1));
                        tutar += Convert.ToDouble(tlSiparis.Nodes[i].Nodes[j].GetValue("Fiyat").ToString().Replace(" TL", ""));
                    }
                }
            }
            lblTutar.Text = string.Format("{0:#,##0.00}", tutar);
            lblTutar.Text = "TUTAR: " + lblTutar.Text + " TL";
            lblToplamSayi.Text = "T. Ürün Sayısı: " + tlSiparis.Nodes.Count.ToString();
        }

        public void siparisTabloDoldur()
        {
            tlSiparis.Nodes.Clear();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select Siparis.SiraNo,G.Adi,M.Adi,M.Fiyat,Siparis.Durum, Siparis.SiparisNotu from Siparis inner join Menu AS M on Siparis.UrunKodu= M.SiraNo inner join Gruplar AS G on M.Grubu=G.ID where Siparis.MasaID=@masaid order by Siparis.SiraNo;";
                komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    TreeListNode tnode = tlSiparis.AppendNode(null, null);
                    tnode.SetValue("SiraNo", dr[0].ToString());
                    tnode.SetValue("Grubu", dr[1].ToString());
                    tnode.SetValue("UrunAdi", dr[2].ToString());
                    tnode.SetValue("Durum", dr[4].ToString());
                    tnode.SetValue("Fiyat", dr[3].ToString() + " TL");
                }
                dr.Close();
                komut.CommandText = "select S.MasaID,S.SiraNo,S.ParentID,N.SiparisNotu,N.Fiyat from SiparisNotlari as S join Notlar as N on S.NotID=N.ID where S.MasaID=@masaid order by S.Sirano;";
                SqlDataReader dr2 = komut.ExecuteReader();
                TreeListNode parentNode = new TreeListNode();
                while (dr2.Read())
                {
                    for (int i = 0; i < tlSiparis.Nodes.Count; i++)
                    {
                        if (tlSiparis.Nodes[i].GetValue("SiraNo").ToString() == dr2[2].ToString())
                        {
                            parentNode = tlSiparis.Nodes[i];
                            TreeListNode tnode = tlSiparis.AppendNode(null, parentNode);
                            tnode.SetValue("SiraNo", dr2[1].ToString());
                            tnode.SetValue("Grubu", "Not:");
                            tnode.SetValue("UrunAdi", dr2[3].ToString());
                            tnode.SetValue("Durum", "");
                            tnode.SetValue("Fiyat", dr2[4].ToString() + " TL");
                        }
                    }  
                }
                tlSiparis.ExpandAll();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            siraTutarHesap();
            tlSiparis.FocusedNode = tlSiparis.Nodes.LastNode;
        }

        private void btnUrunSil_Click(object sender, EventArgs e)
        {
            siparisSilVt();
            tlSiparis.DeleteSelectedNodes();
            siraTutarHesap();
            tutarGuncelleVt();
        }

        public void siparisSilVt()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                baglanti.Open();
                for (int i = 0; i < tlSiparis.Nodes.Count; i++)
                {
                    if (tlSiparis.Nodes[i].Selected)
                    {
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "delete Siparis where MasaID=@masaid and SiraNo=@sirano";
                        komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                        komut.Parameters.AddWithValue("@sirano", tlSiparis.Nodes[i].GetValue("SiraNo"));
                        komut.ExecuteNonQuery();

                        komut.CommandText = "delete SiparisNotlari where MasaID=@masaid and ParentID=@parentid";
                        komut.Parameters.AddWithValue("@parentid", tlSiparis.Nodes[i].GetValue("SiraNo"));
                        komut.ExecuteNonQuery();
                    }
                    else if (tlSiparis.Nodes[i].HasChildren)
                    {
                        for (int j = 0; j < tlSiparis.Nodes[i].Nodes.Count; j++)
                        {
                            if (tlSiparis.Nodes[i].Nodes[j].Selected)
                            {
                                SqlCommand komut = new SqlCommand();
                                komut.Connection = baglanti;
                                komut.CommandText = "delete SiparisNotlari where MasaID=@masaid and SiraNo=@sirano and ParentID=@parentid";
                                komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                                komut.Parameters.AddWithValue("@sirano", tlSiparis.Nodes[i].Nodes[j].GetValue("SiraNo"));
                                komut.Parameters.AddWithValue("@parentid",tlSiparis.Nodes[i].GetValue("SiraNo"));
                                komut.ExecuteNonQuery();
                            }
                        }
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void tutarGuncelleVt()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                baglanti.Open();
                komut.CommandText = "update Masalar set ToplamTutar=@tutar where SiraNo=@masaid";
                komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim()));
                komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void durumGuncelle()
        {
            List<string> siparisler = new List<string>();
            int siparisSayisi = 0;
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                baglanti.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = baglanti;
                cmd.CommandText = "SELECT S.UrunKodu, COUNT(S.UrunKodu)[Miktar], G.BarNo FROM Siparis AS S JOIN Menu AS M ON S.UrunKodu=M.SiraNo JOIN Gruplar AS G ON M.Grubu=G.ID WHERE Durum='Yeni Siparis' and MasaID=@masaid GROUP BY S.UrunKodu,G.BarNo,S.MasaID;";
                cmd.Parameters.AddWithValue("@masaid",secilenMasaID);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    siparisler.Add(dr[0].ToString()+"-"+dr[1].ToString() + "-"+ dr[2].ToString());
                }
                dr.Close();

                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = baglanti;
                cmd2.CommandText = "select COUNT(*) from YeniSiparisler";
                SqlDataReader dr2 = cmd2.ExecuteReader();
                if (dr2.Read()) siparisSayisi = dr2.GetInt32(0);
                dr2.Close();

                for (int i = 0; i < siparisler.Count; i++)
                {
                    SqlCommand cmd3 = new SqlCommand();
                    cmd3.Connection = baglanti;
                    cmd3.CommandText = "INSERT YeniSiparisler SELECT @sirano,@masaid,@urunkodu,@miktar,@durum,@saat,@barno";
                    cmd3.Parameters.AddWithValue("@sirano",siparisSayisi+i+1);
                    cmd3.Parameters.AddWithValue("@masaid",secilenMasaID);
                    cmd3.Parameters.AddWithValue("@urunkodu",siparisler[i].Split('-')[0]);
                    cmd3.Parameters.AddWithValue("@miktar", siparisler[i].Split('-')[1]);
                    cmd3.Parameters.AddWithValue("@durum","Hazırlanıyor");
                    cmd3.Parameters.AddWithValue("@saat",DateTime.Now.TimeOfDay);
                    cmd3.Parameters.AddWithValue("@barno", siparisler[i].Split('-')[2]);
                    cmd3.ExecuteNonQuery();
                }

                for (int i = 0; i < tlSiparis.Nodes.Count; i++)
                {
                    if (tlSiparis.Nodes[i].GetValue("Durum").ToString()=="Yeni Siparis")
                    {
                        tlSiparis.Nodes[i].SetValue("Durum", "Hazırlanıyor");
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "Update Siparis set Durum='Hazırlanıyor' where MasaID=@masaid and SiraNo=@sirano";
                        komut.Parameters.AddWithValue("@masaid", secilenMasaID);
                        komut.Parameters.AddWithValue("@sirano", tlSiparis.Nodes[i].GetValue("SiraNo"));
                        komut.ExecuteNonQuery();
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSiparislerGonderildi_Click(object sender, EventArgs e)
        {
            durumGuncelle();
        }

        private void tümüGönderildiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tlSiparis.SelectAll();
            durumGuncelle();
            tlSiparis.UnselectNodes(tlSiparis.Nodes.FirstNode.Id, tlSiparis.Nodes.LastNode.Id);
        }

        private void tmrMasa_Tick(object sender, EventArgs e)
        {
            masaZamanArtır();
        }

        private void yonetimTileBarItem_ItemClick(object sender, TileItemEventArgs e)
        {
            menuTabloCek();
            masaTabloCek();
        }

        public void menuGrupDoldur()
        {
            cmbUrunGrubu.Properties.Items.Clear();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if(rbUrun.Checked)komut.CommandText = "select Adi from Gruplar where Adi!='' order by ID";
                else komut.CommandText = "select Adi from Barlar where Adi!='' order by ID";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    cmbUrunGrubu.Properties.Items.Add(dr[0].ToString());
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tblMasalar_SelectionChanged(object sender, EventArgs e)
        {
            if (tblMasalar.RowCount > 0 && rbMasa.Checked && tblMasalar.ColumnCount > 3)
            {
                pnlMasaGrup.Visible = true;
                pnlMasaAktiflik.Visible = true;
                lblMasaNo.Text = "Masa No";
                lblMasaAdiAyar.Text = "Masa Adı";
                pnlMasaAdi.Width = 110;
                txtMasaNo.Text = tblMasalar.CurrentRow.Cells[0].Value.ToString();
                txtMasaAdi.Text = tblMasalar.CurrentRow.Cells[2].Value.ToString();
                cmbMasaGrubu.SelectedItem = tblMasalar.CurrentRow.Cells[1].Value.ToString();
                cbMasaAktif.Checked = Convert.ToBoolean(tblMasalar.CurrentRow.Cells[3].Value);
            }
            else if (tblMasalar.RowCount > 0 && rbMasaGrup.Checked)
            {
                pnlMasaGrup.Visible = false;
                pnlMasaAktiflik.Visible = false;
                lblMasaNo.Text = "Grup No";
                lblMasaAdiAyar.Text = "Grup Adı";
                pnlMasaAdi.Width = 250;
                txtMasaNo.Text = tblMasalar.CurrentRow.Cells[0].Value.ToString();
                txtMasaAdi.Text = tblMasalar.CurrentRow.Cells[1].Value.ToString();
            }
        }

        public void masaKaydet()
        {
            if (txtMasaNo.Text != "" && txtMasaAdi.Text.Trim() != "" && !masaAdiKontrol())
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Masalar set GrupNo=@grupno,Adi=@adi,Aktif=@aktif where SiraNo=@sirano";
                    komut.Parameters.AddWithValue("@grupno", cmbMasaGrubu.SelectedIndex);
                    komut.Parameters.AddWithValue("@adi", txtMasaAdi.Text);
                    komut.Parameters.AddWithValue("@aktif", cbMasaAktif.Checked);
                    komut.Parameters.AddWithValue("@sirano", Convert.ToInt32(txtMasaNo.Text));
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    masaTabloCek();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtMasaAdi.Text.Trim() != "" && masaAdiKontrol())
            {
                XtraMessageBox.Show("Aynı isimde bir masa kayıtlıdır. Başka bir isim seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtMasaAdi.Text.Trim() == "")
            {
                XtraMessageBox.Show("Masa ismini boş bırakmayınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void masaGrupKaydet()
        {
            if (txtMasaNo.Text != "" && !txtMasaAdi.Text.Trim().Equals(""))
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update MasaGruplar set Adi=@adi where ID=@sirano";
                    komut.Parameters.AddWithValue("@adi", txtMasaAdi.Text);
                    komut.Parameters.AddWithValue("@sirano", Convert.ToInt32(txtMasaNo.Text));
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    masaTabloCek();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (txtMasaAdi.Text.Trim().Equals("")) XtraMessageBox.Show("Grup adı boş bırakılamaz!", "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnMasaKaydet_Click(object sender, EventArgs e)
        {
            if (rbMasa.Checked) masaKaydet();
            else masaGrupKaydet();
        }


        private bool masaAdiKontrol()
        {
            bool masaVarmi = false;
            SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
            SqlCommand komut = new SqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT COUNT(*) FROM Masalar WHERE SiraNo!=@sirano AND Adi=@adi;";
            komut.Parameters.AddWithValue("@adi", txtMasaAdi.Text);
            komut.Parameters.AddWithValue("@sirano", Convert.ToInt32(txtMasaNo.Text));
            baglanti.Open();
            SqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                if (dr[0].ToString() == "1")
                {
                    masaVarmi = true;
                }
                else masaVarmi = false;

            }
            baglanti.Close();
            return masaVarmi;

        }

        private void tblMenu_SelectionChanged(object sender, EventArgs e)
        {
            if (tblMenu.RowCount > 0 && rbUrun.Checked == true && tblMenu.SelectedRows.Count > 0)
            {
                txtUrunNo.Text = tblMenu.CurrentRow.Cells[0].Value.ToString();
                txtUrunAdi.Text = tblMenu.CurrentRow.Cells[1].Value.ToString();
                cmbUrunGrubu.SelectedItem = tblMenu.CurrentRow.Cells[2].Value.ToString();
                txtUrunFiyati.Text = tblMenu.CurrentRow.Cells[3].Value.ToString();
                if (tblMenu.CurrentRow.Cells[2].Value.ToString().Equals(""))
                {
                    cmbUrunGrubu.SelectedIndex = -1;
                    cmbUrunGrubu.Text = "Ürün Grubu Seçiniz...";
                }
                if (tblMenu.CurrentRow.Cells[1].Value.ToString() == "") txtUrunAdi.Focus();
                txtUrunFiyati.Visible = true;
                cmbUrunGrubu.Visible = true;
                lblUrunFiyati.Visible = true;
                lblUrunGrubu.Visible = true;
                txtUrunAdi.Width = 163;
            }
            else if(tblMenu.RowCount > 0 && rbGrup.Checked == true && tblMenu.SelectedRows.Count > 0)
            {
                if (tblMenu.CurrentRow.Cells[1].Value.ToString() == "") txtUrunAdi.Focus();

                txtUrunNo.Text = tblMenu.CurrentRow.Cells[0].Value.ToString();
                txtUrunAdi.Text = tblMenu.CurrentRow.Cells[1].Value.ToString();
                cmbUrunGrubu.SelectedItem = tblMenu.CurrentRow.Cells[2].Value.ToString();
                txtUrunFiyati.Visible = false;
                lblUrunFiyati.Visible = false;
                cmbUrunGrubu.Visible = true;
                lblUrunGrubu.Visible = true;
                lblUrunGrubu.Text = "Mutfak:";
                txtUrunAdi.Width = 389;
            }
            else
            {
                if (tblMenu.CurrentRow.Cells[1].Value.ToString() == "") txtUrunAdi.Focus();

                txtUrunNo.Text = tblMenu.CurrentRow.Cells[0].Value.ToString();
                txtUrunAdi.Text = tblMenu.CurrentRow.Cells[1].Value.ToString();
                txtUrunFiyati.Visible = false;
                lblUrunFiyati.Visible = false;
                cmbUrunGrubu.Visible = false;
                lblUrunGrubu.Visible = false;
                txtUrunAdi.Width = 389;
            }
        }

        private void btnGeri_Click(object sender, EventArgs e)
        {
            ztbUrunler.Visible = false;
            pnlMenuUrunler.SendToBack();
            btnGeri.Visible = false;
            gcUrunGruplari.Text = "Ürün Grupları";
        }

        private void btnMenuAyarKaydet_Click(object sender, EventArgs e)
        {
            if (rbUrun.Checked == true) urunKaydet();
            else if (rbGrup.Checked) grupKaydet();
            else barKaydet();
        }

        public void tabloSonuSec(int sira)
        {
            try
            {
                tblMenu.CurrentCell = tblMenu.Rows[sira].Cells[0];
                tblMenu.ClearSelection();
                tblMenu.Rows[sira].Selected = true;
            }
            catch
            {
                tblMenu.CurrentCell = tblMenu.Rows[sira-1].Cells[0];
                tblMenu.ClearSelection();
                tblMenu.Rows[sira-1].Selected = true;
            }
        }

        public void barKaydet()
        {
            int siraNo = Convert.ToInt32(txtUrunNo.Text);
            if (txtUrunAdi.Text.Trim().Equals("")) XtraMessageBox.Show("Mutfak/Bar adı boş bırakılamaz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (barVarmi()) XtraMessageBox.Show("Aynı isimde başka bir Mutfak/Bar zaten kayıtlı!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Barlar set Adi=@adi where ID=@sirano";
                    komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                    komut.Parameters.AddWithValue("@sirano", siraNo);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    menuTabloCek();
                    tabloSonuSec(siraNo);
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void grupKaydet()
        {
            int siraNo = Convert.ToInt32(txtUrunNo.Text);
            if (txtUrunAdi.Text.Trim().Equals("")) XtraMessageBox.Show("Grup adı boş bırakılamaz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (grupVarmi()) XtraMessageBox.Show("Aynı isimde başka bir grup zaten kayıtlı!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Gruplar set Adi=@adi,BarNo=@barno where ID=@sirano";
                    komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                    komut.Parameters.AddWithValue("@sirano", siraNo);
                    komut.Parameters.AddWithValue("@barno",cmbUrunGrubu.SelectedIndex+1);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    menuTabloCek();
                    tabloSonuSec(siraNo);
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool grupVarmi()
        {
            bool varmi = false;
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select Adi from Gruplar where Adi=@adi and ID!=@id";
                komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtUrunNo.Text));
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read()) varmi = true;
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return varmi;
        }

        public bool barVarmi()
        {
            bool varmi = false;
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select Adi from Barlar where Adi=@adi and ID!=@id";
                komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@id", Convert.ToInt32(txtUrunNo.Text));
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read()) varmi = true;
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return varmi;
        }

        public void urunKaydet()
        {
            int siraNo = Convert.ToInt32(txtUrunNo.Text);
            double fiyat;
            try
            {
                fiyat = Convert.ToDouble(txtUrunFiyati.Text);
            }
            catch
            {
                txtUrunFiyati.Text = "0,0";
                fiyat = 0;
            }
            if (txtUrunAdi.Text.Trim().Equals("")) XtraMessageBox.Show("Ürün adı boş bırakılamaz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (cmbUrunGrubu.SelectedIndex == -1) XtraMessageBox.Show("Ürün grubu boş bırakılamaz!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "update Menu set Adi=@adi,Grubu=@grup,Fiyat=@fiyat where SiraNo=@sirano";
                    komut.Parameters.AddWithValue("@adi", txtUrunAdi.Text);
                    komut.Parameters.AddWithValue("@grup", cmbUrunGrubu.SelectedIndex + 1);
                    komut.Parameters.AddWithValue("@fiyat", fiyat);
                    komut.Parameters.AddWithValue("@sirano", siraNo);
                    baglanti.Open();
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    menuTabloCek();
                    tabloSonuSec(siraNo);
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnMenuAyarSil_Click(object sender, EventArgs e)
        {
            int siraNo = Convert.ToInt32(txtUrunNo.Text);
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if (rbUrun.Checked == true)
                {
                    komut.CommandText = "update Menu set Adi='',Grubu='',Fiyat='' where SiraNo=@sirano";
                }
                else if(rbGrup.Checked)
                {
                    komut.CommandText = "update Gruplar set Adi='', BarNo='' where ID=@sirano";
                }
                else
                {
                    komut.CommandText = "update Barlar set Adi='' where ID=@sirano";
                }
                komut.Parameters.AddWithValue("@sirano", siraNo);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                menuTabloCek();
                tblMenu.CurrentCell = tblMenu.Rows[siraNo - 1].Cells[0];
                tblMenu.ClearSelection();
                tblMenu.Rows[siraNo - 1].Selected = true;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbUrun_CheckedChanged(object sender, EventArgs e)
        {
            menuTabloCek();
        }

        private void siparisTileBarItem_ItemClick(object sender, TileItemEventArgs e)
        {
            gcSiparis.Text = "Sipariş Paneli: " + masalar[secilenMasaID - 1].Text;
        }

        private void tümünüSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tlSiparis.SelectAll();
        }

        private void tümünüSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tlSiparis.SelectAll();
            siparisSilVt();
            tlSiparis.DeleteSelectedNodes();
            siraTutarHesap();
            tutarGuncelleVt();
        }

        private void btnHesapIptal_Click(object sender, EventArgs e)
        {
            double tutar = Convert.ToDouble(lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim());
            if (tutar > 0)
            {
                if (XtraMessageBox.Show(gcSiparis.Text.Replace("Sipariş Paneli: ", "") + " Toplam Tutarı: " + lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim() + "TL\nYine de masa hesabını iptal etmek istediğinize emin misiniz?", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    hesapIptal();
                }
            }
            else hesapIptal();
        }

        public void hesapIptal()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "update Masalar set BaslangicSaat=NULL,ToplamTutar=0 where SiraNo=@sira";
                komut.Parameters.AddWithValue("@sira", secilenMasaID);
                baglanti.Open();
                komut.ExecuteNonQuery();
                komut.CommandText = "delete from Siparis where MasaID=@sira";
                komut.ExecuteNonQuery();
                komut.CommandText = "delete from SiparisNotlari where MasaID=@sira";
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tileBar.SelectedItem = masalarTileBarItem;
            navigationFrame.SelectedPage = sayfaMasalar;
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if(XtraMessageBox.Show("Masayı taşımak istediğinize emin misiniz?", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                tileBar.SelectedItem = masalarTileBarItem;
                navigationFrame.SelectedPage = sayfaMasalar;
                masalar[secilenMasaID - 1].Appearance.BackColor = Color.Maroon;
                sureler[secilenMasaID - 1].Appearance.BackColor = Color.Maroon;
                tutarlar[secilenMasaID - 1].Appearance.BackColor = Color.Maroon;
                paneller[secilenMasaID - 1].Enabled = false;

                int ID;
                for (int i = 0; i < flpnlMasalar.Controls.Count; i++)
                {
                    ID = Convert.ToInt32(flpnlMasalar.Controls[i].Controls[0].Name.Replace("btnMasa", "")) - 1;
                    if (masalar[ID].Appearance.BackColor == Color.DarkOrange)
                    {
                        paneller[ID].Enabled = false;
                    }
                }
                tileBar.Enabled = false;
                pnlMasaGruplar.Enabled = false;
                lblMasaTasi.Visible = true;
                tmrMasaTasi.Start();
            }
        }

        private void tmrMasaTasi_Tick(object sender, EventArgs e)
        {
            if (lblMasaTasi.ForeColor == Color.Maroon)
            {
                tmrMasaTasi.Interval = 300;
                lblMasaTasi.ForeColor = Color.White;
            }
            else
            {
                tmrMasaTasi.Interval = 800;
                lblMasaTasi.ForeColor = Color.Maroon;
            }
        }

        private void btnMasalaraDon_Click(object sender, EventArgs e)
        {
            navigationFrame.SelectedPage = sayfaMasalar;
            tileBar.SelectedItem = masalarTileBarItem;
        }

        string numaraUret(int kod)
        {
            DateTime sontarih = DateTime.Today;
            string tarih = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd");
            string maxkayit = "";
            string numara = "";
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select MAX(Tarih) from Kasa where FisNo like '" + tarih + kod.ToString() + "%';";
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    try
                    {
                        sontarih = dr.GetDateTime(0);
                    }
                    catch
                    {
                        maxkayit = "0";
                    }
                }
                dr.Close();
                if (maxkayit != "0")
                {
                    SqlCommand komut2 = new SqlCommand();
                    komut2.Connection = baglanti;
                    komut2.CommandText = "select FisNo from Kasa where Tarih=@tarih;";
                    komut2.Parameters.AddWithValue("@tarih", sontarih);
                    SqlDataReader dr2 = komut2.ExecuteReader();
                    if (dr2.Read())
                    {
                        maxkayit = dr2[0].ToString();
                        if (maxkayit == "") maxkayit = "0";
                        else
                        {
                            maxkayit = maxkayit.Remove(0, 7);
                        }
                    }
                }
                maxkayit = (Convert.ToInt32(maxkayit) + 1).ToString();
                baglanti.Close();
                numara = DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + kod.ToString() + maxkayit;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Hatası!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return numara;
        }

        public void hesapAl(string nakitpos,string ekAciklama)
        {
            if (tlSiparis.Nodes.Count == 0) btnHesapIptal.PerformClick();
            else
            {
                try
                {
                    string fisno = numaraUret(0);
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    baglanti.Open();

                    for (int i = 0; i < tlSiparis.Nodes.Count; i++)
                    {
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "INSERT Fis SELECT @fisno,@sirano,@urunadi,@grubu,@fiyat;";
                        komut.Parameters.AddWithValue("@fisno", fisno);
                        komut.Parameters.AddWithValue("@sirano", tlSiparis.Nodes[i].GetValue("SiraNoGorunen"));
                        komut.Parameters.AddWithValue("@urunadi", tlSiparis.Nodes[i].GetValue("UrunAdi"));
                        komut.Parameters.AddWithValue("@grubu", tlSiparis.Nodes[i].GetValue("Grubu"));
                        komut.Parameters.AddWithValue("@fiyat", Convert.ToDouble(tlSiparis.Nodes[i].GetValue("Fiyat").ToString().Replace(" TL", "")));
                        komut.ExecuteNonQuery();
                        if (tlSiparis.Nodes[i].HasChildren)
                        {
                            for (int j = 0; j < tlSiparis.Nodes[i].Nodes.Count; j++)
                            {
                                SqlCommand komut3 = new SqlCommand();
                                komut3.Connection = baglanti;
                                komut3.CommandText = "INSERT Fis SELECT @fisno,@sirano,@urunadi,@grubu,@fiyat;";
                                komut3.Parameters.AddWithValue("@fisno", fisno);
                                komut3.Parameters.AddWithValue("@sirano","-"+ tlSiparis.Nodes[i].Nodes[j].GetValue("SiraNoGorunen"));
                                komut3.Parameters.AddWithValue("@urunadi", tlSiparis.Nodes[i].Nodes[j].GetValue("UrunAdi"));
                                komut3.Parameters.AddWithValue("@grubu", "Not:");
                                komut3.Parameters.AddWithValue("@fiyat", Convert.ToDouble(tlSiparis.Nodes[i].Nodes[j].GetValue("Fiyat").ToString().Replace(" TL", "")));
                                komut3.ExecuteNonQuery();
                            }
                        }
                    }

                    SqlCommand komut2 = new SqlCommand();
                    komut2.Connection = baglanti;
                    komut2.CommandText = "INSERT Kasa SELECT @fisno,@tur,@aciklama,@tutar,@odemetipi,@tarih;";
                    komut2.Parameters.AddWithValue("@fisno", fisno);
                    komut2.Parameters.AddWithValue("@tur", "Giren");
                    komut2.Parameters.AddWithValue("@aciklama", masalar[secilenMasaID - 1].Text + " Tahsilatı "+ekAciklama);
                    komut2.Parameters.AddWithValue("@tutar", Convert.ToDouble(lblTutar.Text.Replace("TUTAR: ", "").Replace(" TL", "").Trim()));
                    komut2.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut2.Parameters.AddWithValue("@odemetipi", nakitpos);
                    komut2.ExecuteNonQuery();

                    baglanti.Close();

                    tlSiparis.Nodes.Clear();
                    siraTutarHesap();
                    btnHesapIptal.PerformClick();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tmrOtoGuncelle_Tick(object sender, EventArgs e)
        {
            if (paneller[secilenMasaID - 1].Enabled == true)//Masa taşıma yapılmıyorsa.
            {
                if (navigationFrame.SelectedPage == sayfaMasalar)
                {
                    //masaCek(secilenMasaGrup);
                }
                else if (navigationFrame.SelectedPage == sayfaSiparisler)
                {
                    masaKontrol();
                    siparisKontrol();
                }
            }
        }

        public void siparisKontrol()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select COUNT(*) from Siparis where MasaID=@id";
                komut.Parameters.AddWithValue("@id", secilenMasaID);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if (dr.GetInt32(0) != tlSiparis.Nodes.Count)
                    {
                        siparisTabloDoldur();
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void masaKontrol()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select ISNULL(YEAR(BaslangicSaat),0),Adi from Masalar where SiraNo=@sirano";
                komut.Parameters.AddWithValue("@sirano", secilenMasaID);
                SqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    if (dr[0].ToString() == "0")
                    {
                        btnMasalaraDon.PerformClick();
                        XtraMessageBox.Show(dr[1].ToString() + " hesabı başka bir kullanıcı tarafından kapatıldı!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void kasaTabloDoldur()
        {
            string tur = "";
            string odemeTur = "";
            string gunluk = "";
            string aylik = "";
            string ozel = "";
            string metin = txtKasaAramaMetin.Text;
            if (metin == "Fiş No,Açıklama...") metin = "";
            if (cmbKasaAramaTur.SelectedIndex != -1) tur = " and Tur=@tur";
            if (cmbKasaAramaOdemeTipi.SelectedIndex != -1) odemeTur = " and OdemeTipi=@odemetipi";
            if (rbGunluk.Checked) gunluk = " and DAY(Tarih)=@gun and MONTH(Tarih)=@ay and YEAR(Tarih)=@yil";
            if (rbAylik.Checked) aylik = " and MONTH(Tarih)=@ay and YEAR(Tarih)=@yil";
            if (rbOzelTarih.Checked) ozel = " and (Tarih between @dt1 and @dt2)";
            tlKasa.Nodes.Clear();
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select * from Kasa where (FisNo=@metin or Aciklama like '%'+@metin+'%') " + tur + odemeTur + gunluk + aylik + ozel+" order by Tarih desc";
                komut.Parameters.AddWithValue("@metin", metin);
                if (!tur.Equals("")) komut.Parameters.AddWithValue("@tur", cmbKasaAramaTur.SelectedItem.ToString());
                if (!odemeTur.Equals("")) komut.Parameters.AddWithValue("@odemetipi", cmbKasaAramaOdemeTipi.SelectedItem.ToString());
                if (!gunluk.Equals(""))
                {
                    komut.Parameters.AddWithValue("@gun", dtGunluk.DateTime.Day);
                    komut.Parameters.AddWithValue("@ay", dtGunluk.DateTime.Month);
                    komut.Parameters.AddWithValue("@yil", dtGunluk.DateTime.Year);
                }
                if (!aylik.Equals(""))
                {
                    komut.Parameters.AddWithValue("@ay", cmbAylar.SelectedIndex + 1);
                    komut.Parameters.AddWithValue("@yil", DateTime.Today.Year);
                }
                if (!ozel.Equals(""))
                {
                    komut.Parameters.AddWithValue("@dt1", dtBaslangic.DateTime);
                    komut.Parameters.AddWithValue("@dt2", dtBitis.DateTime);
                }
                baglanti.Open();
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    TreeListNode tnode = tlKasa.AppendNode(null, null);
                    tnode.SetValue("fisNo", dr[0].ToString());
                    tnode.SetValue("tur", dr[1].ToString());
                    tnode.SetValue("aciklama", dr[2].ToString());
                    tnode.SetValue("tutar", dr[3].ToString() + " TL");
                    tnode.SetValue("odemeTipi", dr[4].ToString());
                    tnode.SetValue("tarih", dr[5].ToString());
                }
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            kasaAltBilgi();
        }

        public void kasaAltBilgi()
        {
            double toplamGiren = 0;
            double toplamCikan = 0;
            double toplam = 0;

            for (int i = 0; i < tlKasa.Nodes.Count; i++)
            {
                if (tlKasa.Nodes[i].GetValue("tur").ToString() == "Giren") toplamGiren += Convert.ToDouble(tlKasa.Nodes[i].GetValue("tutar").ToString().Replace(" TL", ""));
                else toplamCikan += Convert.ToDouble(tlKasa.Nodes[i].GetValue("tutar").ToString().Replace(" TL", ""));
            }
            toplam = toplamGiren - toplamCikan;

            lblKasaIslemSayisi.Text = tlKasa.Nodes.Count.ToString();
            lblKasaToplamGiren.Text = string.Format("{0:#,##0.00}", toplamGiren) + " TL";
            lblKasaToplamCikan.Text = string.Format("{0:#,##0.00}", toplamCikan) + " TL";
            lblKasaToplamTutar.Text = string.Format("{0:#,##0.00}", toplam) + " TL";
        }

        private void rbGunluk_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGunluk.Checked)
            {
                dtGunluk.Enabled = true;
                kasaTabloDoldur();
            }
            else dtGunluk.Enabled = false;
        }

        private void rbAylik_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAylik.Checked)
            {
                cmbAylar.Enabled = true;
                cmbAylar.SelectedIndex = DateTime.Today.Month - 1;
            }
            else
            {
                cmbAylar.Enabled = false;
                cmbAylar.SelectedIndex = -1;
                cmbAylar.Text = "Ay Seçiniz...";
            }

        }

        private void rbOzelTarih_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOzelTarih.Checked)
            {
                dtBaslangic.Enabled = true;
                dtBitis.Enabled = true;
                dtBaslangic.EditValue = DateTime.Today;
                dtBitis.EditValue = DateTime.Today.AddDays(1);
            }
            else
            {
                dtBaslangic.Enabled = false;
                dtBitis.Enabled = false;
                dtBaslangic.EditValue = null;
                dtBitis.EditValue = null;
            }
        }

        public void kasaTarihKontrol()
        {
            dtGunluk.Enabled = false;
            cmbAylar.Enabled = false;
            dtBaslangic.Enabled = false;
            dtBitis.Enabled = false;
            cmbAylar.Text = "Ay seçiniz...";
            dtBaslangic.EditValue = null;
            dtBitis.EditValue = null;
            if (rbGunluk.Checked)
            {
                dtGunluk.Enabled = true;
                dtGunluk.EditValue = DateTime.Today;
            }
            else if (rbAylik.Checked)
            {
                cmbAylar.Enabled = true;

            }
            else if (rbOzelTarih.Checked)
            {
                dtBaslangic.Enabled = true;
                dtBitis.Enabled = true;
                dtBaslangic.EditValue = DateTime.Today.AddDays(-1);
                dtBitis.EditValue = DateTime.Today;
            }
        }

        private void txtKasaAramaMetin_EditValueChanged(object sender, EventArgs e)
        {
            if (txtKasaAramaMetin.EditValue != null) kasaTabloDoldur();
        }

        private void cmbKasaAramaTur_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKasaAramaTur.SelectedIndex != -1) kasaTabloDoldur();
        }

        private void cmbKasaAramaOdemeTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKasaAramaOdemeTipi.SelectedIndex != -1) kasaTabloDoldur();
        }

        private void dtGunluk_EditValueChanged(object sender, EventArgs e)
        {
            if (rbGunluk.Enabled && rbGunluk.Checked) kasaTabloDoldur();
        }

        private void cmbAylar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAylar.Text != "Ay Seçiniz..." && cmbAylar.SelectedIndex != -1) kasaTabloDoldur();
        }

        public void kasaAramaTemizle(bool tumu)
        {
            if (tumu) rbGunluk.Checked = false;
            else
            {
                dtGunluk.EditValue = DateTime.Today;
                rbGunluk.Checked = true;
            } 
            rbAylik.Checked = false;
            rbOzelTarih.Checked = false;
            dtGunluk.Enabled = false;
            cmbAylar.Enabled = false;
            dtBaslangic.Enabled = false;
            dtBitis.Enabled = false;
            cmbAylar.Text = "Ay seçiniz...";
            dtBaslangic.EditValue = null;
            dtBitis.EditValue = null;
            cmbKasaAramaOdemeTipi.SelectedIndex = -1;
            cmbKasaAramaOdemeTipi.Text = "Ödeme Tipi seçiniz...";
            cmbKasaAramaTur.SelectedIndex = -1;
            cmbKasaAramaTur.Text = "Tür seçiniz...";
            txtKasaAramaMetin.EditValue = null;
            kasaTabloDoldur();
        }

        private void btnKasaTumunuGoster_Click(object sender, EventArgs e)
        {
            kasaAramaTemizle(true);
        }

        private void dtBaslangic_EditValueChanged(object sender, EventArgs e)
        {
            if (dtBaslangic.EditValue != null && dtBitis.EditValue != null) kasaTabloDoldur();
        }

        private void dtBitis_EditValueChanged(object sender, EventArgs e)
        {
            if (dtBitis.EditValue != null && dtBaslangic.EditValue != null) kasaTabloDoldur();
        }

        private void tsmIcerik_Click(object sender, EventArgs e)
        {
            kasaIcerikAc();
        }

        public void kasaIcerikAc()
        {
            tlFisIcerik.Nodes.Clear();
            lblFisNoIcerik.Text = tlKasa.FocusedNode.GetValue("fisNo").ToString();
            lblFisTutarı.Text = tlKasa.FocusedNode.GetValue("tutar").ToString();
            lblFisTarihi.Text = tlKasa.FocusedNode.GetValue("tarih").ToString();
            lblKasaIcerikAciklama.Text = "Açıklama: "+ tlKasa.FocusedNode.GetValue("aciklama").ToString();
            try
            {
                SqlConnection baglanti = new SqlConnection(Properties.Settings.Default.conString);
                baglanti.Open();
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                komut.CommandText = "select SiraNo,Grubu,UrunAdi,Fiyat from Fis where FisNo=@fisno";
                komut.Parameters.AddWithValue("@fisno", tlKasa.FocusedNode.GetValue("fisNo").ToString());
                SqlDataReader dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.GetString(0).Substring(0, 1).Equals("-"))
                    {
                        TreeListNode tnode = tlFisIcerik.AppendNode(null, tlFisIcerik.Nodes.LastNode);
                        tnode.SetValue("fissiraNo", dr.GetString(0).Replace("-",""));
                        tnode.SetValue("fisgrubu", dr.GetString(2));
                        tnode.SetValue("fisurunadi", dr.GetString(1));
                        tnode.SetValue("fistutar", dr.GetDouble(3).ToString() + " TL");
                    }
                    else
                    {
                        TreeListNode tnode = tlFisIcerik.AppendNode(null, null);
                        tnode.SetValue("fissiraNo", dr.GetString(0));
                        tnode.SetValue("fisgrubu", dr.GetString(2));
                        tnode.SetValue("fisurunadi", dr.GetString(1));
                        tnode.SetValue("fistutar", dr.GetDouble(3).ToString() + " TL");
                    }
                }
                dr.Close();
                baglanti.Close();
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            pnlKasa.SendToBack();
            tlFisIcerik.ExpandAll();
            btnKasaSeciliSatirSil.Enabled = false;
        }

        private void btnFisIcerikGeriDon_Click(object sender, EventArgs e)
        {
            pnlKasa.BringToFront();
            btnKasaSeciliSatirSil.Enabled = true;
        }

        private void tlKasa_DoubleClick(object sender, EventArgs e)
        {
            kasaIcerikAc();
        }

        private void btnHesapAlNakit_Click(object sender, EventArgs e)
        {
            hesapAl("Nakit","");
        }

        private void btnHesapAlPos_Click(object sender, EventArgs e)
        {
            hesapAl("POS","");
        }

        private void btnKasaNumaraUret_Click(object sender, EventArgs e)
        {
            txtKasaIslemNo.Text = numaraUret(1);
        }

        private void txtKasaIslemTutar_Leave(object sender, EventArgs e)
        {
            try
            {
                txtKasaIslemTutar.Text = string.Format("{0:#,##0.00}", Convert.ToDouble(txtKasaIslemTutar.Text));
            }
            catch
            {
                txtKasaIslemTutar.Text = "0,00";
            }
        }

        private void txtKasaIslemTutar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',';
        }

        private void btnKasaParaGiris_Click(object sender, EventArgs e)
        {
            kasaIslemYap("Giren");
        }

        private void btnKasaParaCikis_Click(object sender, EventArgs e)
        {
            kasaIslemYap("Çıkan");
        }

        public void kasaIslemYap(string tur)
        {
            if (Convert.ToDouble(txtKasaIslemTutar.Text) <= 0) XtraMessageBox.Show("İşlem tutarı sıfırdan büyük olmalıdır!", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand();
                    komut.Connection = baglanti;
                    komut.CommandText = "INSERT Kasa SELECT @fisno,@tur,@aciklama,@tutar,@odemetipi,@tarih;";
                    komut.Parameters.AddWithValue("@fisno", txtKasaIslemNo.Text);
                    komut.Parameters.AddWithValue("@tur", tur);
                    komut.Parameters.AddWithValue("@aciklama", txtKasaIslemAciklama.Text);
                    komut.Parameters.AddWithValue("@tutar", Convert.ToDouble(txtKasaIslemTutar.Text));
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now);
                    komut.Parameters.AddWithValue("@odemetipi", cmbKasaIslemTur.SelectedItem.ToString());
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    kasaAramaTemizle(false);
                    kasaTabloDoldur();
                    btnKasaNumaraUret.PerformClick();
                }
                catch (SqlException ex)
                {
                    XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ztbMasalar_EditValueChanged(object sender, EventArgs e)
        {
            int ID;
            for (int i = 0; i < flpnlMasalar.Controls.Count; i++)
            {
                ID = Convert.ToInt32(flpnlMasalar.Controls[i].Controls[0].Name.Replace("btnMasa", "")) - 1;
                paneller[ID].Height = ztbMasalar.Value * 24;
                if (ztbMasalar.Value >= 5) paneller[ID].Width = ztbMasalar.Value * 24;
            }
            Properties.Settings.Default.masaBoyut = ztbMasalar.Value;
            Properties.Settings.Default.Save();
        }

        private void ztbUrunler_EditValueChanged(object sender, EventArgs e)
        {
            int boyut = 136;
            float font = 9.75F;
            if (ztbUrunler.Value == 1)
            {
                boyut = 207;
                font = 11.25F;
            }
            else if (ztbUrunler.Value == 2)
            {
                boyut = 420;
                font = 12.75F;
            }
            for (int i = 0; i < pnlMenuUrunler.Controls.Count; i++)
            {
                pnlMenuUrunler.Controls[i].Width = boyut;
                pnlMenuUrunler.Controls[i].Font = new Font("Tahoma", font, FontStyle.Bold);//9.75-11.25-12.75
            }
        }

        private void tlSiparis_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.ParentNode != null)
            {
                e.Appearance.ForeColor = Color.Maroon;
                e.Appearance.BackColor = Color.White;
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                e.Appearance.Font = new Font("Tahoma", 12, FontStyle.Regular);
            }
        }

        private void rbMasa_CheckedChanged(object sender, EventArgs e)
        {
            masaTabloCek();
        }

        private void btnKasaSeciliSatirSil_Click(object sender, EventArgs e)
        {
            if (tlKasa.Nodes.Count > 0)
            {
                string fisNo = tlKasa.FocusedNode.GetValue("fisNo").ToString();
                string tutar = tlKasa.FocusedNode.GetValue("tutar").ToString();
                if (XtraMessageBox.Show(tutar + " tutarındaki " + fisNo + " numaralı kasa hareket kaydını silmek istediğinize emin misiniz?\nBu hareket kaydına bağlı içerik de silinecektir!", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                        baglanti.Open();
                        SqlCommand komut = new SqlCommand();
                        komut.Connection = baglanti;
                        komut.CommandText = "delete from Kasa where FisNo=@fisno";
                        komut.Parameters.AddWithValue("@fisno", fisNo);
                        komut.ExecuteNonQuery();
                        komut.CommandText = "delete from Fis where FisNo=@fisno";
                        komut.ExecuteNonQuery();
                        baglanti.Close();
                        kasaTabloDoldur();
                    }
                    catch (SqlException ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tlFisIcerik_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.ParentNode != null)
            {
                e.Appearance.ForeColor = Color.Maroon;
                e.Appearance.BackColor = Color.White;
                e.Appearance.Font = new Font("Tahoma", 9.75F, FontStyle.Regular);
            }
        }

        public void notAc()
        {
            try
            {
                SqlConnection baglanti = new SqlConnection(Adisyon.Properties.Settings.Default.conString);
                SqlCommand komut = new SqlCommand();
                komut.Connection = baglanti;
                if (rbUrun.Checked) komut.CommandText = "select Menu.SiraNo[No],Menu.Adi[Ürün Adı],Gruplar.Adi[Grubu],Menu.Fiyat from Menu left join Gruplar on Menu.Grubu=Gruplar.ID order by Menu.SiraNo";
                else komut.CommandText = "select ID[No],Adi[Grup Adı] from Gruplar";
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                da.Fill(dt);
                tblMenu.DataSource = dt;
                baglanti.Close();
                tblMenu.AutoResizeColumns();
                tblMenu.Columns[0].Width = 30;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message, "Veritabanı Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notlarıGörüntüleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtUrunNo.Visible = false;
            lblUrunGrubu.Visible = false;
            cmbUrunGrubu.Visible = false;
            lblMenuAyarNo.Text = "Not Bilgileri";
            btnMenuAyarKaydet.Text = "Not Ekle";
            btnMenuAyarSil.Text = "Notu Sil!";
            pnlRadioButton.Visible = false;
        }

        private void rbGrup_CheckedChanged(object sender, EventArgs e)
        {
            menuTabloCek();
        }

        private void rbBar_CheckedChanged(object sender, EventArgs e)
        {
            menuTabloCek();
        }

        private void panel31_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}