using AutoMapper;
using UserApi.Authorization;
using UserApi.Entities;
using UserApi.Helpers;
using UserApi.Models;
using UserApi.Repositories;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;


    public UserService(IUserRepository userRepository, IJwtUtils jwtUtils, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model) 
    {
        // get user from database
        var user = await _userRepository.GetUserByUsernameAsync(model.Username);

        // return null if user not found
        if (user == null) return null;

        // Check if the user has exceeded the maximum number of failed login attempts
        if (user.FailedLoginAttempts >= 3)
        {
            // Check if the elapsed time since the last failed attempt is greater than an hour
            var elapsedHours = DateTime.UtcNow - (user.LastFailedLoginAttempt ?? DateTime.UtcNow.AddHours(-1));
            if (elapsedHours.TotalHours >= 1)
            {
                // Reset failed login attempts after an hour
                user.FailedLoginAttempts = 0;
                await _userRepository.UpdateFailedLoginAttemptsAsync(user.Id, 0);
            }
            else
            {
                // You may want to log this attempt or take other actions (e.g., lock the account)
                return null;
            }
        }

        // check if the provided password matches the password in the database and return null if it doesn't
        if (!_passwordHasher.ValidatePassword(model.Password, user.PasswordHash, user.PasswordSalt))
        {
            // Increment failed login attempts in the user entity and update database
            user.FailedLoginAttempts++;
            user.LastFailedLoginAttempt = DateTime.UtcNow; // Update the timestamp of the last failed attempt
            await _userRepository.UpdateFailedLoginAttemptsAsync(user.Id, user.FailedLoginAttempts);
            return null;
        }

        // Reset failed login attempts upon successful login
        user.FailedLoginAttempts = 0;
        await _userRepository.UpdateFailedLoginAttemptsAsync(user.Id, 0);

        // or use your preferred method to get the current time
        await _userRepository.UpdateLastLoginAsync(user.Id.ToString());

        // authentication successful so generate jwt token
        var token = _jwtUtils.GenerateJwtToken(user);

        // map user and token to response model with AutoMapper and return
        return _mapper.Map<AuthenticateResponse>(user, opts => opts.Items["Token"] = token);
    }

    public async Task<CreateUserResponse?> CreateUserAsync(CreateUserRequest userRequest)
    {
        // Hash and salt the password
        (byte[] passwordHash, byte[] passwordSalt) = _passwordHasher.HashPassword(userRequest.Password);

        // Map CreateUserRequest model to User entity with Automapper
        var userEntity = _mapper.Map<User>(userRequest);

        // Assign hashed and salted password to user entity
        userEntity.PasswordHash = passwordHash;
        userEntity.PasswordSalt = passwordSalt;

        // Create user in database
        var createdUser = await _userRepository.CreateUserAsync(userEntity)
            ?? throw new Exception("An error occurred when creating user. Try again later.");

        // Map User entity to CreateUserResponse model with Automapper
        return _mapper.Map<CreateUserResponse>(createdUser);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
    
        return _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<UserResponse?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<UserResponse>(user);
    }
}
