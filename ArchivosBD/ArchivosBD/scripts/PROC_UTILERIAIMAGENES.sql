USE archivosbd
GO

IF EXISTS(SELECT NAME FROM SYSOBJECTS(NOLOCK) WHERE NAME = 'proc_utileriaimagenes')
	DROP TABLE proc_utileriaimagenes
GO

CREATE PROCEDURE dbo.proc_utileriaimagenes @doc VARBINARY(MAX), @nombre VARCHAR(100)
AS
BEGIN
	INSERT INTO archivosbd.dbo.imagenes (doc, nombre) VALUES (@doc, @nombre)
END