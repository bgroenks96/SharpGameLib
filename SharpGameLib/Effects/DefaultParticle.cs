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
using SharpGameLib.Effects.Interfaces;
using SharpGameLib.Sprites.Interfaces;
using SharpGameLib.Interfaces;
using Microsoft.Xna.Framework;

using IDrawable = SharpGameLib.Interfaces.IDrawable;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Effects
{
    public class DefaultParticle : IParticle, IMovable, IDrawable, IUpdatable
    {
        public DefaultParticle(ISprite sprite, TimeSpan duration)
        {
            this.Sprite = sprite;
            this.Duration = duration;

            this.Id = Guid.NewGuid();
        }

        public ISprite Sprite { get; }

        public TimeSpan Duration { get; }

        public Guid Id { get; }

        public void Update(GameTime gameTime)
        {
            this.Sprite.Update(gameTime);
        }

        public void Draw(ICanvas canvas)
        {
            this.Sprite.Draw(canvas);
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

        public Vector2 Acceleration
        {
            get
            {
                return this.Sprite.Acceleration;
            }
            set
            {
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

        public virtual void OnExpired()
        {
        }

        public virtual void OnEmit(IParticleEmitter emitter)
        {
        }

        public virtual void OnEnter(IStage stage)
        {
        }

        public virtual void OnExit(IStage stage)
        {
        }
    }
}

