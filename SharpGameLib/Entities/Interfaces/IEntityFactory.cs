
namespace SharpGameLib.Entities.Interfaces
{
    public interface IEntityFactory
    {
        IEntity Create(IEntityData entityData);
    }

    public interface IEntityFactory<TEntity> : IEntityFactory where TEntity : IEntity
    {
        new TEntity Create(IEntityData entityData);
    }
}
