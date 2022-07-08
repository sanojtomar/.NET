CREATE PROCEDURE [dbo].[GenerateOutput]
	@firstName varchar(100) ,
	@lastName varchar(100) 
AS

BEGIN
	DECLARE @Counter INT 
	SET @Counter=1

	DECLARE @TempTable TABLE(    
		item varchar(100)    
	)  
	
	WHILE (@Counter <= 100)
	BEGIN
		IF (@Counter % 3 = 0 AND @Counter % 5 = 0 )
			INSERT INTO @TempTable VALUES(@firstName + ' ' + @lastName)
		ELSE IF	 (@Counter % 3 = 0 )
			INSERT INTO @TempTable VALUES(@firstName)
		ELSE IF	 (@Counter % 5 = 0 )
			INSERT INTO @TempTable VALUES(@lastName)
		ELSE
			INSERT INTO @TempTable VALUES(@Counter)

		SET @Counter  = @Counter  + 1
	END

	SELECT * from @TempTable
END