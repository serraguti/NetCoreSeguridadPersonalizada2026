using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NetCoreSeguridadPersonalizada.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login
            (string username, string password)
        {
            if (username.ToLower() == "admin"
                && password == "12345")
            {
                //POR MEDIDAS DE SEGURIDAD, SE GENERA UNA COOKIE 
                //CIFRADA QUE ES PARA SABER SI EL USER SE HA VALIDADO 
                //EN ESTE EXPLORADOR O NO.
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                //UN CLAIM ES INFORMACION DEL USUARIO
                Claim claimUserName =
                    new Claim(ClaimTypes.Name, username);
                Claim claimRole =
                    new Claim(ClaimTypes.Role, "USUARIO");
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                //CREAMOS UN USUARIO PRINCIPAL CON ESTA IDENTIDAD
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                //DAMOS DE ALTA AL USER EN EL SISTEMA
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal, 
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(10)
                    });
                return RedirectToAction("Perfil", "Usuarios");
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }
    }
}
