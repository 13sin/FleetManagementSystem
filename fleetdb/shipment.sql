USE [fleet]
GO

/****** Object:  Table [dbo].[shipment]    Script Date: 2/27/2019 7:53:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[shipment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[customerId] [int] NULL,
	[brokerId] [int] NULL,
	[originId] [int] NULL,
	[destinationId] [int] NULL,
	[origin_appt_number] [nvarchar](max) NULL,
	[origin_appt_datetime] [datetime2](7) NULL,
	[destination_appt_number] [nvarchar](max) NULL,
	[destination_appt_datetime] [datetime2](7) NULL,
	[freightType] [nchar](10) NULL,
	[commodity] [nvarchar](max) NULL,
	[weight] [float] NULL,
	[equipmentType] [nvarchar](50) NULL,
	[broker_rate] [money] NULL,
	[notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_shipment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[shipment]  WITH CHECK ADD  CONSTRAINT [FK_shipment_Broker] FOREIGN KEY([brokerId])
REFERENCES [dbo].[Broker] ([Id])
GO

ALTER TABLE [dbo].[shipment] CHECK CONSTRAINT [FK_shipment_Broker]
GO

ALTER TABLE [dbo].[shipment]  WITH CHECK ADD  CONSTRAINT [FK_shipment_Customer] FOREIGN KEY([customerId])
REFERENCES [dbo].[Customer] ([Id])
GO

ALTER TABLE [dbo].[shipment] CHECK CONSTRAINT [FK_shipment_Customer]
GO

ALTER TABLE [dbo].[shipment]  WITH CHECK ADD  CONSTRAINT [FK_shipment_destination] FOREIGN KEY([destinationId])
REFERENCES [dbo].[destination] ([Id])
GO

ALTER TABLE [dbo].[shipment] CHECK CONSTRAINT [FK_shipment_destination]
GO

ALTER TABLE [dbo].[shipment]  WITH CHECK ADD  CONSTRAINT [FK_shipment_origin] FOREIGN KEY([originId])
REFERENCES [dbo].[origin] ([Id])
GO

ALTER TABLE [dbo].[shipment] CHECK CONSTRAINT [FK_shipment_origin]
GO

