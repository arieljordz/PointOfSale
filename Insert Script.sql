USE [POSDB]
GO
/****** Object:  Table [dbo].[tbl_bank]    Script Date: 5/31/2023 1:31:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_bank](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_bank] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_item]    Script Date: 5/31/2023 1:31:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_item](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Brand] [nvarchar](max) NULL,
	[Supplier] [nvarchar](max) NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DateAdded] [datetime2](7) NOT NULL,
	[DateExpired] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_tbl_item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_user]    Script Date: 5/31/2023 1:31:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_user](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[MiddleName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[UsertypeId] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[DateRegistered] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_tbl_user] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_userType]    Script Date: 5/31/2023 1:31:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_userType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_tbl_userType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_bank] ON 

INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (1, N'BDO')
INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (2, N'RCBC')
INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (3, N'Land Bank')
INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (4, N'Metro Bank')
INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (5, N'BPI')
INSERT [dbo].[tbl_bank] ([Id], [Description]) VALUES (6, N'China Bank')
SET IDENTITY_INSERT [dbo].[tbl_bank] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_item] ON 

INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (1, N'Product 1', N'Product 1', N'Product 1', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:25:05.4573244' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (2, N'Product 2', N'Product 2', N'Product 2', 100, CAST(15.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:25:26.1962560' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (3, N'Product 3', N'Product 3', N'Product 3', 100, CAST(5.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:25:45.1026149' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (4, N'Product 4', N'Product 4', N'Product 4', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:26:07.0406945' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (5, N'Product 5', N'Product 5', N'Product 5', 100, CAST(5.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:26:32.5965059' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (6, N'Product 6', N'Product 6', N'Product 6', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:26:49.7946193' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (7, N'Product 7', N'Product 7', N'Product 7', 100, CAST(20.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:27:08.3274218' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (8, N'Product 8', N'Product 8', N'Product 8', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:27:29.1102247' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (9, N'Product 9', N'Product 9', N'Product 9', 100, CAST(10.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:27:44.9830339' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[tbl_item] ([Id], [Description], [Brand], [Supplier], [Quantity], [Price], [DateAdded], [DateExpired]) VALUES (10, N'Product 10', N'Product 10', N'Product 10', 100, CAST(5.00 AS Decimal(18, 2)), CAST(N'2023-05-31T01:28:01.9164138' AS DateTime2), CAST(N'2023-05-31T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[tbl_item] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_user] ON 

INSERT [dbo].[tbl_user] ([Id], [FirstName], [MiddleName], [LastName], [FullName], [UsertypeId], [UserName], [Password], [DateRegistered]) VALUES (4, N'Jordz', N'Jordz', N'Jordz', N'Jordz J. Jordz', 1, N'jordz', N'jordz', CAST(N'2023-05-20T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[tbl_user] OFF
GO
SET IDENTITY_INSERT [dbo].[tbl_userType] ON 

INSERT [dbo].[tbl_userType] ([Id], [Description]) VALUES (1, N'Manager')
INSERT [dbo].[tbl_userType] ([Id], [Description]) VALUES (2, N'Cashier')
SET IDENTITY_INSERT [dbo].[tbl_userType] OFF
GO
