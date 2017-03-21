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
using SharpGameLib.Entities.Interfaces;
using SharpGameLib.Sprites.Interfaces;
using Microsoft.Xna.Framework;
using SharpGameLib.Interfaces;
using SharpGameLib.Collision.Interfaces;
using SharpGameLib.Graphics.Interfaces;
using SharpGameLib.Collision;

namespace SharpGameLib.Entities
{
    public abstract class EntityBase : IEntity
    {
        public Guid Id { get; } = Guid.NewGuid();

        public ISprite Sprite { get; protected set; }

        public ICollidable Collidable { get; protected set; }

        public RectangleF Bounds
        {
            get
            {
                var bounds = this.Sprite.Config.BoundsWith(this.Position);
                bounds.Width *= (int)this.Sprite.Scale.X;
                bounds.Height *= (int)this.Sprite.Scale.Y;
                return bounds;
            }
        }

        public bool Falling {
            get {
                return this.Velocity.Y > 0;
            }
        }

        public bool HasGravity { get; set; } = false;

		public Vector2 Gravity { get; set; } = Vector2.Zero;

        public bool IsCollisionEnabled
        {
            get
            {
                return this.Collidable?.IsEnabled ?? false;
            }

            set
            {
                if (this.Collidable == null)
                {
                    return;
                }

                this.Collidable.IsEnabled = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.Sprite.Position;
            }

            set
            {
                this.Sprite.Position = value;
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return this.Sprite.Velocity;
            }

            set
            {
                this.Sprite.Velocity = value;
            }
        }

        public Vector2 Acceleration {
            get {
                return this.Sprite.Acceleration;
            }

            set {
                this.Sprite.Acceleration = value;
            }
        } 

        public int DrawPriority
        {
            get
            {
                return this.Sprite.DrawPriority;
            }

            set
            {
                this.Sprite.DrawPriority = value;
            }
        }

        protected IStage CurrentStage { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            this.Velocity += this.Acceleration;
            this.Position += this.Velocity;

            // clip precision on move vectors
            this.Acceleration = MathUtils.Round(this.Acceleration);
            this.Velocity = MathUtils.Round(this.Velocity);
            this.Position = MathUtils.Round(this.Position);

            this.Sprite.Update(gameTime);

			if (this.HasGravity)
			{
				this.Acceleration = this.Gravity;
			}
        }

        public virtual void Draw(ICanvas canvas)
        {
            this.Sprite.Draw(canvas);
        }

        public virtual void OnEnter(IStage stage)
        {
			this.CurrentStage = stage;
            if (this.Collidable == null)
            {
                return;
            }

            stage.Add(this.Collidable);
        }

        public virtual void OnExit(IStage stage)
        {
			this.CurrentStage = null;
            if (this.Collidable == null)
            {
                return;
            }

            stage.Remove(this.Collidable);
        }

        public override bool Equals(object obj)
        {
            return (obj as IEntity)?.Id.Equals(this.Id) ?? false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public virtual Vector2 StartVelocity()
        {
            return Vector2.Zero;
        }
    }
}

