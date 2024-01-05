public struct Client
{
    public string ClientId { get; set; }
    public List<string> UniqueSongs { get; set; }

    public Client(string clientId)
    {
        ClientId = clientId;
        UniqueSongs = new List<string>();
    }

}

class Program
{
    static void Main()
    {
        List<Client> clientList = new List<Client>();
        string fileName = "exhibitA-input.csv";
        string executablePath = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(Directory.GetParent(executablePath).FullName, fileName);
        int totalLines = File.ReadLines(filePath).Count();
        int linesRead = 0;



        using (StreamReader reader = new StreamReader(filePath))
        {
            string line = reader.ReadLine(); //skip first column
            
            while (!reader.EndOfStream)          
            {
                linesRead++;
                line = reader.ReadLine();
                string[] columns = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < 4; i++)
                {
                    if (columns[3] == "10/08/2016") //specified date
                    {
                        if (clientList.Any(client => client.ClientId == columns[2])) //does the client exist? // columns[2] = clientID
                        {
                            // Client exists, check if the song exists in the client's listened song list
                            Client targetClient = clientList.First(client => client.ClientId == columns[2]);

                            string targetSongId = columns[1];
                            if (!targetClient.UniqueSongs.Contains(targetSongId))
                                targetClient.UniqueSongs.Add(targetSongId);
                        
                        }
                        else
                        {
                            // Client does not exist, add client and song
                            Client _client = new Client(columns[2]);
                            _client.UniqueSongs.Add(columns[1]);
                            clientList.Add(_client);
                        }

                    }
                            
                }

                // Calculate and display the percentage
                double percentage = ((double)linesRead / totalLines) * 100;
                Console.WriteLine($"Percentage of CSV file read: {percentage:F2}%");
            }
        }

        // Count clients who played 346 distinct songs
        int clientsWith346Songs = clientList.Count(client => client.UniqueSongs.Distinct().Count() == 346);

        // Find the client with the most unique songs
        Client clientWithMostSongs = clientList.OrderByDescending(client => client.UniqueSongs.Count).FirstOrDefault();

        // Display results
        Console.WriteLine($"Number of clients who played 346 distinct songs: {clientsWith346Songs}");

        if (clientWithMostSongs.ClientId != null)
        {
            Console.WriteLine($"Client with the most unique songs listened (ID: {clientWithMostSongs.ClientId}): {clientWithMostSongs.UniqueSongs.Count}");
        }
        else
        {
            Console.WriteLine("No clients found.");
        }



        // Specify the file path for output
        string outputPath = "output.txt";

        // Redirect output to the file
        using (StreamWriter sw = new StreamWriter(outputPath))
        {
            Console.SetOut(sw);

            // Display client information
            Console.WriteLine("Client List:");

            foreach (var client in clientList)
            {
                Console.WriteLine($"Client ID: {client.ClientId}");
                Console.WriteLine("Unique Songs:");

                foreach (var song in client.UniqueSongs)
                {
                    Console.WriteLine($"- {song}");
                }

                Console.WriteLine();
            }
        }

    }
}
