namespace SecureChild.Models
{
    public class EmailGenerator
    {

        public string GenerateEmail(string name)
        {
            // Ensure the name has no whitespace and is lowercase
            string cleanName = name.Replace(" ", "").ToLower();

            // Append the domain to the name
            string email = $"{cleanName}@securechild.com";

            return email;
        }


    }
}
