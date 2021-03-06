USE [master]
GO
/****** Object:  Database [TrainCommander]    Script Date: 20/01/2016 04:01:18 ******/
CREATE DATABASE [TrainCommander]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TrainCommander', FILENAME = N'C:\Users\anthony\TrainCommander.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TrainCommander_log', FILENAME = N'C:\Users\anthony\TrainCommander_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TrainCommander] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TrainCommander].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TrainCommander] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TrainCommander] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TrainCommander] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TrainCommander] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TrainCommander] SET ARITHABORT OFF 
GO
ALTER DATABASE [TrainCommander] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TrainCommander] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TrainCommander] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TrainCommander] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TrainCommander] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TrainCommander] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TrainCommander] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TrainCommander] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TrainCommander] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TrainCommander] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TrainCommander] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TrainCommander] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TrainCommander] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TrainCommander] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TrainCommander] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TrainCommander] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TrainCommander] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TrainCommander] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TrainCommander] SET  MULTI_USER 
GO
ALTER DATABASE [TrainCommander] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TrainCommander] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TrainCommander] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TrainCommander] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [TrainCommander]
GO
/****** Object:  Table [dbo].[City]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order_SuperTrip]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order_SuperTrip](
	[order_id] [int] NOT NULL,
	[supertrip_id] [int] NOT NULL,
 CONSTRAINT [PK_ORDER_SUPERTRIP] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[supertrip_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Station]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Station](
	[city_id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SuperTrip]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuperTrip](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_departure_station] [int] NOT NULL,
	[id_arrival_station] [int] NOT NULL,
	[departure_date] [datetime] NOT NULL,
	[arrival_date] [datetime] NOT NULL,
	[price] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Train]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Train](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_station] [int] NOT NULL,
	[seat_number] [int] NOT NULL,
	[remaining_seat] [int] NULL,
	[is_double] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trip]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_departure_station] [int] NOT NULL,
	[id_arrival_station] [int] NOT NULL,
	[id_train] [int] NOT NULL,
	[departure_date] [datetime] NOT NULL,
	[arrival_date] [datetime] NOT NULL,
	[price] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Trip_SuperTrip]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip_SuperTrip](
	[id_supertrip] [int] NOT NULL,
	[id_trip] [int] NOT NULL,
 CONSTRAINT [PK_TRIP_SUPERTRIP] PRIMARY KEY CLUSTERED 
(
	[id_supertrip] ASC,
	[id_trip] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User_Order]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Order](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NOT NULL,
	[status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Order_SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_ORDERSUPERTRIP_ORDER_ID] FOREIGN KEY([order_id])
REFERENCES [dbo].[User_Order] ([id])
GO
ALTER TABLE [dbo].[Order_SuperTrip] CHECK CONSTRAINT [FK_ORDERSUPERTRIP_ORDER_ID]
GO
ALTER TABLE [dbo].[Order_SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_ORDERSUPERTRIP_SUPERTRIP_ID] FOREIGN KEY([supertrip_id])
REFERENCES [dbo].[SuperTrip] ([id])
GO
ALTER TABLE [dbo].[Order_SuperTrip] CHECK CONSTRAINT [FK_ORDERSUPERTRIP_SUPERTRIP_ID]
GO
ALTER TABLE [dbo].[Station]  WITH CHECK ADD  CONSTRAINT [FK_STATION_CITY_ID] FOREIGN KEY([city_id])
REFERENCES [dbo].[City] ([id])
GO
ALTER TABLE [dbo].[Station] CHECK CONSTRAINT [FK_STATION_CITY_ID]
GO
ALTER TABLE [dbo].[SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_SUPERTRIP_ARRIVAL_STATION_ID] FOREIGN KEY([id_arrival_station])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[SuperTrip] CHECK CONSTRAINT [FK_SUPERTRIP_ARRIVAL_STATION_ID]
GO
ALTER TABLE [dbo].[SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_SUPERTRIP_DEPARTURE_STATION_ID] FOREIGN KEY([id_departure_station])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[SuperTrip] CHECK CONSTRAINT [FK_SUPERTRIP_DEPARTURE_STATION_ID]
GO
ALTER TABLE [dbo].[Train]  WITH CHECK ADD  CONSTRAINT [FK_TRAIN_STATION_ID] FOREIGN KEY([id_station])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[Train] CHECK CONSTRAINT [FK_TRAIN_STATION_ID]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_TRAIN_TRAINID] FOREIGN KEY([id_train])
REFERENCES [dbo].[Train] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_TRAIN_TRAINID]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_TRIP_ARRIVAL_STATION_ID] FOREIGN KEY([id_arrival_station])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_TRIP_ARRIVAL_STATION_ID]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_TRIP_DEPARTURE_STATION_ID] FOREIGN KEY([id_departure_station])
REFERENCES [dbo].[Station] ([Id])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_TRIP_DEPARTURE_STATION_ID]
GO
ALTER TABLE [dbo].[Trip_SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_SUPERTRIP_ID] FOREIGN KEY([id_supertrip])
REFERENCES [dbo].[SuperTrip] ([id])
GO
ALTER TABLE [dbo].[Trip_SuperTrip] CHECK CONSTRAINT [FK_SUPERTRIP_ID]
GO
ALTER TABLE [dbo].[Trip_SuperTrip]  WITH CHECK ADD  CONSTRAINT [FK_TRIP_ID] FOREIGN KEY([id_trip])
REFERENCES [dbo].[Trip] ([id])
GO
ALTER TABLE [dbo].[Trip_SuperTrip] CHECK CONSTRAINT [FK_TRIP_ID]
GO
/****** Object:  StoredProcedure [dbo].[ps_city_getAllCity]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ps_city_getAllCity]
AS
	SELECT name FROM City;
GO
/****** Object:  StoredProcedure [dbo].[ps_station_getStationByCityId]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ps_station_getStationByCityId] @id_city INT
AS
	SELECT name 
	FROM Station
	WHERE city_id = @id_city
GO
/****** Object:  StoredProcedure [dbo].[ps_supertrip_getTripsBySuperTripID]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ps_supertrip_getTripsBySuperTripID] @id_supertrip INT
AS
	SELECT * FROM Trip as t
	WHERE id IN (SELECT id FROM Trip_SuperTrip WHERE id_supertrip = @id_supertrip);
GO
/****** Object:  StoredProcedure [dbo].[ps_trip_getTripByStationsId]    Script Date: 20/01/2016 04:01:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ps_trip_getTripByStationsId] @id_departure INT, @id_arrival INT
AS
	SELECT * FROM Trip
	WHERE id_departure_station = @id_departure AND id_arrival_station = @id_arrival;
GO
USE [master]
GO
ALTER DATABASE [TrainCommander] SET  READ_WRITE 
GO
