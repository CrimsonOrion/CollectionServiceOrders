-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        02/19/2019
-- Description: Toggles archive in service order by ID
-----------------------------------------------------------

CREATE PROCEDURE [dbo].[spServiceOrders_UpdateArchiveFlagByID]
    @ID int,
	@Archived bit,
    @SignalChange varchar(255)
AS
IF @Archived = 1
	BEGIN
		UPDATE ServiceOrders
		SET Archived = 1,
			DateArchived = GETDATE(),
			SignalChange = @SignalChange
		WHERE ID = @ID;
	END
ELSE
	BEGIN
		UPDATE ServiceOrders
		SET Archived = 0,
			DateArchived = NULL,
			SignalChange = @SignalChange
		WHERE ID = @ID;
	END