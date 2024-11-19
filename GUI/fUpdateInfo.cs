using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D; // Thư viện vẽ
using System.Drawing.Imaging;
using QLTV.DTO;
using QLTV.DAO;
using System.IO;
using System.Data.SqlClient;

namespace QLTV
{
    public partial class fUpdateInfo : Form
    {  
        private string connectionSTR = "Data Source=DESKTOP-0CRD70D\\MSSQLSERVER_1;Initial Catalog=QLTV2;Integrated Security=True";
        SqlConnection connection;
        SqlCommand command;
        public fUpdateInfo(string studentID)
        {
            
            InitializeComponent();
            this.Load += fUpdateInfo_Load;
            // Đường dẫn tới file ảnh
            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.txbStudentID.Text = studentID;
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fUpdateInfo_Load(object sender, EventArgs e)
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

                // Điều chỉnh font chữ nếu control hỗ trợ và chỉ thay đổi nếu cần thiết
                if (ctrl is Label || ctrl is Button || ctrl is TextBox)
                {
                    if (initialFontSizes.ContainsKey(ctrl))
                    {
                        float newFontSize = initialFontSizes[ctrl] * Math.Min(widthRatio, heightRatio);
                        if (Math.Abs(ctrl.Font.Size - newFontSize) > 0.01f) // Chỉ thay đổi khi kích thước khác biệt đáng kể
                        {
                            ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize, ctrl.Font.Style);
                        }
                    }
                }

                // Xử lý riêng cho DateTimePicker để tránh tăng kích thước không kiểm soát
                if (ctrl is DateTimePicker dateTimePicker)
                {
                    AdjustDateTimePicker(dateTimePicker, widthRatio, heightRatio);
                }
            }
        }

        private void AdjustDateTimePicker(DateTimePicker dateTimePicker, float widthRatio, float heightRatio)
        {
            if (initialFontSizes.ContainsKey(dateTimePicker))
            {
                float newFontSize = initialFontSizes[dateTimePicker] * Math.Min(widthRatio, heightRatio);
                if (Math.Abs(dateTimePicker.Font.Size - newFontSize) > 0.01f) // Chỉ thay đổi khi kích thước khác biệt đáng kể
                {
                    dateTimePicker.Font = new Font(dateTimePicker.Font.FontFamily, newFontSize, dateTimePicker.Font.Style);
                }
            }
        }

        // Xử lý sự kiện Resize
        private void fUpdateInfo_Resize(object sender, EventArgs e)
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

        void InsertAccount()
        {
            string fullName = txbFullName.Text;
            string studentIDString = txbStudentID.Text; // Stored as a string to check
            string className = txbClass.Text;
            string faculty = txbFaculty.Text;
            DateTime dateOfBirth = dtpkFromDate.Value;
            string email = txbEmail.Text;
            string phoneNumberString = txbPhoneNumber.Text; // Stored as a string to check

            if (ptbImage.Image == null)
            {
                MessageBox.Show("Please upload an image before inserting account information", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            byte[] profilePicture = null;

            using (connection = new SqlConnection(connectionSTR))
            {
                // Check if the user has entered all required information
                if (string.IsNullOrWhiteSpace(fullName) ||
                    string.IsNullOrWhiteSpace(studentIDString) ||
                    string.IsNullOrWhiteSpace(className) ||
                    string.IsNullOrWhiteSpace(faculty) ||
                    string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(phoneNumberString))
                {
                    // Check if the date of birth is valid
                    if (dateOfBirth == dtpkFromDate.MinDate || dateOfBirth == dtpkFromDate.MaxDate)
                    {
                        MessageBox.Show("Please enter a valid date of birth", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    MessageBox.Show("Please enter all required information", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                        connection.Open();
                        string query = "USP_InsertAccountInfo @FullName , @StudentID , @ClassRoom , @Faculty , @DateOfBirth , @Email , @PhoneNumber , @profilePicture";
                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@FullName", fullName);
                        command.Parameters.AddWithValue("@StudentID", studentIDString);
                        command.Parameters.AddWithValue("@ClassRoom", className);
                        command.Parameters.AddWithValue("@Faculty", faculty);
                        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumberString);

                        if (btnUploadImgage != null)
                        {
                            profilePicture = ImageToByteArray(ptbImage.Image);
                            command.Parameters.AddWithValue("@profilePicture", profilePicture);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@profilePicture", DBNull.Value);
                        }

                        command.ExecuteNonQuery();
                        MessageBox.Show("Insert account information successfully", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        string studentID = txbStudentID.Text;
                        fInformation information = new fInformation(studentID, fullName);
                }
            }

        }   

        private byte[] ImageToByteArray(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
        #endregion

        #region events
        private void btnUploadImgage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*jpg;*jpeg;*png;*bmp"; // Filter for image files
            openFileDialog.Title = "Select an image file"; // Title of the dialog

            // Show the dialog and check if the user selects a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the image into the PictureBox
                ptbImage.Image = new Bitmap(openFileDialog.FileName);
                ptbImage.SizeMode = PictureBoxSizeMode.StretchImage; // Make sure image fits the box
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInsertInfo_Click(object sender, EventArgs e)
        {
            InsertAccount();
        }
    }
    #endregion
}