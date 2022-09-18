namespace Impexpdata.Logging
{
    public class CustomerLogger
    {
        public string LogPath => "log.txt";
        public void Log(string message)
        {
            if (!File.Exists(LogPath))
            {
                var file = File.Create(LogPath);
                file.Close();
            }
            using (var writer = new StreamWriter(LogPath, true))
            {
                writer.WriteLine($"{DateTime.Now} LOG : {message}");
            } 
        }
    }
}
