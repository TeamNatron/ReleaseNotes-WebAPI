using ReleaseNotes_WebAPI.Domain.Models;

namespace ReleaseNotes_WebAPI.Domain.Services.Communication
{
    public class ArticleResponse : BaseResponse
    {

        public Article Article { get; private set; }
        
        public ArticleResponse(bool success, string message, Article article) : base(success, message)
        {
            Article = article;
        }
        
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="category">Saved category.</param>
        /// <returns>Response.</returns>
        public ArticleResponse(Article article) : this(true, string.Empty, article)
        {
        }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ArticleResponse(string message) : this(false, message, null)
        {
        }
    }
}