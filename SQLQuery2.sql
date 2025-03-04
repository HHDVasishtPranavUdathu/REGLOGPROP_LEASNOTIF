CREATE PROCEDURE GetPropertyAndTenantIdFromLease
    @LeaseId INT
AS
BEGIN
    SELECT PropertyId, TenantId
    FROM Lease
    WHERE LeaseId = @LeaseId;
END
