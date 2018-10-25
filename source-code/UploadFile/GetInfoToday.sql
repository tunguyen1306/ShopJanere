select a.Email,a.PassWord,c.Title,c.DateReup,a.TypeRegister,a.[UpdateInfo] from [dbo].[BDSAccounts] a
left join [dbo].[BDSEmployers] b on b.[IdAccount]=a.ID
left join [dbo].[BDSEmpNews] c on b.ID=c.EmployeeId
where a.PassWord not in ('c4ca4238a0b923820dcc509a6f75849b','81dc9bdb52d04dc20036dbd8313ed055','c4ca4238a0b923820dcc509a6f75849b','477f92e968e04ba71650f311fd1f6a94','e10adc3949ba59abbe56e057f20f883e') 
order by a.ID desc