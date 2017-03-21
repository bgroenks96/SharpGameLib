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

﻿using SharpGameLib.Interfaces;
using System.Collections.Generic;
using SharpGameLib.Sprites.Interfaces;
using System.Threading.Tasks;

namespace SharpGameLib.Effects.Interfaces
{
    public delegate IParticle ParticleFactory(int n);

    public interface IParticleEmitter : IUpdatable, IDrawable, IStageActor
    {
		ParticleFactory Factory { get; set; }

        bool HasParticles { get; }

        /// <summary>
        /// Emits the specified number of particles. The task returned will
        /// be completed when all newly emitted particles have exhausted their
        /// duration.
        /// </summary>
        /// <param name="count">Count.</param>
        Task Emit(int count);

        /// <summary>
        /// Removes the given particle from the current collection, if it exists.
        /// </summary>
        /// <returns><c>true</c> if the particle existed in the emitter and was canceled; otherwise, <c>false</c>.</returns>
        /// <param name="particle">Particle.</param>
        bool Cancel(IParticle particle);

        Task AwaitExpirationAsync();
    }
}

