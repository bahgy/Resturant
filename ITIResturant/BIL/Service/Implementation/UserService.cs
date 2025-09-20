
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

        if (userVm.UserType == "Admin")
            return (true, "Cannot create Admin user", null);

        var user = _mapper.Map<Customer>(userVm);

        var result = await _userManager.CreateAsync(user, userVm.Password);
        if (!result.Succeeded)
            return (true, string.Join(", ", result.Errors.Select(e => e.Description)), null);

        var newVm = _mapper.Map<UserVM>(user);
        return (false, null, newVm);
    }


    public async Task<(bool hasError, string? message, UserVM? user)> UpdateUserAsync(UserVM userVm)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userVm.Id);
        if (user == null)
            return (true, "User not found", null);
        // if any user exist with the same email added
        if (await _userManager.FindByEmailAsync(userVm.Email) is AppUser existingUser && existingUser.Id != userVm.Id)
            return (true, "Email already exists", null);
        _mapper.Map(userVm, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return (true, string.Join(", ", result.Errors.Select(e => e.Description)), null);

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
}
