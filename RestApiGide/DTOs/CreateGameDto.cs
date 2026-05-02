namespace RestApiGide.DTOs
{
    public class CreateGameDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BuildUrl { get; set; }
        public string FullProjectUrl { get; set; }
        public string NameExe { get; set; }
        public int AuthorId { get; set; }
    }
}
