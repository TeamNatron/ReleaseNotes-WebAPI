namespace ReleaseNotes_WebAPI.Utilities
{
    // https://code-maze.com/filtering-aspnet-core-webapi/
    public class ReleaseNoteParameters
    {
        public int product { get; set; }
        // TODO: remove if unnec
        public string sort { get; set; } = "";
    }
}