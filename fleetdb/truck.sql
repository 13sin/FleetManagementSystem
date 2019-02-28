USE [fleet]
GO

/****** Object:  Table [dbo].[Truck]    Script Date: 2/27/2019 7:54:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Truck](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[carrierId] [int] NOT NULL,
	[odometerId] [int] NULL,
	[license_plate] [nvarchar](50) NULL,
	[make] [nchar](10) NULL,
	[model] [nchar](10) NULL,
	[year] [nchar](10) NULL,
	[vin] [nvarchar](max) NULL,
	[location] [nvarchar](max) NULL,
	[TruckType] [nchar](10) NULL,
 CONSTRAINT [PK_Truck] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Truck]  WITH CHECK ADD  CONSTRAINT [FK_Truck_Carrier] FOREIGN KEY([carrierId])
REFERENCES [dbo].[Carrier] ([Id])
GO

ALTER TABLE [dbo].[Truck] CHECK CONSTRAINT [FK_Truck_Carrier]
GO

