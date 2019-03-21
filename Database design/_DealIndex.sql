USE [LJDT]
GO

/****** Object:  Table [dbo].[DealIndex]    Script Date: 15/03/2019 11:05:54 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DealIndex](
	[ID] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Scrapped] [bit] NOT NULL,
 CONSTRAINT [PK_DealIndex] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

