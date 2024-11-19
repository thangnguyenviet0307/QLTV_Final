CREATE DATABASE QLTV2
GO
USE QLTV2
GO

-- Tao Bang 
-- Account

CREATE TABLE Account
(
    ID INT PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL UNIQUE,
    StudentID NVARCHAR(100) NOT NULL UNIQUE,
    DisplayName NVARCHAR(100) NOT NULL UNIQUE,
    PassWord NVARCHAR(100) NOT NULL DEFAULT 123456,
    Type INT NOT NULL DEFAULT 0 -- 1: Admin && 0: Sinh viên
);
GO

CREATE TABLE AccountInfo
(
    ID INT PRIMARY KEY,
    FullName NVARCHAR(100),
    StudentID NVARCHAR(100),
    ClassRoom NVARCHAR(100),
    Faculty NVARCHAR(100),
    DateOfBirth DATE,
    Email NVARCHAR(100),
    PhoneNumber NVARCHAR(100),
    ProfilePicture VARBINARY(MAX),
    FOREIGN KEY (StudentID) REFERENCES dbo.Account (StudentID) ON DELETE CASCADE ON UPDATE CASCADE, -- Ràng buộc khóa ngoại với StudentID
);
GO

CREATE PROC	USP_GetAccountByUserName
@userName NVARCHAR(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE	UserName = @userName
END
GO

CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO	

CREATE TRIGGER trg_InsertAccountInfoID
ON dbo.Account 
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewID INT;

    -- Tìm ID nhỏ nhất không tồn tại trong bảng Account
    SELECT @NewID = MIN(T.ID + 1)
    FROM (SELECT 0 AS ID UNION ALL SELECT ID FROM dbo.Account) AS T
    LEFT JOIN dbo.Account AS A ON T.ID + 1 = A.ID
    WHERE A.ID IS NULL;

    -- Nếu không có ID nào trống, gán ID mới là ID lớn nhất + 1
    IF @NewID IS NULL
    BEGIN
        SELECT @NewID = ISNULL(MAX(ID), 0) + 1 FROM dbo.Account;
    END

    -- Chèn dữ liệu vào Account với ID mới
    INSERT INTO dbo.Account (ID, UserName, StudentID, DisplayName, PassWord, Type)
    SELECT @NewID, UserName, StudentID, DisplayName, PassWord, Type
    FROM inserted;

    -- Chèn dữ liệu vào AccountInfo với StudentID và FullName
    INSERT INTO dbo.AccountInfo (ID, StudentID, FullName)
    SELECT @NewID, StudentID, DisplayName
    FROM inserted;
END
GO
	
CREATE TRIGGER trg_DeleteAccountInfo
ON dbo.Account 
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.AccountInfo
    WHERE StudentID IN (SELECT StudentID FROM deleted);
END
GO

CREATE TRIGGER trg_UpdateStudentIDAndFullNameInAccountInfo
ON dbo.Account
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu DisplayName hoặc StudentID đã thay đổi
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deleted d ON d.StudentID = i.StudentID
        WHERE i.DisplayName <> d.DisplayName OR i.StudentID <> d.StudentID
    )
    BEGIN
        -- Vô hiệu hóa trigger trg_UpdateDisplayNameInAccount để tránh vòng lặp
        DISABLE TRIGGER trg_UpdateDisplayNameInAccount ON dbo.AccountInfo;

        -- Cập nhật AccountInfo với thông tin mới
        UPDATE ai
        SET ai.StudentID = i.StudentID,
            ai.FullName = i.DisplayName
        FROM dbo.AccountInfo ai
        INNER JOIN inserted i ON ai.StudentID = i.StudentID;

        -- Bật lại trigger sau khi cập nhật
        ENABLE TRIGGER trg_UpdateDisplayNameInAccount ON dbo.AccountInfo;
    END
END
GO

CREATE TRIGGER trg_UpdateDisplayNameInAccount
ON dbo.AccountInfo
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu FullName đã thay đổi
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN deleted d ON d.StudentID = i.StudentID
        WHERE i.FullName <> d.FullName
    )
    BEGIN
        -- Vô hiệu hóa trigger trg_UpdateStudentIDAndFullNameInAccountInfo để tránh vòng lặp
        DISABLE TRIGGER trg_UpdateStudentIDAndFullNameInAccountInfo ON dbo.Account;

        -- Cập nhật DisplayName trong Account
        UPDATE a
        SET a.DisplayName = i.FullName
        FROM dbo.Account a
        INNER JOIN inserted i ON a.StudentID = i.StudentID;

        -- Bật lại trigger sau khi cập nhật
        ENABLE TRIGGER trg_UpdateStudentIDAndFullNameInAccountInfo ON dbo.Account;
    END
END
GO

CREATE PROC USP_UpdateAccount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @passWord NVARCHAR(100), @newPassWord NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT

	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
	IF (@isRightPass = 1)
	BEGIN
		IF (@newPassWord = NULL OR @newPassWord = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName, PassWord = @newPassWord WHERE UserName = @userName
	END
END
GO

CREATE PROCEDURE USP_UpdateAccountBbyID
    @ID INT,
    @userName NVARCHAR(100),
    @displayName NVARCHAR(100),
    @passWord NVARCHAR(100),
    @newPassWord NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra nếu mật khẩu hiện tại khớp
    IF EXISTS (SELECT 1 FROM dbo.Account WHERE ID = @ID AND PassWord = @passWord)
    BEGIN
        -- Cập nhật DisplayName và mật khẩu mới
        UPDATE dbo.Account
        SET UserName = @userName,
            DisplayName = @displayName,
            PassWord = @newPassWord
        WHERE ID = @ID;
    END
    ELSE
    BEGIN
        RETURN -1;
    END
END
GO

CREATE PROC USP_GetAccountInfoByStudentID
    @StudentID NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
	SELECT * FROM dbo.AccountInfo WHERE StudentID = @StudentID;
	SET NOCOUNT OFF;
END
GO

CREATE PROCEDURE USP_InsertAccountInfo
    @FullName NVARCHAR(100),
    @StudentID NVARCHAR(100),
    @ClassRoom NVARCHAR(100),
    @Faculty NVARCHAR(100),
    @DateOfBirth DATE,
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(100),
	@profilePicture VARBINARY(MAX)
AS
BEGIN
	DECLARE @isExistStudentID INT

	SELECT @isExistStudentID = COUNT(*) FROM dbo.AccountInfo WHERE StudentID = @StudentID
    -- Kiểm tra nếu StudentID đã tồn tại trong bảng AccountInfo
    IF (@isExistStudentID = 1)
    BEGIN
        -- Nếu StudentID đã tồn tại, cập nhật các thông tin còn lại
        UPDATE AccountInfo
        SET 
            FullName = @FullName,
            ClassRoom = @ClassRoom,
            Faculty = @Faculty,
            DateOfBirth = @DateOfBirth,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
			ProfilePicture = @profilePicture
        WHERE StudentID = @StudentID;
    END
    ELSE
    BEGIN
        -- Nếu StudentID chưa tồn tại, chèn dữ liệu mới vào bảng AccountInfo
        INSERT INTO AccountInfo (FullName, StudentID, ClassRoom, Faculty, DateOfBirth, Email, PhoneNumber, ProfilePicture)
        VALUES (@FullName, @StudentID, @ClassRoom, @Faculty, @DateOfBirth, @Email, @PhoneNumber, @profilePicture);
    END
END
GO	

CREATE PROC	USP_SearchAccountByStudentID
@studentID NVARCHAR(100)
AS
BEGIN
	SELECT UserName, StudentID, DisplayName, Type FROM dbo.Account WHERE dbo.fuConvertToUnsign1(StudentID) LIKE N'%' + dbo.fuConvertToUnsign1(@studentID) + '%'
END
GO

CREATE PROC USP_GetPagedAccounts
    @page INT
AS
BEGIN
    DECLARE @pageRows INT = 15;  -- Số hàng trên mỗi trang
    DECLARE @offsetRows INT = (@page - 1) * @pageRows;  -- Số hàng cần bỏ qua dựa trên trang hiện tại

    SELECT ID, UserName, StudentID, DisplayName, Type
    FROM dbo.Account
    ORDER BY ID ASC
    OFFSET @offsetRows ROWS FETCH NEXT @pageRows ROWS ONLY;
END
GO

CREATE PROC USP_GetTotalPages
AS
BEGIN
    DECLARE @pageRows INT = 15;  -- Số hàng trên mỗi trang
    SELECT CEILING(COUNT(*) * 1.0 / @pageRows) AS TotalPages
    FROM dbo.Account;
END
GO

-- File (Word, Excel, PDF)
CREATE TABLE FileOrProject
(
	idFile INT PRIMARY KEY,
	FileName NVARCHAR(255),
	FileType NVARCHAR(50),
	UpLoader NVARCHAR(100),
	UploadTime DATETIME DEFAULT GETDATE(),
	FileData VARBINARY(MAX)
	FOREIGN KEY (UpLoader) REFERENCES dbo.Account (UserName) ON DELETE CASCADE ON UPDATE CASCADE
)
GO	

CREATE TRIGGER trg_InsertUploader
ON dbo.Account 
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewID INT;

    -- Tìm ID nhỏ nhất không tồn tại trong bảng FileOrProject
    SELECT @NewID = MIN(T.ID + 1)
    FROM (SELECT 0 AS ID UNION ALL SELECT idFile FROM dbo.FileOrProject) AS T
    LEFT JOIN dbo.FileOrProject AS A ON T.ID + 1 = A.idFile
    WHERE A.idFile IS NULL;

    -- Nếu không có ID nào trống, gán ID mới là ID lớn nhất + 1
    IF @NewID IS NULL
    BEGIN
        SELECT @NewID = ISNULL(MAX(idFile), 0) + 1 FROM dbo.FileOrProject;
    END

    -- Thêm bản ghi mới vào FileOrProject với ID và UpLoader là UserName từ bảng inserted
    INSERT INTO dbo.FileOrProject (idFile, UpLoader)
    SELECT @NewID, UserName
    FROM inserted;
END
GO

CREATE TRIGGER trg_AfterDelete_Account
ON dbo.Account
AFTER DELETE
AS
BEGIN
    DELETE FROM FileOrProject
    WHERE UpLoader IN (SELECT UserName FROM deleted);
END
GO

CREATE TRIGGER trg_AfterUpdate_Account
ON dbo.Account
AFTER UPDATE
AS
BEGIN
    -- Chỉ thực hiện khi UserName thay đổi
    IF UPDATE(UserName)
    BEGIN
        UPDATE fp
        SET fp.UpLoader = i.UserName
        FROM FileOrProject fp
        JOIN deleted d ON d.UserName = fp.UpLoader
        JOIN inserted i ON i.UserName = d.UserName;  -- Thay đổi ở đây
    END
END
GO

CREATE PROCEDURE USP_ImportFileOrProject
    @FileName NVARCHAR(255),
    @FileType NVARCHAR(50),
    @UpLoader NVARCHAR(100),
    @FileData VARBINARY(MAX)
AS
BEGIN
    DECLARE @NewID INT;

    -- Tìm ID nhỏ nhất không tồn tại trong bảng FileOrProject
    SELECT @NewID = MIN(T.ID + 1)
    FROM (SELECT 0 AS ID UNION ALL SELECT idFile FROM dbo.FileOrProject) AS T
    LEFT JOIN dbo.FileOrProject AS A ON T.ID + 1 = A.idFile
    WHERE A.idFile IS NULL;

    -- Nếu không có ID nào trống, gán ID mới là ID lớn nhất + 1
    IF @NewID IS NULL
    BEGIN
        SELECT @NewID = ISNULL(MAX(idFile), 0) + 1 FROM dbo.FileOrProject;
    END

    -- Kiểm tra nếu đã có bản ghi với UpLoader mà các giá trị còn lại là NULL
    IF EXISTS (SELECT 1 
               FROM dbo.FileOrProject 
               WHERE UpLoader = @UpLoader 
               AND FileName IS NULL 
               AND FileType IS NULL 
               AND FileData IS NULL)
    BEGIN
        -- Cập nhật bản ghi với các giá trị mới
        UPDATE dbo.FileOrProject
        SET FileName = @FileName,
            FileType = @FileType,
            UploadTime = GETDATE(),
            FileData = @FileData
        WHERE UpLoader = @UpLoader 
          AND FileName IS NULL 
          AND FileType IS NULL 
          AND FileData IS NULL;
    END
    ELSE
    BEGIN
        -- Thêm bản ghi mới vào bảng FileOrProject
        INSERT INTO dbo.FileOrProject (idFile, FileName, FileType, UpLoader, UploadTime, FileData)
        VALUES (@NewID, @FileName, @FileType, @UpLoader, GETDATE(), @FileData);
    END
END
GO

CREATE PROCEDURE USP_DeleteFileByUploader
    @UpLoader NVARCHAR(100),
    @FileName NVARCHAR(255) = NULL
AS
BEGIN
    -- Bắt đầu giao dịch
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Xóa bản ghi trong bảng FileOrProject cho uploader
        DELETE FROM dbo.FileOrProject
        WHERE UpLoader = @UpLoader
        AND (@FileName IS NULL OR FileName = @FileName);

        -- Commit giao dịch
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Nếu có lỗi, rollback giao dịch
        ROLLBACK TRANSACTION;

        -- Ném lại lỗi
        THROW;
    END CATCH;
END
GO

CREATE PROCEDURE USP_GetFileDataByUploader
    @Uploader NVARCHAR(100),
    @FileName NVARCHAR(255),
	@FileType NVARCHAR(100)
AS
BEGIN
    -- Đảm bảo không trả về lỗi nghiêm trọng khi có lỗi
    SET NOCOUNT ON;

    -- Lấy dữ liệu file dựa trên Uploader và FileName
    SELECT FileData
    FROM dbo.FileOrProject
    WHERE Uploader = @Uploader AND FileName = @FileName AND	FileType = @FileType;
END
GO

CREATE PROCEDURE USP_GetFileNameByUploader
    @Uploader NVARCHAR(100),
    @FileName NVARCHAR(255)
AS
BEGIN
    SELECT FileName, FileData -- Bao gồm cả FileData
    FROM dbo.FileOrProject
    WHERE Uploader = @Uploader AND FileName = @FileName;
END
GO

CREATE PROCEDURE USP_UpdateFileConvertByUploader
    @OriginalFileName NVARCHAR(255),
    @FileName NVARCHAR(255),
    @FileType NVARCHAR(50),
    @UpLoader NVARCHAR(255),
    @UpLoadDate DATETIME,
    @FileData VARBINARY(MAX)
AS
BEGIN
    -- Declare a variable to store the current file data
    DECLARE @ExistingFileId INT;

    -- Find the existing file record by the original file name and uploader
    SELECT @ExistingFileId = idFile
    FROM dbo.FileOrProject
    WHERE FileName = @OriginalFileName
      AND UpLoader = @UpLoader;

    -- If a record exists, update it
    IF @ExistingFileId IS NOT NULL
    BEGIN
        UPDATE dbo.FileOrProject
        SET 
            FileName = @FileName,
            FileType = @FileType,
            UploadTime = @UpLoadDate,
            FileData = @FileData
        WHERE idFile = @ExistingFileId;

        -- Return 1 for success
        SELECT 1 AS Result;
    END
    ELSE
    BEGIN
        -- If the file does not exist, return 0 (indicating failure)
        SELECT 0 AS Result;
    END
END
GO	

CREATE PROC USP_CheckFileIfExists
@fileName NVARCHAR(100), @fileType NVARCHAR(100), @upLoader NVARCHAR(100)
AS
BEGIN
    SELECT COUNT(*) FROM dbo.FileOrProject WHERE FileName = @fileName AND FileType = @fileType AND Uploader = @upLoader
END
GO	

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO

CREATE PROC	USP_SearchByName
@fileName NVARCHAR(100)
AS
BEGIN
	SELECT * FROM dbo.FileOrProject WHERE dbo.fuConvertToUnsign1(FileName) LIKE N'%' + dbo.fuConvertToUnsign1(@fileName) + '%'
END
GO	

CREATE PROC USP_GetListFileByUploaderAndFileName
    @uploader NVARCHAR(100),
    @fileName NVARCHAR(255),
    @page INT
AS
BEGIN
    DECLARE @pageRows INT = 15;  -- Số hàng trên mỗi trang
    DECLARE @offsetRows INT = (@page - 1) * @pageRows;  -- Số hàng cần bỏ qua dựa trên trang hiện tại

    SELECT FileName, 
           FileType, 
           Uploader, 
           UploadTime
    FROM dbo.FileOrProject
    WHERE Uploader = @uploader AND FileName LIKE '%' + @fileName + '%'
    ORDER BY UploadTime ASC
    OFFSET @offsetRows ROWS FETCH NEXT @pageRows ROWS ONLY;
END
GO

CREATE PROCEDURE USP_GetTotalPagesByUploaderAndFileName
    @uploader NVARCHAR(100),  -- Người tải lên
    @fileName NVARCHAR(255)   -- Tên file
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @pageRows INT = 15;  -- Số hàng trên mỗi trang

    -- Tính tổng số trang
    SELECT 
        CEILING(COUNT(*) * 1.0 / @pageRows) AS TotalPages
    FROM dbo.FileOrProject
    WHERE 
        (@uploader IS NULL OR Uploader = @uploader) AND  -- Điều kiện lọc người tải lên (có thể NULL)
        (@fileName IS NULL OR FileName LIKE '%' + @fileName + '%'); -- Tìm kiếm theo tên file
END
GO
--
INSERT INTO dbo.Account (UserName, StudentID, DisplayName, PassWord, Type)
VALUES (N'admin', 106200177, N'Admin', N'123456', 1);
GO	

SELECT * FROM dbo.Account
SELECT * FROM dbo.AccountInfo
SELECT * FROM dbo.FileOrProject

SELECT FileName, FileData FROM dbo.FileOrProject WHERE UpLoader = N'admin' AND FileName = N'K mean.docx'