-----------------------------------------------------------
-- Author:      Jim Lynch
-- Date:        02/18/2019
-- Description: Updates Completed for a service order by ID
-----------------------------------------------------------

CREATE PROCEDURE  [dbo].[spServiceOrders_UpdateCompletedFlagByID]
	@ID int,
    @Completed bit,
    @DateCompleted datetime2(7)
AS
IF @Completed = 1
    BEGIN
      UPDATE ServiceOrders
      SET Completed = @Completed,
          DateCompleted = @DateCompleted 	
      WHERE [ID] = @ID
    END
ELSE
    BEGIN
      UPDATE ServiceOrders
      SET Completed = @Completed,
          DateCompleted = NULL
      WHERE [ID] = @ID
    END