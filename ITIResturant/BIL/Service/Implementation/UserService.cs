
using System.Security.Claims;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public UserService(IMapper mapper, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<(bool hasError, string? message, UserVM? user)> GetUserByIdAsync(int id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return (true, "User not found", null);

        var userVm = _mapper.Map<UserVM>(user);
        return (false, null, userVm);
    }

    public async Task<(bool hasError, string? message, IEnumerable<UserVM>? users)> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var usersVm = _mapper.Map<IEnumerable<UserVM>>(users);
        return (false, null, usersVm);
    }

    public async Task<(bool hasError, string? message, UserVM? user)> CreateUserAsync(UserVM userVm)
    {
        if (await _userManager.FindByEmailAsync(userVm.Email) != null)
            return (true, "Email already exists", null);

        if (userVm.UserType.ToString() == "Admin")
            return (true, "Cannot create Admin user", null);

        AppUser user = userVm.UserType.ToString() switch
        {
            "Delivery" => _mapper.Map<Delivery>(userVm),
            _ => _mapper.Map<Customer>(userVm)
        };

        var result = await _userManager.CreateAsync(user, userVm.Password);
        if (!result.Succeeded)
            return (true, string.Join(", ", result.Errors.Select(e => e.Description)), null);

        await _userManager.AddToRoleAsync(user, userVm.UserType.ToString());

        var newVm = _mapper.Map<UserVM>(user);
        return (false, null, newVm);
    }


    public async Task<(bool hasError, string? message, UserVM? user)> UpdateUserAsync(UserVM userVm)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userVm.Id);
        if (user == null)
            return (true, "User not found", null);

        // check if email already exists for another user
        if (await _userManager.FindByEmailAsync(userVm.Email) is AppUser existingUser && existingUser.Id != userVm.Id)
            return (true, "Email already exists", null);

        // map changes
        _mapper.Map(userVm, user);

        // 🔹 keep UserType in sync with selected role
        user.UserType = userVm.UserType ?? UserTypeEnum.Customer;


        // update user in DB
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return (true, string.Join(", ", result.Errors.Select(e => e.Description)), null);

        // 🔹 remove old roles
        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        // 🔹 add the new role chosen by admin
        await _userManager.AddToRoleAsync(user, userVm.UserType.ToString());

        // prepare updated VM
        var updatedVm = _mapper.Map<UserVM>(user);
        return (false, null, updatedVm);
    }


    public async Task<(bool hasError, string? message, bool isDeleted)> DeleteUserAsync(int id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return (true, "User not found", false);

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return (true, string.Join(", ", result.Errors.Select(e => e.Description)), false);

        return (false, null, true);
    }

    public int? GetCurrentCustomerId(ClaimsPrincipal user)
    {
        if (user == null) return null;

        // check if role is Customer
        if (!user.IsInRole("Customer"))
            return null;

        var customerIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out int customerId))
            return customerId;

        return null;
    }

    public int? GetCurrentDeliveryId(ClaimsPrincipal user)
    {
        if (user == null) return null;

        // check if logged in and role is Delivery
        if (user.IsInRole("Delivery"))
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && int.TryParse(idClaim.Value, out int deliveryId))
                return deliveryId;
        }
        return null;
    }
}
