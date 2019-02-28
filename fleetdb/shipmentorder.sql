USE [fleet]
GO

/****** Object:  Table [dbo].[shipmentOrder]    Script Date: 2/27/2019 7:53:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[shipmentOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[shipmentId] [int] NULL,
	[carrierId] [int] NULL,
	[truckId] [int] NULL,
	[trailerId] [int] NULL,
	[driverId] [int] NULL,
	[carrier_rate] [money] NULL,
	[notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_shipmentOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[shipmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_shipmentOrder_Carrier] FOREIGN KEY([carrierId])
REFERENCES [dbo].[Carrier] ([Id])
GO

ALTER TABLE [dbo].[shipmentOrder] CHECK CONSTRAINT [FK_shipmentOrder_Carrier]
GO

ALTER TABLE [dbo].[shipmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_shipmentOrder_Driver] FOREIGN KEY([driverId])
REFERENCES [dbo].[Driver] ([Id])
GO

ALTER TABLE [dbo].[shipmentOrder] CHECK CONSTRAINT [FK_shipmentOrder_Driver]
GO

ALTER TABLE [dbo].[shipmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_shipmentOrder_shipment] FOREIGN KEY([shipmentId])
REFERENCES [dbo].[shipment] ([Id])
GO

ALTER TABLE [dbo].[shipmentOrder] CHECK CONSTRAINT [FK_shipmentOrder_shipment]
GO

ALTER TABLE [dbo].[shipmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_shipmentOrder_Trailer] FOREIGN KEY([trailerId])
REFERENCES [dbo].[Trailer] ([Id])
GO

ALTER TABLE [dbo].[shipmentOrder] CHECK CONSTRAINT [FK_shipmentOrder_Trailer]
GO

ALTER TABLE [dbo].[shipmentOrder]  WITH CHECK ADD  CONSTRAINT [FK_shipmentOrder_Truck] FOREIGN KEY([truckId])
REFERENCES [dbo].[Truck] ([Id])
GO

ALTER TABLE [dbo].[shipmentOrder] CHECK CONSTRAINT [FK_shipmentOrder_Truck]
GO

