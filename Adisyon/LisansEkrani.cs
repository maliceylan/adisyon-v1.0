using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace Adisyon
{
    public partial class LisansEkrani : DevExpress.XtraEditors.XtraForm
    {
        public LisansEkrani()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LisansEkrani_Load(object sender, EventArgs e)
        {
            txtkey.Text = Lisans.YeniKey();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Lisans.KeyDogrula(txtkey.Text, txtlic.Text);
            if (Lisans.LICKontrol())
            {
                XtraMessageBox.Show("Program ömür boyu kullanım olarak lisanslanmıştır.\nGüle güle kullanın.", "Lisanslama Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form frm = new frmAdisyon();
                frm.Show();
            }
        }
        private void btnKopyala_Click(object sender, EventArgs e)
        {
            txtkey.SelectAll();
            txtkey.Copy();
        }

        private void txtlic_MouseClick(object sender, MouseEventArgs e)
        {
            txtlic.SelectAll();
        }
    }
}