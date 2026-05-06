using System.ComponentModel.DataAnnotations;
using FlowerShopOnlineOrderSystem.Data;
using FlowerShopOnlineOrderSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Flower_Shop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            [Display(Name = "First name")]
            [StringLength(50)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Last name")]
            [StringLength(50)]
            public string LastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [StringLength(254)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = GetSafeReturnUrl(returnUrl);
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = GetSafeReturnUrl(returnUrl);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = Input.Email.Trim().ToLowerInvariant();

            if (await _userManager.FindByEmailAsync(email) != null)
            {
                ModelState.AddModelError(nameof(Input.Email), "An account with this email already exists.");
                return Page();
            }

            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            await _userManager.AddToRoleAsync(user, RoleNames.Customer);
            await CreateCustomerProfileAsync(email);

            _logger.LogInformation("Customer account {Email} registered.", email);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return LocalRedirect(ReturnUrl);
        }

        private async Task CreateCustomerProfileAsync(string email)
        {
            var profileExists = await _context.Customers.AnyAsync(c => c.Email == email);

            if (profileExists)
            {
                return;
            }

            _context.Customers.Add(new Customer
            {
                FirstName = Input.FirstName.Trim(),
                LastName = Input.LastName.Trim(),
                Email = email
            });

            await _context.SaveChangesAsync();
        }

        private string GetSafeReturnUrl(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            return Url.Content("~/");
        }
    }
}
