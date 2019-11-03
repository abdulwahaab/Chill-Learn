USE [master]
GO
/****** Object:  Database [ChillLearn]    Script Date: 03/11/2019 3:15:09 PM ******/
CREATE DATABASE [ChillLearn]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ChillLearn', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ChillLearn.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ChillLearn_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ChillLearn_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ChillLearn] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ChillLearn].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ChillLearn] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ChillLearn] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ChillLearn] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ChillLearn] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ChillLearn] SET ARITHABORT OFF 
GO
ALTER DATABASE [ChillLearn] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ChillLearn] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ChillLearn] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ChillLearn] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ChillLearn] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ChillLearn] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ChillLearn] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ChillLearn] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ChillLearn] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ChillLearn] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ChillLearn] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ChillLearn] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ChillLearn] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ChillLearn] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ChillLearn] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ChillLearn] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ChillLearn] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ChillLearn] SET RECOVERY FULL 
GO
ALTER DATABASE [ChillLearn] SET  MULTI_USER 
GO
ALTER DATABASE [ChillLearn] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ChillLearn] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ChillLearn] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ChillLearn] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ChillLearn] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ChillLearn] SET QUERY_STORE = OFF
GO
USE [ChillLearn]
GO
/****** Object:  Table [dbo].[AppSettings]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSettings](
	[ID] [int] NOT NULL,
	[UnitPrice] [decimal](18, 0) NULL,
	[FBURL] [nvarchar](50) NULL,
	[TwitterURL] [nvarchar](50) NULL,
	[FeaturedClassPrice] [decimal](18, 0) NULL,
	[FeaturedTeacherPrice] [decimal](18, 0) NULL,
 CONSTRAINT [PK_AppSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Claims]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Claims](
	[ClaimID] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Claims] PRIMARY KEY CLUSTERED 
(
	[ClaimID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[ClassID] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[TeacherID] [nvarchar](50) NULL,
	[SubjectID] [int] NOT NULL,
	[Title] [nvarchar](500) NULL,
	[Description] [nchar](10) NULL,
	[Type] [int] NULL,
	[ClassDay] [int] NULL,
	[ClassFrom] [datetime] NULL,
	[ClassTo] [nvarchar](10) NULL,
	[Duration] [int] NULL,
	[Record] [bit] NULL,
	[MaxStudents] [int] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Classes] PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClassInvitations]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClassInvitations](
	[InvitationID] [int] IDENTITY(1,1) NOT NULL,
	[ClassID] [nvarchar](50) NULL,
	[UserID] [nvarchar](50) NULL,
	[Email] [nvarchar](250) NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ClassInvitations] PRIMARY KEY CLUSTERED 
(
	[InvitationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[BidID] [nvarchar](50) NULL,
	[FromUser] [nvarchar](50) NULL,
	[ToUser] [nvarchar](50) NULL,
	[Message] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[ViewedDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[SubscriptionID] [nvarchar](50) NOT NULL,
	[Type] [int] NULL,
	[Amount] [decimal](18, 0) NULL,
	[Source] [int] NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Plans]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Plans](
	[PlanID] [nvarchar](50) NOT NULL,
	[PlanName] [nvarchar](150) NULL,
	[Price] [decimal](18, 0) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Plans] PRIMARY KEY CLUSTERED 
(
	[PlanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Refunds]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Refunds](
	[RefundID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[SubscriptionID] [nvarchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NULL,
	[Reason] [nvarchar](max) NULL,
	[CreationDate] [datetime] NULL,
	[ApproveDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Refunds] PRIMARY KEY CLUSTERED 
(
	[RefundID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stages]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stages](
	[StageID] [int] IDENTITY(1,1) NOT NULL,
	[StageName] [nvarchar](50) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Stages] PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentClasses]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentClasses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [nvarchar](50) NULL,
	[ClassID] [nvarchar](50) NOT NULL,
	[JoiningDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_StudentClasses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentProblemBids]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentProblemBids](
	[BidID] [nvarchar](50) NOT NULL,
	[ProblemID] [nvarchar](50) NULL,
	[UserID] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[FileName] [nvarchar](50) NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ProblemBids] PRIMARY KEY CLUSTERED 
(
	[BidID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentProblems]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentProblems](
	[ProblemID] [nvarchar](50) NOT NULL,
	[StudentID] [nvarchar](50) NULL,
	[Type] [int] NULL,
	[SubjectID] [int] NULL,
	[FileName] [nvarchar](max) NULL,
	[HoursNeeded] [decimal](18, 0) NULL,
	[HoursSpent] [decimal](18, 0) NULL,
	[Description] [nvarchar](max) NULL,
	[ExpireDate] [datetime] NULL,
	[CompletedDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Problems] PRIMARY KEY CLUSTERED 
(
	[ProblemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectPrices]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectPrices](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectID] [int] NULL,
	[HourlyRate] [decimal](18, 0) NULL,
	[StageID] [int] NULL,
	[TeacherShare] [decimal](18, 0) NULL,
 CONSTRAINT [PK_SubjectPrices] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 03/11/2019 3:15:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[SubjectID] [int] NOT NULL,
	[SubjectName] [nvarchar](150) NULL,
	[Description] [nvarchar](500) NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[SubscriptionID] [nvarchar](50) NOT NULL,
	[PlanID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED 
(
	[SubscriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherDetails]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [nvarchar](50) NULL,
	[Title] [nvarchar](50) NULL,
	[Qualification] [nvarchar](250) NULL,
	[YearsExperience] [int] NULL,
	[SubjectID] [int] NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_TeacherDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherQualifications]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherQualifications](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [nvarchar](50) NULL,
	[DegreeTitle] [nvarchar](150) NULL,
	[InstituteName] [nvarchar](150) NULL,
	[YearPassed] [int] NULL,
	[SubjectID] [int] NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_TeacherQualifications] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherReviews]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherReviews](
	[ReviewID] [nvarchar](50) NOT NULL,
	[TutorID] [nvarchar](50) NULL,
	[StudentID] [nvarchar](50) NULL,
	[ProblemID] [nvarchar](50) NOT NULL,
	[Hours] [decimal](18, 0) NULL,
	[Rating] [int] NULL,
	[Feedback] [nvarchar](500) NULL,
	[IsRecommended] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_TutorReviews] PRIMARY KEY CLUSTERED 
(
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherStages]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherStages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SubjectID] [int] NOT NULL,
	[TeacherID] [nvarchar](50) NULL,
	[StageID] [int] NULL,
	[HourlyRate] [decimal](18, 0) NULL,
 CONSTRAINT [PK_TeacherStages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[ID] [int] NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[ClaimID] [int] NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [nvarchar](50) NOT NULL,
	[NationalID] [nvarchar](500) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](500) NULL,
	[Password] [nvarchar](max) NULL,
	[Picture] [nvarchar](max) NULL,
	[ContactNumber] [nvarchar](50) NULL,
	[Grade] [nvarchar](50) NULL,
	[Class] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Address] [nvarchar](500) NULL,
	[BirthDate] [date] NULL,
	[UserRole] [int] NULL,
	[Source] [int] NULL,
	[ValidationToken] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Status] [int] NULL,
	[IsOnline] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 03/11/2019 3:15:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentID] [int] NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[Amount] [int] NULL,
	[TransactionType] [nvarchar](10) NULL,
	[CreationDate] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Classes] ([ClassID], [CreatedBy], [TeacherID], [SubjectID], [Title], [Description], [Type], [ClassDay], [ClassFrom], [ClassTo], [Duration], [Record], [MaxStudents], [CreationDate], [UpdateDate], [Status]) VALUES (N'1ba159ea-08be-4a7b-aaaf-edb5676482cd', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', 3, N'Title 1', N'dfdfdf    ', 2, NULL, CAST(N'2019-10-30T00:00:00.000' AS DateTime), N'09:30', 3, 0, NULL, CAST(N'2019-10-29T15:27:35.283' AS DateTime), NULL, 1)
INSERT [dbo].[Classes] ([ClassID], [CreatedBy], [TeacherID], [SubjectID], [Title], [Description], [Type], [ClassDay], [ClassFrom], [ClassTo], [Duration], [Record], [MaxStudents], [CreationDate], [UpdateDate], [Status]) VALUES (N'4c06fd1b-887b-4d1b-a3c7-2e5e7c594731', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', 1, N'2nd Class', N'fgdfgdgfd ', 2, NULL, CAST(N'2019-11-07T00:00:00.000' AS DateTime), N'09:30', 4, 0, NULL, CAST(N'2019-10-29T15:29:52.453' AS DateTime), NULL, 1)
INSERT [dbo].[Classes] ([ClassID], [CreatedBy], [TeacherID], [SubjectID], [Title], [Description], [Type], [ClassDay], [ClassFrom], [ClassTo], [Duration], [Record], [MaxStudents], [CreationDate], [UpdateDate], [Status]) VALUES (N'9005d987-f1ab-4256-abc8-ceb3baa1cec2', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', 1, N'3nd Class', N'ds sd sd  ', 1, NULL, CAST(N'2019-10-22T00:00:00.000' AS DateTime), N'09:30', 3, 0, NULL, CAST(N'2019-10-29T15:35:49.087' AS DateTime), NULL, 1)
INSERT [dbo].[Classes] ([ClassID], [CreatedBy], [TeacherID], [SubjectID], [Title], [Description], [Type], [ClassDay], [ClassFrom], [ClassTo], [Duration], [Record], [MaxStudents], [CreationDate], [UpdateDate], [Status]) VALUES (N'e57d4649-31de-49ea-b1bd-e94b9402d38f', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', 2, N'fgdfgdfgdfg', N'fgdfg     ', 2, NULL, CAST(N'2019-10-16T00:00:00.000' AS DateTime), N'14:10', 4, 0, NULL, CAST(N'2019-10-29T15:50:11.923' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (3, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'Student to teacher', CAST(N'2019-09-27T16:23:40.137' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (4, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'121212', CAST(N'2019-09-27T16:24:41.053' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (5, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'3rd msg from student', CAST(N'2019-09-27T16:25:57.180' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (6, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'123', CAST(N'2019-09-27T16:26:18.533' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (7, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'5th msg', CAST(N'2019-09-27T16:29:30.890' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (8, N'2fa35a2f-a402-478f-8625-e46017741e71', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'66919287-d63d-4b30-931f-30532b68c2f1', N'1st msg from teacher', CAST(N'2019-09-27T16:29:30.890' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (9, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'6th msg from student', CAST(N'2019-09-27T16:34:15.340' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (10, N'8b28fe44-7150-40a3-8185-2001f5f58d8c', N'66919287-d63d-4b30-931f-30532b68c2f1', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'Student Response to tayib Teacher fb', CAST(N'2019-09-27T16:35:25.950' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (11, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'123', CAST(N'2019-09-27T17:11:12.397' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (12, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'ready 123', CAST(N'2019-09-27T17:35:08.440' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (13, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'Abdul''s first message', CAST(N'2019-09-27T17:40:50.460' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (14, N'38a5e2b6-d084-4017-b085-bb3adf093cf8', N'66919287-d63d-4b30-931f-30532b68c2f1', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'ds', CAST(N'2019-09-30T12:29:19.333' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (15, N'38a5e2b6-d084-4017-b085-bb3adf093cf8', N'66919287-d63d-4b30-931f-30532b68c2f1', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'sd', CAST(N'2019-09-30T12:29:23.550' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (16, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'111222', CAST(N'2019-09-30T15:37:45.287' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (17, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'1221', CAST(N'2019-09-30T15:38:59.863' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (18, N'2fa35a2f-a402-478f-8625-e46017741e71', N'66919287-d63d-4b30-931f-30532b68c2f1', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'1231', CAST(N'2019-09-30T15:40:38.593' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (19, N'8b28fe44-7150-40a3-8185-2001f5f58d8c', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'teacher to stydebnt', CAST(N'2019-09-30T16:01:09.707' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (20, N'8b28fe44-7150-40a3-8185-2001f5f58d8c', N'66919287-d63d-4b30-931f-30532b68c2f1', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'11223344', CAST(N'2019-09-30T16:02:15.157' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (21, N'8b28fe44-7150-40a3-8185-2001f5f58d8c', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'1122121212gfv xcbfbbv fbfbv', CAST(N'2019-09-30T16:02:26.983' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (22, N'38a5e2b6-d084-4017-b085-bb3adf093cf8', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'problem statement Mirza Reply to student', CAST(N'2019-09-30T16:04:14.417' AS DateTime), NULL, 1)
INSERT [dbo].[Messages] ([MessageID], [BidID], [FromUser], [ToUser], [Message], [CreationDate], [ViewedDate], [Status]) VALUES (23, N'38a5e2b6-d084-4017-b085-bb3adf093cf8', N'66919287-d63d-4b30-931f-30532b68c2f1', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'Problem Statment Student Faisal reply to techer Mirza', CAST(N'2019-09-30T16:04:45.690' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Messages] OFF
SET IDENTITY_INSERT [dbo].[Stages] ON 

INSERT [dbo].[Stages] ([StageID], [StageName], [Description], [Status]) VALUES (1, N'Primery', N'1-5 class', 1)
INSERT [dbo].[Stages] ([StageID], [StageName], [Description], [Status]) VALUES (2, N'Middle', N'5-8 class', 1)
SET IDENTITY_INSERT [dbo].[Stages] OFF
SET IDENTITY_INSERT [dbo].[StudentClasses] ON 

INSERT [dbo].[StudentClasses] ([ID], [StudentID], [ClassID], [JoiningDate], [Status]) VALUES (1, N'66919287-d63d-4b30-931f-30532b68c2f1', N'1ba159ea-08be-4a7b-aaaf-edb5676482cd', CAST(N'2019-10-30T15:57:13.587' AS DateTime), 3)
INSERT [dbo].[StudentClasses] ([ID], [StudentID], [ClassID], [JoiningDate], [Status]) VALUES (5, N'66919287-d63d-4b30-931f-30532b68c2f1', N'e57d4649-31de-49ea-b1bd-e94b9402d38f', CAST(N'2019-10-30T16:31:51.537' AS DateTime), 2)
INSERT [dbo].[StudentClasses] ([ID], [StudentID], [ClassID], [JoiningDate], [Status]) VALUES (12, N'd0a20f5e-746c-4cf5-adf3-be031aafb9cb', N'1ba159ea-08be-4a7b-aaaf-edb5676482cd', CAST(N'2019-10-31T11:28:22.933' AS DateTime), 1)
INSERT [dbo].[StudentClasses] ([ID], [StudentID], [ClassID], [JoiningDate], [Status]) VALUES (13, N'd0a20f5e-746c-4cf5-adf3-be031aafb9cb', N'4c06fd1b-887b-4d1b-a3c7-2e5e7c594731', CAST(N'2019-11-01T21:43:41.043' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[StudentClasses] OFF
INSERT [dbo].[StudentProblemBids] ([BidID], [ProblemID], [UserID], [Description], [FileName], [CreationDate], [UpdateDate], [Status]) VALUES (N'2fa35a2f-a402-478f-8625-e46017741e71', N'14aee98b-4c06-449f-8e4e-66680704c427', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'Fasi 2', NULL, CAST(N'2019-09-27T10:40:37.200' AS DateTime), NULL, 1)
INSERT [dbo].[StudentProblemBids] ([BidID], [ProblemID], [UserID], [Description], [FileName], [CreationDate], [UpdateDate], [Status]) VALUES (N'38a5e2b6-d084-4017-b085-bb3adf093cf8', N'1a68b9ae-95bd-4693-8533-1ce09b128784', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'Tayib @nd proposal
', NULL, CAST(N'2019-09-27T10:19:44.187' AS DateTime), NULL, 1)
INSERT [dbo].[StudentProblemBids] ([BidID], [ProblemID], [UserID], [Description], [FileName], [CreationDate], [UpdateDate], [Status]) VALUES (N'8b28fe44-7150-40a3-8185-2001f5f58d8c', N'9b3cefd0-af40-4280-afde-18203ec37488', N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'But these methods don''t work when UTC is a day ahead. For my client in California that means at 5:00 pm my client can no longer select his local current date as a valid date. In order to fix this I am temporarily using', NULL, CAST(N'2019-09-26T12:52:29.903' AS DateTime), NULL, 1)
INSERT [dbo].[StudentProblemBids] ([BidID], [ProblemID], [UserID], [Description], [FileName], [CreationDate], [UpdateDate], [Status]) VALUES (N'b8a4c289-8c1f-4663-9805-40eaea5092e5', N'1a68b9ae-95bd-4693-8533-1ce09b128784', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'Fasi 3', NULL, CAST(N'2019-09-27T10:40:44.420' AS DateTime), NULL, 1)
INSERT [dbo].[StudentProblemBids] ([BidID], [ProblemID], [UserID], [Description], [FileName], [CreationDate], [UpdateDate], [Status]) VALUES (N'd152ca2b-bea5-433b-8fca-ed5198953f36', N'9b3cefd0-af40-4280-afde-18203ec37488', N'c7caf73a-04bb-4efd-b7bd-426cacb51842', N'science', NULL, CAST(N'2019-09-26T15:54:40.410' AS DateTime), NULL, 1)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'013a32a7-7ea0-42f1-9dc6-edd1d91e7f83', N'd0a20f5e-746c-4cf5-adf3-be031aafb9cb', 2, 3, N'', CAST(4 AS Decimal(18, 0)), NULL, N'Problem description here', CAST(N'2019-07-11T00:00:00.000' AS DateTime), NULL, CAST(N'2019-11-01T20:42:14.723' AS DateTime), NULL, NULL)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'14aee98b-4c06-449f-8e4e-66680704c427', N'66919287-d63d-4b30-931f-30532b68c2f1', 1, 2, N'', CAST(6 AS Decimal(18, 0)), NULL, N'This is test problem posted by Faisal', CAST(N'2019-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2019-09-27T10:21:17.120' AS DateTime), NULL, NULL)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'1a68b9ae-95bd-4693-8533-1ce09b128784', N'66919287-d63d-4b30-931f-30532b68c2f1', 1, 3, N'', CAST(2 AS Decimal(18, 0)), NULL, N'A problem statement is a concise description of an issue to be addressed or a condition to be improved upon. It identifies the gap between the current (problem) state and desired (goal) state of a process or product. Focusing on the facts, the problem statement should be designed to address the Five Ws. The first condition of solving a problem is understanding the problem, which can be done by way of a problem statement', CAST(N'2019-05-10T00:00:00.000' AS DateTime), NULL, CAST(N'2019-09-25T14:55:03.870' AS DateTime), NULL, NULL)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'3515caf3-1d1c-498a-ac9e-fac28911a00c', N'66919287-d63d-4b30-931f-30532b68c2f1', 2, 3, N'e2a7505a-52c9-472f-892c-5ded2b7a620cimg5.jpg', CAST(1220 AS Decimal(18, 0)), NULL, N'Ali Sethi has sung a few Coke Studio songs like Ranjish hi Sahi, Tinak Dhin, Aaqa and now Luddi Hai Jamalo. Accompanied by the beautiful voice of Humaira Arshad, Ali Sethi sings this wedding song that will make it to the top shadi songs and top desi songs of 2018', CAST(N'2019-05-11T00:00:00.000' AS DateTime), NULL, CAST(N'2019-10-24T12:39:30.550' AS DateTime), NULL, NULL)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'83ada42f-f012-44e1-9fab-dd04cb55dc8c', N'66919287-d63d-4b30-931f-30532b68c2f1', 2, 3, N'f3f81073-e5e0-4806-adca-b425b8fe227fimg5.jpg', CAST(1220 AS Decimal(18, 0)), NULL, N'Ali Sethi has sung a few Coke Studio songs like Ranjish hi Sahi, Tinak Dhin, Aaqa and now Luddi Hai Jamalo. Accompanied by the beautiful voice of Humaira Arshad, Ali Sethi sings this wedding song that will make it to the top shadi songs and top desi songs of 2018', CAST(N'2019-05-11T00:00:00.000' AS DateTime), NULL, CAST(N'2019-10-24T12:39:14.460' AS DateTime), NULL, NULL)
INSERT [dbo].[StudentProblems] ([ProblemID], [StudentID], [Type], [SubjectID], [FileName], [HoursNeeded], [HoursSpent], [Description], [ExpireDate], [CompletedDate], [CreationDate], [UpdateDate], [Status]) VALUES (N'9b3cefd0-af40-4280-afde-18203ec37488', N'66919287-d63d-4b30-931f-30532b68c2f1', 2, 1, N'', CAST(14 AS Decimal(18, 0)), NULL, N'The Supreme Court of the United Kingdom unanimously rules that the September 2019 prorogation of Parliament was unlawful and void.
Climate strikes take place in 150 countries (protesters in Sydney pictured) as part of the Fridays for Future protests.', CAST(N'2019-05-10T00:00:00.000' AS DateTime), NULL, CAST(N'2019-09-25T16:16:03.820' AS DateTime), NULL, NULL)
INSERT [dbo].[Subjects] ([SubjectID], [SubjectName], [Description], [Status]) VALUES (1, N'Science', N'Fsc pre eng subjects', 1)
INSERT [dbo].[Subjects] ([SubjectID], [SubjectName], [Description], [Status]) VALUES (2, N'Math', N'Math Descriptipn', 1)
INSERT [dbo].[Subjects] ([SubjectID], [SubjectName], [Description], [Status]) VALUES (3, N'Computer', N'Computer Science', 1)
SET IDENTITY_INSERT [dbo].[TeacherDetails] ON 

INSERT [dbo].[TeacherDetails] ([ID], [TeacherID], [Title], [Qualification], [YearsExperience], [SubjectID], [CreationDate], [Status]) VALUES (1, N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'Qu Title', N'Fsc, Bscs, Web Designer', 2, 1, CAST(N'2019-09-23T10:31:56.370' AS DateTime), 1)
INSERT [dbo].[TeacherDetails] ([ID], [TeacherID], [Title], [Qualification], [YearsExperience], [SubjectID], [CreationDate], [Status]) VALUES (2, N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', N'Math Enge', N'Bs Math', 1, 2, CAST(N'2019-09-23T10:31:56.370' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[TeacherDetails] OFF
SET IDENTITY_INSERT [dbo].[TeacherQualifications] ON 

INSERT [dbo].[TeacherQualifications] ([ID], [TeacherID], [DegreeTitle], [InstituteName], [YearPassed], [SubjectID], [CreationDate], [Status]) VALUES (1, N'622f466e-0fc1-4b7f-87f8-947cfed85632', N'Fsc', N'Superior Collage', 2012, 1, CAST(N'2019-09-23T10:31:56.370' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[TeacherQualifications] OFF
SET IDENTITY_INSERT [dbo].[TeacherStages] ON 

INSERT [dbo].[TeacherStages] ([ID], [SubjectID], [TeacherID], [StageID], [HourlyRate]) VALUES (1, 1, N'622f466e-0fc1-4b7f-87f8-947cfed85632', 1, CAST(10 AS Decimal(18, 0)))
INSERT [dbo].[TeacherStages] ([ID], [SubjectID], [TeacherID], [StageID], [HourlyRate]) VALUES (2, 2, N'622f466e-0fc1-4b7f-87f8-947cfed85632', 2, CAST(15 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[TeacherStages] OFF
INSERT [dbo].[Users] ([UserID], [NationalID], [FirstName], [LastName], [Email], [Password], [Picture], [ContactNumber], [Grade], [Class], [Country], [City], [Address], [BirthDate], [UserRole], [Source], [ValidationToken], [CreatedBy], [CreationDate], [UpdateDate], [Status], [IsOnline]) VALUES (N'5a22f025-ae64-49b8-9782-32bc2f1ccef6', NULL, N'Mirza', N'Tayyab', N'012904c2cdf716c4b4f8f7e186efece9000000000000000000000000000000003f39fca60a552a3eb4a50d59358be01ea737006073e770ad7cc4dea3827e5ab2', NULL, N'8ea1e784-8d8c-4d57-8d51-e42bd31d7bdfpayment-roll-profile-small2 (2).png', NULL, NULL, NULL, N'Pakistan', N'Faisalabad', N'Taj deen', NULL, 2, 2, N'012904c2cdf716c4b4f8f7e186efece90000000000000000000000000000000048a5f10f3a18f1ad672a6d38af0ac29ad081ae2aec6e82f3425e2236b69340c3', NULL, CAST(N'2019-09-23T16:25:51.643' AS DateTime), CAST(N'2019-10-28T14:15:11.677' AS DateTime), 3, NULL)
INSERT [dbo].[Users] ([UserID], [NationalID], [FirstName], [LastName], [Email], [Password], [Picture], [ContactNumber], [Grade], [Class], [Country], [City], [Address], [BirthDate], [UserRole], [Source], [ValidationToken], [CreatedBy], [CreationDate], [UpdateDate], [Status], [IsOnline]) VALUES (N'66919287-d63d-4b30-931f-30532b68c2f1', NULL, N'Faisal', N'Student', N'012904c2cdf716c4b4f8f7e186efece90000000000000000000000000000000040a411a4e0685f3ad273964c606e32907e00aa0ed73429bba0da0505e9f17885', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000048fa2b15640410e446936fb66cff2ae2565131c78d351dfca6ba250b6efc70c', N'12c2ea0d-c873-4fda-9e65-7dec24bc31931330e535c2e28c541b78f754342fc225@2x.png', N'0300213654789', N'A', N'Six', N'Pakistan', N'Faisalabad', N'Faisalabad Punjab Pakistan', NULL, 1, 1, N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000b97f3f39d6c9a11e0338b4df5da2259b72f72c1e2eb695b9fe5060696c182bba', NULL, CAST(N'2019-09-19T13:45:09.937' AS DateTime), CAST(N'2019-10-28T12:21:24.490' AS DateTime), 3, NULL)
INSERT [dbo].[Users] ([UserID], [NationalID], [FirstName], [LastName], [Email], [Password], [Picture], [ContactNumber], [Grade], [Class], [Country], [City], [Address], [BirthDate], [UserRole], [Source], [ValidationToken], [CreatedBy], [CreationDate], [UpdateDate], [Status], [IsOnline]) VALUES (N'9b9e392b-47bd-47eb-8c37-97913a211b36', NULL, N'Abdul', N'Wahaab', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000a480279adc1637b220a2eff914284d9e2a00dfbc923bc6f4e559f81036cdefd9', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000d882f66aa088fd5d651f2a63efdac40cfa6f3846b0381d4d9346d17dc44873fc', NULL, N'123456', NULL, NULL, NULL, NULL, NULL, NULL, 2, 1, NULL, NULL, CAST(N'2019-09-03T19:32:32.400' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[Users] ([UserID], [NationalID], [FirstName], [LastName], [Email], [Password], [Picture], [ContactNumber], [Grade], [Class], [Country], [City], [Address], [BirthDate], [UserRole], [Source], [ValidationToken], [CreatedBy], [CreationDate], [UpdateDate], [Status], [IsOnline]) VALUES (N'c7caf73a-04bb-4efd-b7bd-426cacb51842', NULL, N'Faisal Ikram', N'Fasi', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000e824f8ab290356e496eb0bd8c1b7227135b2aa65d2cd8f64aaead0f042deeafa', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000d13b0e2e22f36aaa25a490fc2044469a33939f783c4570ed8f3bc18ca3f4a43d', N'afd161d9-4348-4c76-83d1-3dd5e8a7294fpayment-roll-porfile (2).png', N'0300213654789', N'A', N'Six', N'Pakistan', NULL, NULL, NULL, 2, 1, N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000bb91b9d8036235a81fbfad71a9ace8f64738f086a30e1f9013939f826cd47a03', NULL, CAST(N'2019-09-26T12:55:34.360' AS DateTime), CAST(N'2019-11-01T23:06:39.460' AS DateTime), 3, NULL)
INSERT [dbo].[Users] ([UserID], [NationalID], [FirstName], [LastName], [Email], [Password], [Picture], [ContactNumber], [Grade], [Class], [Country], [City], [Address], [BirthDate], [UserRole], [Source], [ValidationToken], [CreatedBy], [CreationDate], [UpdateDate], [Status], [IsOnline]) VALUES (N'd0a20f5e-746c-4cf5-adf3-be031aafb9cb', NULL, N'Faisal', N'Ikram', N'012904c2cdf716c4b4f8f7e186efece9000000000000000000000000000000002266b41e3ca3da059f8c2907f4e2a9e8913ab7ff9fba1894da480cc19e1ddea9', N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000b22172d2666711868c0d19d61a4c6af1b5ce8625d2debdcb93b48f7b1bdf6aec', N'453f4c90-1932-481f-ab95-537b74466982profile-Tutor.png', N'0300213654789', N'A', N'Six', NULL, NULL, NULL, NULL, 1, 1, N'012904c2cdf716c4b4f8f7e186efece900000000000000000000000000000000d2c96df20fcba7c69b9840ea9142852c2875bc7f422ae6e935c274e4925ddb13', NULL, CAST(N'2019-10-23T15:40:15.283' AS DateTime), CAST(N'2019-10-23T15:42:17.010' AS DateTime), 3, NULL)
USE [master]
GO
ALTER DATABASE [ChillLearn] SET  READ_WRITE 
GO
