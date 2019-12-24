using System.Collections.Generic;

namespace FavProducts.Core.Rest.Transport
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class AuthSuccessResponse
    {
        public string Token { get; set; }
    }

    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}