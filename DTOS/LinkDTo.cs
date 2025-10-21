namespace puc.DTOS
{
#pragma warning disable CS1591

    public class LinkDto
    {
        public int LinkDtoId { get; set; }
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Metodo { get; set; }

        public LinkDto(int linkDtoId, string href, string rel, string metodo)
        {
            LinkDtoId = linkDtoId;
            Href = href;
            Rel = rel;
            Metodo = metodo;
        }
    }

    public class LinksHATEOAS
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    }

}
