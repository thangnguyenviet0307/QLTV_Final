using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLTV.DTO
{
    public class AccountInfo
    {
        public AccountInfo(int iD, string fullName, string studentID, string classRoom, string faculty, DateTime? dateOfBirth, string email, string phoneNumber, byte[] profilePicture)
        {
            this.ID = iD;
            this.StudentID = studentID;
            this.FullName = fullName;
            this.ClassRoom = classRoom;
            this.Faculty = faculty;
            this.DateOfBirth = dateOfBirth;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.ProfilePicture = profilePicture;
        }

        public AccountInfo(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.FullName = row["FullName"].ToString();
            this.StudentID = row["StudentID"].ToString();
            this.ClassRoom = row["ClassRoom"].ToString();
            this.Faculty = row["Faculty"].ToString();
            this.DateOfBirth = (DateTime?)row["DateOfBirth"];
            this.Email = row["Email"].ToString();
            this.PhoneNumber = row["PhoneNumber"].ToString();
            this.ProfilePicture = (byte[])row["ProfilePicture"];
        }

        public int iD;
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string studentID;
        public string StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        private string fullName;
        public string FullName 
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string classRoom;
        public string ClassRoom
        {
            get { return classRoom; }
            set { classRoom = value; }
        }

        private string faculty;
        public string Faculty
        {
            get { return faculty; }
            set { faculty = value; }
        }

        private DateTime? dateOfBirth;
        public DateTime? DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private byte[] profilePicture;
        public byte[] ProfilePicture
        {
            get { return profilePicture; }
            set { profilePicture = value; }
        }
    }
}
