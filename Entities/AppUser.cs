namespace EcommerceBackend.Entities
{
    public class AppUser
    {   
        private static int _nextUserId = 1; // Static field to keep track of the next user ID
        public  int UserId { get; set; }  
        public string UserName { get; set; } = string.Empty;
        private int _Age;
        public string Address { get; set; } = string.Empty;

        public int Age
        {
            get { return _Age; }
            set
            {
                if (value <15 || value > 90)
                {
                    throw new ArgumentOutOfRangeException("Age must be between 15 and 90.");
                }
                _Age = value;
            }
        }
        public AppUser(string userName, int age, string address)
        {
            UserId = _nextUserId++; // Assign the next user ID and increment it
            UserName = userName;
            Age = age; // This will invoke the Age property setter and validate the age
            Address = address;
        }
        public AppUser()
        {
            UserId = _nextUserId++; // Assign the next user ID and increment it
            UserName = "Unknown";
            Age = 15;
            Address = string.Empty;
        }
    }
}
