namespace MilanWebStore.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using MilanWebStore.Common;
    using MilanWebStore.Data.Models;
    using MilanWebStore.Services.Data.Contracts;
    using MilanWebStore.Web.Areas.Identity.Pages.Account.InputModels;
    using MilanWebStore.Web.Helpers;
    using MilanWebStore.Web.ViewModels.ShoppingCarts;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IShoppingCartsService shoppingCartService;
        private readonly IWebHostEnvironment enviroment;
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IShoppingCartsService shoppingCartService,
            IWebHostEnvironment enviroment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.shoppingCartService = shoppingCartService;
            this.enviroment = enviroment;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                this.ReturnUrl = returnUrl;
                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                return this.Page();
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (this.ModelState.IsValid)
            {
                var user = await this.CreateUserAsync(this.Input, $"{this.enviroment.WebRootPath}");

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code = code,
                            returnUrl = returnUrl,
                        },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email",
#pragma warning disable SA1117
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
#pragma warning restore SA1117

                    var cart = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.ShoppingCartKey);
                    if (cart != null)
                    {
                        foreach (var product in cart)
                        {
                            await this.shoppingCartService.AddProductToShoppingCartAsync(product.Id, this.Input.Email, product.Quantity);
                        }

                        this.HttpContext.Session.Remove(GlobalConstants.ShoppingCartKey);
                    }

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.Page();
        }


        private async Task<ApplicationUser> CreateUserAsync(RegisterInputModel input, string imagePath)
        {
            var user = new ApplicationUser
            {
                UserName = input.Email,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                ShoppingCart = new ShoppingCart(),
                Images = new List<Image>(),
            };

            Directory.CreateDirectory($"{imagePath}/images/users");

            foreach (var image in input.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var dbImage = new Image
                {
                    ApplicationUserId = user.Id,
                    Extention = extension,
                };
                user.Images.Add(dbImage);

                var physicalPath = $"{imagePath}/images/users/{dbImage.Id}.{extension}";
                using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                await image.CopyToAsync(fileStream);
            }

            return user;
        }
    }
}
