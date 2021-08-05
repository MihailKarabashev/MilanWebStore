namespace MilanWebStore.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.Areas.Identity.Pages.Account.InputModels;
    using MilanWebStore.Web.Helpers;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    [AllowAnonymous]
#pragma warning disable SA1649
    public class LoginModel : PageModel
#pragma warning restore SA1649
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IShoppingCartsService shoppingCartsService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<LoginModel> logger;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IShoppingCartsService shoppingCartsService)
        {
            this.shoppingCartsService = shoppingCartsService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
            }

            if (this.User.Identity.IsAuthenticated)
            {
                this.Response.Redirect("/Home/Error");
            }

            returnUrl = returnUrl ?? this.Url.Content("~/");

            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            if (this.ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(this.Input.Username, this.Input.Password, this.Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var shoppingCart = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);

                    if (shoppingCart != null)
                    {
                        foreach (var item in shoppingCart)
                        {
                            await this.shoppingCartsService.AddProductToShoppingCartAsync(item.Id, this.Input.Username, item.Quantity);
                        }

                        this.HttpContext.Session.Remove(GlobalConstants.ShoppingCartKey);
                    }

                    this.logger.LogInformation("User logged in.");
                    return this.LocalRedirect(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = this.Input.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    this.logger.LogWarning("User account locked out.");
                    return this.RedirectToPage("./Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return this.Page();
                }
            }

            return this.Page();
        }
    }
}
