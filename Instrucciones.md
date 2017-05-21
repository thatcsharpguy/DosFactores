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

7. Habilitar SSL en Propiedades -> Debug
8. Agregar el código en ApplicationUser.cs:12

```
public virtual string TfaKey { get; set; }
```

9. Agregar el código en 00000000000000_CreateIdentitySchema.cs:60

```
TfaKey = table.Column<string>(maxLength: 32, nullable: true),
```

10. Agregar paquete Microsoft.AspNetCore.Session
11. Agregar el código en Startup.cs:60

```
// Add session support
services.AddSession(options =>
{
	// Set a short timeout for easy testing.
	options.IdleTimeout = TimeSpan.FromSeconds(10);
	options.CookieHttpOnly = true;
});
```

12. Agrear el código en Startup.cs:98

```
app.UseSession();
```

13. Agregar nuevo proyecto (Google.Authenticator): Class Library (.NET Core)
14. Añadir clases: SetupCode y TwoFactorAuthenticator (Ver https://github.com/BrandonPotter/GoogleAuthenticator)
15. Agregar referencia de Goolge.Authenticator -> DosFactores  