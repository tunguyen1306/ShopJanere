Update [dbo].[BDSEmpNews] set DateReup=GETDATE()
 where [DeadLine] > GETDATE()