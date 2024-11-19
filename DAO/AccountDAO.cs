using QLTV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLTV.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });

            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @userName , @displayName , @passWord , @newPassWord", new object[] { userName, displayName, pass, newPass });

            return result > 0;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT ID , UserName , StudentID , DisplayName , Type FROM dbo.Account ORDER BY ID ASC");
        }

        public DataTable GetListAccountByPage(int page)
        {
            return DataProvider.Instance.ExecuteQuery("USP_GetPagedAccounts @page", new object[] { page });
        }

        public int GetTotalPages()
        {
            string query = "EXEC USP_GetTotalPages"; // Không cần tham số ID nữa
            object result = DataProvider.Instance.ExecuteScalar(query);

            // Nếu result có giá trị, chuyển đổi sang int; nếu không, trả về 1
            return result != null ? Convert.ToInt32(result) : 1;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Account WHERE UserName = '" + userName + "' ORDER BY ID ASC");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public bool IsStudentIDExists(string studentID, int currentID)
        {
            string query = "SELECT COUNT(*) FROM dbo.Account WHERE StudentID = @studentID AND ID <> @currentID";
            int count = (int)DataProvider.Instance.ExecuteScalar(query, new object[] { studentID, currentID });
            return count > 0;
        }

        public bool IsUserNameExists(string userName, int currentID)
        {
            string query = "SELECT COUNT(*) FROM dbo.Account WHERE UserName = @userName AND ID <> @currentID";
            int count = (int)DataProvider.Instance.ExecuteScalar(query, new object[] { userName, currentID });
            return count > 0;
        }

        public bool IsDisplayNameExists(string displayName, int currentID)
        {
            string query = "SELECT COUNT(*) FROM dbo.Account WHERE DisplayName = @displayName AND ID <> @currentID";
            object result = DataProvider.Instance.ExecuteScalar(query, new object[] { displayName, currentID });
            return Convert.ToInt32(result) > 0;
        }

        public DataTable SearchAccountByStudentID(string studentID)
        {
            string query = "EXEC USP_SearchAccountByStudentID @studentID";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { studentID });
        }

        public bool InsertAccount(string name, string studentID, string displayName, int type)
        {
            string query = string.Format(@"
                IF NOT EXISTS (
                    SELECT 1 
                    FROM dbo.Account 
                    WHERE UserName = N'{0}' OR StudentID = {1} OR DisplayName = N'{2}'
                )
                BEGIN
                    INSERT INTO dbo.Account (UserName, StudentID, DisplayName, Type)
                    VALUES (N'{0}', {1}, N'{2}', {3})
                END",
                name, studentID, displayName, type);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccountByID(int id, string userName, string studentID, string displayName, int type)
        {
            string query = "UPDATE dbo.Account SET UserName = @userName , StudentID = @studentID , DisplayName = @displayName , Type = @type WHERE ID = @id";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, studentID, displayName, type, id });

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("DELETE dbo.Account WHERE UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)
        {
            string query = string.Format("UPDATE dbo.Account SET PassWord = N'123456' WHERE UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
