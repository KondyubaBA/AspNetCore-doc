#### 1. UserAuthenticationToken
```csharp
public virtual async Task<IdentityResult> SetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName, string? tokenValue)
public virtual async Task<IdentityResult> RemoveAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName)
public virtual Task<string?> GetAuthenticationTokenAsync(TUser user, string loginProvider, string tokenName)
```

#### 2. UserAuthenticatorKey
```csharp
public virtual Task<string?> GetAuthenticatorKeyAsync(TUser user)
public virtual async Task<IdentityResult> ResetAuthenticatorKeyAsync(TUser user)
```

#### 3. UserTwoFactorRecoveryCode
```csharp
public virtual async Task<IEnumerable<string>?> GenerateNewTwoFactorRecoveryCodesAsync(TUser user, int number)
public virtual async Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(TUser user, string code)
public virtual Task<int> CountRecoveryCodesAsync(TUser user)
```

#### 4. UserTwoFactor
```csharp
public virtual async Task<bool> GetTwoFactorEnabledAsync(TUser user)
public virtual async Task<IdentityResult> SetTwoFactorEnabledAsync(TUser user, bool enabled)
```

#### 5. UserPassword
```csharp
public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)  !!!
public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
public virtual Task<bool> HasPasswordAsync(TUser user)
public virtual async Task<IdentityResult> AddPasswordAsync(TUser user, string password)
public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string currentPassword, string newPassword)
public virtual async Task<IdentityResult> RemovePasswordAsync(TUser user)
public virtual async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
```

#### 6. UserSecurityStamp
```csharp
public virtual async Task<string> GetSecurityStampAsync(TUser user)
public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)  !!!
public virtual async Task<IdentityResult> UpdateSecurityStampAsync(TUser user)
```

#### 7. UserRole
```csharp
public virtual async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
public virtual async Task<IdentityResult> AddToRolesAsync(TUser user, IEnumerable<string> roles)
public virtual async Task<IdentityResult> RemoveFromRoleAsync(TUser user, string role)
public virtual async Task<IdentityResult> RemoveFromRolesAsync(TUser user, IEnumerable<string> roles)
public virtual async Task<IList<string>> GetRolesAsync(TUser user)
public virtual async Task<bool> IsInRoleAsync(TUser user, string role)
public virtual Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
```

#### 8. UserLogin
```csharp
public virtual Task<TUser?> FindByLoginAsync(string loginProvider, string providerKey)
public virtual async Task<IdentityResult> RemoveLoginAsync(TUser user, string loginProvider, string providerKey)
public virtual async Task<IdentityResult> AddLoginAsync(TUser user, UserLoginInfo login)
public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
```

#### 9. UserEmail
```csharp
public virtual async Task<string?> GetEmailAsync(TUser user)
public virtual async Task<IdentityResult> SetEmailAsync(TUser user, string? email)
public virtual async Task<TUser?> FindByEmailAsync(string email)
public virtual async Task UpdateNormalizedEmailAsync(TUser user)
public virtual async Task<IdentityResult> ChangeEmailAsync(TUser user, string newEmail, string token)
public virtual async Task<IdentityResult> ConfirmEmailAsync(TUser user, string token)
public virtual async Task<bool> IsEmailConfirmedAsync(TUser user)
```

#### 10. UserPhoneNumber
```csharp
public virtual async Task<string?> GetPhoneNumberAsync(TUser user)
public virtual async Task<IdentityResult> SetPhoneNumberAsync(TUser user, string? phoneNumber)
public virtual async Task<IdentityResult> ChangePhoneNumberAsync(TUser user, string phoneNumber, string token)
public virtual Task<bool> IsPhoneNumberConfirmedAsync(TUser user)
```

#### 11. UserClaim
```csharp
public virtual Task<IdentityResult> AddClaimAsync(TUser user, Claim claim)
public virtual async Task<IdentityResult> AddClaimsAsync(TUser user, IEnumerable<Claim> claims)
public virtual async Task<IdentityResult> ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim)
public virtual Task<IdentityResult> RemoveClaimAsync(TUser user, Claim claim)
public virtual async Task<IdentityResult> RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims)
public virtual async Task<IList<Claim>> GetClaimsAsync(TUser user)
public virtual Task<IList<TUser>> GetUsersForClaimAsync(Claim claim)
```

#### 12. UserLockout
```csharp
public virtual async Task<bool> IsLockedOutAsync(TUser user)
public virtual async Task<IdentityResult> SetLockoutEnabledAsync(TUser user, bool enabled)
public virtual async Task<bool> GetLockoutEnabledAsync(TUser user)
public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user)
public virtual async Task<IdentityResult> SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd)
public virtual async Task<IdentityResult> AccessFailedAsync(TUser user)
public virtual async Task<IdentityResult> ResetAccessFailedCountAsync(TUser user)
public virtual async Task<int> GetAccessFailedCountAsync(TUser user)
```

#### 13. QueryableUser
```csharp
public virtual IQueryable<TUser> Users
```

```csharp
public virtual async Task<IdentityResult> CreateAsync(TUser user)
public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)
public virtual Task<IdentityResult> DeleteAsync(TUser user)
public virtual Task<TUser?> FindByIdAsync(string userId)
public virtual async Task<TUser?> FindByNameAsync(string userName)
public virtual Task<TUser?> GetUserAsync(ClaimsPrincipal principal)
public virtual string? GetUserId(ClaimsPrincipal principal)
public virtual async Task<string> GetUserIdAsync(TUser user)
public virtual string? GetUserName(ClaimsPrincipal principal)
public virtual async Task<string?> GetUserNameAsync(TUser user)
public virtual async Task<IdentityResult> SetUserNameAsync(TUser user, string? userName)
public virtual Task<IdentityResult> UpdateAsync(TUser user)


public virtual async Task<byte[]> CreateSecurityTokenAsync(TUser user)
public virtual Task<string> GenerateChangeEmailTokenAsync(TUser user, string newEmail)
public virtual Task<string> GenerateChangePhoneNumberTokenAsync(TUser user, string phoneNumber)
public virtual Task<string> GenerateConcurrencyStampAsync(TUser user)
public virtual Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
public virtual string GenerateNewAuthenticatorKey()
public virtual Task<string> GeneratePasswordResetTokenAsync(TUser user)
public virtual Task<string> GenerateTwoFactorTokenAsync(TUser user, string tokenProvider)
public virtual Task<string> GenerateUserTokenAsync(TUser user, string tokenProvider, string purpose)
public virtual Task<bool> VerifyChangePhoneNumberTokenAsync(TUser user, string token, string phoneNumber)
public virtual async Task<bool> VerifyTwoFactorTokenAsync(TUser user, string tokenProvider, string token)
public virtual async Task<bool> VerifyUserTokenAsync(TUser user, string tokenProvider, string purpose, string token)


public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(TUser user)



public virtual string? NormalizeEmail(string? email)
public virtual string? NormalizeName(string? name)
public virtual async Task UpdateNormalizedEmailAsync(TUser user)
public virtual async Task UpdateNormalizedUserNameAsync(TUser user)


public virtual void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<TUser> provider)
```
