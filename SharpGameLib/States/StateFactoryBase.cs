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

﻿
using SharpGameLib.States.Interfaces;
using System;

namespace SharpGameLib.States
{
    public class StateFactoryBase<TStateBase> : IStateFactory<TStateBase> where TStateBase : IState
	{
        /// <summary>
        /// Fetches a new block state of type TState using simple reflection and a default
        /// constructor parameter pattern.
        /// </summary>
        /// <typeparam name="TBlockState">The 1st type parameter.</typeparam>
        public TStateBase Create<TState>() where TState : TStateBase
        {
            var instance = this.InvokeConstructor<TState>();
            if (instance == null)
            {
                throw new InvalidOperationException(
                    $"Unable to create block state with type {typeof(TState)}; is there a valid constructor?");

            }

            return (TState)instance;
        }

        public TStateBase CreateWithCache<TState>() where TState : TStateBase
        {
            return new AutoCacheStateFactory<TStateBase>(this).Create<TState>();
        }

        protected virtual object InvokeConstructor<T>()
        {
            var constructorParams = new object[] { this };
            return Activator.CreateInstance(typeof(T), constructorParams);
        }
	}
}

