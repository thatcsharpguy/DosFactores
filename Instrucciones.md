1. Nuevo proyecto: ASP.NET Core Web Application (.NET Core)
2. Template: Web Application
3. Change Authentication: Individual User Accounts
4. Agregar el paquete Microsoft.AspNetCore.Rewrite
5. Agregar el código en Startup.cs:44 

```
// Enforce SSL
services.Configure<MvcOptions>(options =>
{
	options.Filters.Add(new RequireHttpsAttribute());
});
```

6. Agregar el código en Startup.cs:81 

```
// Requires using Microsoft.AspNetCore.Rewrite;
var options = new RewriteOptions()
   .AddRedirectToHttps();
```