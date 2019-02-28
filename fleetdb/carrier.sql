USE [fleet]
GO

/****** Object:  Table [dbo].[Carrier]    Script Date: 2/27/2019 7:51:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Carrier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AddressId] [int] NULL,
	[MC] [nvarchar](50) NULL,
	[USDOT] [nvarchar](50) NULL,
	[CVOR] [nvarchar](50) NULL,
	[CTPAT] [nvarchar](50) NULL,
 CONSTRAINT [PK_Carrier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Carrier]  WITH CHECK ADD  CONSTRAINT [FK_Carrier_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO

ALTER TABLE [dbo].[Carrier] CHECK CONSTRAINT [FK_Carrier_Address]
GO

