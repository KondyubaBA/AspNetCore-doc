### Таблицы
#### AspNetRoleClaims
     - Id
     - RoleId
     - ClaimType
     - ClaimValue      
#### AspNetRoles
     - Id
     - Name (nvarchar(256), NULL)
     - NormalizedName (nvarchar(256), NULL)
     - ConcurrencyStamp (nvarchar(max), NULL)
#### AspNetUserClaims
     - Id
     - UserId
     - ClaimType (nvarchar(max), NULL)
     - ClaimValue (nvarchar(max), NULL)
#### AspNetUserLogins
     - LoginProvider
     - ProviderKey (PK, nvarchar(450), NULL)
     - ProviderDisplayName (nvarchar(max), NULL)
     - UserId
#### AspNetUserRoles
    - UserId
    - RoleId
#### AspNetUsers
    - Id
    - UserName
    - NormalizedUserName
    - Email
    - NormalizedEmail
    - EmailConfirmed
    - PasswordHash
    - SecurityStamp
    - ConcurrencyStamp
    - PhoneNumber
    - PhoneNumberConfirmed
    - TwoFactorEnabled
    - LockoutEnd
    - LockoutEnabled
    - AccessFailedCount
#### AspNetUserTokens
     - UserId
     - LoginProvider
     - Name
     - Value

### UserManager
#### 1. IUserAuthenticationTokenStore
```csharp
public interface IUserAuthenticationTokenStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetTokenAsync(TUser user, string loginProvider, string name, string? value, CancellationToken cancellationToken);
    Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken);
    Task<string?> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken);
}
```
#### 2. IUserAuthenticatorKeyStore
```csharp
public interface IUserAuthenticatorKeyStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken);
    Task<string?> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken);
}
```
#### 3. IUserTwoFactorRecoveryCodeStore
```csharp
public interface IUserTwoFactorRecoveryCodeStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken);
    Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken);
    Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken);
}
```
#### 4. IUserTwoFactorRecoveryCodeStore
```csharp
public interface IUserTwoFactorRecoveryCodeStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken);
    Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken);
    Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken);
}
```
#### 5. IUserTwoFactorStore
```csharp
public interface IUserTwoFactorStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken);
    Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken);
}
```
