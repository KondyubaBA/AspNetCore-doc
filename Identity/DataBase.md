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
