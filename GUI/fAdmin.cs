using QLTV.DAO;
using QLTV.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTV
{
    public partial class fAdmin : Form
    {
        BindingSource accountList = new BindingSource();
        public Account LoginAccount;
        public fAdmin()
        {
            InitializeComponent();
            this.Load += fAdmin_Load;
            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            LoadAccounts(1);
        }
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

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fAdmin_Load(object sender, EventArgs e)
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
                        if (Math.Abs(ctrl.Font.Size - newFontSize) > 0.01f) // Chỉ thay đổi khi kích thước khác biệt đáng kể
                        {
                            ctrl.Font = new Font(ctrl.Font.FontFamily, newFontSize, ctrl.Font.Style);
                        }
                    }
                }

                // Xử lý riêng cho NumericUpDown
                if (ctrl is NumericUpDown numericUpDown)
                {
                    AdjustNumericUpDown(numericUpDown, widthRatio, heightRatio);
                }
            }
        }

        private void AdjustNumericUpDown(NumericUpDown numericUpDown, float widthRatio, float heightRatio)
        {
            // Kiểm tra nếu kích thước đã được cập nhật trước đó để tránh tăng kích thước nhiều lần
            Control upDownButtons = numericUpDown.Controls[0];
            if (upDownButtons != null)
            {
                int newWidth = (int)(20 * Math.Min(widthRatio, heightRatio));
                if (upDownButtons.Width != newWidth) // Chỉ thay đổi khi kích thước khác biệt đáng kể
                {
                    upDownButtons.Width = newWidth;
                }
            }

            // Điều chỉnh Font chữ của NumericUpDown chỉ khi cần thiết
            float newFontSize = initialFontSizes[numericUpDown] * Math.Min(widthRatio, heightRatio);
            if (Math.Abs(numericUpDown.Font.Size - newFontSize) > 0.01f) // Chỉ thay đổi khi kích thước khác biệt đáng kể
            {
                numericUpDown.Font = new Font(numericUpDown.Font.FontFamily, newFontSize, numericUpDown.Font.Style);
            }
        }

        // Xử lý sự kiện Resize
        private void fAdmin_Resize(object sender, EventArgs e)
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
        void LoadAccounts(int page = 1)
        {
            try
            {
                
                DataTable data;

                if (page > 1)
                {
                    // Lấy dữ liệu cho trang cụ thể
                    data = AccountDAO.Instance.GetListAccountByPage(page);
                }
                else
                {
                    // Luôn phân trang từ trang đầu tiên
                    data = AccountDAO.Instance.GetListAccountByPage(1);
                }

                if (data != null && data.Rows.Count > 0)
                {
                    accountList.DataSource = data;
                    dtgvAccount.DataSource = accountList;
                    ClearAccountBindings();
                    AddAccountBinding();
                }
                else
                {
                    MessageBox.Show("No data found.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    accountList.DataSource = null;
                    dtgvAccount.DataSource = null;
                }

                // Đảm bảo `txbPageAccount` luôn đặt đúng số trang
                txbPageAccount.Text = page.ToString();

                // Cập nhật nút phân trang
                UpdatePaginationButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            txbStudentID.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "StudentID", true, DataSourceUpdateMode.Never));
            numericUpDown.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void ClearAccountBindings()
        {
            txbUserName.DataBindings.Clear();
            txbDisplayName.DataBindings.Clear();
            txbStudentID.DataBindings.Clear();
            numericUpDown.DataBindings.Clear();
        }

        void AddAccount(int id, string userName, string studentID, string displayName, int type)
        {
            try
            {
                // Kiểm tra nếu UserName đã tồn tại và không phải tài khoản đang được chỉnh sửa
                if (AccountDAO.Instance.IsUserNameExists(userName, id))
                {
                    MessageBox.Show("UserName already exists. Cannot edit account.");
                    return;
                }

                // Kiểm tra nếu StudentID đã tồn tại và không phải tài khoản đang được chỉnh sửa
                if (AccountDAO.Instance.IsStudentIDExists(studentID, id))
                {
                    MessageBox.Show("Student ID already exists. Cannot edit account.");
                    return;
                }

                // Kiểm tra nếu DisplayName đã tồn tại và không phải tài khoản đang được chỉnh sửa
                if (AccountDAO.Instance.IsDisplayNameExists(displayName, id))
                {
                    MessageBox.Show("Display Name already exists. Cannot edit account.");
                    return;
                }


                // Nếu tất cả điều kiện đều thỏa mãn, tiến hành thêm tài khoản
                if (AccountDAO.Instance.InsertAccount(userName, studentID, displayName, type))
                {
                    MessageBox.Show("Add account successfully");
                }
                else
                {
                    MessageBox.Show("Add account failed");
                }

                // Tải lại dữ liệu từ trang đầu tiên
                LoadAccounts(1);
            }
            catch (Exception ex) {
                MessageBox.Show("Error adding account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void EditAccount(int id, string userName, string studentID, string displayName, int type)
        {
            // Kiểm tra nếu UserName đã tồn tại và không phải tài khoản đang được chỉnh sửa
            if (AccountDAO.Instance.IsUserNameExists(userName, id))
            {
                MessageBox.Show("UserName already exists. Cannot edit account.");
                return;
            }

            // Kiểm tra nếu StudentID đã tồn tại và không phải tài khoản đang được chỉnh sửa
            if (AccountDAO.Instance.IsStudentIDExists(studentID, id))
            {
                MessageBox.Show("Student ID already exists. Cannot edit account.");
                return;
            }

            // Kiểm tra nếu DisplayName đã tồn tại và không phải tài khoản đang được chỉnh sửa
            if (AccountDAO.Instance.IsDisplayNameExists(displayName, id))
            {
                MessageBox.Show("Display Name already exists. Cannot edit account.");
                return;
            }

            // Nếu tất cả điều kiện đều thỏa mãn, tiến hành cập nhật tài khoản
            if (AccountDAO.Instance.UpdateAccountByID(id, userName, studentID, displayName, type))
            {
                MessageBox.Show("Edit account successfully");
            }
            else
            {
                MessageBox.Show("Edit account failed");
            }

            // Tải lại dữ liệu từ trang đầu tiên
            LoadAccounts(1);
        }

        void DeleteAccount(string userName)
        {
            if (LoginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("You can not delete your own account");
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Delete account successfully");
            }
            else
            {
                MessageBox.Show("Delete account failed");
            }

            LoadAccounts(1); // Tải lại dữ liệu từ trang đầu tiên
        }


        void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Reset password successfully");
            }
            else
            {
                MessageBox.Show("Reset password failed");
            }
        }

        private void txbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txbSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadAccounts(1);
            }
        }

        private void txbPageAccount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txbPageAccount.Text, out int page))
            {
                int totalPage = AccountDAO.Instance.GetTotalPages();

                if (page >= 1 && page <= totalPage)
                {
                    LoadAccounts(page);
                }
                else
                {
                    MessageBox.Show("Page number is out of range.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txbPageAccount.Text = "1";
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid page number.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPageAccount.Text = "1";
            }
        }

        void UpdatePaginationButtons()
        {
            int currentPage = Convert.ToInt32(txbPageAccount.Text);
            int totalPage = AccountDAO.Instance.GetTotalPages();

            btnFirst.Enabled = currentPage > 1;
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPage;
            btnLast.Enabled = currentPage < totalPage;
        }
        #endregion

        #region events
        private void btnAdd_Click(object sender, EventArgs e)
        {
                // Lấy ID từ dòng đã chọn trong DataGridView
                int id = LoginAccount.ID;
                string userName = txbUserName.Text;
                string studentID = txbStudentID.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)numericUpDown.Value;

                AddAccount(id, userName, studentID, displayName, type);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dtgvAccount.SelectedRows.Count > 0) // Kiểm tra xem có dòng nào được chọn không
            {
                // Lấy ID từ dòng đã chọn trong DataGridView
                int id = Convert.ToInt32(dtgvAccount.SelectedRows[0].Cells["ID"].Value); // "ID" là tên cột chứa ID

                string userName = txbUserName.Text;
                string studentID = txbStudentID.Text;
                string displayName = txbDisplayName.Text;
                int type = (int)numericUpDown.Value;

                // Gọi hàm EditAccount với ID và các thông tin khác
                EditAccount(id, userName, studentID, displayName, type);
            }
            else
            {
                MessageBox.Show("Please select an account to edit.");
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPassword(userName);
        }

        private void btnSee_Click(object sender, EventArgs e)
        {
            if (dtgvAccount.SelectedRows.Count > 0)
            {
                string userName = dtgvAccount.SelectedRows[0].Cells["UserName"].Value.ToString();
                string selectedStudentID = dtgvAccount.SelectedRows[0].Cells["StudentID"].Value.ToString();
                string displayName = dtgvAccount.SelectedRows[0].Cells["DisplayName"].Value.ToString();
                int type = (int)dtgvAccount.SelectedRows[0].Cells["Type"].Value;

                fInformation f = new fInformation(selectedStudentID, displayName);
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a row in the table to display information.");
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            txbPageAccount.Text = "1";
            LoadAccounts(1);
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txbPageAccount.Text);

            if (currentPage > 1)
            {
                currentPage--;
                txbPageAccount.Text = currentPage.ToString();
                LoadAccounts(currentPage);
            }
            else
            {
                MessageBox.Show("You are already on the first page.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txbPageAccount.Text);
            int totalPage = AccountDAO.Instance.GetTotalPages();

            if (currentPage < totalPage)
            {
                currentPage++;
                txbPageAccount.Text = currentPage.ToString();
                LoadAccounts(currentPage);
            }
            else
            {
                MessageBox.Show("You are already on the last page.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int totalPage = AccountDAO.Instance.GetTotalPages();
            txbPageAccount.Text = totalPage.ToString();
            LoadAccounts(totalPage);
        }

        private void btnSearchAcc_Click(object sender, EventArgs e)
        {
            string searchText = txbSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a keyword to search.");
                LoadAccounts(1);
                return;
            }

            System.Data.DataTable searchResults = AccountDAO.Instance.SearchAccountByStudentID(searchText);

            // Kiểm tra kết quả tìm kiếm
            if (searchResults == null || searchResults.Rows.Count == 0)
            {
                MessageBox.Show("No matching results found.");
                LoadAccounts(1);
            }
            else
            {
                accountList.DataSource = searchResults;
                dtgvAccount.DataSource = accountList;
                dtgvAccount.Refresh();

                // Xóa tất cả DataBindings và thêm lại để tránh lỗi liên kết nhiều lần
                ClearAccountBindings();
                AddAccountBinding();
            }
        }
        #endregion
    }
}
