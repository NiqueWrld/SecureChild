namespace SecureChild.Models
{
    public class Messaging
    {

        public async Task sendWhatsappMessage(string phoneNumber, string message, string apiKey)
        {

            message = message += "\n \n - " + DateTime.Now;

            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message) || string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("Phone number, message, and API key cannot be null or empty.");
            }

            // Define the URL
            string encodedMessage = System.Net.WebUtility.UrlEncode(message);

            string url = $"https://api.callmebot.com/whatsapp.php?phone={phoneNumber}&text={encodedMessage}&apikey={apiKey}";


            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Send a GET request to the specified URL
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Output the response content
                    Console.WriteLine("Response received:");
                    Console.WriteLine(responseBody);
                }
                catch (HttpRequestException e)
                {
                    // Handle any exceptions that occur during the request
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }

    }
}
