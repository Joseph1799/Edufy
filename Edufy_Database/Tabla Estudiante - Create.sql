USE [edufy_db]
GO

/****** Object:  Table [dbo].[Estudiante]    Script Date: 2/25/2024 7:30:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Estudiante](
	[ID] [int] NOT NULL PRIMARY KEY,
	[Nombre] [varchar](255) NOT NULL,
	[Apellido] [varchar](255) NOT NULL,
	[CorreoElectronico] [varchar](255) NOT NULL,
	[Contrasenia] [varchar](255) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Genero] [varchar](50) NOT NULL,
	[Carrera] [varchar](255) NOT NULL,
	[AnioIngreso] [int] NOT NULL,
	[EstadoCuenta] [varchar](50) NOT NULL,
	[FotoPerfil] [varchar](255) NULL,
	[Rol] [varchar](15) NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Estudiante] ADD  DEFAULT ('Estudiante') FOR [Rol]
GO


