namespace TenmoClient.Models
{
    /// <summary>
    /// Return value from login endpoint
    /// </summary>
    public class ApiUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public int AccountId { get; set; }
    }

    /// <summary>
    /// Model to provide login parameters
    /// </summary>
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Return value from list endpoint
    /// </summary>
    public class ListUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int AccountId { get; set; }

        public string FormattedUser()
        {
            return $"{AccountId}: {Username}";
        }
    }
}
