USE [AnimeTemp]
GO

/****** Object:  Table [dbo].[AnimeEpisode]    Script Date: 24/11/2015 16:23:17 ******/
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

ALTER TABLE [dbo].[AnimeEpisode]  WITH CHECK ADD  CONSTRAINT [FK_AnimeEpisode_Anime] FOREIGN KEY([AnimeId])
REFERENCES [dbo].[Anime] ([Id])
GO

ALTER TABLE [dbo].[AnimeEpisode] CHECK CONSTRAINT [FK_AnimeEpisode_Anime]
GO


