using QLTV.DAO;
using QLTV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class fChangePass : Form
    {
        private Account loginAccout;

        public Account LoginAccout
        {
            get { return loginAccout; }
            set { loginAccout = value; ChangeAccount(LoginAccout); }
        }
        public fChangePass(Account acc)
        {
            InitializeComponent();
            this.Load += fChangePass_Load;

            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;

            LoginAccout = acc;
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fChangePass_Load(object sender, EventArgs e)
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
        private void fChangePass_Resize(object sender, EventArgs e)
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

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = LoginAccout.UserName;
            txbDisplayName.Text = LoginAccout.DisplayName;
        }

        void UpdateAccount()
        {
            string displayName = txbDisplayName.Text;
            string passWord = txbPassWord.Text;
            string newPass = txbNewPass.Text;
            string reEnterPass = txbReEnterPass.Text;
            string userName = txbUserName.Text;

            // Check if all fields are entered
            if (string.IsNullOrWhiteSpace(displayName) || string.IsNullOrWhiteSpace(passWord) ||
                string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(reEnterPass) ||
                string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("All fields must be filled in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop execution if any field is empty
            }

            // Check if new password is greater than 6 characters
            if (newPass.Length < 6)
            {
                MessageBox.Show("New password must be longer than 6 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop execution if the new password is too short
            }

            // Check if new password matches the current password
            if (newPass.Equals(passWord))
            {
                MessageBox.Show("New password cannot be the same as the current password. Please enter a different password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Stop execution if the new password is the same as the current password
            }

            if (!newPass.Equals(reEnterPass))
            {
                MessageBox.Show("Re-enter password does not match the new password", "Notification");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, passWord, newPass))
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to change your password?", "Notification", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    MessageBox.Show("Update successful", "Notification");
                }
                else
                {
                    MessageBox.Show("Wrong password", "Notification");
                }
            }
        }
        #endregion

        #region events
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                this.Close();
            }
        }
        #endregion
    }
}
