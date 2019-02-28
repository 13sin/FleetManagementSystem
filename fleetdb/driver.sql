USE [fleet]
GO

/****** Object:  Table [dbo].[Driver]    Script Date: 2/27/2019 7:52:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Driver](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AddressId] [int] NULL,
	[carrierId] [int] NOT NULL,
	[licenseType] [nvarchar](50) NULL,
	[licenseNumber] [nvarchar](max) NULL,
	[licenseState] [nvarchar](50) NULL,
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_Address] FOREIGN KEY([AddressId])
REFERENCES [dbo].[Address] ([Id])
GO

ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_Address]
GO

ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_Carrier] FOREIGN KEY([carrierId])
REFERENCES [dbo].[Carrier] ([Id])
GO

ALTER TABLE [dbo].[Driver] CHECK CONSTRAINT [FK_Driver_Carrier]
GO

