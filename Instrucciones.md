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
16. Agregar clase GoogleAuthenticatorProvider (Ver: http://www.domstamand.com/two-factor-authentication-in-asp-net-identity-3-using-totp-authenticator/)
17. Agregar el código en Startup.cs:57

```
.AddTokenProvider(GoogleAuthenticatorProvider.ProviderName, typeof(GoogleAuthenticatorProvider))
```

18. Agregar el código en IndexViewModel.cs:17

```
public string TwoFactorAuthenticatorQrCode { get; set; }
```

19. Agregar el código correspondiente para la generación del QR en el archivo ManageController.cs: (64, 123 y 168)

```
TwoFactorAuthenticatorQrCode = TempData["AuthenticatorQr"]?.ToString(),
```

```
// POST: /Manage/RequestTwoFactorAuthentication
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RequestTwoFactorAuthentication()
{
  var user = await GetCurrentUserAsync();
  if (user != null)
  {
	  var tfaKey = Guid.NewGuid().ToString("N");
	  user.TfaKey = tfaKey;
	  await _userManager.UpdateAsync(user);
	  var authenticator = new TwoFactorAuthenticator();
	  var code = authenticator.GenerateSetupCode(user.UserName, tfaKey, 300, 300);
	  TempData["AuthenticatorQr"] = code.QrCodeSetupImageUrl;
	  _logger.LogInformation(1, "User enabled two-factor authentication.");
  }
  return RedirectToAction(nameof(Index), "Manage");
}
```

```
user.TfaKey = null;
await _userManager.UpdateAsync(user);
```

20. Reemplazar el código del <dd></dd> de la autenticación de dos factores por 

```
@if (Model.TwoFactorAuthenticatorQrCode != null)
{
	<img src="@Model.TwoFactorAuthenticatorQrCode" />
	<form asp-controller="Manage" asp-action="EnableTwoFactorAuthentication" method="post" class="form-horizontal">
		<button type="submit" class="btn-link btn-bracketed">Yes, I've scanned the code</button> Disabled
	</form>
}
else
{
	if (Model.TwoFactor)
	{
		<form asp-controller="Manage" asp-action="DisableTwoFactorAuthentication" method="post" class="form-horizontal">
			Enabled <button type="submit" class="btn-link btn-bracketed">Disable</button>
		</form>
	}
	else
	{
		<form asp-controller="Manage" asp-action="RequestTwoFactorAuthentication" method="post" class="form-horizontal">
			<button type="submit" class="btn-link btn-bracketed">Enable</button> Disabled
		</form>
	}
}
```

21. Ejecutar Update-Database