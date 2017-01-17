using System;
using Microsoft.Xna.Framework;
using SharpGameLib.Collision;

namespace SharpGameLib
{
    public sealed class SpriteConfig
    {
        public SpriteConfig(
            int xoffs = 0,
            int yoffs = 0,
            int width = 0,
            int height = 0,
            int xstride = 0,
            int ystride = 0,
            int frameCount = 1,
            double animationDuration = 500,
            uint stateCode = 0)
        {
            this.XOffs = xoffs;
            this.YOffs = yoffs;
            this.Width = width;
            this.Height = height;
            this.XStride = xstride;
            this.YStride = ystride;
            this.FrameCount = frameCount;
            this.AnimationDuration = animationDuration;
            this.StateCode = stateCode;
        }

        public int XOffs { get; }

        public int YOffs { get; }

        public int Width { get; }

        public int Height { get; }

        public int XStride { get; }

        public int YStride { get; }

        public int FrameCount { get; }

        public double AnimationDuration { get; }

        public uint StateCode { get; }

        public RectangleF BoundsWith(Vector2 position)
        {
            return new RectangleF(position.X, position.Y, this.Width, this.Height);
        }
    }
}

