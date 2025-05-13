### 1. OptionsFactory<TOptions>
В конструкторе собираются настройки
```csharp
public OptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, IEnumerable<IValidateOptions<TOptions>> validations)
{
  _setups = (setups as IConfigureOptions<TOptions>[]) ?? new List<IConfigureOptions<TOptions>>(setups).ToArray();
  _postConfigures = (postConfigures as IPostConfigureOptions<TOptions>[]) ?? new List<IPostConfigureOptions<TOptions>>(postConfigures).ToArray();
  _validations = (validations as IValidateOptions<TOptions>[]) ?? new List<IValidateOptions<TOptions>>(validations).ToArray();
}
```

Логика создания опции
```csharp
public TOptions Create(string name)
{
  TOptions val = CreateInstance(name);
  IConfigureOptions<TOptions>[] setups = _setups;
  foreach (IConfigureOptions<TOptions> configureOptions in setups)
  {
    if (configureOptions is IConfigureNamedOptions<TOptions> configureNamedOptions)
    {
      configureNamedOptions.Configure(name, val);
    }
    else if (name == Options.DefaultName)
    {
      configureOptions.Configure(val);
    }
  }
  IPostConfigureOptions<TOptions>[] postConfigures = _postConfigures;
  for (int i = 0; i < postConfigures.Length; i++)
  {
    postConfigures[i].PostConfigure(name, val);
  }
  if (_validations.Length != 0)
  {
    List<string> list = new List<string>();
    IValidateOptions<TOptions>[] validations = _validations;
    for (int i = 0; i < validations.Length; i++)
    {
      ValidateOptionsResult validateOptionsResult = validations[i].Validate(name, val);
      if (validateOptionsResult != null && validateOptionsResult.Failed)
      {
        list.AddRange(validateOptionsResult.Failures);
      }
    }
    if (list.Count > 0)
    {
      throw new OptionsValidationException(name, typeof(TOptions), list);
    }
  }
  return val;
}
```
1. Создаем наш тип
2. Получаем все IConfigureOptions<TOptions> и выполняем метод Configure
3. Получаем все IPostConfigureOptions<TOptions> и выполняем метод PostConfigure
