using System;
using System.IO;
using System.Threading.Tasks;

namespace ReleaseNotes_WebAPI.Utilities
{
    public class FileHandler
    {
        public static async Task WriteFileAsync(string dir, string file, string content)
        {
            Console.WriteLine("Async Write File has started");
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(dir, file)) )
            {
                await outputFile.WriteAsync(content);
            }
            Console.WriteLine("Async Write File has completed");
        }

        public static async Task<String> ReadTextFileAsync(string dir, string file)
        {
            using (StreamReader inputFile = new StreamReader(Path.Combine(dir, file)))
            {
                var readingTask = await inputFile.ReadToEndAsync();
                return readingTask;
            }
        }
    }
}