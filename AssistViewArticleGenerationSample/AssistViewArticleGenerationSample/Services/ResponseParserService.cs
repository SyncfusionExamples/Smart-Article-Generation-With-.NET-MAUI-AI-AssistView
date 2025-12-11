using AssistViewArticleGenerationSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AssistViewArticleGenerationSample.Services
{
    /// <summary>
    /// Service for parsing and structuring AI responses.
    /// </summary>
    public static partial class ResponseParserService
    {
        // Common words and source types used by helper methods (static to avoid per-call allocations).
        private static readonly string[] CommonWords = new[]
        {
            "about", "after", "before", "between", "during", "where", "which", "should",
            "would", "could", "might", "their", "other", "these", "those", "than", "with",
            "from", "what", "when", "this", "that", "have", "been", "being", "also"
        };

        private static readonly string[] SourceTypes = new[] { "Article", "Documentation", "Tutorial", "Guide", "Blog" };

        /// <summary>
        /// Parses response HTML into structured sections.
        /// </summary>
        public static List<ResponseSection> ParseResponse(string htmlContent)
        {
            var sections = new List<ResponseSection>();

            if (string.IsNullOrWhiteSpace(htmlContent))
                return sections;

            // Split by <b> tags to identify sections
            var parts = SectionSplitRegex().Split(htmlContent);

            int order = 0;
            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                var section = ParseSection(part, order);
                if (section != null)
                {
                    sections.Add(section);
                    order++;
                }
            }

            // If no sections found, create a default one
            if (sections.Count == 0)
            {
                sections.Add(new ResponseSection
                {
                    Heading = "Response",
                    Content = htmlContent,
                    SectionType = "Content",
                    Order = 0,
                    IsExpanded = true
                });
            }

            return sections;
        }

        /// <summary>
        /// Parse individual section
        /// </summary>
        private static ResponseSection? ParseSection(string content, int order)
        {
            var section = new ResponseSection { Order = order };

            // Extract heading from <b> tag
            var headingMatch = BoldHeadingRegex().Match(content);
            if (headingMatch.Success)
            {
                section.Heading = headingMatch.Groups[1].Value;
                content = content.Replace(headingMatch.Value, "");
            }
            else
            {
                section.Heading = $"Section {order + 1}";
            }

            // Determine section type
            section.SectionType = DetermineSectionType(section.Heading);
            section.Content = content.Trim();

            return section;
        }

        /// <summary>
        /// Determine section type based on heading
        /// </summary>
        private static string DetermineSectionType(string heading)
        {
            heading = heading.ToLower();

            if (heading.Contains("introduction") || heading.Contains("overview"))
                return "Introduction";
            if (heading.Contains("conclusion") || heading.Contains("summary"))
                return "Conclusion";
            if (heading.Contains("key features") || heading.Contains("characteristics"))
                return "Features";
            if (heading.Contains("example") || heading.Contains("instance"))
                return "Example";

            return "Details";
        }

        /// <summary>
        /// Extract first few words for title
        /// </summary>
        public static string ExtractTitleFromHtml(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                return "Response";

            // Remove HTML tags
            var cleanedContent = HtmlTagsForTitleRegex().Replace(htmlContent, " ");

            // Remove extra whitespace
            cleanedContent = RegexWhitespace().Replace(cleanedContent, " ").Trim();

            var words = cleanedContent.Split(new[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var firstFewWords = words.Take(5).Aggregate((current, next) => current + " " + next);

            if (words.Length > 5)
                firstFewWords += "...";

            return string.IsNullOrWhiteSpace(firstFewWords) ? "Response" : firstFewWords.Trim();
        }

        /// <summary>
        /// Generate mock resources based on response content
        /// </summary>
        public static List<ResourceItem> ExtractResourcesFromResponse(string htmlContent, string userQuery)
        {
            // 1) Prefer hyperlinks already present in the generated HTML (from References section)
            var linkResources = ParseAnchorsFromHtml(htmlContent);
            if (linkResources.Count > 0)
            {
                // Cap to top 5 links for brevity
                return linkResources.Take(5).ToList();
            }

            // 2) Fallback to the existing keyword-based approach
            var resources = new List<ResourceItem>();

            // Extract keywords from content
            var keywords = ExtractKeywords(htmlContent);

            // Generate related resources based on keywords
            int resourceId = 1;

            // Create a search URL for the user query so the resource has a link
            var querySearchUrl = GetSearchUrl(userQuery);

            resources.Add(new ResourceItem
            {
                Id = resourceId++,
                Title = $"Article: {userQuery}",
                Description = "Detailed article with insights, explanations, and relevant information related to your query.",
                SourceType = "Article",
                RelevanceScore = 92,
                Author = "Content Database",
                Icon = "📰",
                Url = querySearchUrl
            });

            foreach (var keyword in keywords.Take(4))
            {
                resources.Add(new ResourceItem
                {
                    Id = resourceId++,
                    Title = $"Deep Dive: {keyword}",
                    Description = $"Comprehensive guide about {keyword}. Explore in-depth information, best practices, and real-world applications.",
                    SourceType = GetRandomSourceType(),
                    RelevanceScore = 85 + (resourceId % 15),
                    Author = "Syncfusion Research",
                    Icon = GetIconForKeyword(keyword),
                    Url = GetSearchUrl(keyword)
                });
            }

            return resources;
        }

        /// <summary>
        /// Extract keywords from HTML content
        /// </summary>
        private static List<string> ExtractKeywords(string htmlContent)
        {
            // Remove HTML tags
            var cleanContent = HtmlTagsRegex().Replace(htmlContent, " ");

            // Split into words
            var words = WordSplitRegex().Split(cleanContent)
                .Where(w => w.Length > 4 && !IsCommonWord(w))
                .Distinct()
                .Take(6)
                .ToList();

            return words;
        }

        /// <summary>
        /// Check if word is a common English word
        /// </summary>
        private static bool IsCommonWord(string word)
        {
            var value = word.ToLowerInvariant();
            return Array.IndexOf(CommonWords, value) >= 0;
        }

        /// <summary>
        /// Get random source type
        /// </summary>
        private static string GetRandomSourceType()
        {
            return SourceTypes[new Random().Next(SourceTypes.Length)];
        }

        /// <summary>
        /// Get icon for keyword
        /// </summary>
        private static string GetIconForKeyword(string keyword)
        {
            keyword = keyword.ToLower();
            return keyword.Contains("feature") ? "⭐" :
                   keyword.Contains("guide") ? "📖" :
                   keyword.Contains("tutorial") ? "🎓" :
                   keyword.Contains("best") ? "✅" :
                   keyword.Contains("practice") ? "💼" :
                   "📄";
        }

        /// <summary>
        /// Build a simple web search URL for a query or keyword
        /// </summary>
        private static string GetSearchUrl(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return string.Empty;

            try
            {
                var encoded = Uri.EscapeDataString(query);
                return $"https://www.google.com/search?q={encoded}";
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parse anchor tags from HTML into ResourceItem entries
        /// </summary>
        private static List<ResourceItem> ParseAnchorsFromHtml(string htmlContent)
        {
            var results = new List<ResourceItem>();
            if (string.IsNullOrWhiteSpace(htmlContent)) return results;

            var matches = AnchorTagRegex().Matches(htmlContent);
            int id = 1;
            foreach (Match m in matches)
            {
                var url = m.Groups[1].Value?.Trim();
                var text = RegexWhitespace().Replace(m.Groups[2].Value ?? string.Empty, " ").Trim();
                if (string.IsNullOrWhiteSpace(url)) continue;

                // Basic normalization and title fallback
                string title = string.IsNullOrWhiteSpace(text) ? GetHostFromUrl(url) : text;
                if (string.IsNullOrWhiteSpace(title)) title = url;

                results.Add(new ResourceItem
                {
                    Id = id++,
                    Title = title,
                    Description = $"Referenced link related to '{title}'.",
                    SourceType = "Link",
                    RelevanceScore = 90 - Math.Min(30, id),
                    Author = "Unknown",
                    Icon = "🔗",
                    Url = url
                });
            }

            // Dedupe by URL
            return results
                .GroupBy(r => r.Url, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();
        }

        private static string GetHostFromUrl(string url)
        {
            try
            {
                var uri = new Uri(url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? url : $"https://{url}");
                return uri.Host;
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Format sections into HTML
        /// </summary>
        public static string FormatSectionsToHtml(List<ResponseSection> sections)
        {
            if (sections.Count == 0)
                return string.Empty;

            var sb = new StringBuilder();
            sb.Append("<div style='font-family: Arial, sans-serif; line-height: 1.6;'>");

            foreach (var section in sections.OrderBy(s => s.Order))
            {
                if (!string.IsNullOrWhiteSpace(section.Heading))
                {
                    sb.Append($"<h3 style='color: #0066cc; margin-top: 15px; margin-bottom: 10px;'>{section.Heading}</h3>");
                }

                sb.Append($"<div style='margin-bottom: 15px;'>{section.Content}</div>");
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        // Compiled regex helpers
        [GeneratedRegex("(?=<b>)", RegexOptions.IgnoreCase)]
        private static partial Regex SectionSplitRegex();

        [GeneratedRegex("<b>(.*?)</b>", RegexOptions.IgnoreCase)]
        private static partial Regex BoldHeadingRegex();

        [GeneratedRegex("<.*?>|&nbsp;|<br>|<br/>")]
        private static partial Regex HtmlTagsForTitleRegex();

        [GeneratedRegex("<.*?>")]
        private static partial Regex HtmlTagsRegex();

        [GeneratedRegex("\\s+")]
        private static partial Regex RegexWhitespace();

        [GeneratedRegex("[\\s,.:;!?]+")]
        private static partial Regex WordSplitRegex();

        [GeneratedRegex("<a[^>]*href=['\"](.*?)['\"][^>]*>(.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline)]
        private static partial Regex AnchorTagRegex();
    }
}
