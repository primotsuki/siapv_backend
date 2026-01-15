using HtmlAgilityPack;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using siapv_backend.Helpers;
namespace siapv_backend.Helpers
{
   public class HtmlParser
    {
        public void Render(string html, ColumnDescriptor column)
        {
            var cleanHtml = HtmlSanitizerHelper.Sanitize(html);
            var doc = new HtmlDocument();
            doc.LoadHtml(cleanHtml);

            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                RenderNode(node, column);
            }
        }
        private void RenderNode(HtmlNode node, ColumnDescriptor column)
        {
            switch (node.Name)
            {
                case "p":
                RenderParagraph(node, column);
                break;
                case "ul":
                RenderUnorderedList(node, column);
                break;
                case "ol":
                RenderOrderedList(node, column);
                break;
            }
        }
        private void RenderParagraph(HtmlNode p, ColumnDescriptor column)
        {
            column.Item().PaddingBottom(6).Text(text =>
            {
                text.Justify();
                foreach (var child in p.ChildNodes)
                {
                    if (child.Name == "strong")
                        text.Span(child.InnerText).Bold();
                    else if (child.NodeType == HtmlNodeType.Text)
                        text.Span(child.InnerText);
                }
            });
        }
        private void RenderUnorderedList(HtmlNode ul, ColumnDescriptor column)
        {
            column.Item().PaddingBottom(6).Column(col =>
            {
                foreach (var li in ul.Elements("li"))
                {
                    col.Item().Row(row =>
                    {
                        row.ConstantItem(14).Text("•");

                        row.RelativeItem().Text(text =>
                        {
                            foreach (var child in li.ChildNodes)
                            {
                                if (child.Name == "strong")
                                    text.Span(child.InnerText).Bold();
                                else if (child.NodeType == HtmlNodeType.Text)
                                    text.Span(child.InnerText);
                            }
                        });
                    });
                }
            });
        }
        private void RenderOrderedList(HtmlNode ol, ColumnDescriptor column)
        {
            int index = 1;

            column.Item().PaddingBottom(6).Column(col =>
            {
                foreach (var li in ol.Elements("li"))
                {
                    col.Item().Row(row =>
                    {
                        row.ConstantItem(20).Text($"{index}.");

                        row.RelativeItem().Text(text =>
                        {
                            foreach (var child in li.ChildNodes)
                            {
                                if (child.Name == "strong")
                                    text.Span(child.InnerText).Bold();
                                else if (child.NodeType == HtmlNodeType.Text)
                                    text.Span(child.InnerText);
                            }
                        });
                    });

                    index++;
                }
            });
        }
    } 
}