namespace TenmoServer.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }

        //Modified to include AccountId
        public int AccountId { get; set; }
    }

    /// <summary>
    /// Model to return upon successful login
    /// </summary>
    public class ReturnUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        //public string Role { get; set; }
        public string Token { get; set; }
        //Modified to include AccountId
        public int AccountId { get; set; }
    }

    /// <summary>
    /// Model to accept login parameters
    /// </summary>
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // A special, limited type of user which only includes non-sensitive information we are prepared to send to the client
    public class ListUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int AccountId { get; set; }
    }
}
