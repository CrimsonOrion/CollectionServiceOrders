-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        02/18/2019
-- Description: Archives all service orders in SqlDB
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spServiceOrders_UpdateArchiveAll] 
	@SignalChange varchar(255)
AS
BEGIN
	UPDATE ServiceOrders
    SET Archived = 1,
        DateArchived = GETDATE(),
		SignalChange = @SignalChange
    WHERE Archived = 0 OR Archived IS NULL
END