namespace Adisyon
{
    partial class LisansEkrani
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LisansEkrani));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtlic = new System.Windows.Forms.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.hyperLinkEdit1 = new DevExpress.XtraEditors.HyperLinkEdit();
            this.txtkey = new System.Windows.Forms.MaskedTextBox();
            this.btnKopyala = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtlic, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.hyperLinkEdit1, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtkey, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnKopyala, 4, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.11454F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.55507F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(640, 454);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F);
            this.label3.Location = new System.Drawing.Point(0, 112);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 51);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ürün Kodu:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 5);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(5, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(630, 47);
            this.label2.TabIndex = 1;
            this.label2.Text = "Program lisansı doğrulayamadığı için lisans ekranına yönlendirildiniz. Programı k" +
    "ullanmaya devam edebilmek için lütfen bizimle irtibata geçiniz. \r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 26F);
            this.label1.Location = new System.Drawing.Point(133, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(374, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "HOŞGELDİNİZ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 5);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label4.Location = new System.Drawing.Point(5, 229);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(630, 46);
            this.label4.TabIndex = 4;
            this.label4.Text = "Lisanslama işlemi için yukarıdaki ürün kodunu bize iletmeniz gerekmektedir. Ürün " +
    "kodunu gönderdiğiniz taktirde ürün kodunun kaybolmaması için lütfen programı açı" +
    "k tutunuz...";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtlic
            // 
            this.txtlic.AllowPromptAsInput = false;
            this.tableLayoutPanel1.SetColumnSpan(this.txtlic, 3);
            this.txtlic.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtlic.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.txtlic.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtlic.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtlic.Location = new System.Drawing.Point(131, 296);
            this.txtlic.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtlic.Mask = ">CCCCC-CCCCC-CCCCC-CCCCC-CCCCC";
            this.txtlic.Name = "txtlic";
            this.txtlic.Size = new System.Drawing.Size(378, 30);
            this.txtlic.TabIndex = 5;
            this.txtlic.TabStop = false;
            this.txtlic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtlic_MouseClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14F);
            this.label5.Location = new System.Drawing.Point(0, 280);
            this.label5.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 44);
            this.label5.TabIndex = 6;
            this.label5.Text = "Lisans Kodu:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnOK
            // 
            this.btnOK.Appearance.BackColor = System.Drawing.Color.Peru;
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnOK.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btnOK.Appearance.Options.UseBackColor = true;
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOK.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnOK.ImageOptions.SvgImage")));
            this.btnOK.Location = new System.Drawing.Point(515, 296);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(115, 30);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "DOĞRULA";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // hyperLinkEdit1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.hyperLinkEdit1, 3);
            this.hyperLinkEdit1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hyperLinkEdit1.EditValue = "www.yazilimmarket.com";
            this.hyperLinkEdit1.Location = new System.Drawing.Point(131, 431);
            this.hyperLinkEdit1.Name = "hyperLinkEdit1";
            this.hyperLinkEdit1.Properties.Appearance.Options.UseTextOptions = true;
            this.hyperLinkEdit1.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.hyperLinkEdit1.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.hyperLinkEdit1.Size = new System.Drawing.Size(378, 20);
            this.hyperLinkEdit1.TabIndex = 8;
            // 
            // txtkey
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtkey, 3);
            this.txtkey.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtkey.Font = new System.Drawing.Font("Tahoma", 14F);
            this.txtkey.Location = new System.Drawing.Point(131, 135);
            this.txtkey.Mask = "CCCCC-CCCCC-CCCCC-CCCCC-CCCCC";
            this.txtkey.Name = "txtkey";
            this.txtkey.ReadOnly = true;
            this.txtkey.Size = new System.Drawing.Size(378, 30);
            this.txtkey.TabIndex = 3;
            this.txtkey.TabStop = false;
            // 
            // btnKopyala
            // 
            this.btnKopyala.Appearance.BackColor = DevExpress.LookAndFeel.DXSkinColors.FillColors.Question;
            this.btnKopyala.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnKopyala.Appearance.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.btnKopyala.Appearance.Options.UseBackColor = true;
            this.btnKopyala.Appearance.Options.UseFont = true;
            this.btnKopyala.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnKopyala.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnKopyala.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnKopyala.ImageOptions.SvgImage")));
            this.btnKopyala.ImageOptions.SvgImageSize = new System.Drawing.Size(25, 25);
            this.btnKopyala.Location = new System.Drawing.Point(515, 135);
            this.btnKopyala.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnKopyala.Name = "btnKopyala";
            this.btnKopyala.Size = new System.Drawing.Size(115, 30);
            this.btnKopyala.TabIndex = 9;
            this.btnKopyala.Text = "KOPYALA";
            this.btnKopyala.Click += new System.EventHandler(this.btnKopyala_Click);
            // 
            // LisansEkrani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 454);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(640, 480);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "LisansEkrani";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Adisyon Lisans Kontrol ";
            this.Load += new System.EventHandler(this.LisansEkrani_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hyperLinkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtkey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MaskedTextBox txtlic;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.HyperLinkEdit hyperLinkEdit1;
        private DevExpress.XtraEditors.SimpleButton btnKopyala;
    }
}