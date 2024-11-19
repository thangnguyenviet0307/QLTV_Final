using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.Xml.Linq;
using QLTV.DTO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace QLTV.DAO
{
    public class LibraryDAO
    {

        private static LibraryDAO instance;

        public static LibraryDAO Instance
        {
            get { if (instance == null) instance = new LibraryDAO(); return LibraryDAO.instance; }
            private set { LibraryDAO.instance = value; }
        }

        private LibraryDAO() { }

        public DataTable GetListFile(string upLoader)
        {
            string query = "SELECT FileName, FileType, Uploader, UploadTime FROM dbo.FileOrProject WHERE UpLoader = @Uploader";

            return DataProvider.Instance.ExecuteQuery(query, new object[] { upLoader });
        }


        public DataTable SearchFileByName(string fileName)
        {
            string query = "USP_SearchByName @fileName";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { fileName });
        }

        public bool InsertLibrary(string fileName, string fileType, string upLoader, DateTime upLoadDate, byte[] fileData)
        {
            string query = "USP_ImportFileOrProject @FileName , @FileType , @UpLoader , @FileData";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { fileName, fileType, upLoader, fileData });

            return result > 0;
        }

        public bool DeleteLibrary(string fileName, string upLoader)
        {
            string query = "USP_DeleteFileByUploader @UpLoader , @FileName";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { upLoader, fileName });

            return result > 0;
        }

        public byte[] GetFileDataByUploader(string uploader, string fileName, string fileType)
        {
            string query = "USP_GetFileDataByUploader @Uploader , @FileName , @FileType";

            byte[] result = (byte[])DataProvider.Instance.ExecuteScalar(query, new object[] { uploader, fileName, fileType });

            return result;
        }

        public bool UpdateLibrary(string originalFileName, string fileName, string fileType, string upLoader, DateTime upLoadDate, byte[] fileData)
        {
            string query = "EXEC USP_UpdateFileConvertByUploader @OriginalFileName , @FileName , @FileType , @UpLoader , @UpLoadDate , @FileData";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { originalFileName, fileName, fileType, upLoader, upLoadDate, fileData });

            return result > 0;
        }

        public string GetFileNameByUploader(string uploader, string fileName)
        {
            string query = "USP_GetFileNameByUploader @FileName , @upLoader";

            string result = (string)DataProvider.Instance.ExecuteScalar(query, new object[] { fileName, uploader });

            return result;
        }

        public bool CheckFileIfExists(string fileName, string fileType, string upLoader)
        {
            string query = "USP_CheckFileIfExists @fileName , @fileType , @upLoader";
            int result = (int)DataProvider.Instance.ExecuteScalar(query, new object[] { fileName, fileType, upLoader });
            return result > 0;
        }

        public DataTable GetListFileByUploaderAndFileName(string uploader, string fileName, int page)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListFileByUploaderAndFileName @uploader , @fileName , @page", new object[] { uploader, fileName, page });
        }

        public int GetTotalFilePagesByUploaderAndFileName(string uploader, string fileName)
        {
            string query = "EXEC USP_GetTotalPagesByUploaderAndFileName @uploader , @fileName";
            object result = DataProvider.Instance.ExecuteScalar(query, new object[] { uploader, fileName });
            return result != null ? Convert.ToInt32(result) : 1;
        }
    }
}
