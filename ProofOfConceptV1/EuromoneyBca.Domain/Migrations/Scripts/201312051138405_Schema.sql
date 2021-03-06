/****** Object:  Table [dbo].[Trade_Performance]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Performance](
	[trade_performance_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_id] [int] NULL,
	[measure_type_id] [int] NULL,
	[return_apl_function] [varchar](255) NULL,
	[return_currency_id] [int] NULL,
	[return_benchmark_id] [int] NULL,
	[return_value] [varchar](255) NULL,
	[return_date] [datetime] NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Trade_Performance] PRIMARY KEY CLUSTERED 
(
	[trade_performance_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade_Line_Group_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Line_Group_Type](
	[trade_line_group_type_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_line_group_type_uri] [varchar](255) NULL,
	[trade_line_group_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Trade_Line_Group_Type] PRIMARY KEY CLUSTERED 
(
	[trade_line_group_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade_Line_Group]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Line_Group](
	[trade_line_group_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_line_group_type_id] [int] NULL,
	[trade_line_group_uri] [varchar](255) NULL,
	[trade_line_group_label] [varchar](255) NULL,
	[trade_line_group_editorial_label] [varchar](255) NULL,
 CONSTRAINT [PK_Trade_Line_Group] PRIMARY KEY CLUSTERED 
(
	[trade_line_group_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade_Line]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Line](
	[trade_line_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_line_uri] [varchar](255) NULL,
	[trade_id] [int] NULL,
	[trade_line_group_id] [int] NULL,
	[tradable_thing_id] [int] NULL,
	[position_id] [int] NULL,
	[trade_line_label] [varchar](255) NULL,
	[trade_line_editorial_label] [varchar](255) NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Trade_Line] PRIMARY KEY CLUSTERED 
(
	[trade_line_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade_Instruction]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Instruction](
	[trade_instruction_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_id] [int] NULL,
	[relativity_id] [int] NULL,
	[instruction_type_id] [int] NULL,
	[hedge_id] [int] NULL,
	[instruction_entry] [decimal](18, 5) NULL,
	[instruction_entry_date] [datetime] NULL,
	[instruction_exit] [decimal](18, 5) NULL,
	[instruction_exit_date] [datetime] NULL,
	[instruction_label] [varchar](255) NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Trade_Instruction] PRIMARY KEY CLUSTERED 
(
	[trade_instruction_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade_Comment]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade_Comment](
	[comment_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_id] [int] NULL,
	[comment_label] [varchar](255) NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Trade_Comment] PRIMARY KEY CLUSTERED 
(
	[comment_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Trade]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Trade](
	[trade_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_uri] [varchar](255) NULL,
	[relativity_id] [int] NULL,
	[length_type_id] [int] NULL,
	[structure_type_id] [int] NULL,
	[service_id] [int] NULL,
	[currency_id] [int] NULL,
	[benchmark_id] [int] NULL,
	[trade_label] [varchar](255) NULL,
	[trade_editorial_label] [varchar](255) NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
	[status] [int] NULL,
 CONSTRAINT [PK_Trade] PRIMARY KEY CLUSTERED 
(
	[trade_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Tradable_Thing_Class]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Tradable_Thing_Class](
	[tradable_thing_class_id] [int] IDENTITY(1,1) NOT NULL,
	[tradable_thing_class_uri] [varchar](255) NULL,
	[tradable_thing_class_label] [varchar](255) NULL,
	[tradable_thing_class_editorial_label] [varchar](255) NULL,
 CONSTRAINT [PK_Tradable_Thing_Class] PRIMARY KEY CLUSTERED 
(
	[tradable_thing_class_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Tradable_Thing]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Tradable_Thing](
	[tradable_thing_id] [int] IDENTITY(1,1) NOT NULL,
	[tradable_thing_uri] [varchar](255) NULL,
	[tradable_thing_class_id] [int] NULL,
	[location_id] [int] NULL,
	[tradable_thing_code] [varchar](255) NULL,
	[tradable_thing_label] [varchar](255) NULL,
 CONSTRAINT [PK_Tradable_Thing] PRIMARY KEY CLUSTERED 
(
	[tradable_thing_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Structure_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Structure_Type](
	[structure_type_id] [int] IDENTITY(1,1) NOT NULL,
	[structure_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Structure_Type] PRIMARY KEY CLUSTERED 
(
	[structure_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Service]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Service](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[service_uri] [varchar](255) NULL,
	[service_code] [varchar](50) NULL,
	[service_label] [varchar](255) NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[service_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Relativity]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Relativity](
	[relativity_id] [int] IDENTITY(1,1) NOT NULL,
	[relativity_label] [varchar](255) NULL,
 CONSTRAINT [PK_Relativity] PRIMARY KEY CLUSTERED 
(
	[relativity_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Related_Trade]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Related_Trade](
	[trade_id] [int] NOT NULL,
	[related_trade_id] [int] NOT NULL,
	[created_on] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Related_Trade] PRIMARY KEY CLUSTERED 
(
	[trade_id] ASC,
	[related_trade_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[Position]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Position](
	[position_id] [int] IDENTITY(1,1) NOT NULL,
	[position_uri] [varchar](255) NULL,
	[position_label] [varchar](255) NULL,
	[position_relativity_id] [int] NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[position_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Measure_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Measure_Type](
	[measure_type_id] [int] IDENTITY(1,1) NOT NULL,
	[measure_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Measure_Type] PRIMARY KEY CLUSTERED 
(
	[measure_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Length_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Length_Type](
	[length_type_id] [int] IDENTITY(1,1) NOT NULL,
	[length_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Length_Type] PRIMARY KEY CLUSTERED 
(
	[length_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Instruction_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Instruction_Type](
	[instruction_type_id] [int] IDENTITY(1,1) NOT NULL,
	[instruction_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Instruction_Type] PRIMARY KEY CLUSTERED 
(
	[instruction_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[Hedge_Type]    Script Date: 10/11/2013 12:48:10 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

CREATE TABLE [dbo].[Hedge_Type](
	[hedge_id] [int] IDENTITY(1,1) NOT NULL,
	[hedge_label] [varchar](255) NULL,
 CONSTRAINT [PK_Hedge_Type] PRIMARY KEY CLUSTERED 
(
	[hedge_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Currency](
	[currency_id] [int] IDENTITY(1,1) NOT NULL,
	[currency_uri] [varchar](255) NULL,
	[currency_code] [varchar](50) NULL,
	[currency_symbol] [varchar](50) NULL,
	[currency_label] [varchar](255) NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[currency_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Benchmark](
	[benchmark_id] [int] IDENTITY(1,1) NOT NULL,
	[benchmark_uri] [varchar](255) NULL,
	[benchmark_code] [varchar](50) NULL,
	[benchmark_label] [varchar](255) NULL,
 CONSTRAINT [PK_Benchmark] PRIMARY KEY CLUSTERED 
(
	[benchmark_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Location](
	[location_id] [int] IDENTITY(1,1) NOT NULL,
	[location_uri] [varchar](255) NULL,
	[location_code] [varchar](255) NULL,
	[location_label] [varchar](255) NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[location_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Status](
	[status_id] [int] IDENTITY(1,1) NOT NULL,
	[status_label] [varchar](255) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Track_Record_Type](
	[track_record_type_id] [int] IDENTITY(1,1) NOT NULL,
	[track_record_type_label] [varchar](255) NULL,
 CONSTRAINT [PK_Track_Record_Type] PRIMARY KEY CLUSTERED 
(
	[track_record_type_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Track_Record](
	[track_record_id] [int] IDENTITY(1,1) NOT NULL,
	[trade_id] [int] NULL,
	[track_record_type_id] [int] NULL,
	[track_record_value] [decimal](18, 5) NULL,
	[track_record_date] [datetime] NULL,
	[created_by] [int] NULL,
	[last_updated] [datetime] NULL,
 CONSTRAINT [PK_Track_Record] PRIMARY KEY CLUSTERED 
(
	[track_record_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

SET ANSI_PADDING OFF

/****** Object:  Default [DF_Trade_created_on]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade] ADD  CONSTRAINT [DF_Trade_created_on]  DEFAULT (getutcdate()) FOR [created_on]

/****** Object:  Default [DF_Trade_last_updated]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade] ADD  CONSTRAINT [DF_Trade_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  Default [DF_Trade_Line_created_on]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line] ADD  CONSTRAINT [DF_Trade_Line_created_on]  DEFAULT (getutcdate()) FOR [created_on]

/****** Object:  Default [DF_Trade_Line_last_updated]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line] ADD  CONSTRAINT [DF_Trade_Line_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  Default [DF_Trade_Instruction_created_on]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction] ADD  CONSTRAINT [DF_Trade_Instruction_created_on]  DEFAULT (getutcdate()) FOR [created_on]

/****** Object:  Default [DF_Trade_Instruction_last_updated]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction] ADD  CONSTRAINT [DF_Trade_Instruction_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  Default [DF_Trade_Comment_created_on]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Comment] ADD  CONSTRAINT [DF_Trade_Comment_created_on]  DEFAULT (getutcdate()) FOR [created_on]

/****** Object:  Default [DF_Trade_Comment_last_updated]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Comment] ADD  CONSTRAINT [DF_Trade_Comment_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  Default [DF_Track_Record_track_record_date]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Track_Record] ADD  CONSTRAINT [DF_Track_Record_track_record_date]  DEFAULT (getutcdate()) FOR [track_record_date]

/****** Object:  Default [DF_Track_Record_last_updated]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Track_Record] ADD  CONSTRAINT [DF_Track_Record_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  Default [DF_Trade_Preformance_created_on]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance] ADD  CONSTRAINT [DF_Trade_Preformance_created_on]  DEFAULT (getutcdate()) FOR [created_on]

/****** Object:  Default [DF_Trade_Preformance_last_updated]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance] ADD  CONSTRAINT [DF_Trade_Preformance_last_updated]  DEFAULT (getutcdate()) FOR [last_updated]

/****** Object:  ForeignKey [FK_Trade_Benchmark]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Benchmark] FOREIGN KEY([benchmark_id])
REFERENCES [dbo].[Benchmark] ([benchmark_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Benchmark]

/****** Object:  ForeignKey [FK_Trade_Currency]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Currency] FOREIGN KEY([currency_id])
REFERENCES [dbo].[Currency] ([currency_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Currency]

/****** Object:  ForeignKey [FK_Trade_Length_Type]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Length_Type] FOREIGN KEY([length_type_id])
REFERENCES [dbo].[Length_Type] ([length_type_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Length_Type]

/****** Object:  ForeignKey [FK_Trade_Relativity]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Relativity] FOREIGN KEY([relativity_id])
REFERENCES [dbo].[Relativity] ([relativity_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Relativity]

/****** Object:  ForeignKey [FK_Trade_Service]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Service] FOREIGN KEY([service_id])
REFERENCES [dbo].[Service] ([service_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Service]

/****** Object:  ForeignKey [FK_Trade_Status]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Status] FOREIGN KEY([status])
REFERENCES [dbo].[Status] ([status_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Status]

/****** Object:  ForeignKey [FK_Trade_Structure_Type]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Structure_Type] FOREIGN KEY([structure_type_id])
REFERENCES [dbo].[Structure_Type] ([structure_type_id])

ALTER TABLE [dbo].[Trade] CHECK CONSTRAINT [FK_Trade_Structure_Type]

/****** Object:  ForeignKey [FK_Trade_Line_Group_Trade_Line_Group_Type]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Trade_Line_Group]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Line_Group_Trade_Line_Group_Type] FOREIGN KEY([trade_line_group_type_id])
REFERENCES [dbo].[Trade_Line_Group_Type] ([trade_line_group_type_id])

ALTER TABLE [dbo].[Trade_Line_Group] CHECK CONSTRAINT [FK_Trade_Line_Group_Trade_Line_Group_Type]

/****** Object:  ForeignKey [FK_Tradable_Thing_Location]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Tradable_Thing]  WITH CHECK ADD  CONSTRAINT [FK_Tradable_Thing_Location] FOREIGN KEY([location_id])
REFERENCES [dbo].[Location] ([location_id])

ALTER TABLE [dbo].[Tradable_Thing] CHECK CONSTRAINT [FK_Tradable_Thing_Location]

/****** Object:  ForeignKey [FK_Tradable_Thing_Tradable_Thing_Class]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Tradable_Thing]  WITH CHECK ADD  CONSTRAINT [FK_Tradable_Thing_Tradable_Thing_Class] FOREIGN KEY([tradable_thing_class_id])
REFERENCES [dbo].[Tradable_Thing_Class] ([tradable_thing_class_id])

ALTER TABLE [dbo].[Tradable_Thing] CHECK CONSTRAINT [FK_Tradable_Thing_Tradable_Thing_Class]

/****** Object:  ForeignKey [FK_Position_Relativity]    Script Date: 10/31/2013 11:17:24 ******/
ALTER TABLE [dbo].[Position]  WITH CHECK ADD  CONSTRAINT [FK_Position_Relativity] FOREIGN KEY([position_relativity_id])
REFERENCES [dbo].[Relativity] ([relativity_id])

ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [FK_Position_Relativity]

/****** Object:  ForeignKey [FK_Related_Trade_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Related_Trade]  WITH CHECK ADD  CONSTRAINT [FK_Related_Trade_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Related_Trade] CHECK CONSTRAINT [FK_Related_Trade_Trade]

/****** Object:  ForeignKey [FK_Related_Trade_Trade1]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Related_Trade]  WITH CHECK ADD  CONSTRAINT [FK_Related_Trade_Trade1] FOREIGN KEY([related_trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Related_Trade] CHECK CONSTRAINT [FK_Related_Trade_Trade1]

/****** Object:  ForeignKey [FK_Trade_Line_Position]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Line_Position] FOREIGN KEY([position_id])
REFERENCES [dbo].[Position] ([position_id])

ALTER TABLE [dbo].[Trade_Line] CHECK CONSTRAINT [FK_Trade_Line_Position]

/****** Object:  ForeignKey [FK_Trade_Line_Tradable_Thing]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Line_Tradable_Thing] FOREIGN KEY([tradable_thing_id])
REFERENCES [dbo].[Tradable_Thing] ([tradable_thing_id])

ALTER TABLE [dbo].[Trade_Line] CHECK CONSTRAINT [FK_Trade_Line_Tradable_Thing]

/****** Object:  ForeignKey [FK_Trade_Line_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Line_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Trade_Line] CHECK CONSTRAINT [FK_Trade_Line_Trade]

/****** Object:  ForeignKey [FK_Trade_Line_Trade_Line_Group]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Line]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Line_Trade_Line_Group] FOREIGN KEY([trade_line_group_id])
REFERENCES [dbo].[Trade_Line_Group] ([trade_line_group_id])

ALTER TABLE [dbo].[Trade_Line] CHECK CONSTRAINT [FK_Trade_Line_Trade_Line_Group]

/****** Object:  ForeignKey [FK_Trade_Instruction_Hedge_Type]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Instruction_Hedge_Type] FOREIGN KEY([hedge_id])
REFERENCES [dbo].[Hedge_Type] ([hedge_id])

ALTER TABLE [dbo].[Trade_Instruction] CHECK CONSTRAINT [FK_Trade_Instruction_Hedge_Type]

/****** Object:  ForeignKey [FK_Trade_Instruction_Instruction_Type]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Instruction_Instruction_Type] FOREIGN KEY([instruction_type_id])
REFERENCES [dbo].[Instruction_Type] ([instruction_type_id])

ALTER TABLE [dbo].[Trade_Instruction] CHECK CONSTRAINT [FK_Trade_Instruction_Instruction_Type]

/****** Object:  ForeignKey [FK_Trade_Instruction_Relativity]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Instruction_Relativity] FOREIGN KEY([relativity_id])
REFERENCES [dbo].[Relativity] ([relativity_id])

ALTER TABLE [dbo].[Trade_Instruction] CHECK CONSTRAINT [FK_Trade_Instruction_Relativity]

/****** Object:  ForeignKey [FK_Trade_Instruction_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Instruction]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Instruction_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Trade_Instruction] CHECK CONSTRAINT [FK_Trade_Instruction_Trade]

/****** Object:  ForeignKey [FK_Trade_Comment_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Comment]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Comment_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Trade_Comment] CHECK CONSTRAINT [FK_Trade_Comment_Trade]

/****** Object:  ForeignKey [FK_Track_Record_Track_Record_Type]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Track_Record]  WITH CHECK ADD  CONSTRAINT [FK_Track_Record_Track_Record_Type] FOREIGN KEY([track_record_type_id])
REFERENCES [dbo].[Track_Record_Type] ([track_record_type_id])

ALTER TABLE [dbo].[Track_Record] CHECK CONSTRAINT [FK_Track_Record_Track_Record_Type]

/****** Object:  ForeignKey [FK_Track_Record_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Track_Record]  WITH CHECK ADD  CONSTRAINT [FK_Track_Record_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Track_Record] CHECK CONSTRAINT [FK_Track_Record_Trade]

/****** Object:  ForeignKey [FK_Trade_Performance_Benchmark]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Performance_Benchmark] FOREIGN KEY([return_benchmark_id])
REFERENCES [dbo].[Benchmark] ([benchmark_id])

ALTER TABLE [dbo].[Trade_Performance] CHECK CONSTRAINT [FK_Trade_Performance_Benchmark]

/****** Object:  ForeignKey [FK_Trade_Performance_Currency]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Performance_Currency] FOREIGN KEY([return_currency_id])
REFERENCES [dbo].[Currency] ([currency_id])

ALTER TABLE [dbo].[Trade_Performance] CHECK CONSTRAINT [FK_Trade_Performance_Currency]

/****** Object:  ForeignKey [FK_Trade_Performance_Measure_Type]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Performance_Measure_Type] FOREIGN KEY([measure_type_id])
REFERENCES [dbo].[Measure_Type] ([measure_type_id])

ALTER TABLE [dbo].[Trade_Performance] CHECK CONSTRAINT [FK_Trade_Performance_Measure_Type]

/****** Object:  ForeignKey [FK_Trade_Performance_Trade]    Script Date: 10/31/2013 11:17:25 ******/
ALTER TABLE [dbo].[Trade_Performance]  WITH CHECK ADD  CONSTRAINT [FK_Trade_Performance_Trade] FOREIGN KEY([trade_id])
REFERENCES [dbo].[Trade] ([trade_id])

ALTER TABLE [dbo].[Trade_Performance] CHECK CONSTRAINT [FK_Trade_Performance_Trade]