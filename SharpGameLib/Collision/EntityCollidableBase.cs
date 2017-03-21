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
using SharpGameLib.Collision.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Entities.Interfaces;
using SharpGameLib.Interfaces;

namespace SharpGameLib.Collision
{
    public abstract class EntityCollidableBase<TEntity> : CollidableBase, IEntityCollidable<TEntity> where TEntity : IEntity
    {
        protected EntityCollidableBase(TEntity entity)
        {
            this.Entity = entity;
            this.DebugColor = Color.Blue;
        }

        public TEntity Entity { get; }

        public override RectangleF Bounds
        {
            get
            {
                return this.Entity.Bounds;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return this.Entity.Position;
            }
            set
            {
                this.Entity.Position = value;
            }
        }
        public override Vector2 Velocity
        {
            get
            {
                return this.Entity.Velocity;
            }
            set
            {
                this.Entity.Velocity = value;
            }
        }
        public override Vector2 Acceleration
        {
            get
            {
                return this.Entity.Acceleration;
            }
            set
            {
                this.Entity.Acceleration = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void OnContainerExit(Vector2 edgePosition, ICollisionContainer container)
        {
        }
    }
}

