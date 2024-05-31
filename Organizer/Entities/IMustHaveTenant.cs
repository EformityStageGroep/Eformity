namespace Organizer.Entities
{
    public interface IMustHaveTenant
    {
        string tenant_id { get; set; }
    }
}