IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'AspNetForums')
	DROP DATABASE [AspNetForums]
GO

CREATE DATABASE [AspNetForums]  ON (NAME = N'AspNetForums', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\AspNetForums.mdf' , SIZE = 2, FILEGROWTH = 10%) LOG ON (NAME = N'AspNetForums_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\AspNetForums_log.LDF' , FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

exec sp_dboption N'AspNetForums', N'autoclose', N'false'
GO

exec sp_dboption N'AspNetForums', N'bulkcopy', N'false'
GO

exec sp_dboption N'AspNetForums', N'trunc. log', N'false'
GO

exec sp_dboption N'AspNetForums', N'torn page detection', N'true'
GO

exec sp_dboption N'AspNetForums', N'read only', N'false'
GO

exec sp_dboption N'AspNetForums', N'dbo use', N'false'
GO

exec sp_dboption N'AspNetForums', N'single', N'false'
GO

exec sp_dboption N'AspNetForums', N'autoshrink', N'false'
GO

exec sp_dboption N'AspNetForums', N'ANSI null default', N'false'
GO

exec sp_dboption N'AspNetForums', N'recursive triggers', N'false'
GO

exec sp_dboption N'AspNetForums', N'ANSI nulls', N'false'
GO

exec sp_dboption N'AspNetForums', N'concat null yields null', N'false'
GO

exec sp_dboption N'AspNetForums', N'cursor close on commit', N'false'
GO

exec sp_dboption N'AspNetForums', N'default to local cursor', N'false'
GO

exec sp_dboption N'AspNetForums', N'quoted identifier', N'false'
GO

exec sp_dboption N'AspNetForums', N'ANSI warnings', N'false'
GO

exec sp_dboption N'AspNetForums', N'auto create statistics', N'true'
GO

exec sp_dboption N'AspNetForums', N'auto update statistics', N'true'
GO

use [AspNetForums]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Posts_Forums]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Posts] DROP CONSTRAINT FK_Posts_Forums
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_PrivateForums_UserRoles]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[PrivateForums] DROP CONSTRAINT FK_PrivateForums_UserRoles
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_UserRoles]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_UserRoles
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Moderators_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Moderators] DROP CONSTRAINT FK_Moderators_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Posts_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Posts] DROP CONSTRAINT FK_Posts_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ThreadTrackings_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ThreadTrackings] DROP CONSTRAINT FK_ThreadTrackings_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_UsersInRoles_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[UsersInRoles] DROP CONSTRAINT FK_UsersInRoles_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AnonymousUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AnonymousUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Emails]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Emails]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ForumGroups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ForumGroups]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Forums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Forums]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ForumsRead]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ForumsRead]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Messages]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Messages]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModerationAction]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ModerationAction]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModerationAudit]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ModerationAudit]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Moderators]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Moderators]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Posts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Posts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PostsRead]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PostsRead]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PrivateForums]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[PrivateForums]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ThreadTrackings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ThreadTrackings]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UserRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UserRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UsersInRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[UsersInRoles]
GO

CREATE TABLE [dbo].[AnonymousUsers] (
	[UserId] [char] (36) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LastLogin] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Emails] (
	[EmailID] [int] IDENTITY (1, 1) NOT NULL ,
	[Subject] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Message] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Importance] [int] NOT NULL ,
	[FromAddress] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[ForumGroups] (
	[ForumGroupId] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[SortOrder] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Forums] (
	[ForumID] [int] IDENTITY (1, 1) NOT NULL ,
	[ForumGroupId] [int] NOT NULL ,
	[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[Moderated] [bit] NOT NULL ,
	[DaysToView] [int] NOT NULL ,
	[Active] [bit] NOT NULL ,
	[SortOrder] [int] NOT NULL ,
	[TotalPosts] [int] NOT NULL ,
	[TotalThreads] [int] NOT NULL ,
	[MostRecentPostID] [int] NOT NULL ,
	[MostRecentThreadID] [int] NOT NULL ,
	[MostRecentPostDate] [datetime] NULL ,
	[MostRecentPostAuthor] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ForumsRead] (
	[ForumId] [int] NOT NULL ,
	[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MarkReadAfter] [int] NOT NULL ,
	[LastActivity] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Messages] (
	[MessageId] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Body] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ModerationAction] (
	[ModerationAction] [int] NOT NULL ,
	[Description] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ModerationAudit] (
	[ModeratedOn] [datetime] NOT NULL ,
	[PostId] [int] NOT NULL ,
	[ModeratedBy] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ModerationAction] [int] NOT NULL ,
	[Notes] [nvarchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Moderators] (
	[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ForumID] [int] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[EmailNotification] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Posts] (
	[PostID] [int] IDENTITY (1, 1) NOT NULL ,
	[ThreadID] [int] NOT NULL ,
	[ParentID] [int] NOT NULL ,
	[PostLevel] [int] NOT NULL ,
	[SortOrder] [int] NOT NULL ,
	[Subject] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PostDate] [datetime] NOT NULL ,
	[Body] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Approved] [bit] NOT NULL ,
	[ForumID] [int] NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ThreadDate] [datetime] NOT NULL ,
	[TotalViews] [int] NOT NULL ,
	[IsLocked] [bit] NOT NULL ,
	[IsPinned] [bit] NOT NULL ,
	[PinnedDate] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[PostsRead] (
	[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PostId] [int] NOT NULL ,
	[HasRead] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PrivateForums] (
	[ForumId] [int] NOT NULL ,
	[RoleName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ThreadTrackings] (
	[ThreadID] [int] NOT NULL ,
	[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[UserRoles] (
	[RoleName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Users] (
	[UserName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Email] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ForumView] [int] NOT NULL ,
	[ProfileApproved] [bit] NOT NULL ,
	[Approved] [bit] NOT NULL ,
	[Trusted] [bit] NOT NULL ,
	[FakeEmail] [nvarchar] (75) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[URL] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Signature] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[TrackYourPosts] [bit] NOT NULL ,
	[LastLogin] [datetime] NOT NULL ,
	[LastActivity] [datetime] NOT NULL ,
	[TimeZone] [int] NOT NULL ,
	[Location] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Occupation] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Interests] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[MSN] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Yahoo] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[AIM] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ICQ] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[TotalPosts] [int] NOT NULL ,
	[HasIcon] [bit] NOT NULL ,
	[ShowUnreadTopicsOnly] [bit] NOT NULL ,
	[Style] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ImageType] [nvarchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ShowIcon] [bit] NOT NULL ,
	[DateFormat] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[PostViewOrder] [bit] NOT NULL ,
	[FlatView] [bit] NOT NULL ,
	[DisplayInMemberList] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[UsersInRoles] (
	[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Rolename] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AnonymousUsers] WITH NOCHECK ADD 
	CONSTRAINT [PK_AnonymousUsers] PRIMARY KEY  CLUSTERED 
	(
		[UserId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Emails] WITH NOCHECK ADD 
	CONSTRAINT [PK_Emails] PRIMARY KEY  CLUSTERED 
	(
		[EmailID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ForumGroups] WITH NOCHECK ADD 
	CONSTRAINT [PK_ForumGroup] PRIMARY KEY  CLUSTERED 
	(
		[ForumGroupId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Forums] WITH NOCHECK ADD 
	CONSTRAINT [PK_Forums] PRIMARY KEY  CLUSTERED 
	(
		[ForumID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Moderators] WITH NOCHECK ADD 
	CONSTRAINT [PK_Moderators] PRIMARY KEY  CLUSTERED 
	(
		[UserName],
		[ForumID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Posts] WITH NOCHECK ADD 
	CONSTRAINT [PK_Posts] PRIMARY KEY  CLUSTERED 
	(
		[PostID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ThreadTrackings] WITH NOCHECK ADD 
	CONSTRAINT [PK_ThreadTrackings] PRIMARY KEY  CLUSTERED 
	(
		[ThreadID],
		[UserName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [PK_Users] PRIMARY KEY  CLUSTERED 
	(
		[UserName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AnonymousUsers] WITH NOCHECK ADD 
	CONSTRAINT [DF_AnonymousUsers_LastLogin] DEFAULT (getdate()) FOR [LastLogin]
GO

ALTER TABLE [dbo].[Emails] WITH NOCHECK ADD 
	CONSTRAINT [DF_Emails_Importance] DEFAULT (1) FOR [Importance],
	CONSTRAINT [DF_Emails_Description] DEFAULT ('') FOR [Description]
GO

ALTER TABLE [dbo].[ForumGroups] WITH NOCHECK ADD 
	CONSTRAINT [DF__ForumGrou__SortO__25518C17] DEFAULT (0) FOR [SortOrder],
	CONSTRAINT [IX_ForumGroups] UNIQUE  NONCLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Forums] WITH NOCHECK ADD 
	CONSTRAINT [DF_Forums_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Forums_Moderated] DEFAULT (0) FOR [Moderated],
	CONSTRAINT [DF_Forums_DaysToView] DEFAULT (30) FOR [DaysToView],
	CONSTRAINT [DF_Forums_Active] DEFAULT (1) FOR [Active],
	CONSTRAINT [DF_Forums_SortOrder] DEFAULT (0) FOR [SortOrder],
	CONSTRAINT [DF_Forums_TotalPosts] DEFAULT (0) FOR [TotalPosts],
	CONSTRAINT [DF_Forums_TotalThreads] DEFAULT (0) FOR [TotalThreads],
	CONSTRAINT [DF_Forums_MostRecentPostID] DEFAULT (0) FOR [MostRecentPostID],
	CONSTRAINT [DF_Forums_MostRecentThreadID] DEFAULT (0) FOR [MostRecentThreadID],
	CONSTRAINT [DF_Forums_MostRecentPostAuthor] DEFAULT ('') FOR [MostRecentPostAuthor]
GO

ALTER TABLE [dbo].[ForumsRead] WITH NOCHECK ADD 
	CONSTRAINT [DF_ForumsReadByDate_MarkReadAfter] DEFAULT (0) FOR [MarkReadAfter],
	CONSTRAINT [DF_ForumsRead_LastActivity] DEFAULT (getdate()) FOR [LastActivity]
GO

ALTER TABLE [dbo].[Moderators] WITH NOCHECK ADD 
	CONSTRAINT [DF_Moderators_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Moderators_EmailNotification] DEFAULT (0) FOR [EmailNotification]
GO

ALTER TABLE [dbo].[Posts] WITH NOCHECK ADD 
	CONSTRAINT [DF_Posts_PostDate] DEFAULT (getdate()) FOR [PostDate],
	CONSTRAINT [DF_Posts_Approved] DEFAULT (1) FOR [Approved],
	CONSTRAINT [DF_Posts_ForumID] DEFAULT (1) FOR [ForumID],
	CONSTRAINT [DF_Posts_ThreadDate] DEFAULT (getdate()) FOR [ThreadDate],
	CONSTRAINT [DF_Posts_Views] DEFAULT (0) FOR [TotalViews],
	CONSTRAINT [DF_Posts_IsLocked] DEFAULT (0) FOR [IsLocked],
	CONSTRAINT [DF_Posts_IsPinned] DEFAULT (0) FOR [IsPinned],
	CONSTRAINT [DF_Posts_PinnedDate] DEFAULT (getdate()) FOR [PinnedDate]
GO

ALTER TABLE [dbo].[PostsRead] WITH NOCHECK ADD 
	CONSTRAINT [DF_PostsReadDateByUser_HasRead] DEFAULT (1) FOR [HasRead]
GO

ALTER TABLE [dbo].[ThreadTrackings] WITH NOCHECK ADD 
	CONSTRAINT [DF_ThreadTrackings_DateCreated] DEFAULT (getdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[UserRoles] WITH NOCHECK ADD 
	CONSTRAINT [IX_UserRoles] UNIQUE  NONCLUSTERED 
	(
		[RoleName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [DF_Users_ForumView] DEFAULT (2) FOR [ForumView],
	CONSTRAINT [DF_Users_ProfileApproved] DEFAULT (1) FOR [ProfileApproved],
	CONSTRAINT [DF_Users_Approved] DEFAULT (1) FOR [Approved],
	CONSTRAINT [DF_Users_Trusted] DEFAULT (0) FOR [Trusted],
	CONSTRAINT [DF_Users_FakeEmail] DEFAULT ('') FOR [FakeEmail],
	CONSTRAINT [DF_Users_URL] DEFAULT ('') FOR [URL],
	CONSTRAINT [DF_Users_Signature] DEFAULT ('') FOR [Signature],
	CONSTRAINT [DF_Users_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Users_TrackYourPosts] DEFAULT (0) FOR [TrackYourPosts],
	CONSTRAINT [DF_Users_LastLogin] DEFAULT (getdate()) FOR [LastLogin],
	CONSTRAINT [DF_Users_LastActivity] DEFAULT (getdate()) FOR [LastActivity],
	CONSTRAINT [DF_Users_TimeZone] DEFAULT ((-5)) FOR [TimeZone],
	CONSTRAINT [DF_Users_Location] DEFAULT ('') FOR [Location],
	CONSTRAINT [DF_Users_Occupation] DEFAULT ('') FOR [Occupation],
	CONSTRAINT [DF_Users_Interests] DEFAULT ('') FOR [Interests],
	CONSTRAINT [DF_Users_MSN] DEFAULT ('') FOR [MSN],
	CONSTRAINT [DF_Users_Yahoo] DEFAULT ('') FOR [Yahoo],
	CONSTRAINT [DF_Users_AIM] DEFAULT ('') FOR [AIM],
	CONSTRAINT [DF_Users_ICQ] DEFAULT ('') FOR [ICQ],
	CONSTRAINT [DF_Users_TotalPosts] DEFAULT (0) FOR [TotalPosts],
	CONSTRAINT [DF_Users_HasIcon] DEFAULT (0) FOR [HasIcon],
	CONSTRAINT [DF_Users_ShowUnreadTopicsOnly] DEFAULT (0) FOR [ShowUnreadTopicsOnly],
	CONSTRAINT [DF_Users_Style_1] DEFAULT (N'default') FOR [Style],
	CONSTRAINT [DF_Users_ImageType] DEFAULT (N'gif') FOR [ImageType],
	CONSTRAINT [DF_Users_ShowIcon] DEFAULT (0) FOR [ShowIcon],
	CONSTRAINT [DF_Users_DateFormat] DEFAULT (N'MM-dd-yyyy') FOR [DateFormat],
	CONSTRAINT [DF_Users_PostViewOrder] DEFAULT (0) FOR [PostViewOrder],
	CONSTRAINT [DF_Users_FlatView] DEFAULT (1) FOR [FlatView],
	CONSTRAINT [DF_Users_DisplayInMemberList] DEFAULT (1) FOR [DisplayInMemberList],
	CONSTRAINT [IX_Users_UniqueEmail] UNIQUE  NONCLUSTERED 
	(
		[Email]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_Forums_Active] ON [dbo].[Forums]([Active]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ForumsReadByDate] ON [dbo].[ForumsRead]([ForumId]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_ForumsReadByDate_1] ON [dbo].[ForumsRead]([Username]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ParentID] ON [dbo].[Posts]([ParentID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ThreadID] ON [dbo].[Posts]([ThreadID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_SortOrder] ON [dbo].[Posts]([SortOrder]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_PostLevel] ON [dbo].[Posts]([PostLevel]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_Approved] ON [dbo].[Posts]([Approved]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_ForumID] ON [dbo].[Posts]([ForumID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_Posts_Username] ON [dbo].[Posts]([UserName]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PostsRead] ON [dbo].[PostsRead]([PostId]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PostsRead_1] ON [dbo].[PostsRead]([Username]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_PrivateForums] ON [dbo].[PrivateForums]([ForumId]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_UsersInRoles] ON [dbo].[UsersInRoles]([Username]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_UsersInRoles_1] ON [dbo].[UsersInRoles]([Rolename]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Moderators] ADD 
	CONSTRAINT [FK_Moderators_Users] FOREIGN KEY 
	(
		[UserName]
	) REFERENCES [dbo].[Users] (
		[UserName]
	)
GO

ALTER TABLE [dbo].[Posts] ADD 
	CONSTRAINT [FK_Posts_Forums] FOREIGN KEY 
	(
		[ForumID]
	) REFERENCES [dbo].[Forums] (
		[ForumID]
	),
	CONSTRAINT [FK_Posts_Users] FOREIGN KEY 
	(
		[UserName]
	) REFERENCES [dbo].[Users] (
		[UserName]
	)
GO

ALTER TABLE [dbo].[PrivateForums] ADD 
	CONSTRAINT [FK_PrivateForums_UserRoles] FOREIGN KEY 
	(
		[RoleName]
	) REFERENCES [dbo].[UserRoles] (
		[RoleName]
	)
GO

ALTER TABLE [dbo].[ThreadTrackings] ADD 
	CONSTRAINT [FK_ThreadTrackings_Users] FOREIGN KEY 
	(
		[UserName]
	) REFERENCES [dbo].[Users] (
		[UserName]
	)
GO

ALTER TABLE [dbo].[UsersInRoles] ADD 
	CONSTRAINT [FK_UsersInRoles_UserRoles] FOREIGN KEY 
	(
		[Rolename]
	) REFERENCES [dbo].[UserRoles] (
		[RoleName]
	),
	CONSTRAINT [FK_UsersInRoles_Users] FOREIGN KEY 
	(
		[Username]
	) REFERENCES [dbo].[Users] (
		[UserName]
	)
GO

