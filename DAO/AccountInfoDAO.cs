using QLTV.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLTV.DAO
{
    public class AccountInfoDAO
    {
        private static AccountInfoDAO instance;

        public static AccountInfoDAO Instance
        {
            get { if (instance == null) instance = new AccountInfoDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountInfoDAO() { }

        //public bool UploadProfilePicture(string studentID, byte[] profilePicture)
        //{
        //    string query = "EXEC USP_ImportImage @StudentID , @ProfilePicture";

        //    int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { studentID, profilePicture });

        //    return result > 0;
        //}

    }
}
