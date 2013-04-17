using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Platformer
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>

    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public World mWorld { get; set; }
        public Camera camera { get; set; }
        public MenuScreen menuScreen;

        private AnimatedObject playerSprite;
        private SoundEffect jumpSound, landSound;
        private WorldObject playerInWorld;
        private Texture2D playerTexture, playerFlipped, playerTextureWalk, playerJumpText;
        private PlayerData[] playerData;
        private float jumpDistance, jumpSpeed, jumpDestination;
        private float playerSpeed, topSpeed;
        private float animTimer, frameCount;
        private double floatTimer;
        private float fallTimer;
        private bool isJumping, isAccelerating, isFloating, isFalling = true;
        private KeyboardState currentKeys, lastKeys;
        private Vector2 posOnObject;
        private bool win;
        private bool fallTimerBool;
        private SpriteFont mFont;
        private Vector2 startPos;

        private List<Texture2D> walkAnimList;
        private int currentFrame;

        GamePadState currPadOne;

        public Player(Game game)
            : base(game)
        {
        }

        public void LoadContent(ContentManager content)
        {
            playerData = content.Load<PlayerData[]>("PlayerSprite");
            playerTexture = content.Load<Texture2D>(playerData[0].spriteName);
            playerTexture.Name = "regular";
            playerTextureWalk = content.Load<Texture2D>(playerData[0].spriteWalking);
            playerJumpText = content.Load<Texture2D>(playerData[0].spriteJumping);
            playerTextureWalk.Name = playerData[0].spriteWalking;
            playerFlipped = content.Load<Texture2D>(playerData[0].spriteFlipped);
           
            jumpSound = content.Load<SoundEffect>(playerData[0].jumpSound);
            landSound = content.Load<SoundEffect>(playerData[0].landSound);
            mFont = content.Load<SpriteFont>("SpriteFont1");
            topSpeed = playerData[0].topSpeed;
            playerSpeed = playerData[0].speed;
            jumpSpeed = playerData[0].jumpSpeed;
            jumpDistance = playerData[0].jumpDistance;
            currentKeys = Keyboard.GetState();

            walkAnimList = new List<Texture2D>();
            walkAnimList.Add(playerTexture);
            walkAnimList.Add(playerTextureWalk);

            playerInWorld = new WorldObject(playerTexture, playerData[0].startingPoint);
            playerInWorld.TextureOrigin = playerData[0].center;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //playerSprite.Update(gameTime);
            lastKeys = currentKeys;
            currentKeys = Keyboard.GetState();
            //Vector2 playerPos = playerSprite.Position;
            Vector2 playerPos = playerInWorld.PositionInWorldSpace;
            Vector2 cameraPos = camera.Position;
            GamePadState prevStat = currPadOne;
            currPadOne = GamePad.GetState(PlayerIndex.One);

            if (isAccelerating)
            {
                if (playerSpeed < topSpeed)
                    playerSpeed += 0.1f;
                else
                    isAccelerating = false;
            }
            else
            {
                if (playerSpeed > 2)
                    playerSpeed -= 0.5f;

            }

            //if (currentKeys.IsKeyDown(Keys.F))
            //    isAccelerating = true;
            //else
            //    isAccelerating = false;
            isAccelerating = true;

            if (((currentKeys.IsKeyDown(Keys.Space) && lastKeys.IsKeyUp(Keys.Space)) ||
                (currPadOne.Buttons.A == ButtonState.Pressed && prevStat.Buttons.A == ButtonState.Released)) 
                && !isFalling && !isJumping & !isFloating)
            {
                playerInWorld.Texture = playerJumpText;
                isJumping = true;
                //jumpSound.Play();
                jumpDestination = playerPos.Y - jumpDistance;
            }

            if (isJumping)
            {
                playerPos = playerJump(gameTime, jumpDestination);
            }

            if (isFloating && floatTimer >= 180)
            {
                isFalling = true;
                isFloating = false;
                floatTimer = 0;
            }

            if (isFalling)
            {
                playerPos = playerFall(gameTime, playerInWorld);
            }

            if(currentKeys.IsKeyDown(Keys.A) != currentKeys.IsKeyDown(Keys.D))
            {

                playerAnims(gameTime, "walking");
                if (currentKeys.IsKeyDown(Keys.A) || currPadOne.ThumbSticks.Left.X < 0)
                {
                    playerInWorld.spEffect = SpriteEffects.FlipHorizontally;
                    if (CheckBounds(1))
                    {
                        playerPos.X -= playerSpeed;
                    }
                }
                else if (currentKeys.IsKeyDown(Keys.D) || currPadOne.ThumbSticks.Left.X > 0)
                {
                    playerInWorld.spEffect = SpriteEffects.None;
                    if(CheckBounds(2))
                    {
                        playerPos.X += playerSpeed;
                    }
                }
            }

            if (checkFalling(playerInWorld, ref playerPos) && !isFalling && !isJumping && !isFloating)
                isFalling = true;
            checkFalling(playerInWorld, ref playerPos);

            if (playerPos.X < mWorld.worldLeftBound + Game1.Width / 2)
                cameraPos.X = mWorld.worldLeftBound + Game1.Width / 2;
            else if (playerPos.X > mWorld.worldRightBound - Game1.Width/2)
                cameraPos.X = mWorld.worldRightBound - Game1.Width/2;
            else
                cameraPos.X = playerPos.X;

            if (playerPos.Y > mWorld.worldBottomBound + Game1.Height/2)
                cameraPos.Y = Game1.Height / 2;
            else if (playerPos.Y < mWorld.worldTopBound)
                cameraPos.Y = mWorld.worldTopBound;
            else
                cameraPos.Y = playerPos.Y;

            if (playerPos.Y >= Game1.Height)
                playerPos = playerData[0].startingPoint;
            camera.Position = cameraPos;
            camera.Update(gameTime);
            mWorld.camera = camera;
            playerInWorld.PositionInWorldSpace = playerPos;

            if (((currentKeys.IsKeyDown(Keys.B) && lastKeys.IsKeyUp(Keys.B)) ||
                (currPadOne.Buttons.B == ButtonState.Pressed && prevStat.Buttons.B == ButtonState.Released))
                && !isFalling && !isJumping & !isFloating)
            {
                objInteract(playerInWorld);
            }
            floatTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (menuScreen.restart) playerInWorld.PositionInWorldSpace = playerData[0].startingPoint;
            base.Update(gameTime);
        }

        public void playerAnims(GameTime gameTime, String action)
        {
            if (action == "walking" && !isFalling && !isJumping &&
                ((animTimer > 200 && isAccelerating) || (animTimer > 400 && !isAccelerating)))
            {
                currentFrame++;
                if (currentFrame >= walkAnimList.Count)
                    currentFrame = 0;
                playerInWorld.Texture = walkAnimList[currentFrame];
                animTimer = 0;
            }
            else
            {
                animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void objInteract(WorldObject playerObject)
        {
            foreach (WorldObject obj in mWorld.worldObjectList)
            {
                if (playerObject.Collides(obj))
                {
                    if (obj.TextureName == "treasure") gameOver(1);
                    if (obj.TextureName == "bottomPort")
                    {
                        playerInWorld.PositionInWorldSpace = obj.PosToPort;
                    }
                    if (obj.DoesTalk) 
                    {
                        obj.isTalking = true;
                        camera.textTime = 0;
                    }
                }
            }
        }

        public void gameOver(int status)
        {
            menuScreen.gameStatus = 1;
            
        }

        public Vector2 playerJump(GameTime gameTime, float destY)
        {
            Vector2 place = playerInWorld.PositionInWorldSpace;
            float distToTravel = jumpSpeed;
            if (distToTravel > place.Y - destY)
            {
                place.Y = destY;
                isJumping = false;
                isFloating = true;
                floatTimer = 0;
            }
            else
            {
                place.Y -= (float)(distToTravel);
            }

            //Vector2 disp = new Vector2(place.X, destY) - place;
            //disp.Normalize();
            //place += disp * distToTravel;
            return place;
        }


        /// <summary>
        /// Checks if sprite is it collides with other objects
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool checkFalling(WorldObject playerObj, ref Vector2 position)
        {
            bool falling = true;
            foreach (WorldObject obj in mWorld.worldObjectList)
            {
                if (playerObj.Collides(obj) && !obj.IsTransparent)
                {
                    if (obj.DoesHurt)
                    {
                        position = playerData[0].startingPoint;
                       
                    }
                    else if (obj.CanFall)
                    {
                        obj.isFalling = true;
                        obj.color = Color.Blue;
                        falling = false;
                    }
                    else
                    {
                        if (playerObj.PositionInWorldSpace.Y <= obj.PositionInWorldSpace.Y - obj.TextureOrigin.Y)
                        {
                            if (obj.IsMoving)
                            {
                                //position = obj.PositionInWorldSpace - obj.WorldSpaceToObjectSpace(position);
                                position.X += 1f * obj.direction;
                            }
                            falling = false;
                        }
                    }    
                }
                else 
                {
                    obj.color = Color.White;
                }
            }
            return falling;
        }

        public Vector2 playerFall(GameTime gameTime, WorldObject playerObj)
        {
            Vector2 place = playerInWorld.PositionInWorldSpace;
            place.Y += jumpSpeed;
            float distToTravel = jumpSpeed;

            foreach (WorldObject obj in mWorld.worldObjectList)
            {
                if (playerObj.PositionInWorldSpace.Y <= obj.PositionInWorldSpace.Y - obj.TextureOrigin.Y)
                {
                    if (playerObj.Collides(obj) && !obj.IsTransparent)
                    {
                        isFalling = false;
                        //landSound.Play();
                        playerInWorld.Texture = playerTexture;
                        place.Y = obj.PositionInWorldSpace.Y - obj.TextureOrigin.Y;
                        if (!playerObj.Collides(obj)) place.Y = 100;
                        return playerObj.PositionInWorldSpace;
                    }
                }
            }
            return place;
            
        }

        public void Draw(SpriteBatch sb)
        {
            camera.Draw(playerInWorld);
        }

        /// <summary>
        /// Checks to see if the player has room to move without walking past the screen
        /// </summary>
        /// <param name="direction">1 = left, 2 = right</param>
        /// <returns></returns>
        public bool CheckBounds(int direction)
        {
            if (direction == 1 && playerInWorld.PositionInWorldSpace.X > mWorld.worldLeftBound + playerTexture.Width/4)
                return true;
            else if (direction == 2 && playerInWorld.PositionInWorldSpace.X < mWorld.worldRightBound - playerTexture.Width / 4)
                return true;
            return false;
        }
    }
}
