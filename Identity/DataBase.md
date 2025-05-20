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
#### 6. IUserPasswordStore
```csharp
public interface IUserPasswordStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetPasswordHashAsync(TUser user, string? passwordHash, CancellationToken cancellationToken);
    Task<string?> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken);
}
```
#### 7. IUserSecurityStampStore
```csharp
public interface IUserSecurityStampStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken);
    Task<string?> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken);
}
```
#### 8. IUserRoleStore
```csharp
public interface IUserRoleStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken);
    Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken);
    Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken);
    Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken);
}
```
#### 9. IUserLoginStore
```csharp
public interface IUserLoginStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken);
    Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken);
    Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken);
    Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
}
```
#### 10. IUserEmailStore
```csharp
public interface IUserEmailStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetEmailAsync(TUser user, string? email, CancellationToken cancellationToken);
    Task<string?> GetEmailAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken);
    Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken);
    Task<TUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    Task<string?> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken);
    Task SetNormalizedEmailAsync(TUser user, string? normalizedEmail, CancellationToken cancellationToken);
}
```
#### 11. IUserPhoneNumberStore
```csharp
public interface IUserPhoneNumberStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task SetPhoneNumberAsync(TUser user, string? phoneNumber, CancellationToken cancellationToken);
    Task<string?> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken);
    Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken);
}
```
#### 12. IUserClaimStore
```csharp
public interface IUserClaimStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken);
    Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
    Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken);
    Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
    Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);
}
```
#### 13. IUserLockoutStore
```csharp
public interface IUserLockoutStore<TUser> : IUserStore<TUser> where TUser : class
{
    Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken);
    Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken);
    Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);
    Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);
    Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken);
    Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken);
}
```
#### 14. IQueryableUserStore
```csharp
public interface IQueryableUserStore<TUser> : IUserStore<TUser> where TUser : class
{
    IQueryable<TUser> Users { get; }
}
```
