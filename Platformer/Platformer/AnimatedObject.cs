using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{

    public class AnimatedObject
    {
        public bool Looping { get; set; }
        public Vector2 Position { get; set; }
        public int Width { get { return mWidth; } }
        public float SecondsPerFrame { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Texture2D mTexture { get; set; }

        public int mWidth { get; set; }
        public int mNumFrames { get; set; } 
        public int mCurrentFrame { get; set; }
        public Vector2 mCenter { get; set; }
        public float mTimeSinceLastFrame { get; set; }

        public AnimatedObject(Texture2D texture, int width, Vector2 center = default(Vector2), bool looping = false, float secondsPerFrame = 0.1f, Vector2 position = default(Vector2))
        {
            mTexture = texture;
            mWidth = width;
            
            SecondsPerFrame = secondsPerFrame;
            Position = position;
            mCenter = center;
            mCurrentFrame = 0;
            Rotation = 0;
            mTimeSinceLastFrame = 0;
            Scale = 1;
            mNumFrames = texture.Width / width;
            Looping = looping;
        }

        public void Update(GameTime gameTime)
        {
            mTimeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastFrame > SecondsPerFrame)
            {
                if (mCurrentFrame == mNumFrames - 1)
                {
                    if (Looping)
                    {
                        mCurrentFrame = 0;
                    }
                }
                else
                {
                    mCurrentFrame++;
                }
                mTimeSinceLastFrame = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(mTexture, Position, new Rectangle(mCurrentFrame * Width, 0, Width, mTexture.Height), Color.White, Rotation, mCenter, Scale, SpriteEffects.None, 0);
        }
    }
}
