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
using System.Linq;
using Microsoft.Xna.Framework;
using SharpGameLib.Gui.Interfaces;
using SharpGameLib.Commands.Interfaces;
using System.Collections.Generic;
using SharpGameLib.Interfaces;

using IDrawable = SharpGameLib.Interfaces.IDrawable;
using SharpGameLib.Graphics.Interfaces;

namespace SharpGameLib.Gui
{
	public class GuiElementChooser : IDrawable, IUpdatable, IGuiCommandReceiver
    {
        private readonly IList<IGuiElement> elements = new List<IGuiElement>();

        private readonly IDictionary<IGuiElement, Action<IGuiElement>> actions = new Dictionary<IGuiElement, Action<IGuiElement>>();

        private Rectangle canvasBounds = default(Rectangle);

        public GuiElementChooser(Rectangle canvasBounds, int options = 0)
        {
            this.canvasBounds = canvasBounds;
            this.Options = options;
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

		public int DrawPriority { get; set; } = int.MaxValue;

        public int Options { get; set; }

        public IGuiElement CurrentSelection { get; private set; }

        public bool IsVertical
        {
            get
            {
                return (this.Options & GuiOptions.OptionVerticalOrientation) != 0;
            }
        }

        public bool IsHorizontal
        {
            get
            {
                return (this.Options & GuiOptions.OptionHorizontalOrientation) != 0;
            }
        }

        public void Add(IGuiElement element, Action<IGuiElement> selectionCallback = null)
        {
            this.elements.Add(element);
            this.actions[element] = selectionCallback;
            if (this.CurrentSelection == null)
            {
                this.CurrentSelection = element;
            }
        }

        public void Remove(IGuiElement element)
        {
            this.elements.Remove(element);
        }

        public void Up()
        {
            if (!this.IsVertical)
            {
                return;
            }

            var next = this.NearestY(1);
            this.ChangeSelectionTo(next);
        }

        public void Down()
        {
            if (!this.IsVertical)
            {
                return;
            }

            var next = this.NearestY(-1);
            this.ChangeSelectionTo(next);
        }

        public void Left()
        {
            if (!this.IsHorizontal)
            {
                return;
            }

            var next = this.NearestX(-1);
            this.ChangeSelectionTo(next);
        }

        public void Right()
        {
            if (!this.IsHorizontal)
            {
                return;
            }

            var next = this.NearestX(1);
            this.ChangeSelectionTo(next);
        }

        public void Select()
        {
            if (this.CurrentSelection == null)
            {
                return;
            }

            this.actions[this.CurrentSelection]?.Invoke(this.CurrentSelection);
        }

		public void Draw(ICanvas canvas)
		{
			foreach (var element in this.elements)
			{
				element.Draw(canvas);
			}
		}

		public void Update(GameTime gameTime)
		{
			foreach (var element in this.elements)
			{
				element.Update(gameTime);
			}
		}

        public void ChangeSelectionTo(IGuiElement next)
        {
            var prev = this.CurrentSelection;
            this.CurrentSelection = next;
            this.SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(this.CurrentSelection, prev));
        }

        private IGuiElement NearestX(int dir)
        {
            var selectedX = this.CurrentSelection?.GetAbsolutePosition(this.canvasBounds).X ?? 0;
			return this.elements.Where(e => dir * (e.GetAbsolutePosition(this.canvasBounds).X - selectedX) > 0)
				.OrderBy(e => dir * (e.GetAbsolutePosition(this.canvasBounds).X - selectedX)).FirstOrDefault() ?? this.CurrentSelection;
        }

        private IGuiElement NearestY(int dir)
        {
            var selectedY = this.CurrentSelection?.GetAbsolutePosition(this.canvasBounds).Y ?? 0;
			return this.elements.Where(e => dir * (selectedY - e.GetAbsolutePosition(this.canvasBounds).Y) > 0)
				.OrderBy(e => dir * (selectedY - e.GetAbsolutePosition(this.canvasBounds).Y)).FirstOrDefault() ?? this.CurrentSelection;
        }

        public class SelectionChangedEventArgs : EventArgs
        {
            public SelectionChangedEventArgs(IGuiElement currentlySelected, IGuiElement previouslySelected)
            {
                this.CurrentLySelected = currentlySelected;
                this.PreviouslySelected = previouslySelected;
            }

            public IGuiElement CurrentLySelected { get; }

            public IGuiElement PreviouslySelected { get; }
        }
    }
}

