USE [master]
GO
/****** Object:  Database [AnimeTemp]    Script Date: 24/11/2015 16:25:17 ******/
CREATE DATABASE [AnimeTemp] ON  PRIMARY 
( NAME = N'AnimeTemp', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\AnimeTemp.mdf' , SIZE = 14336KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AnimeTemp_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\AnimeTemp_1.ldf' , SIZE = 3136KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AnimeTemp] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AnimeTemp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AnimeTemp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AnimeTemp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AnimeTemp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AnimeTemp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AnimeTemp] SET ARITHABORT OFF 
GO
ALTER DATABASE [AnimeTemp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AnimeTemp] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [AnimeTemp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AnimeTemp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AnimeTemp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AnimeTemp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AnimeTemp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AnimeTemp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AnimeTemp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AnimeTemp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AnimeTemp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AnimeTemp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AnimeTemp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AnimeTemp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AnimeTemp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AnimeTemp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AnimeTemp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AnimeTemp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AnimeTemp] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AnimeTemp] SET  MULTI_USER 
GO
ALTER DATABASE [AnimeTemp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AnimeTemp] SET DB_CHAINING OFF 
GO
USE [AnimeTemp]
GO
/****** Object:  User [omgwtfbbq2]    Script Date: 24/11/2015 16:25:17 ******/
CREATE USER [omgwtfbbq2] FOR LOGIN [omgwtfbbq2] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'omgwtfbbq2'
GO
/****** Object:  Table [dbo].[Anime]    Script Date: 24/11/2015 16:25:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Anime](
	[Id] [int] NOT NULL,
	[Title] [nvarchar](500) NULL,
	[EnglishTitle] [nvarchar](500) NULL,
	[Synonyms] [nvarchar](500) NULL,
	[Episodes] [int] NULL,
	[Score] [float] NULL,
	[AnimeType] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Synopsis] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[SynopsisFileLastGenerated] [datetime] NULL,
	[DataUpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Anime] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AnimeEpisode]    Script Date: 24/11/2015 16:25:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnimeEpisode](
	[AnimeEpisodeId] [int] IDENTITY(1,1) NOT NULL,
	[AnimeId] [int] NULL,
	[Filename] [nvarchar](500) NULL,
	[Extension] [nchar](5) NULL,
	[FileCreatedDate] [datetime] NULL,
 CONSTRAINT [PK_AnimeEpisode] PRIMARY KEY CLUSTERED 
(
	[AnimeEpisodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AnimeHome]    Script Date: 24/11/2015 16:25:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnimeHome](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AnimeTitle] [nvarchar](200) NULL,
	[downloaded] [bit] NULL,
 CONSTRAINT [PK_AnimeHome] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AnimeEpisode]  WITH CHECK ADD  CONSTRAINT [FK_AnimeEpisode_Anime] FOREIGN KEY([AnimeId])
REFERENCES [dbo].[Anime] ([Id])
GO
ALTER TABLE [dbo].[AnimeEpisode] CHECK CONSTRAINT [FK_AnimeEpisode_Anime]
GO
USE [master]
GO
ALTER DATABASE [AnimeTemp] SET  READ_WRITE 
GO
