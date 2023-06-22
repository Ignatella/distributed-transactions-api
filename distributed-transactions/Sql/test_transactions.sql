USE master;
GO

CREATE OR ALTER PROCEDURE dbo.DoDistributedTransaction
AS
BEGIN
    BEGIN DISTRIBUTED TRANSACTION;

    WITH CTE_Leaf1 AS (
        SELECT Name AS Leaf_1_Name, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
        FROM Leaf_1.AdventureWorks2022.Person.AddressType
    ),
    CTE_Leaf2 AS (
        SELECT Name AS Leaf_2_Name, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
        FROM Leaf_2.AdventureWorks2022.Person.AddressType
    ),
    CTE_Leaf3 AS (
        SELECT Name AS Leaf_3_Name, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
        FROM Leaf_3.AdventureWorks2022.Person.AddressType
    ),
    CTE_Leaf4 AS (
        SELECT Name AS Leaf_4_Name, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
        FROM Leaf_4.AdventureWorks2022.Person.AddressType
    )
    SELECT
        CTE_Leaf1.Leaf_1_Name,
        CTE_Leaf2.Leaf_2_Name,
        CTE_Leaf3.Leaf_3_Name,
        CTE_Leaf4.Leaf_4_Name
    FROM
        CTE_Leaf1
        FULL OUTER JOIN CTE_Leaf2 ON CTE_Leaf1.RowNum = CTE_Leaf2.RowNum
        FULL OUTER JOIN CTE_Leaf3 ON CTE_Leaf1.RowNum = CTE_Leaf3.RowNum
        FULL OUTER JOIN CTE_Leaf4 ON CTE_Leaf1.RowNum = CTE_Leaf4.RowNum

    COMMIT TRANSACTION;
END;