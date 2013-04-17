using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }

        private KeyboardState currentKeys, lastKeys;
        private SpriteBatch mSpriteBatch;
        private bool zooming = true;
        private SpriteFont sFont;
        public float textTime;

        public Camera(SpriteBatch sb, Game game)
        {
            mSpriteBatch = sb;
            Position = new Vector2(Game1.Width / 2, Game1.Height / 2);
            Rotation = 0.0f;
            Zoom = 1.0f;
            currentKeys = Keyboard.GetState();
            sFont = game.Content.Load<SpriteFont>("SpriteFont1"); 
            
        }

        public void Begin()
        {
            mSpriteBatch.Begin();
        }

        public void End()
        {
            mSpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            textTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;

            lastKeys = currentKeys;
            currentKeys = Keyboard.GetState();
            Vector2 newPos = Position;

            if (currentKeys.IsKeyDown(Keys.Z) && lastKeys.IsKeyUp(Keys.Z) && Zoom < 5.0f)
            {
                Zoom += 0.2f;
            }

            if (currentKeys.IsKeyDown(Keys.X) && lastKeys.IsKeyUp(Keys.X) && Zoom > 1.0f)
            {
                Zoom -= 0.2f;
            }
           
            Position = newPos;
        }

        public void Draw(WorldObject obj)
        {
            Vector2 objPosInCameraSpace = obj.PositionInWorldSpace - Position;

            objPosInCameraSpace = new Vector2(objPosInCameraSpace.X * (float)Math.Cos(-Rotation) -
                                              objPosInCameraSpace.Y * (float)Math.Sin(-Rotation),
                                              objPosInCameraSpace.X * (float)Math.Sin(-Rotation) +
                                              objPosInCameraSpace.Y * (float)Math.Cos(-Rotation));
            objPosInCameraSpace *= Zoom;

            Vector2 objPosInScreenSpace = objPosInCameraSpace + new Vector2(Game1.Width / 2, Game1.Height / 2);


            float rotationInScreenSpace = obj.RotationInWorldSpace - Rotation;
    
            if(obj.isAnimated)
                mSpriteBatch.Draw(obj.Texture, objPosInScreenSpace, 
                                new Rectangle(obj.animatedObject.mCurrentFrame * obj.animatedObject.Width, 0, 
                                    obj.animatedObject.Width, obj.animatedObject.mTexture.Height),
                                        obj.color, Rotation, obj.TextureOrigin, Zoom, SpriteEffects.None, 0);
            else if(obj.spEffect == SpriteEffects.None)
                mSpriteBatch.Draw(obj.Texture, objPosInScreenSpace, null, obj.color, rotationInScreenSpace, obj.TextureOrigin, Zoom, SpriteEffects.None, 0);
            else
                mSpriteBatch.Draw(obj.Texture, objPosInScreenSpace, null, obj.color, rotationInScreenSpace, obj.TextureOrigin, Zoom, obj.spEffect, 0);


            if (textTime < 3500 && obj.isTalking)
            {

                mSpriteBatch.DrawString(sFont, obj.DialogueText.Replace("@", System.Environment.NewLine),
                                            new Vector2(objPosInScreenSpace.X + 20, objPosInScreenSpace.Y - 70),
                                            Color.White, 0, Vector2.Zero, 0.2f, SpriteEffects.None, 0);
            }
            else
            {
                obj.isTalking = false;
            }
        }
    }
}
