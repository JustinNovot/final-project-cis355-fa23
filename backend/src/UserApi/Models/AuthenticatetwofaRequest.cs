namespace UserApi.Models;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a request to authenticate a user.
/// </summary>
public class AuthenticatetwofaRequest
{
    /// <summary>
    /// The username of the user to authenticate.
    /// </summary>
    [Required]
    public string TwoFactorCode { get; set; } = null!;


}