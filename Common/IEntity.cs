 
namespace Common
{    
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
