using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV.DTO
{
    public class Account
    {
        public Account(int id, string userName, string studentID, string displayName, int type, string password = null)
        {   
            this.ID = id;
            this.UserName = userName;
            this.StudentID = studentID;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        public Account(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.UserName = row["UserName"].ToString();
            this.StudentID = row["StudentID"].ToString();
            this.DisplayName = row["DisplayName"].ToString();
            this.Type = (int)row["Type"];
            this.Password = row["PassWord"].ToString();
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string studentID;

        public string StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

    }
}
