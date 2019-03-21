USE [LJDT]
GO

/****** Object:  Table [dbo].[DealInfo]    Script Date: 15/03/2019 11:06:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DealInfo](
	[ID] [nvarchar](50) NOT NULL,
	[DealPrice] [float] NOT NULL,
	[InitPrice] [float] NULL,
	[UnitPrice] [float] NOT NULL,
	[PriceChange] [int] NULL,
	[Visitor] [int] NULL,
	[Starer] [int] NULL,
	[WebViewer] [int] NULL,
	[BedroomNum] [int] NOT NULL,
	[LivingroomNum] [int] NULL,
	[KitchenNum] [int] NULL,
	[RestroomNum] [int] NULL,
	[Floor] [text] NOT NULL,
	[TotalFloor] [int] NOT NULL,
	[OutArea] [float] NOT NULL,
	[InArea] [float] NULL,
	[Direction] [text] NULL,
	[Year] [int] NOT NULL,
	[Decoration] [text] NULL,
	[HasElevator] [bit] NULL,
	[PublishDate] [date] NULL,
	[DealDate] [date] NOT NULL,
	[Community] [text] NOT NULL,
 CONSTRAINT [PK_DealInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

