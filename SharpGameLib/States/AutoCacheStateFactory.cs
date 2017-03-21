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
using SharpGameLib.Interfaces;
using System.Collections.Generic;
using SharpGameLib.States.Interfaces;

namespace SharpGameLib.States
{
    /// <summary>
    /// IStateFactory decorated that automatically caches created instances by type.
    /// </summary>
    public sealed class AutoCacheStateFactory<TStateBase> : IStateFactory<TStateBase> where TStateBase : IState
    {
        private readonly IDictionary<Type, TStateBase> StateTypeCache = new Dictionary<Type, TStateBase>();

        private readonly IStateFactory<TStateBase> factory;

        public AutoCacheStateFactory(IStateFactory<TStateBase> factory)
        {
            this.factory = factory;
        }

        public TStateBase Create<TState>() where TState : TStateBase
        {
            return this.ShouldCreate(typeof(TState)) ? this.factory.Create<TState>() : this.StateTypeCache[typeof(TState)];
        }

        public TStateBase CreateWithCache<TState>() where TState : TStateBase
        {
            return this.Create<TState>();
        }

        private bool ShouldCreate(Type type)
        {
            return !this.StateTypeCache.ContainsKey(type);
        }
    }
}

