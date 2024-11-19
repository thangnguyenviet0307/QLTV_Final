using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; 
using System.Drawing.Imaging;
using BCrypt.Net;
using QLTV.DAO;
using QLTV.DTO;

namespace QLTV
{
    public partial class fLogin : Form
    {   
        public fLogin()
        {
            InitializeComponent();
            this.Load += fLogin_Load;
            // Đường dẫn tới file ảnh
            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.txbUserName.KeyDown += new KeyEventHandler(btnLogin_KeyDown);
            this.txbPassWord.KeyDown += new KeyEventHandler(btnLogin_KeyDown);
            cbShowPassword.Paint += new PaintEventHandler(cbShowPassword_Paint);
            cbShowPassword.CheckedChanged += new EventHandler(cbShowPassword_CheckedChanged);
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fLogin_Load(object sender, EventArgs e)
        {
            initialNormalSize = this.ClientSize; // Lưu kích thước form ban đầu
            initialControlBounds = new Dictionary<Control, Rectangle>();
            initialFontSizes = new Dictionary<Control, float>();

            SaveInitialBounds(this); // Lưu vị trí và kích thước ban đầu
            EnableDoubleBuffering(this); // Kích hoạt DoubleBuffered để giảm nhấp nháy
        }

        // Kích hoạt DoubleBuffered cho form và các control con
        private void EnableDoubleBuffering(Control control)
        {
            control.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, control, new object[] { true });

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }

        // Lưu vị trí, kích thước và font chữ ban đầu của tất cả điều khiển
        private void SaveInitialBounds(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (!initialControlBounds.ContainsKey(ctrl))
                {
                    initialControlBounds[ctrl] = ctrl.Bounds; // Lưu kích thước và vị trí ban đầu
                }

                if (!initialFontSizes.ContainsKey(ctrl) && ctrl.Font != null)
                {
                    initialFontSizes[ctrl] = ctrl.Font.Size; // Lưu kích thước font chữ ban đầu
                }

                if (ctrl.HasChildren)
                {
                    SaveInitialBounds(ctrl); // Đệ quy cho các control con
                }
            }
        }

        // Hàm điều chỉnh vị trí, kích thước và font chữ của các control theo tỷ lệ
        private void ResizeControls(float widthRatio, float heightRatio)
        {
            foreach (var entry in initialControlBounds)
            {
                Control ctrl = entry.Key;
                Rectangle initialBounds = entry.Value;

                // Tính toán vị trí và kích thước mới
                ctrl.Bounds = new Rectangle(
                    (int)(initialBounds.Left * widthRatio),
                    (int)(initialBounds.Top * heightRatio),
                    (int)(initialBounds.Width * widthRatio),
                    (int)(initialBounds.Height * heightRatio)
                );

                // Điều chỉnh font chữ nếu control hỗ trợ
                if (ctrl is Label || ctrl is Button || ctrl is TextBox)
                {
                    if (initialFontSizes.ContainsKey(ctrl))
                    {
                        float newFontSize = initialFontSizes[ctrl] * Math.Min(widthRatio, heightRatio);
                        ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize, ctrl.Font.Style);
                    }
                }
            }
        }

        // Xử lý sự kiện Resize
        private void fLogin_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (!isMaximized)
                {
                    isMaximized = true;
                    ApplyResize(true); // Áp dụng resize khi lần đầu Maximize
                }
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                if (isMaximized)
                {
                    isMaximized = false;
                    ApplyResize(false); // Áp dụng resize khi quay về Normal
                }
            }
        }

        // Hàm áp dụng resize (cập nhật kích thước và font chữ)
        private void ApplyResize(bool isMaximizedState)
        {
            if (initialControlBounds == null || initialControlBounds.Count == 0)
                return;

            float widthRatio, heightRatio;

            if (isMaximizedState)
            {
                // Tính tỷ lệ khi ở trạng thái Maximized
                widthRatio = (float)this.ClientSize.Width / initialNormalSize.Width;
                heightRatio = (float)this.ClientSize.Height / initialNormalSize.Height;
            }
            else
            {
                // Trở về tỷ lệ gốc
                widthRatio = 1.0f;
                heightRatio = 1.0f;
            }

            ResizeControls(widthRatio, heightRatio); // Resize control theo tỷ lệ
        }
        #endregion

        #region methods
        public Bitmap ApplyGaussianBlur(Bitmap image)
        {

            using (Graphics g = Graphics.FromImage(image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }

        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                txbPassWord.UseSystemPasswordChar = false; // Hiển thị mật khẩu
            }
            else
            {
                txbPassWord.UseSystemPasswordChar = true; // Ẩn mật khẩu
            }
        }

        private void cbShowPassword_Paint(object sender, PaintEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            e.Graphics.Clear(cb.BackColor);

            Rectangle rect = new Rectangle(0, 0, 20, 20); // Tùy chỉnh kích thước của hộp kiểm
            ControlPaint.DrawCheckBox(e.Graphics, rect,
                cb.Checked ? ButtonState.Checked : ButtonState.Normal);

            // Vẽ văn bản bên cạnh hộp kiểm
            using (Font font = new Font("Microsoft Sans Serif", 10))
            {
                e.Graphics.DrawString(cb.Text, font, Brushes.Black, rect.Right + 5, 2);
            }
        }
        #endregion

        #region events
        private void button1_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string passWord = txbPassWord.Text;

            if (passWord.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPassWord.Clear();
                return;
            }

            if (Login(userName, passWord))
            {
                Account loginAccout = AccountDAO.Instance.GetAccountByUserName(userName);
                fInterface f = new fInterface(loginAccout);
                this.Hide();  // Ẩn form đăng nhập hiện tại
                f.Show();  // Hiển thị form chính
                this.DialogResult = DialogResult.OK;
                //// Đăng ký sự kiện khi form chính (fInterface) đóng sẽ mở lại form login (fLogin)
                f.FormClosed += (s, args) => this.Show();
                
            }
            else
            {
                MessageBox.Show("Username or password is incorrect, please try again.", "Login error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPassWord.Clear();

                // Đưa con trỏ về TextBox mật khẩu
                txbPassWord.Focus();
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ hiển thị thông báo nếu chương trình thực sự đang thoát
            if (this.DialogResult != DialogResult.OK) // Chỉ khi không đăng nhập thành công
            {
                if (MessageBox.Show("Do you really want to exit the program?", "Notification", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    e.Cancel = true; // Hủy sự kiện đóng
                }
            }
        }
        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }
        #endregion
    }
}
