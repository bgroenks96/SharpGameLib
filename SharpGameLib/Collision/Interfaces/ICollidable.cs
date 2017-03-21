/*
 * Copyright (C) 2016-2017 (See COPYRIGHT.txt for holders)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

﻿using System;
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

