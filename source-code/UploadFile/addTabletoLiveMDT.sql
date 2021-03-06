
GO
/****** Object:  Table [dbo].[mdt_friend_relation]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_friend_relation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[UserIdFriend] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Status] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[EmailFriend] [nvarchar](500) NULL,
	[Token] [nvarchar](500) NULL,
	[InvitationWaiting] [bit] NULL,
	[IsProfile1] [int] NULL,
	[IsProfile2] [int] NULL,
 CONSTRAINT [PK_mdt_friend_relation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_gallery]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_gallery](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEvent] [int] NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Caption] [nvarchar](50) NULL,
 CONSTRAINT [PK_mdt_gallary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_group]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](200) NOT NULL,
	[Status] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[UpdatedDate] [datetime] NULL,
	[GroupLogo] [nvarchar](max) NULL,
	[GroupSlogan] [nvarchar](500) NULL,
 CONSTRAINT [PK_mdt_group] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_group_detail]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_group_detail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[KeyData] [nvarchar](50) NULL,
	[ValueData] [nvarchar](max) NULL,
 CONSTRAINT [PK_mdt_group_detail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_group_post]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_group_post](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[PostId] [int] NOT NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_mdt_group_post] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_group_user]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_group_user](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Status] [int] NULL,
	[Remark] [nvarchar](50) NULL,
 CONSTRAINT [PK_mdt_group_user] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_notification_addfriend]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mdt_notification_addfriend](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdUser] [int] NULL,
	[IdFriend] [int] NULL,
	[Type] [nvarchar](50) NULL,
	[StatusRead] [bit] NULL CONSTRAINT [DF_mdt_notification_addfriend_StatusRead]  DEFAULT ((0)),
	[StatusReadCount] [bit] NULL CONSTRAINT [DF_mdt_notification_addfriend_StatusReadCount]  DEFAULT ((0)),
	[CreateDate] [datetime] NULL,
	[IdPost] [int] NULL,
 CONSTRAINT [PK_mdt_notification_addfriend] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[mdt_post_permession]    Script Date: 08/16/2018 10:50:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mdt_post_permession](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPost] [int] NULL,
	[IdUser] [varchar](max) NULL,
	[IdGroup] [nvarchar](50) NULL,
 CONSTRAINT [PK_mdt_post_permession] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[mdt_friend_relation]  WITH CHECK ADD  CONSTRAINT [FK_mdt_friend_relation_mdt_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[mdt_user] ([ID])
GO
ALTER TABLE [dbo].[mdt_friend_relation] CHECK CONSTRAINT [FK_mdt_friend_relation_mdt_user]
GO
ALTER TABLE [dbo].[mdt_friend_relation]  WITH CHECK ADD  CONSTRAINT [FK_mdt_friend_relation_mdt_user_friend] FOREIGN KEY([UserIdFriend])
REFERENCES [dbo].[mdt_user] ([ID])
GO
ALTER TABLE [dbo].[mdt_friend_relation] CHECK CONSTRAINT [FK_mdt_friend_relation_mdt_user_friend]
GO
ALTER TABLE [dbo].[mdt_group_detail]  WITH CHECK ADD  CONSTRAINT [FK_mdt_group_detail_mdt_group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[mdt_group] ([Id])
GO
ALTER TABLE [dbo].[mdt_group_detail] CHECK CONSTRAINT [FK_mdt_group_detail_mdt_group]
GO
ALTER TABLE [dbo].[mdt_group_post]  WITH CHECK ADD  CONSTRAINT [FK_mdt_group_post_mdt_group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[mdt_group] ([Id])
GO
ALTER TABLE [dbo].[mdt_group_post] CHECK CONSTRAINT [FK_mdt_group_post_mdt_group]
GO
ALTER TABLE [dbo].[mdt_group_post]  WITH CHECK ADD  CONSTRAINT [FK_mdt_group_post_mdt_post] FOREIGN KEY([PostId])
REFERENCES [dbo].[mdt_post] ([Id])
GO
ALTER TABLE [dbo].[mdt_group_post] CHECK CONSTRAINT [FK_mdt_group_post_mdt_post]
GO
ALTER TABLE [dbo].[mdt_group_user]  WITH CHECK ADD  CONSTRAINT [FK_mdt_group_user_mdt_group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[mdt_group] ([Id])
GO
ALTER TABLE [dbo].[mdt_group_user] CHECK CONSTRAINT [FK_mdt_group_user_mdt_group]
GO
ALTER TABLE [dbo].[mdt_group_user]  WITH CHECK ADD  CONSTRAINT [FK_mdt_group_user_mdt_user] FOREIGN KEY([UserId])
REFERENCES [dbo].[mdt_user] ([ID])
GO
ALTER TABLE [dbo].[mdt_group_user] CHECK CONSTRAINT [FK_mdt_group_user_mdt_user]
GO
