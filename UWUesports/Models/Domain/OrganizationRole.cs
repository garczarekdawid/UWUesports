namespace UWUesports.Web.Models.Domain
{
    public class OrganizationRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Klucz obcy
        public int OrganizationId { get; set; }

        // Właściwość nawigacyjna
        public Organization Organization { get; set; }
    }
}
