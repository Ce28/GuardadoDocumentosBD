USE archivosbd
GO

IF EXISTS(SELECT NAME FROM SYSOBJECTS(NOLOCK) WHERE NAME = 'imagenes')
	DROP TABLE imagenes
GO

CREATE TABLE archivosbd.dbo.imagenes
(
	id		INT IDENTITY,
	doc		VARBINARY(MAX) NOT NULL,
	nombre	VARCHAR(100) NOT NULL 
)
GO
/*****************************************************************
BASE DE DATOS:	archivobd
TABLA:			imagenes
FECHA:			20/02/2018
DESCRIPCION:	Tabla para guardar imagenes
REALIZO:		Sapien Gamez Christian Enrique
*****************************************************************/