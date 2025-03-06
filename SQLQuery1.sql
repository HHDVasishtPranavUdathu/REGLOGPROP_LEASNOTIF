CREATE PROCEDURE UpdatePaymentStatus
    @PaymentID INT,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    -- Update the Ownerstatus based on the input
    UPDATE Payments
    SET Ownerstatus = 
        CASE 
            WHEN @Status IN ('done', 'yes') THEN 'True'
            WHEN @Status = 'false' THEN 'False'
            ELSE Ownerstatus -- Keep existing status if input is not recognized
        END
    WHERE PaymentID = @PaymentID;
END