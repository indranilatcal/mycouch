using MyCouch.EntitySchemes.Reflections;

namespace MyCouch.EntitySchemes
{
    public class EntityReflector
    {
        public IEntityMember IdMember { get; set; }
        public IEntityMember RevMember { get; set; }

        public EntityReflector(IDynamicPropertyFactory dynamicPropertyFactory)
        {
            IdMember = new EntityIdMember(dynamicPropertyFactory);
            RevMember = new EntityRevMember(dynamicPropertyFactory);
        }
    }
}