### Сервисы DI
```csharp
public static IdentityBuilder AddIdentityCore<TUser>(this IServiceCollection services) where TUser : class
{
  return services.AddIdentityCore<TUser>(delegate
  {
  });
}

public static IdentityBuilder AddIdentityCore<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction) where TUser : class
{
  services.AddOptions().AddLogging();
  services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
  services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
  services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
  services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
  services.TryAddScoped<IUserConfirmation<TUser>, DefaultUserConfirmation<TUser>>();
  services.TryAddScoped<IdentityErrorDescriber>();
  services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
  services.TryAddScoped<UserManager<TUser>>();
  if (setupAction != null)
  {
    services.Configure(setupAction);
  }
  return new IdentityBuilder(typeof(TUser), services);
}
```

### Builder IdentityBuilder
#### AddClaimsPrincipalFactory<TFactory>()
```csharp
AddScoped(typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(UserType), typeof(TFactory));
```
#### AddErrorDescriber<TDescriber>()
```csharp
Services.AddScoped<IdentityErrorDescriber, TDescriber>();
```
#### AddPasswordValidator<TValidator>()
```csharp
AddScoped(typeof(IPasswordValidator<>).MakeGenericType(UserType), typeof(TValidator));
```
#### AddPersonalDataProtection<TProtector, TKeyRing>()
```csharp
Services.AddSingleton<IPersonalDataProtector, DefaultPersonalDataProtector>();
Services.AddSingleton<ILookupProtector, TProtector>();
Services.AddSingleton<ILookupProtectorKeyRing, TKeyRing>();
```
#### AddRoleManager<TRoleManager>
```csharp
AddScoped(typeof(RoleManager<>).MakeGenericType(RoleType), typeof(TRoleManager));
```
#### AddRoles<TRole>
```csharp
Services.TryAddScoped<RoleManager<TRole>>();
Services.AddScoped(typeof(IUserClaimsPrincipalFactory<>).MakeGenericType(UserType), typeof(UserClaimsPrincipalFactory<, >).MakeGenericType(UserType, RoleType));
```
#### AddRoleStore<TStore>
```csharp
AddScoped(typeof(IRoleStore<>).MakeGenericType(RoleType), typeof(TStore));
```
#### AddRoleValidator<TRole>
```csharp
AddScoped(typeof(IRoleValidator<>).MakeGenericType(RoleType), typeof(TRole));
```
#### AddTokenProvider
#### AddUserConfirmation
#### AddUserManager
#### AddUserStore
#### AddUserValidator


