using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // Thư viện vẽ
using QLTV.DTO;
using System.Collections.Generic;
namespace QLTV
{
    public partial class fInterface : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fInterface(Account acc)
        {
            InitializeComponent();
            this.Load += fInterface_Load;
            this.Resize += fInterface_Resize;
            // Đường dẫn tới file ảnh
            Bitmap originalImage = Properties.Resources.KhuS;

            Bitmap blurredImage = ApplyGaussianBlur(originalImage);

            panel2.BackgroundImage = blurredImage;
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            this.LoginAccount = acc;
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fInterface_Load(object sender, EventArgs e)
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
        private void fInterface_Resize(object sender, EventArgs e)
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
        // Hàm Gaussian Blur đơn giản
        public Bitmap ApplyGaussianBlur(Bitmap image)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                // Sử dụng Graphics để vẽ mờ
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Tạo Rectangle để áp dụng độ mờ
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem1.Enabled = type == 1;
        }

        #endregion

        #region events
        private void studentPersonalInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string studentID = LoginAccount.StudentID;
            string displayName = LoginAccount.DisplayName;
            fInformation f = new fInformation(studentID, displayName);
            f.ShowDialog();
            this.Show();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị một thông báo xác nhận đăng xuất
            var result = MessageBox.Show("Do you want to log out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();  // Đóng form chính (fInterface)
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fChangePass f = new fChangePass(LoginAccount);
            f.ShowDialog();
            this.Show();
        }

        private void changeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string studentID = LoginAccount.StudentID;
            fUpdateInfo f = new fUpdateInfo(studentID);
            f.ShowDialog();
            this.Show();
        }

        private void adminToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            this.Hide();
            f.LoginAccount = LoginAccount;
            f.ShowDialog();
            this.Show();
        }

        private void LMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fLibrary f = new fLibrary();
            this.Hide();
            f.LoginAccount = LoginAccount;
            f.ShowDialog();
            this.Show();
        }
        #endregion
    }
}
