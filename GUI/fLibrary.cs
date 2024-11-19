using System;
using QLTV.DAO;
using QLTV.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using Aspose.Words;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Word.Application;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace QLTV
{
    public partial class fLibrary : Form
    {
        public Account LoginAccount;

        public fLibrary()
        {
            InitializeComponent();
            this.Load += fLibrary_Load;
            this.Resize += fLibrary_Resize;
            Bitmap originalImage = Properties.Resources.KhuS;
            Bitmap blurredImage = ApplyGaussianBlur(originalImage);
            panel1.BackgroundImage = blurredImage;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            this.AcceptButton = btnSearch;
        }

        #region resize
        // Các biến toàn cục
        private Size initialNormalSize; // Kích thước ban đầu ở trạng thái Normal
        private Dictionary<Control, System.Drawing.Rectangle> initialControlBounds; // Lưu vị trí và kích thước ban đầu
        private Dictionary<Control, float> initialFontSizes; // Lưu kích thước font chữ ban đầu
        private bool isMaximized = false; // Trạng thái của cửa sổ

        // Khi form load, lưu trạng thái ban đầu
        private void fLibrary_Load(object sender, EventArgs e)
        {
            initialNormalSize = this.ClientSize; // Lưu kích thước form ban đầu
            initialControlBounds = new Dictionary<Control, System.Drawing.Rectangle>();
            initialFontSizes = new Dictionary<Control, float>();

            SaveInitialBounds(this); // Lưu vị trí và kích thước ban đầu
            EnableDoubleBuffering(this); // Kích hoạt DoubleBuffered để giảm nhấp nháy

            LoadFileList();
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
                System.Drawing.Rectangle initialBounds = entry.Value;

                // Tính toán vị trí và kích thước mới
                ctrl.Bounds = new System.Drawing.Rectangle(
                    (int)(initialBounds.Left * widthRatio),
                    (int)(initialBounds.Top * heightRatio),
                    (int)(initialBounds.Width * widthRatio),
                    (int)(initialBounds.Height * heightRatio)
                );

                // Điều chỉnh font chữ nếu control hỗ trợ
                if (    ctrl is System.Reflection.Emit.Label || ctrl is System.Windows.Forms.Button || ctrl is System.Windows.Forms.TextBox)
                {
                    if (initialFontSizes.ContainsKey(ctrl))
                    {
                        float newFontSize = initialFontSizes[ctrl] * Math.Min(widthRatio, heightRatio);
                        ctrl.Font = new System.Drawing.Font(ctrl.Font.FontFamily, newFontSize, ctrl.Font.Style);
                    }
                }
            }
        }

        // Xử lý sự kiện Resize
        private void fLibrary_Resize(object sender, EventArgs e)
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
            else if (this.WindowState == FormWindowState.Minimized)
            {
                // Không cần thay đổi gì khi form Minimized
                return;
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
                // Sử dụng Graphics để vẽ mờ
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Tạo Rectangle để áp dụng độ mờ
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.White)), new System.Drawing.Rectangle(0, 0, image.Width, image.Height));
            }

            return image;
        }

        private void LoadFileList()
        {
            try
            {
                string upLoader = LoginAccount.UserName; // Lấy tên người dùng
                int currentPage = Convert.ToInt32(txbPageFile.Text);

                string fileName = GetFileName();
                System.Data.DataTable data = LibraryDAO.Instance.GetListFileByUploaderAndFileName(upLoader, fileName, currentPage);

                if (data.Rows.Count > 0)
                {
                    dtgvLibrary.DataSource = data; // Gán dữ liệu vào `DataGridView`
                }
                dtgvLibrary.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file list: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Xử lý ngoại lệ
            }
        }


        private void UploadFileOrProject(string fileName, string fileType, string upLoader, DateTime upLoadDate, byte[] fileData)
        {
            if (LibraryDAO.Instance.InsertLibrary(fileName, fileType, upLoader, upLoadDate, fileData))
            {
                MessageBox.Show("File uploaded successfully.");
                LoadFileList();
            }
            else
            {
                MessageBox.Show("Failed to upload the file.");
            }
        }

        private void DeleteFileOrProject(string fileName, string upLoader)
        {
            if (LibraryDAO.Instance.DeleteLibrary(fileName, upLoader))
            {
                MessageBox.Show("File deleted successfully.");
                LoadFileList();
            }
            else
            {
                MessageBox.Show("Failed to delete the file.");
            }
        }

        private string ConvertFileToPDF(string filename, string filetype, string uploader, byte[] filedata)
        {
            if (filetype == ".doc" || filetype == ".docx")
            {
                using (MemoryStream inputStream = new MemoryStream(filedata))
                {
                    Aspose.Words.Document doc = new Aspose.Words.Document(inputStream);
                    string pdfFileName = Path.GetFileNameWithoutExtension(filename) + ".pdf";
                    string pdfFilePath = Path.Combine(Path.GetTempPath(), pdfFileName);

                    doc.Save(pdfFilePath, SaveFormat.Pdf);
                    return pdfFileName;
                }
            }
            else
            {
                throw new InvalidOperationException("File is not in Word format (.doc or .docx).");
            }
        }

        public void DownloadFile(string filename, string uploader, string savePath)
        {
            string connectionString = "Data Source=DESKTOP-0CRD70D\\MSSQLSERVER_1;Initial Catalog=QLTV2;Integrated Security=True";
            string query = "EXEC USP_GetFileNameByUploader @Uploader , @FileName";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FileName", filename);
                        command.Parameters.AddWithValue("@upLoader", uploader);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("FileData")))
                            {
                                byte[] fileData = (byte[])reader["FileData"];
                                File.WriteAllBytes(savePath, fileData);
                                MessageBox.Show("File downloaded successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to download the file. File not found or data is null.");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error downloading file: " + ex.Message);
            }
        }

        private void ShowWordContent(byte[] fileData, string fileName)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(tempFilePath, fileData);

            Application wordApp = new Application();
            Microsoft.Office.Interop.Word.Document doc = null;

            try
            {
                doc = wordApp.Documents.Open(tempFilePath, ReadOnly: true, Visible: true);
                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Word file: {ex.Message}");
            }
            finally
            {
                if (doc != null)
                {
                    Marshal.ReleaseComObject(doc);
                }
                if (wordApp != null)
                {
                    Marshal.ReleaseComObject(wordApp);
                }
            }
        }

        private void ShowPDFContent(byte[] fileData, string fileName)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(tempFilePath, fileData);

            try
            {
                Process pdfProcess = new Process();
                pdfProcess.StartInfo = new ProcessStartInfo
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                };
                pdfProcess.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF file: {ex.Message}");
            }
        }

        private void ShowExcelContent(byte[] fileData, string extension)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "tempExcelFile" + extension);
            File.WriteAllBytes(tempFilePath, fileData);

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = null;

            try
            {
                workbook = excelApp.Workbooks.Open(tempFilePath, ReadOnly: true);
                excelApp.Visible = true;

                excelApp.DisplayAlerts = false;
                workbook.ReadOnlyRecommended = true;
                workbook.Protect("password", Structure: true, Windows: true);
            }
            catch (COMException ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error opening Excel file: {ex.Message}");
            }
            finally
            {
                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        private void ShowCSVContent(byte[] fileData)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "tempCsvFile.csv");
            File.WriteAllBytes(tempFilePath, fileData);

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = null;

            try
            {
                workbook = excelApp.Workbooks.Open(tempFilePath, ReadOnly: true);
                excelApp.Visible = true;

                excelApp.DisplayAlerts = false;
                workbook.ReadOnlyRecommended = true;
                workbook.Protect("password", Structure: true, Windows: true);
            }
            catch (COMException ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error opening CSV file: {ex.Message}");
            }
            finally
            {
                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }
                if (excelApp != null)
                {
                    Marshal.ReleaseComObject(excelApp);
                }
            }
        }

        private void ShowImageContent(byte[] fileData, string fileName)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(tempFilePath, fileData);

            try
            {
                PictureBox pictureBox = new PictureBox
                {
                    Image = Image.FromFile(tempFilePath),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Dock = DockStyle.Fill
                };
                Form form = new Form { Text = "Image", Width = 800, Height = 600 };
                form.Controls.Add(pictureBox);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening image: {ex.Message}");
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            string searchText = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadFileList();
            }
        }

        private void txbPageFile_TextChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem `txbPageFile` có phải là số hợp lệ không
            if (int.TryParse(txbPageFile.Text, out int page))
            {
                string fileName = GetFileName();
                int totalPage = LibraryDAO.Instance.GetTotalFilePagesByUploaderAndFileName(LoginAccount.UserName, fileName);

                // Chỉ tải trang khi `page` nằm trong khoảng hợp lệ
                if (page >= 1 && page <= totalPage)
                {
                    LoadPage(page, fileName);
                }
                else
                {
                    MessageBox.Show("Page number is out of range.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txbPageFile.Text = "1"; // Đưa về trang đầu tiên nếu số trang không hợp lệ
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid page number.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbPageFile.Text = "1"; // Đưa về trang đầu tiên nếu không phải là số hợp lệ
            }
        }

        private void LoadPage(int page, string fileName)
        {
            try
            {
                string uploader = LoginAccount.UserName;
                var data = LibraryDAO.Instance.GetListFileByUploaderAndFileName(uploader, fileName, page);
                if (data.Rows.Count > 0)
                {
                    dtgvLibrary.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading page data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFileName()
        {
            // Kiểm tra xem cột FileName có tồn tại không và lấy giá trị của nó
            if (dtgvLibrary.Rows.Count > 0 && dtgvLibrary.Columns.Contains("File Name"))
            {
                return dtgvLibrary.Rows[0].Cells["File Name"].Value.ToString();
            }
            return ""; // Trả về chuỗi rỗng nếu không có dữ liệu
        }

        void UpdatePaginationButtons()
        {
            int currentPage = Convert.ToInt32(txbPageFile.Text);
            string fileName = GetFileName();
            int totalPage = LibraryDAO.Instance.GetTotalFilePagesByUploaderAndFileName(LoginAccount.UserName, fileName);
            btnFirst.Enabled = currentPage > 1;
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPage;
            btnLast.Enabled = currentPage < totalPage;
        }
        #endregion

        #region events
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All files (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string[] validExtensions = { ".doc", ".docx", ".xls", ".xlsx", ".csv", ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                        string fileExtension = Path.GetExtension(openFileDialog.FileName).ToLower();

                        if (Array.Exists(validExtensions, ext => ext == fileExtension))
                        {
                            byte[] fileData = File.ReadAllBytes(openFileDialog.FileName);
                            string fileName = Path.GetFileName(openFileDialog.FileName);
                            string fileType = fileExtension;
                            string upLoader = LoginAccount.UserName;
                            DateTime uploadDate = DateTime.Now;

                            if (LibraryDAO.Instance.CheckFileIfExists(fileName, fileType, upLoader))
                            {
                                MessageBox.Show("File already exists in the library. Please rename the file and try again.");
                                return;
                            }

                            UploadFileOrProject(fileName, fileType, upLoader, uploadDate, fileData);
                        }
                        else
                        {
                            MessageBox.Show("Invalid file type. Please select a valid file format.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (dtgvLibrary.SelectedRows.Count > 0)
            {
                string fileName = dtgvLibrary.SelectedRows[0].Cells["FileName"].Value.ToString();
                string fileType = dtgvLibrary.SelectedRows[0].Cells["FileType"].Value.ToString();
                string uploader = dtgvLibrary.SelectedRows[0].Cells["Uploader"].Value.ToString();
                string FileNameDownload = Path.GetFileNameWithoutExtension(fileName);

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "All files (*.*)|*.*";
                    saveFileDialog.FileName = FileNameDownload + fileType;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to download this file?",
                                                              "Confirm Download",
                                                              MessageBoxButtons.YesNo,
                                                              MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            DownloadFile(fileName, uploader, saveFileDialog.FileName);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row in the table to download the file.");
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtgvLibrary.SelectedRows.Count > 0)
                {
                    var selectedRow = dtgvLibrary.SelectedRows[0];
                    string originalFileName = selectedRow.Cells["FileName"].Value.ToString();
                    string fileType = selectedRow.Cells["FileType"].Value.ToString();
                    string upLoader = LoginAccount.UserName;

                    byte[] wordData = LibraryDAO.Instance.GetFileDataByUploader(upLoader, originalFileName, fileType);
                    if (wordData == null)
                    {
                        MessageBox.Show("Cannot load the file from the database.");
                        return;
                    }

                    string pdfFileName = ConvertFileToPDF(originalFileName, fileType, upLoader, wordData);
                    string pdfFilePath = Path.Combine(Path.GetTempPath(), pdfFileName);

                    byte[] pdfData = File.ReadAllBytes(pdfFilePath);

                    if (LibraryDAO.Instance.UpdateLibrary(originalFileName, pdfFileName, ".pdf", upLoader, DateTime.Now, pdfData))
                    {
                        selectedRow.Cells["FileName"].Value = pdfFileName;
                        selectedRow.Cells["FileType"].Value = ".pdf";
                        MessageBox.Show("File converted successfully.");
                        LoadFileList();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update the file.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a file to convert.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dtgvLibrary.SelectedRows.Count > 0)
            {
                string fileName = dtgvLibrary.SelectedRows[0].Cells["FileName"].Value.ToString();
                string upLoader = LoginAccount.UserName;

                DialogResult result = MessageBox.Show($"Are you sure you want to delete '{fileName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    DeleteFileOrProject(fileName, upLoader);
                }
            }
            else
            {
                MessageBox.Show("Please select a file to delete.");
            }
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (dtgvLibrary.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow selectedRow in dtgvLibrary.SelectedRows)
                {
                    string fileName = selectedRow.Cells["FileName"].Value.ToString();
                    string fileType = selectedRow.Cells["FileType"].Value.ToString().ToLower();
                    string uploader = selectedRow.Cells["Uploader"].Value.ToString();

                    byte[] fileData = LibraryDAO.Instance.GetFileDataByUploader(uploader, fileName, fileType);
                    if (fileData == null)
                    {
                        MessageBox.Show($"Cannot load file content for {fileName} from the database.");
                        continue;
                    }

                    if (fileType == ".doc" || fileType == ".docx")
                    {
                        ShowWordContent(fileData, fileName);
                    }
                    else if (fileType == ".pdf")
                    {
                        ShowPDFContent(fileData, fileName);
                    }
                    else if (fileType == ".xls" || fileType == ".xlsx")
                    {
                        ShowExcelContent(fileData, fileType);
                    }
                    else if (fileType == ".csv")
                    {
                        ShowCSVContent(fileData);
                    }
                    else if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png" || fileType == ".gif" || fileType == ".bmp")
                    {
                        ShowImageContent(fileData, fileName);
                    }
                    else
                    {
                        MessageBox.Show($"File type {fileType} is not supported.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select at least one file to display.");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a keyword to search.");
                LoadFileList();
                return;
            }

            System.Data.DataTable searchResults = LibraryDAO.Instance.SearchFileByName(searchText);

            if (searchResults == null || searchResults.Rows.Count == 0)
            {
                MessageBox.Show("No matching results found.");
                dtgvLibrary.DataSource = null;
                LoadFileList();
            }
            else
            {
                dtgvLibrary.DataSource = searchResults;

                if (dtgvLibrary.Columns.Contains("FileData"))
                {
                    dtgvLibrary.Columns["FileData"].Visible = false;
                }
            }

            dtgvLibrary.Refresh();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txbPageFile.Text);
            string fileName = GetFileName();
            int totalPage = LibraryDAO.Instance.GetTotalFilePagesByUploaderAndFileName(LoginAccount.UserName, fileName);
            if (currentPage < totalPage)
            {
                currentPage++;
                txbPageFile.Text = currentPage.ToString();
                LoadPage(currentPage, fileName);
                UpdatePaginationButtons();
            }
            else
            {
                MessageBox.Show("You are already on the last page.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txbPageFile.Text);
            if (currentPage > 1)
            {
                currentPage--;
                txbPageFile.Text = currentPage.ToString();
                LoadPage(currentPage, GetFileName());
                UpdatePaginationButtons();
            }
            else
            {
                MessageBox.Show("You are already on the first page.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            string fileName = GetFileName();
            int totalPage = LibraryDAO.Instance.GetTotalFilePagesByUploaderAndFileName(LoginAccount.UserName, fileName);
            txbPageFile.Text = totalPage.ToString();
            LoadPage(totalPage, fileName);
            UpdatePaginationButtons();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            txbPageFile.Text = "1";
            LoadPage(1, GetFileName());
            UpdatePaginationButtons();
        }
        #endregion
    }
}
