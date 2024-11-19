using QLTV.DAO;
using QLTV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class fInformation : Form
    {
        private string connectionSTR = "Data Source=DESKTOP-0CRD70D\\MSSQLSERVER_1;Initial Catalog=QLTV2;Integrated Security=True";
        SqlConnection connection;

        public fInformation(string studentID, string displayName)
        {
            InitializeComponent();
            this.Load += fInformation_Load;
            // Đường dẫn tới file ảnh nền
            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);

            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            connection = new SqlConnection(connectionSTR);
            this.lblStudentID.Text = studentID;
            this.lblFullName.Text = displayName;
            LoadAccountInfo(studentID);
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fInformation_Load(object sender, EventArgs e)
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
        private void fInformation_Resize(object sender, EventArgs e)
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

        private void LoadAccountInfo(string studentID)
        {
            try
            {
                connection.Open();
                string query = "USP_GetAccountInfoByStudentID @StudentID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", studentID);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblFullName.Text = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "";
                        lblStudentID.Text = reader["StudentID"] != DBNull.Value ? reader["StudentID"].ToString() : "";
                        lblClass.Text = reader["ClassRoom"] != DBNull.Value ? reader["ClassRoom"].ToString() : "";
                        lblFaculty.Text = reader["Faculty"] != DBNull.Value ? reader["Faculty"].ToString() : "";
                        lblDateOfBirth.Text = reader["DateOfBirth"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd/MM/yyyy") : "";
                        lblEmail.Text = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                        lblPhoneNumber.Text = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "";

                        // Kiểm tra nếu có dữ liệu ảnh
                        if (reader["ProfilePicture"] != DBNull.Value)
                        {
                            byte[] img = (byte[])reader["ProfilePicture"];
                            try
                            {
                                using (MemoryStream ms = new MemoryStream(img))
                                {
                                    ptbImage.Image = Image.FromStream(ms);
                                    ptbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                                }
                            }
                            catch
                            {
                                // Hiển thị hình ảnh mặc định nếu dữ liệu ảnh không hợp lệ
                                ptbImage.Image = Properties.Resources.NoImage;
                                ptbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                        else
                        {
                            // Hiển thị hình ảnh mặc định nếu không có dữ liệu ảnh
                            ptbImage.Image = Properties.Resources.NoImage;
                            ptbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                    else
                    {
                        // Nếu không tìm thấy dữ liệu sinh viên, hiển thị hình ảnh mặc định
                        ptbImage.Image = Properties.Resources.NoImage;
                        ptbImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
        #endregion
    }
}
