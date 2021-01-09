USE [master]
GO
/****** Object:  Database [SeithmanSoftwareUsers]    Script Date: 1/9/2021 5:28:48 AM ******/
CREATE DATABASE [SeithmanSoftwareUsers]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SeithmanSoftwareUsers', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SeithmanSoftwareUsers.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SeithmanSoftwareUsers_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\SeithmanSoftwareUsers_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SeithmanSoftwareUsers].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ARITHABORT OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET  MULTI_USER 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET QUERY_STORE = OFF
GO
USE [SeithmanSoftwareUsers]
GO
/****** Object:  Table [dbo].[Token_Table]    Script Date: 1/9/2021 5:28:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token_Table](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[owner] [int] NOT NULL,
	[expires] [datetime] NOT NULL,
	[token] [varchar](50) NULL,
 CONSTRAINT [Pk_Token_Table_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Table]    Script Date: 1/9/2021 5:28:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Table](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[lastlogin] [datetime] NULL,
	[pwsalt] [binary](32) NOT NULL,
	[pwhash] [binary](64) NOT NULL,
 CONSTRAINT [Pk_User_Table_id] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Token_Table]  WITH CHECK ADD  CONSTRAINT [fk_token_table_user_table] FOREIGN KEY([owner])
REFERENCES [dbo].[User_Table] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Token_Table] CHECK CONSTRAINT [fk_token_table_user_table]
GO
/****** Object:  StoredProcedure [dbo].[Token_Create]    Script Date: 1/9/2021 5:28:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Token_Create]
	(
		@Owner int,
		@Expires datetime,
		@Token varchar(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO dbo.Token_Table
		(owner, expires, token)
	SELECT @Owner, @Expires, @Token
END
GO
/****** Object:  StoredProcedure [dbo].[Token_Delete]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Token_Delete]
	(
		@Id int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM dbo.Token_Table WHERE id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Token_Delete_ByToken]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Token_Delete_ByToken]
	(
		@Token varchar(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM dbo.Token_Table WHERE token = @Token
END
GO
/****** Object:  StoredProcedure [dbo].[Token_Get]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Token_Get]
	(
		@Token varchar(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.Token_Table WHERE token = @Token
END
GO
/****** Object:  StoredProcedure [dbo].[Token_Get_ByToken]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Token_Get_ByToken]
	(
		@Token varchar(50)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT  a.id, a.owner, a.token, a.expires, b.id, b.username, b.email, b.lastlogin FROM dbo.Token_Table a
	LEFT JOIN dbo.User_Table b ON a.owner = b.id
	WHERE a.token = @Token
    -- Insert statements for procedure here
END
GO
/****** Object:  StoredProcedure [dbo].[User_ChangePassword]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_ChangePassword]
	(
		@Id int,
		@PwSalt binary(32),
		@PwHash binary(64)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.User_Table
	SET pwsalt = @PwSalt, pwhash = @PwHash
	WHERE id = @Id
    -- Insert statements for procedure here
END
GO
/****** Object:  StoredProcedure [dbo].[User_Create]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_Create]
	(
		@UserName varchar(100),
		@Email varchar(100),
		@PwSalt binary(32),
		@PwHash binary(64)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO dbo.User_Table
		(username, email, pwsalt, pwhash)
	SELECT @UserName, @Email, @PwSalt, @PwHash

	SELECT id FROM dbo.User_Table WHERE id = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[User_Delete]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_Delete]
	(
		@Id int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM dbo.User_Table WHERE id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[User_Get_ById]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_Get_ById]
	(
		@Id int
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.User_Table WHERE id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetByUserNameOrEmail]    Script Date: 1/9/2021 5:28:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[User_GetByUserNameOrEmail]
	(
		@UserNameOrEmail varchar(100)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM dbo.User_Table WHERE username = @UserNameOrEmail OR email = @UserNameOrEmail
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Token ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Token_Table', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Token Owner' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Token_Table', @level2type=N'COLUMN',@level2name=N'owner'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date and Time the token expired' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Token_Table', @level2type=N'COLUMN',@level2name=N'expires'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Access Token' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Token_Table', @level2type=N'COLUMN',@level2name=N'token'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tokens' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Token_Table'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'username'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email Address' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Last Login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'lastlogin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Password Salt' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'pwsalt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Password Hash' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table', @level2type=N'COLUMN',@level2name=N'pwhash'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Users' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Table'
GO
USE [master]
GO
ALTER DATABASE [SeithmanSoftwareUsers] SET  READ_WRITE 
GO
