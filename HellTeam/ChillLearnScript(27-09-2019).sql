USE [master]
GO
/****** Object:  Database [ChillLearn]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[AppSettings]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Claims]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Classes]    Script Date: 27/09/2019 5:54:44 PM ******/
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
	[ClassFrom] [nvarchar](10) NULL,
	[ClassTo] [nvarchar](10) NULL,
	[Duration] [int] NULL,
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
/****** Object:  Table [dbo].[ClassInvitations]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Messages]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Payments]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Plans]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Refunds]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[Stages]    Script Date: 27/09/2019 5:54:44 PM ******/
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
/****** Object:  Table [dbo].[StudentClasses]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[StudentProblemBids]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[StudentProblems]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[SubjectPrices]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[Subjects]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[TeacherDetails]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[TeacherQualifications]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[TeacherReviews]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[TeacherStages]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[UserClaims]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 27/09/2019 5:54:45 PM ******/
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
/****** Object:  Table [dbo].[Wallet]    Script Date: 27/09/2019 5:54:45 PM ******/
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
USE [master]
GO
ALTER DATABASE [ChillLearn] SET  READ_WRITE 
GO
