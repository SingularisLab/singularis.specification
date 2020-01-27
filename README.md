# singularis.specification

![](https://github.com//anatoly-kryzhanovsky/singularis.specification/workflows/build%20and%20publish/badge.svg)

## Short description
Implementation of specification pattern for usage with ORM (currently NHiberante and ef.core are supported)

## Usage
Add reference to nuget package in your domain layer project
```
Install-Package Singularis.Specification.Definition
```

Add reference to nuget package in your data access layer project:
For ef.core use:
```
Install-Package Singularis.Specification.Executor.EntityFramework
```

For NHibernate use:
```
Install-Package Singularis.Specification.Executor.Nhibernate
```

Create a rule (aka specification) in the domain layer. All the rules must be derived from Specification<T> class and must initialize Query property in ctor:
```
public class CreatedAfter: Specification<User>
{
  public CreatedAfter(DateTime date)
  {
     Query = Source().Where(x => x.CreatedAd > date);
  }
}
```
  
Use the rule with repository:
```
var repository = serviceProvider.GetRequiredService<IRepository<int>>();
var newUsers = repository.List(new CreatedAfter(new DateTime(2020, 1, 1));
```

## Build complex rules
You can build a complex rule based on existsing ones using two extension methods: Combine() and Or(). 
Combine appends one rule to another. The rule can contains any operation.
Or can contains only Where operation.

## Additional references
You can read more info about library in the on habr.com (in russian): https://habr.com/ru/company/singularis/blog/485328/

