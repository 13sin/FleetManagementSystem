USE [fleet]
GO

/****** Object:  Table [dbo].[destination]    Script Date: 2/27/2019 7:52:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[destination](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AddressId] [int] NULL,
 CONSTRAINT [PK_destination] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[destination]  WITH CHECK ADD  CONSTRAINT [FK_destination_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO

ALTER TABLE [dbo].[destination] CHECK CONSTRAINT [FK_destination_Address]
GO

