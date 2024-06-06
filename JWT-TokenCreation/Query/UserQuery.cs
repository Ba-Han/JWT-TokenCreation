namespace JWT_TokenCreation.Query;
public class UserQuery
{
    #region getUserQuery
    public static string GetUserQuery()
    {
        return @"SELECT [Id]
      ,[UserName]
      ,[Email]
      ,[Password]
      ,[IsActive]
  FROM [dbo].[userLogin] WHERE Email = @Email AND Password = @Password AND IsActive = @IsActive";
    }
    #endregion

    #region getUserRecordQuery
    public static string GetUserRecordQuery()
    {
        return @"SELECT [Id]
      ,[UserName]
      ,[Email]
      ,[Password]
      ,[IsActive]
  FROM [dbo].[userLogin] WHERE IsActive = @IsActive";
    }
    #endregion

    #region getEachUserQuery
    public static string GetEachUserQuery()
    {
        return @"SELECT [Id]
      ,[UserName]
      ,[Email]
      ,[Password]
      ,[IsActive]
  FROM [dbo].[userLogin] WHERE Id =@Id AND IsActive = @IsActive";
    }
    #endregion

    #region DeleteUserQuery
    public static string DeleteUserQuery()
    {
        return @"UPDATE userLogin
        SET IsActive = @IsActive
        WHERE Id = @Id";
    }
    #endregion

    #region createUserQuery
    public static string CreateUserQuery()
    {
        return @"INSERT INTO [dbo].[userLogin] (UserName,Email,Password,IsActive)
        VALUES (@UserName,@Email,@Password,@IsActive)";
    }
    #endregion

    #region checkDuplicateQuery
    public static string CheckDuplicateQuery()
    {
        return @"SELECT [Id]
      ,[UserName]
      ,[Email]
      ,[Password]
      ,[IsActive]
  FROM [dbo].[userLogin] WHERE Email = @Email AND IsActive = @IsActive";
    }
    #endregion
}
