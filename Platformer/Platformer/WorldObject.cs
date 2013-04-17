using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class WorldObject 
    {

        public Texture2D Texture;
        public String WalkingTextureName { get; set; }
        public Texture2D WalkingTexture;
        public Vector2 TextureOrigin;
        public Vector2 PositionInWorldSpace { get; set; }
        public String ColorName { get; set; }
        public Color color;
        public String TextureName { get; set; }
        public bool isAnimated { get; set; } 
        public Color[] data;
        public float RotationInWorldSpace;
        public SpriteEffects spEffect { get; set; }
        public AnimatedObject animatedObject;
        public Vector2[] PlatPath;
        public bool IsMoving { get; set; }
        public List<Vector2> PathList { get; set; }
        public bool ReverseOnPathEnd { get; set; }
        public float Speed { get; set; }
        public int direction = 1;
        public bool DoesHurt { get; set; }
        public bool CanFall { get; set; }
        public bool DoesWalk { get; set; }
        public double FallThresh { get; set; }
        public Vector2 PosToPort { get; set; }
        public bool isFalling;
        public double fallTimer;
        public double animTimer;
        protected int targetIndex;
        public bool IsTransparent { get; set; }
        public String DialogueText { get; set; }
        public bool isTalking;
        public bool DoesTalk { get; set; }
        protected bool isReversed;
        private int currentFrame;
        public List<Texture2D> walkAnimList;

        static Vector2 rotate(Vector2 v, float theta)
        {
            return new Vector2((float)(v.X * Math.Cos(theta) - v.Y * Math.Sin(theta)), (float)(v.X * Math.Sin(theta) + v.Y * Math.Cos(theta)));
        }

        public static float min4(float a, float b, float c, float d)
        {
            return Math.Min(Math.Min(a, b), Math.Min(c, d));
        }

        public static float max4(float a, float b, float c, float d)
        {
            return Math.Max(Math.Max(a, b), Math.Max(c, d));
        }


        /// <summary>
        /// Given a rectangle defining an AABB (in world space), and a vector from the
        /// upper-left corner of the AABB to the rotation point, rotate this AABB around
        /// that point, and return the smallest possible AABB that encloses this AABB.
        /// </summary>
        /// <param name="source">The AABB to rotate</param>
        /// <param name="rotationPoint">Vector from the upper-left corner of the AABB to the point of rotation</param>
        /// <param name="angle">Amount to rotate (in radians)</param>
        /// <returns>New AABB that contains the rotated AABB</returns>
        public static Rectangle rotateAABB(Rectangle source, Vector2 rotationPoint, float angle)
        {
            // First, extract the 4 extreme points in the AABB
            Vector2 TopLeft = -rotationPoint;
            Vector2 TopRight = new Vector2(source.Width - rotationPoint.X, -rotationPoint.Y);
            Vector2 BottomLeft = new Vector2(-rotationPoint.X, source.Height - rotationPoint.Y);
            Vector2 BottomRight = new Vector2(source.Width - rotationPoint.X, source.Height - rotationPoint.Y);

            // Rotate these 4 points
            TopLeft = WorldObject.rotate(TopLeft, angle);
            TopRight = WorldObject.rotate(TopRight, angle);
            BottomLeft = WorldObject.rotate(BottomLeft, angle);
            BottomRight = WorldObject.rotate(BottomRight, angle);

            // Find the smallest rectangle that contains these 4 points
            int left = (int)WorldObject.min4(TopLeft.X, TopRight.X, BottomLeft.X, BottomRight.X);
            int right = (int)WorldObject.max4(TopLeft.X, TopRight.X, BottomLeft.X, BottomRight.X);
            int top = (int)WorldObject.min4(TopLeft.Y, TopRight.Y, BottomLeft.Y, BottomRight.Y);
            int bottom = (int)WorldObject.max4(TopLeft.Y, TopRight.Y, BottomLeft.Y, BottomRight.Y);
            return new Rectangle(left, top, right - left, bottom - top);
        }


        /// <summary>
        /// Given an x,y position in Texture space, return true if that location
        /// is a valid location in the texture and contains a non-transparent pixel
        /// </summary>
        /// <param name="x">X position in texture space</param>
        /// <param name="y">Y position in texture space</param>
        /// <returns>True if there is a non-trasparent pixel at this location</returns>
        public bool nonTransparentPixel(int x, int y)
        {
            if (x < 0 || x >= Texture.Width || y < 0 || y >= Texture.Height)
                return false;
            return data[x + y * Texture.Width].A > 0;
        }

        /// <summary>
        /// Given an position in Texture space, return true if that location
        /// is a valid location in the texture and contains a non-transparent pixel
        /// </summary>
        /// <param name="pointInTexureSpace">Point in texture space to check</param>
        /// <returns>True if there is a non-transparent pixel at this location</returns>
        public bool nonTransparentPixel(Vector2 pointInTexureSpace)
        {
            return nonTransparentPixel((int)pointInTexureSpace.X, (int)pointInTexureSpace.Y);
        }

        /// <summary>
        /// Returns the smallest possible Axis Aligned Bounding Box (in world space) that 
        /// completely contains the sprite
        /// </summary>
        /// <returns></returns>
        public Rectangle AABB()
        {
            Rectangle myAABB = new Rectangle((int)(PositionInWorldSpace.X - TextureOrigin.X), (int)(PositionInWorldSpace.Y - TextureOrigin.Y), Texture.Width, Texture.Height);
            if (RotationInWorldSpace == 0)
            {
                return myAABB;
            }
            else
            {
                return WorldObject.rotateAABB(myAABB, TextureOrigin, RotationInWorldSpace);
            }
        }

        /// <summary>
        /// Converrt a point from the object space of this object to the texture space
        /// of this object
        /// </summary>
        /// <param name="pointInObjectSpace">Point to convert from object space to texture space</param>
        /// <returns>The point in texture space</returns>
        public Vector2 objectSpaceToTextureSpace(Vector2 pointInObjectSpace)
        {
            return pointInObjectSpace + TextureOrigin;
        }


        /// <summary>
        /// Converrt a point from the texture space of this object to the object space
        /// of this object
        /// </summary>
        /// <param name="pointInObjectSpace"></param>
        /// <returns>The point in object space</returns>
        public Vector2 TextureSpaceToObjectSpace(Vector2 pointInObjectSpace)
        {
            return pointInObjectSpace - TextureOrigin;
        }


        /// <summary>
        /// Converrt a point from world space to the object space
        /// of this object
        /// </summary>
        /// <param name="pointInWorldSpace">Point to convert from world space</param>
        /// <returns>Point in object space</returns>
        public Vector2 WorldSpaceToObjectSpace(Vector2 pointInWorldSpace)
        {
            return WorldObject.rotate(pointInWorldSpace - PositionInWorldSpace, -RotationInWorldSpace);
        }

        /// <summary>
        /// Converrt a point from the object space of this object to
        /// world space
        /// </summary>
        /// <param name="pointInObjectSpace">Point to convert from object space to world space</param>
        /// <returns>Point in world space</returns>
        public Vector2 ObjectSpaceToWorldSpace(Vector2 pointInObjectSpace)
        {
            return PositionInWorldSpace + WorldObject.rotate(pointInObjectSpace, RotationInWorldSpace);
        }

        /// <summary>
        /// Convert a point from the texture space of this object to world space
        /// </summary>
        /// <param name="pointInTextureSpace">The point to convert to world space</param>
        /// <returns>Point in world space</returns>
        public Vector2 TextureSpaceToWorldSpace(Vector2 pointInTextureSpace)
        {
            return ObjectSpaceToWorldSpace(TextureSpaceToObjectSpace(pointInTextureSpace));
        }

        /// <summary>
        /// Convert a point from world space to the texture space of this object
        /// </summary>
        /// <param name="pointInWorldSpace">The point to conver from world space to texture space</param>
        /// <returns>Point in texture space</returns>
        public Vector2 WorldSpaceToTextureSpace(Vector2 pointInWorldSpace)
        {
            return objectSpaceToTextureSpace(WorldSpaceToObjectSpace(pointInWorldSpace));
        }

        public bool Collides(WorldObject other)
        {
            Rectangle myAABB = AABB();
            Rectangle otherAABB = other.AABB();
            if (myAABB.Intersects(otherAABB))
            {
                // No rotation case
                if (RotationInWorldSpace == 0 && other.RotationInWorldSpace == 0)
                {

                    //   Find the top, left, bottom, and right of the rectangle defined by the 
                    //   interesction of the two AABBs

                    int left = Math.Max(myAABB.Left, otherAABB.Left);
                    int top = Math.Max(myAABB.Top, otherAABB.Top);
                    int bottom = Math.Min(myAABB.Bottom, otherAABB.Bottom);
                    int right = Math.Min(myAABB.Right, otherAABB.Right);

                    // Go through every point in this intersection rectangle, and determine if the
                    // corresponding points in the other two textures are both non-transparent

                    for (int i = left; i < right; i++)
                    {
                        for (int j = top; j < bottom; j++)
                        {
                            int index1 = i - myAABB.Left + (j - myAABB.Top) * Texture.Width;
                            int index2 = i - otherAABB.Left + (j - otherAABB.Top) * other.Texture.Width;
                            if (data[index1].A > 0 && other.data[index2].A > 0)
                            {
                                return true;
                            }

                        }
                    }
                    return false;
                }
                // Rotation case
                else
                {
                    // First, find the 4 corners of the other object in my texture space
                    Vector2 upperLeft = WorldSpaceToTextureSpace(other.TextureSpaceToWorldSpace(new Vector2(0, 0)));
                    Vector2 upperRight = WorldSpaceToTextureSpace(other.TextureSpaceToWorldSpace(new Vector2(other.Texture.Width, 0)));
                    Vector2 lowerRight = WorldSpaceToTextureSpace(other.TextureSpaceToWorldSpace(new Vector2(other.Texture.Width, other.Texture.Height)));
                    Vector2 lowerLeft = WorldSpaceToTextureSpace(other.TextureSpaceToWorldSpace(new Vector2(0, other.Texture.Height)));

                    // Next, find the AABB that contains these points, in my texture space
                    int top = (int)min4(upperLeft.Y, upperRight.Y, lowerLeft.Y, lowerRight.Y);
                    int bottom = (int)max4(upperLeft.Y, upperRight.Y, lowerLeft.Y, lowerRight.Y);
                    int left = (int)min4(upperLeft.X, upperRight.X, lowerLeft.X, lowerRight.X);
                    int right = (int)max4(upperLeft.X, upperRight.X, lowerLeft.X, lowerRight.X);

                    // Next, find the intersection of this AABB and my texture in my texture space
                    int top2 = Math.Max(top, 0);
                    int bottom2 = Math.Min(bottom, Texture.Height);
                    int left2 = Math.Max(0, left);
                    int right2 = Math.Min(right, Texture.Width);


                    // Go though each point in this intersection AABB (which has been defined in *my* texture space), 
                    // and find the corresponding point in the other texture.  Check if both points are non-transparent.
                    for (int i = left2; i < right2; i++)
                    {
                        for (int j = top2; j < bottom2; j++)
                        {
                            Vector2 pointInMyTextureSpace = new Vector2(i, j);
                            Vector2 pointInOtherTextureSpace = other.WorldSpaceToTextureSpace(TextureSpaceToWorldSpace(pointInMyTextureSpace));
                            if (nonTransparentPixel(pointInMyTextureSpace) &&
                                other.nonTransparentPixel(pointInOtherTextureSpace))
                            {
                                return true;
                            }

                        }
                    }
                    return false;
                }
            }
            return false;
        }

        public WorldObject()
        {

        }

        public WorldObject(Texture2D texture)
        {
            color = Color.White;
            Texture = texture;
            TextureOrigin = new Vector2(texture.Width, texture.Height) / 2;
            data = new Color[texture.Width * texture.Height];
            Texture.GetData<Color>(data);
            isAnimated = false;
            RotationInWorldSpace = 0.0f;
        }

        public void setTexture(Texture2D texture)
        {
            color = Color.White;
            Texture = texture;
            TextureOrigin = new Vector2(texture.Width, texture.Height) / 2;
            data = new Color[texture.Width * texture.Height];
            Texture.GetData<Color>(data);
            isAnimated = false;
            RotationInWorldSpace = 0.0f;
        }
        
        public void setWalkTexture(Texture2D texture)
        {
            color = Color.White;
            WalkingTexture = texture;
            TextureOrigin = new Vector2(texture.Width, texture.Height) / 2;
            data = new Color[texture.Width * texture.Height];
            Texture.GetData<Color>(data);
            isAnimated = false;
            RotationInWorldSpace = 0.0f;
        }

        public WorldObject(AnimatedObject animatedObj, Texture2D texture)
        {
            color = Color.White;
            Texture = animatedObj.mTexture;
            TextureOrigin = new Vector2(Texture.Width, Texture.Height) / 2;
            data = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(data);
            isAnimated = true;
            animatedObject = animatedObj;
            RotationInWorldSpace = 0.0f;
        }


        public WorldObject(Texture2D texture, Vector2 initialPosition)
            : this(texture)
        {
            PositionInWorldSpace = initialPosition;
        }

        /// <summary>
        /// Update for world objects
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="direction">a direction of 0 means "not moving", 1 means "going up", 2 means "moving right"</param>
        public void Update(GameTime gameTime, int direction)
        {
            


            if (DoesWalk)
            {
                spEffect = SpriteEffects.FlipHorizontally;
                if (isReversed)
                    spEffect = SpriteEffects.FlipHorizontally;
                else
                    spEffect = SpriteEffects.None;
                if (animTimer > 200)
                {
                    currentFrame++;
                    if (currentFrame >= walkAnimList.Count)
                        currentFrame = 0;
                    Texture = walkAnimList[currentFrame];
                    animTimer = 0;
                }
                else
                {
                    animTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }

            if (isAnimated)
            {
                PositionInWorldSpace = animatedObject.Position;
            }

            if (IsMoving)
            {
                Vector2 target = PathList[targetIndex];
                float moveDistance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (moveDistance == 0)
                {
                    return;
                }
                while ((target - PositionInWorldSpace).LengthSquared() < moveDistance * moveDistance)
                {
                    PositionInWorldSpace = target;
                    incrementTargetIndex();
                    target = PathList[targetIndex];
                }
                Vector2 moveVector = (target - PositionInWorldSpace);
                moveVector.Normalize();
                moveVector = moveVector * moveDistance;
                PositionInWorldSpace = PositionInWorldSpace + moveVector;
            }

            if(isFalling)
            {
                fallTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
                color = Color.Black;
                if (fallTimer >= FallThresh)
                {

                    Vector2 target = PathList[1];
                    float moveDistance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    while ((target - PositionInWorldSpace).LengthSquared() < moveDistance * moveDistance)
                    {
                        PositionInWorldSpace = target;
                        isFalling = false;
                        fallTimer = 0;
                        PositionInWorldSpace = PathList[0];
                    }

                    Vector2 moveVector = (target - PositionInWorldSpace);
                    moveVector.Normalize();
                    moveVector = moveVector * moveDistance;
                    PositionInWorldSpace = PositionInWorldSpace + moveVector;

                }
                
            }
        }
        protected void incrementTargetIndex()
        {
            targetIndex += direction;
            if (DoesWalk)
            {
                if (targetIndex == PathList.Count)
                    isReversed = true;
                else if (targetIndex == 1) 
                    isReversed = false;
            }
            if (targetIndex >= PathList.Count)
            {
                if (ReverseOnPathEnd)
                {
                    
                    direction = -1;
                    targetIndex += direction + direction;
                }
                else
                    targetIndex = 0;
            }
            else if (targetIndex < 0)
            {
                targetIndex = 1;
                direction = 1;
            }
        }
        
        


    }
}

