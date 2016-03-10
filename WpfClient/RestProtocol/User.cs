namespace RestProtocol
{ 
    public class User
    {
        public int ID {private set; get; }
        public string Name { set; get; }
        public string LastName { set; get; }
        public string FullName {  get {return $"{Name} {LastName}";} }

        public User(string name, string lastName, int id)
        {
            Name = name;
            LastName = lastName;
            ID = id;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
