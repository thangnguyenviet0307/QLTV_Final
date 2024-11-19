using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTV.DTO
{
    public class Library
    {
        public Library(int idFile, string fileName, string fileType, string upLoader, DateTime upLoadDate, byte[] fileData)
        {
            this.IdFile = idFile;
            this.FileName = fileName;
            this.FileType = fileType;
            this.UpLoader = upLoader;
            this.UpLoadDate = upLoadDate;
            this.FileData = fileData;
        }

        public Library(DataRow row)
        {
            this.IdFile = (int)row["idFile"];
            this.FileName = row["FileName"].ToString();
            this.FileType = row["FileType"].ToString();
            this.UpLoader = row["UpLoader"].ToString();
            this.UpLoadDate = (DateTime)row["UploadTime"];
            this.FileData = (byte[])row["FileData"];
        }

        private int idFile;
        public int IdFile
        {
            get { return idFile; }
            set { idFile = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string fileType;
        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
        }
        private string upLoader;
        public string UpLoader
        {
            get { return upLoader; }
            set { upLoader = value; }
        }
        private DateTime upLoadDate;
        public DateTime UpLoadDate
        {
            get { return upLoadDate; }
            set { upLoadDate = value; }
        }
        private byte[] fileData;
        public byte[] FileData
        {
            get { return fileData; }
            set { fileData = value; }
        }
    }
}

