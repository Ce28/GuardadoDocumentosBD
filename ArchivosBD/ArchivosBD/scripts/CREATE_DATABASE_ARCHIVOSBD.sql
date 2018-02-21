USE master
GO

IF EXISTS(SELECT * FROM DBO.SYSDATABASES WHERE NAME = 'archivosbd')
	DROP DATABASE archivosbd
GO

CREATE DATABASE archivosbd
GO

USE archivosbd
/*****************************************************************
BASE DE DATOS:	archivobd
FECHA:			20/02/2018
DESCRIPCION:	Creacion de base de datos 'archivobd'
REALIZO:		Sapien Gamez Christian Enrique
*****************************************************************/

