namespace Catalog.Application.Dtos
{
    public class OptionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool Varianter { get; set; }
        public bool IsActive { get; set; }
    }
}