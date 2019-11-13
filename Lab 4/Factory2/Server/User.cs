namespace Server
{
    internal class User
    {
        public int Id { get; private set; }
        public string Name { get; set; } 
        public string Password { get; set; }
        public string Role { get; set; }

        public User(int id, string name, string pass, string role)
        {
            Id = id;
            Name = name;
            Password = pass;
            Role = role;
        }
    }
}