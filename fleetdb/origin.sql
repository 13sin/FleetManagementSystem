USE [fleet]
GO

/****** Object:  Table [dbo].[origin]    Script Date: 2/27/2019 7:53:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[origin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AddressId] [int] NULL,
 CONSTRAINT [PK_origin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[origin]  WITH CHECK ADD  CONSTRAINT [FK_origin_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO

ALTER TABLE [dbo].[origin] CHECK CONSTRAINT [FK_origin_Address]
GO

