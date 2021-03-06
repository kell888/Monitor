USE [master]
GO
/****** Object:  Database [SDG]    Script Date: 04/07/2016 20:42:45 ******/
CREATE DATABASE [SDG] ON  PRIMARY 
( NAME = N'SDG', FILENAME = N'D:\sql\Data\SDG.mdf' , SIZE = 6144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SDG_log', FILENAME = N'D:\sql\Data\SDG_log.ldf' , SIZE = 3456KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SDG] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SDG].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SDG] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [SDG] SET ANSI_NULLS OFF
GO
ALTER DATABASE [SDG] SET ANSI_PADDING OFF
GO
ALTER DATABASE [SDG] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [SDG] SET ARITHABORT OFF
GO
ALTER DATABASE [SDG] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [SDG] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [SDG] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [SDG] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [SDG] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [SDG] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [SDG] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [SDG] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [SDG] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [SDG] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [SDG] SET  DISABLE_BROKER
GO
ALTER DATABASE [SDG] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [SDG] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [SDG] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [SDG] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [SDG] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [SDG] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [SDG] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [SDG] SET  READ_WRITE
GO
ALTER DATABASE [SDG] SET RECOVERY FULL
GO
ALTER DATABASE [SDG] SET  MULTI_USER
GO
ALTER DATABASE [SDG] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [SDG] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'SDG', N'ON'
GO
USE [SDG]
GO
/****** Object:  Table [dbo].[s_status_type]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[s_status_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status_type_id] [smallint] NOT NULL,
	[status_type_name] [varchar](40) NULL,
 CONSTRAINT [PK_s_status_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[s_point_type]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[s_point_type](
	[id] [smallint] IDENTITY(1,1) NOT NULL,
	[device_type_id] [smallint] NOT NULL,
	[point_type_id] [smallint] NOT NULL,
	[point_type_name] [varchar](50) NOT NULL,
	[point_type_unit] [varchar](50) NULL,
	[point_type_state] [int] NOT NULL,
 CONSTRAINT [PK_s_point_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'设备类型id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N's_point_type', @level2type=N'COLUMN',@level2name=N'device_type_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N's_point_type', @level2type=N'COLUMN',@level2name=N'point_type_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对象名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N's_point_type', @level2type=N'COLUMN',@level2name=N'point_type_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'测点单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N's_point_type', @level2type=N'COLUMN',@level2name=N'point_type_unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N's_point_type', @level2type=N'COLUMN',@level2name=N'point_type_state'
GO
/****** Object:  Table [dbo].[s_device_type]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[s_device_type](
	[id] [smallint] IDENTITY(1,1) NOT NULL,
	[device_type_id] [smallint] NOT NULL,
	[device_type_name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_s_device_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[s_device_type] ON
INSERT [dbo].[s_device_type] ([id], [device_type_id], [device_type_name]) VALUES (1, 1, N'受电弓')
SET IDENTITY_INSERT [dbo].[s_device_type] OFF
/****** Object:  Table [dbo].[s_argument]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[s_argument](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[device_type_id] [smallint] NULL,
	[point_type_id] [smallint] NULL,
	[argument_name] [varchar](50) NOT NULL,
	[standard_value] [varchar](50) NULL,
	[min_value] [varchar](50) NULL,
	[max_value] [varchar](50) NULL,
	[valueIsNumeric] [bit] NOT NULL,
	[isRange] [bit] NOT NULL,
	[isEnable] [bit] NOT NULL,
 CONSTRAINT [PK_s_argument] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_train]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_train](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[train_no] [varchar](50) NOT NULL,
	[count] [int] NOT NULL,
 CONSTRAINT [PK_m_train] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_station]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_station](
	[id] [int] NOT NULL,
	[line_no] [varchar](50) NOT NULL,
	[station_name] [varchar](50) NOT NULL,
	[Vedio_count] [int] NOT NULL,
 CONSTRAINT [unique_stationid] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[m_device]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[m_device](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[train_id] [int] NOT NULL,
	[device_name] [varchar](50) NOT NULL,
	[device_type_id] [smallint] NOT NULL,
	[address] [varchar](128) NOT NULL,
 CONSTRAINT [PK_m_device] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[d_train_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_train_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[train_id] [int] NOT NULL,
	[direction] [bit] NULL,
	[station_id] [int] NULL,
	[come_time] [datetime] NULL,
	[go_time] [datetime] NULL,
	[alarm_count] [int] NULL,
	[alarm_status] [smallint] NULL,
 CONSTRAINT [PK_d_train_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_ping_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_ping_log](
	[ip] [varchar](50) NOT NULL,
	[onOff] [bit] NOT NULL,
	[pingtime] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[d_ping_log] ([ip], [onOff], [pingtime]) VALUES (N'192.168.16.90', 1, CAST(0x0000A5D1011A8788 AS DateTime))
INSERT [dbo].[d_ping_log] ([ip], [onOff], [pingtime]) VALUES (N'192.168.16.95', 1, CAST(0x0000A5D1011A9330 AS DateTime))
INSERT [dbo].[d_ping_log] ([ip], [onOff], [pingtime]) VALUES (N'192.168.1.10', 0, CAST(0x0000A5DA0118BA1C AS DateTime))
/****** Object:  Table [dbo].[d_picVid_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_picVid_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[filepath] [varchar](250) NULL,
	[train_log_id] [int] NULL,
	[isVideo] [bit] NOT NULL,
	[saveTime] [datetime] NOT NULL,
 CONSTRAINT [PK_d_picVid] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[d_login_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_login_log](
	[N_User] [varchar](10) NULL,
	[N_IP] [varchar](30) NULL,
	[N_Operator] [varchar](20) NULL,
	[N_UserTime] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D10111A309 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D101135C4E AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D10116F63D AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1011B7AB2 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1011BEE0E AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1011DADC4 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1011E1267 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D200F8F894 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D200FFEA17 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D201015170 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D8013C57D0 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D8013DDA11 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA00F36C0D AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA00F89797 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA00FC4157 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA01076734 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA010DDDB9 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA010F1C72 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA010FF6D7 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DB010E12F7 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DB01113692 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DB011E3309 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DB012239A8 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DE0101BB8E AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1012AE7EB AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D101380140 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D101388E6E AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1013A9E08 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1013B1D76 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5D1013E7EDD AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'bill', N'192.168.1.10', N'检修用户', CAST(0x0000A5D1013F3DA5 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA011183DB AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA01127824 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA01141D81 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA01157275 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA0115EF31 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA0118F33D AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA011BE8D2 AS DateTime))
INSERT [dbo].[d_login_log] ([N_User], [N_IP], [N_Operator], [N_UserTime]) VALUES (N'kell', N'192.168.1.10', N'管理员', CAST(0x0000A5DA011D61DC AS DateTime))
/****** Object:  Table [dbo].[d_error_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_error_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[err_level] [tinyint] NOT NULL,
	[err_source] [varchar](500) NULL,
	[err_msg] [varchar](500) NULL,
	[err_client] [varchar](50) NULL,
	[err_time] [datetime] NOT NULL,
 CONSTRAINT [PK_d_err_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误的级别：默认0为一般错误，1为数据库操作错误，2为应用程序内部错误' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_error_log', @level2type=N'COLUMN',@level2name=N'err_level'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息来源，包括程序集+命名空间+类+方法' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_error_log', @level2type=N'COLUMN',@level2name=N'err_source'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_error_log', @level2type=N'COLUMN',@level2name=N'err_msg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误信息产生的客户端，格式：IP(MAC)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_error_log', @level2type=N'COLUMN',@level2name=N'err_client'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误发生的时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'd_error_log', @level2type=N'COLUMN',@level2name=N'err_time'
GO
/****** Object:  Table [dbo].[d_data_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[d_data_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[point_type_id] [smallint] NULL,
	[train_log_id] [int] NULL,
	[device_id] [int] NULL,
	[data_value] [numeric](10, 2) NULL,
	[flash_time] [datetime] NOT NULL,
	[alarm_status] [smallint] NULL,
 CONSTRAINT [PK_d_data_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[d_alarmAction_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_alarmAction_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[alarm_id] [int] NOT NULL,
	[confirm_time] [datetime] NULL,
	[process_time] [datetime] NULL,
	[alarm_msg] [varchar](500) NULL,
	[process_msg] [varchar](500) NULL,
	[confirmer_id] [int] NULL,
	[processer_id] [int] NULL,
 CONSTRAINT [PK_d_alarmAction_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[d_alarm_log]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[d_alarm_log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[train_log_id] [int] NOT NULL,
	[device_id] [int] NULL,
	[point_type_id] [smallint] NULL,
	[start_time] [datetime] NULL,
	[end_time] [datetime] NULL,
	[alarm_value] [numeric](10, 2) NULL,
	[alarm_status] [smallint] NULL,
	[affirmance] [int] NULL,
	[remark] [varchar](500) NULL,
 CONSTRAINT [PK_d_alarm_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[a_user]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[a_user](
	[id] [int] IDENTITY(1001,1) NOT NULL,
	[login_name] [varchar](255) NOT NULL,
	[password] [varchar](255) NULL,
	[operation] [int] NOT NULL,
	[add_time] [datetime] NOT NULL,
	[power_time] [datetime] NOT NULL,
 CONSTRAINT [pk_a_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[a_user] ON
INSERT [dbo].[a_user] ([id], [login_name], [password], [operation], [add_time], [power_time]) VALUES (1001, N'kell', N'f3AiN0vz5CXBjVlP326CKr9a7OFgysEYU5UdalYdqnB79/zj7UkgM3mr0xbijufo36tlf/IcJC3bSkOGFjfSdawCnD/TzugOK7b27XTKQvhz19mauuXUmJA1mczQ5JY13P/sORVdARkdosUWgsFu6b3+iIwDDYrk7LGdhhS0T/g=', 3, CAST(0x0000A5C400CE44B0 AS DateTime), CAST(0x0000B40800CE44B0 AS DateTime))
INSERT [dbo].[a_user] ([id], [login_name], [password], [operation], [add_time], [power_time]) VALUES (1002, N'bill', N'Pdo08myW2Kih6xCtGTHHm0QOKEs/Cu+/YchNTrNc41nE6tJrwrnB8jkpLs/RasG8gHooxFELU4baKGZ2aWAxkbZ8j0ZyKQxHqdi4abBHceRauL0CtyK+zNTN1rJxLbMm1AFOlAe+vBYwx1iCGlXVIc4xgZ3ap+mI5oyx+k86xlA=', 2, CAST(0x0000A5C6014E1346 AS DateTime), CAST(0x0000B40A00CE44B0 AS DateTime))
INSERT [dbo].[a_user] ([id], [login_name], [password], [operation], [add_time], [power_time]) VALUES (1003, N'lily', N'pFt9Zr5n2/rRgjqHjhnBO4TGYaohSttFEhkme7IxRc7GJWrSCKxFTa6srDzii9ZdNmXi8kHUk/6yhJPhTCchzEVo7LD6uBSjguUB9GqwfqlyWlMoDUVIdv2LJgXkKzh1HGlN9I1x7jG4zXiO3mvbCI7pToYmui6WqxWnebyGCM0=', 1, CAST(0x0000A5C6014E24DC AS DateTime), CAST(0x0000B40A00CE44B0 AS DateTime))
SET IDENTITY_INSERT [dbo].[a_user] OFF
/****** Object:  Table [dbo].[a_role]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[a_role](
	[operation] [int] NOT NULL,
	[menuid] [varchar](5000) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[a_role] ([operation], [menuid]) VALUES (3, N'4,1,9,10,2,3,6,5,7,8,11,12,13,14,15,16,17')
INSERT [dbo].[a_role] ([operation], [menuid]) VALUES (2, N'4,1,9,10,2,3,6,5,7,8')
INSERT [dbo].[a_role] ([operation], [menuid]) VALUES (1, N'1,2,7,8')
/****** Object:  Table [dbo].[a_menu]    Script Date: 04/07/2016 20:42:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[a_menu](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[ReferenceForm] [varchar](50) NOT NULL,
	[AssemblyFile] [varchar](500) NULL,
	[Args] [varchar](500) NULL,
	[SortIndex] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_a_menu] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[a_menu] ON
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (1, 2, N'最近过车', N'Monitor.Report.CxTrainReport', N'', N'{OWNER},{STARTTIME},{ENDTIME}', 0, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (2, 0, N'过车信息', N'', N'', N'', 1, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (3, 0, N'监测数据', N'', N'', N'', 2, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (4, 3, N'监测数据查询', N'Monitor.Report.SearchSDG', N'', N'{OWNER}', -1, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (5, 0, N'数据分析', N'', N'', N'', 4, 0)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (6, 3, N'监测数据分析', N'Monitor.Report.chart', N'', N'{OWNER}', 3, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (7, 0, N'系统管理', N'', N'', N'', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (8, 7, N'修改密码', N'Monitor.SystemManager.ChangePwdForm', N'', N'{OWNER},{USERNAME},{PWD}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (9, 2, N'过车信息查询', N'Monitor.Report.SearchTrain', N'', N'{OWNER}', 0, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (10, 2, N'每日过车', N'Monitor.Report.DayReportView', N'', N'{OWNER}', 0, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (11, 7, N'列车管理', N'Monitor.SystemManager.TrainManage', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (12, 7, N'监测站管理', N'Monitor.SystemManager.StationManage', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (13, 7, N'设备管理', N'Monitor.SystemManager.DeviceManage', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (14, 7, N'参数配置', N'Monitor.SystemManager.ArgumentManage', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (15, 7, N'通讯自检日志', N'Monitor.SystemManager.PingList', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (16, 7, N'用户登录日志', N'Monitor.SystemManager.LoginList', N'', N'{OWNER}', 5, 1)
INSERT [dbo].[a_menu] ([ID], [ParentID], [Name], [ReferenceForm], [AssemblyFile], [Args], [SortIndex], [Enabled]) VALUES (17, 7, N'系统错误日志', N'Monitor.SystemManager.ErrorList', N'', N'{OWNER}', 5, 1)
SET IDENTITY_INSERT [dbo].[a_menu] OFF
/****** Object:  View [dbo].[v_train_log]    Script Date: 04/07/2016 20:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_train_log]
AS
SELECT     dbo.d_train_log.id, dbo.m_train.train_no, dbo.m_station.station_name, dbo.d_train_log.come_time, dbo.d_train_log.go_time, dbo.d_train_log.alarm_count, 
                      (CASE WHEN dbo.d_train_log.direction = 0 THEN '正向' ELSE '反向' END) AS direction, dbo.s_status_type.status_type_name, dbo.m_station.line_no
FROM         dbo.d_train_log INNER JOIN
                      dbo.m_train ON dbo.d_train_log.train_id = dbo.m_train.id INNER JOIN
                      dbo.m_station ON dbo.d_train_log.station_id = dbo.m_station.id INNER JOIN
                      dbo.s_status_type ON dbo.d_train_log.alarm_status = dbo.s_status_type.status_type_id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[13] 2[32] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d_train_log"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 191
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_train"
            Begin Extent = 
               Top = 0
               Left = 257
               Bottom = 104
               Right = 399
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_station"
            Begin Extent = 
               Top = 2
               Left = 502
               Bottom = 136
               Right = 656
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s_status_type"
            Begin Extent = 
               Top = 109
               Left = 256
               Bottom = 213
               Right = 435
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_train_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_train_log'
GO
/****** Object:  View [dbo].[v_error_log]    Script Date: 04/07/2016 20:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_error_log]
AS
SELECT     id, (CASE WHEN err_level = 0 THEN '一般错误' WHEN err_level = 1 THEN '数据库操作错误' WHEN err_level = 2 THEN '应用程序内部错误' ELSE '未知错误' END) 
                      AS 错误级别, err_source AS 错误来源, err_msg AS 错误信息, err_client AS 错误客户端, err_time AS 发生时间
FROM         dbo.d_error_log
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d_error_log"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 193
               Right = 188
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_error_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_error_log'
GO
/****** Object:  View [dbo].[v_data_log]    Script Date: 04/07/2016 20:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_data_log]
AS
SELECT     dbo.d_data_log.id, dbo.s_point_type.point_type_name, dbo.d_data_log.data_value, dbo.d_data_log.flash_time, dbo.m_station.station_name, dbo.m_train.train_no, 
                      dbo.m_device.device_name, (CASE WHEN dbo.d_train_log.direction = 0 THEN '正向' ELSE '反向' END) AS direction, dbo.m_station.line_no, 
                      dbo.s_status_type.status_type_name
FROM         dbo.d_data_log INNER JOIN
                      dbo.d_train_log ON dbo.d_data_log.train_log_id = dbo.d_train_log.id INNER JOIN
                      dbo.m_station ON dbo.d_train_log.station_id = dbo.m_station.id INNER JOIN
                      dbo.m_train ON dbo.d_train_log.train_id = dbo.m_train.id INNER JOIN
                      dbo.m_device ON dbo.d_data_log.device_id = dbo.m_device.id INNER JOIN
                      dbo.s_point_type ON dbo.d_data_log.point_type_id = dbo.s_point_type.point_type_id INNER JOIN
                      dbo.s_status_type ON dbo.d_data_log.alarm_status = dbo.s_status_type.status_type_id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[59] 4[5] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "m_station"
            Begin Extent = 
               Top = 5
               Left = 630
               Bottom = 124
               Right = 793
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_train"
            Begin Extent = 
               Top = 149
               Left = 649
               Bottom = 253
               Right = 791
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_device"
            Begin Extent = 
               Top = 201
               Left = 68
               Bottom = 320
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s_point_type"
            Begin Extent = 
               Top = 13
               Left = 0
               Bottom = 197
               Right = 168
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d_train_log"
            Begin Extent = 
               Top = 7
               Left = 421
               Bottom = 207
               Right = 572
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d_data_log"
            Begin Extent = 
               Top = 7
               Left = 223
               Bottom = 190
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s_status_type"
            Begin Extent = 
               Top = 210
               Left = 268
               Bottom = 314
               Right = 447
            End
            Di' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_data_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'splayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_data_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_data_log'
GO
/****** Object:  View [dbo].[v_alarm_log]    Script Date: 04/07/2016 20:42:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_alarm_log]
AS
SELECT     dbo.d_alarm_log.id, dbo.m_train.train_no, dbo.m_device.device_name, dbo.s_point_type.point_type_name, dbo.d_alarm_log.start_time, dbo.d_alarm_log.end_time, 
                      dbo.d_alarm_log.alarm_value, (CASE dbo.d_alarm_log.affirmance WHEN 0 THEN '未确认' WHEN 1 THEN '已确认' WHEN 2 THEN '已处理' END) AS affirmance, 
                      dbo.d_alarm_log.remark, dbo.m_station.station_name, (CASE WHEN dbo.d_train_log.direction = 0 THEN '正向' ELSE '反向' END) AS direction, 
                      dbo.s_status_type.status_type_name, dbo.d_alarmAction_log.alarm_msg, dbo.d_alarmAction_log.process_msg, dbo.d_alarmAction_log.confirm_time, 
                      dbo.d_alarmAction_log.process_time, dbo.m_station.line_no, dbo.m_station.line_no + ' ' + dbo.m_station.station_name AS address
FROM         dbo.d_train_log INNER JOIN
                      dbo.m_train ON dbo.d_train_log.train_id = dbo.m_train.id INNER JOIN
                      dbo.d_alarm_log ON dbo.d_train_log.id = dbo.d_alarm_log.train_log_id INNER JOIN
                      dbo.m_device ON dbo.d_alarm_log.device_id = dbo.m_device.id INNER JOIN
                      dbo.m_station ON dbo.d_train_log.station_id = dbo.m_station.id INNER JOIN
                      dbo.s_point_type ON dbo.d_alarm_log.point_type_id = dbo.s_point_type.point_type_id INNER JOIN
                      dbo.s_status_type ON dbo.d_alarm_log.alarm_status = dbo.s_status_type.status_type_id INNER JOIN
                      dbo.d_alarmAction_log ON dbo.d_alarm_log.id = dbo.d_alarmAction_log.alarm_id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[75] 4[4] 2[11] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d_train_log"
            Begin Extent = 
               Top = 29
               Left = 187
               Bottom = 213
               Right = 338
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_train"
            Begin Extent = 
               Top = 12
               Left = 2
               Bottom = 117
               Right = 154
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d_alarm_log"
            Begin Extent = 
               Top = 12
               Left = 366
               Bottom = 222
               Right = 521
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_device"
            Begin Extent = 
               Top = 255
               Left = 551
               Bottom = 390
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "m_station"
            Begin Extent = 
               Top = 141
               Left = 2
               Bottom = 245
               Right = 156
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s_point_type"
            Begin Extent = 
               Top = 249
               Left = 2
               Bottom = 400
               Right = 156
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s_status_type"
            Begin Extent = 
               Top = 314
               Left = 181
               Bottom = 418
               Right = 335
            End
            ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_alarm_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "d_alarmAction_log"
            Begin Extent = 
               Top = 11
               Left = 556
               Bottom = 191
               Right = 711
            End
            DisplayFlags = 280
            TopColumn = 1
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_alarm_log'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'v_alarm_log'
GO
/****** Object:  Default [DF_s_point_type_point_type_state]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[s_point_type] ADD  CONSTRAINT [DF_s_point_type_point_type_state]  DEFAULT ((0)) FOR [point_type_state]
GO
/****** Object:  Default [DF_s_argument_valueIsNumeric]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[s_argument] ADD  CONSTRAINT [DF_s_argument_valueIsNumeric]  DEFAULT ((1)) FOR [valueIsNumeric]
GO
/****** Object:  Default [DF_s_argument_isRange]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[s_argument] ADD  CONSTRAINT [DF_s_argument_isRange]  DEFAULT ((0)) FOR [isRange]
GO
/****** Object:  Default [DF_s_argument_isEnable]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[s_argument] ADD  CONSTRAINT [DF_s_argument_isEnable]  DEFAULT ((1)) FOR [isEnable]
GO
/****** Object:  Default [DF_m_station_Vedio_count]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[m_station] ADD  CONSTRAINT [DF_m_station_Vedio_count]  DEFAULT ((1)) FOR [Vedio_count]
GO
/****** Object:  Default [DF_d_ping_onOff]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_ping_log] ADD  CONSTRAINT [DF_d_ping_onOff]  DEFAULT ((0)) FOR [onOff]
GO
/****** Object:  Default [DF_d_ping_pingtime]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_ping_log] ADD  CONSTRAINT [DF_d_ping_pingtime]  DEFAULT (getdate()) FOR [pingtime]
GO
/****** Object:  Default [DF_d_picVid_isVideo]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_picVid_log] ADD  CONSTRAINT [DF_d_picVid_isVideo]  DEFAULT ((0)) FOR [isVideo]
GO
/****** Object:  Default [DF_d_picVid_log_saveTime]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_picVid_log] ADD  CONSTRAINT [DF_d_picVid_log_saveTime]  DEFAULT (getdate()) FOR [saveTime]
GO
/****** Object:  Default [DF_d_Loing_N_UserTime]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_login_log] ADD  CONSTRAINT [DF_d_Loing_N_UserTime]  DEFAULT (getdate()) FOR [N_UserTime]
GO
/****** Object:  Default [DF_d_err_log_err_level]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_error_log] ADD  CONSTRAINT [DF_d_err_log_err_level]  DEFAULT ((0)) FOR [err_level]
GO
/****** Object:  Default [DF_d_err_log_err_time]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_error_log] ADD  CONSTRAINT [DF_d_err_log_err_time]  DEFAULT (getdate()) FOR [err_time]
GO
/****** Object:  Default [DF_d_data_log_flash_time]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[d_data_log] ADD  CONSTRAINT [DF_d_data_log_flash_time]  DEFAULT (getdate()) FOR [flash_time]
GO
/****** Object:  Default [DF_a_user_add_time]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[a_user] ADD  CONSTRAINT [DF_a_user_add_time]  DEFAULT (getdate()) FOR [add_time]
GO
/****** Object:  Default [DF_a_menu_ParentID]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[a_menu] ADD  CONSTRAINT [DF_a_menu_ParentID]  DEFAULT ((0)) FOR [ParentID]
GO
/****** Object:  Default [DF_a_menu_SortIndex]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[a_menu] ADD  CONSTRAINT [DF_a_menu_SortIndex]  DEFAULT ((0)) FOR [SortIndex]
GO
/****** Object:  Default [DF_a_menu_Enabled]    Script Date: 04/07/2016 20:42:47 ******/
ALTER TABLE [dbo].[a_menu] ADD  CONSTRAINT [DF_a_menu_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
