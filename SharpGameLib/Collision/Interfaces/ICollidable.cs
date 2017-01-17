using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Interfaces;
using SharpGameLib.Entities.Interfaces;

namespace SharpGameLib.Collision.Interfaces
{
    public interface ICollidable : IMovable, IUpdatable, IStageActor
    {
        /// <summary>
        /// The rectangle bounds of this collidable type.
        /// </summary>
        /// <value>The bounds.</value>
        RectangleF Bounds { get; }

        Color DebugColor { get; set; }

        bool IsEnabled { get; set; }

        /// <summary>
        /// Called by <see cref="ICollisionContainer"/> when this object collides with another
        /// ICollidable in the container space. Implementations of this method should check if
        /// target is of type ICollidable<TSelf> where TSelf is the implementation's concrete or base
        /// type and then invoke the generic version of ICollidable if it implements that type.
        /// </summary>
        /// <param name="target">the collision target</param>
        /// <param name="collisionMoment">information about the collision</param>
        /// <param name="container">the container generating this collision event</param>
        void OnCollideWith(ICollidable target, CollisionMoment collisionMoment, ICollisionContainer container);

        /// <summary>
        /// Called by <see cref="ICollisionContainer"/> when this object is about to exit the bounds of the container.
        /// </summary>
        /// <param name="edgePosition">
        /// the position on one of the container bounds edges where this collidable's movement vector intersects
        /// </param>
        /// <param name="container">the collision container</param>
        void OnContainerExit(Vector2 edgePosition, ICollisionContainer container);
    }

    public interface ICollidable<TCollidable> : ICollidable where TCollidable : ICollidable
    {
        /// <summary>
        /// Called by a TCollidable when it collides with this ICollidable type. Allows for type-specific
        /// processing of collision events. Note that the ordering of a call to this method and a call to
        /// <see cref="OnCollideWith(ICollidable, RectangleF, Vector2, ICollisionContainer)"/> is NOT guarranteed.
        /// </summary>
        /// <param name="target">the target collidable</param>
        /// <param name="collisionMoment">information about the collision</param>
        /// <param name="container">Container.</param>
        void OnCollideWith(TCollidable target, CollisionMoment collisionMoment, ICollisionContainer container);
    }
}

