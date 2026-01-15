using Ganss.Xss;
namespace siapv_backend.Helpers
{
    public static class HtmlSanitizerHelper
    {
        public static string Sanitize(string html)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("li");
            sanitizer.AllowedTags.Add("strong");

            return sanitizer.Sanitize(html);
        }
    }
}