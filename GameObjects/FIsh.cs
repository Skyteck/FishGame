using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace FishGame.GameObjects
{
    class Fish : Sprite
    {
        Random ran;
        double moveTimer;
        Vector2 targetPos;
        Texture2D mouthTex;

        FoodPellet closestPellet;
        bool pelletLastFrame = false;

        FishTail myTail;
        enum CurrentDirection
        {
            kDirectionRight,
            kDirectionLeft
            //kDirectionUpRight,
            //kDirectionDownRight,
            //kDirectionUpLeft,
            //kDirectionDownLeft
        }

        CurrentDirection MyDir = CurrentDirection.kDirectionRight;
        Rectangle FishMouth
        {
            //get
            //{
            //    Rectangle newRect;
            //    if(MyDir == CurrentDirection.kDirectionRight)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Right - (int)(15 * this._Scale.X), (this._BoundingBox.Top + (this._BoundingBox.Height / 2)) + (int)(5 * this._Scale.Y), 5, 5);
            //    }
            //    else if (MyDir == CurrentDirection.kDirectionLeft)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Left + (int)(10 * this._Scale.X), (this._BoundingBox.Top + (this._BoundingBox.Height / 2) + (int)(5 * this._Scale.Y)), 5, 5);
            //    }
            //    else if (MyDir == CurrentDirection.kDirectionUpRight)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Right - (int)(15 * this._Scale.X), (this._BoundingBox.Top + (int)(20 * this._Scale.Y)), 5, 5);
            //    }
            //    else if (MyDir == CurrentDirection.kDirectionDownRight)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Right - 25, (this._BoundingBox.Bottom - 20), 5, 5);
            //    }
            //    else if (MyDir == CurrentDirection.kDirectionUpLeft)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Left + 10, (this._BoundingBox.Top + 20), 5, 5);
            //    }
            //    else if (MyDir == CurrentDirection.kDirectionDownLeft)
            //    {
            //        newRect = new Rectangle(this._BoundingBox.Left + 20, (this._BoundingBox.Bottom - 20), 5, 5);
            //    }
            //    else
            //    {
            //        newRect = new Rectangle();
            //    }
            //    return newRect;
            //}
            get
            {
                Vector2 mouthPos = this._Position;
                if(_FlipX == false)
                {
                    //mouthPos.X = this._Position.X + (16 * this._Scale.X);
                    //mouthPos.Y = this._Position.Y + (5 * this._Scale.Y);
                }

                float radias = Vector2.Distance(mouthPos, this._Position); // 16 at scale = 1;
                radias = 16 * this._Scale.X;
                if(_FlipX)
                {
                    radias *= -1;
                }
                double mathSin = Math.Sin(_Rotation + MathHelper.ToRadians(90));
                double mathCos = Math.Cos(_Rotation + MathHelper.ToRadians(90));
                mouthPos.X = (float)(mouthPos.X + (radias * mathSin));
                mouthPos.Y = (float)(mouthPos.Y - (radias * mathCos));

                return new Rectangle((int)mouthPos.X, (int)mouthPos.Y, 5, 5);
            }
        }

        public Vector2 TailPosition
        {
            get
            {
                if(_FlipX)
                {
                    return new Vector2(this._BoundingBox.Right, this._BoundingBox.Top + (this._BoundingBox.Height / 2));
                }
                else
                {
                    return new Vector2(this._BoundingBox.Left, this._BoundingBox.Top + (this._BoundingBox.Height / 2));
                }
            }
        }
        public int Hunger
        {
            get
            {
                return hunger;
            }
            set
            {
                if(value < 0)
                {
                    hunger = 0;
                }
                else if(value > 100)
                {
                    hunger = 100;
                }
                else
                {
                    hunger = value;
                }

            }
        }

        bool moving = false;

        enum FishStatus
        {
            kStatusFood,
            kStatusRoam,
            kStatusDead
        }

        FishStatus fishStatus = FishStatus.kStatusRoam;

        int hunger = 0;
        double hungerTimer = 0.5f;

        public Fish()
        {
            ran = new Random();
            moveTimer = ran.Next(0, 5);
            this._Scale.X = 1f;
            this._Scale.Y = 1f;
        }

        public override void LoadContent(string path, ContentManager content)
        {
            mouthTex = content.Load<Texture2D>(@"Art/Corner");
            myTail = new FishTail();
            myTail.LoadContent(@"Art/FishTail", content);
            this.AddChild(myTail);
            base.LoadContent(path, content);
        }

        public void Update(GameTime gt, List<FoodPellet> pList)
        {


            RealFishMove(gt, pList);
            //DebugMove();
            base.Update(gt);

        }

        private void RealFishMove(GameTime gt, List<FoodPellet> pList)
        {
            hungerTimer -= gt.ElapsedGameTime.TotalSeconds;

            if(closestPellet!= null && closestPellet._CurrentState == SpriteState.kStateInActive)
            {
                closestPellet = null;
                fishStatus = FishStatus.kStatusRoam;
            }
            if (hungerTimer < 0)
            {
                Hunger++;
                hungerTimer = 0.5f;
                if (fishStatus == FishStatus.kStatusRoam)
                {
                    if (hunger > 20)
                    {
                        fishStatus = FishStatus.kStatusFood;
                    }

                }
            }
            if (fishStatus == FishStatus.kStatusRoam)
            {

                moveTimer -= gt.ElapsedGameTime.TotalSeconds;

                if (moveTimer <= 0)
                {
                    moving = true;
                    Random ran = new Random();

                    moveTimer = ran.Next(0, 5);


                    targetPos.X = ran.Next(20, 700);
                    targetPos.Y = ran.Next(75, 400);
                }
            }
            else if (fishStatus == FishStatus.kStatusFood)
            {
                if (pList.FindAll(x => x._CurrentState == SpriteState.kStateActive).Count > 0)
                {

                    float closestDistance = 100000;
                    foreach (FoodPellet p in pList.FindAll(x => x._CurrentState == SpriteState.kStateActive))
                    {
                        //find closest pellet.
                        if (p._Position.Y < 70)
                        {
                            continue;
                        }
                        float currentDistance = Vector2.Distance(this.FishMouth.Location.ToVector2(), p._Position);
                        if (currentDistance < closestDistance)
                        {
                            closestPellet = p;
                            closestDistance = currentDistance;
                        }
                    }
                }

                if (closestPellet != null)
                {
                    targetPos = closestPellet._Position;
                    moving = true;



                    if (closestPellet._CurrentState == SpriteState.kStateInActive || closestPellet._Position.Y < 70)
                    {
                        moving = false;
                        if (_FlipX)
                        {
                            MyDir = CurrentDirection.kDirectionLeft;
                        }
                        else
                        {
                            MyDir = CurrentDirection.kDirectionRight;
                        }
                    }
                    if (closestPellet._BoundingBox.Intersects(this.FishMouth))
                    {
                        closestPellet.Deactivate();
                        closestPellet = null;
                        Hunger -= 20;
                        moving = false;
                        fishStatus = FishStatus.kStatusRoam;
                        this._Scale.X += 0.3f;
                        this._Scale.Y += 0.3f;

                        if (this._Scale.X > 2.0f)
                        {
                            this._Scale.X = 2.0f;
                            this._Scale.Y = 2.0f;
                        }
                    }
                }
                else
                {
                    fishStatus = FishStatus.kStatusRoam;
                }
            }
            //else if(fishStatus == FishStatus.kStatusDead)
            //{
            //    this._Rotation = MathHelper.ToRadians(180);
            //    if(this._Position.Y > 70)
            //    {
            //        float speed = 100f;
            //        this._Position.Y -= (float)(speed * gt.ElapsedGameTime.TotalSeconds);
            //    }
            //}

            //if (InputHelper.LeftButtonClicked)
            //{
            //    moving = true;
            //    targetPos = InputHelper.MouseScreenPos;
            //}

            //if(Hunger >= 100)
            //{
            //    this.fishStatus = FishStatus.kStatusDead;
            //}

            if (moving)
            {
                float speed = 100f;


                if (Math.Abs((this.FishMouth.Location.ToVector2().X - targetPos.X)) > 5)
                {
                    if (this.FishMouth.Location.ToVector2().X < targetPos.X)
                    {
                        this._Position.X += speed * (float)gt.ElapsedGameTime.TotalSeconds;


                        if (Math.Abs((this.FishMouth.Location.ToVector2().X - targetPos.X)) > 20)
                        {
                            this._FlipX = false;
                            MyDir = CurrentDirection.kDirectionRight;
                        }

                    }
                    else if (this.FishMouth.Location.ToVector2().X > targetPos.X)
                    {
                        this._Position.X -= speed * (float)gt.ElapsedGameTime.TotalSeconds;
                        if (Math.Abs((this.FishMouth.Location.ToVector2().X - targetPos.X)) > 20)
                        {
                            this._FlipX = true;
                            MyDir = CurrentDirection.kDirectionLeft;
                        }



                    }
                }


                if (Math.Abs((this.FishMouth.Location.ToVector2().Y - targetPos.Y)) > 5)
                {

                    if (this.FishMouth.Location.ToVector2().Y < targetPos.Y)
                    {
                        this._Position.Y += speed * (float)gt.ElapsedGameTime.TotalSeconds;

                        if (Math.Abs((this.FishMouth.Location.ToVector2().Y - targetPos.Y)) > 15)
                        {

                            if (_FlipX)
                            {
                                //this._Rotation = MathHelper.ToRadians(-45);
                                MyDir = CurrentDirection.kDirectionLeft;
                            }
                            else
                            {
                                //this._Rotation = MathHelper.ToRadians(45);
                                MyDir = CurrentDirection.kDirectionRight;

                            }
                        }


                    }
                    else if (this.FishMouth.Location.ToVector2().Y > targetPos.Y)
                    {
                        this._Position.Y -= speed * (float)gt.ElapsedGameTime.TotalSeconds;


                        if (Math.Abs((this.FishMouth.Location.ToVector2().Y - targetPos.Y)) > 15)
                        {
                            if (_FlipX)
                            {
                                //this._Rotation = MathHelper.ToRadians(45);
                                MyDir = CurrentDirection.kDirectionLeft;

                            }
                            else
                            {
                                //this._Rotation = MathHelper.ToRadians(-45);
                                MyDir = CurrentDirection.kDirectionRight;

                            }
                        }



                    }
                }
                else
                {
                    this._Rotation = 0;
                }


                if (Vector2.Distance(this.FishMouth.Location.ToVector2(), targetPos) < 10)
                {
                    moving = false;
                    this._Rotation = 0;

                    if (_FlipX)
                    {
                        MyDir = CurrentDirection.kDirectionLeft;
                    }
                    else
                    {
                        MyDir = CurrentDirection.kDirectionRight;
                    }
                }
            }
            

            if(closestPellet != null)
            {
                pelletLastFrame = true;
            }
            else
            {
                pelletLastFrame = false;
            }
        }

        private void DebugMove()
        {

            this._Rotation = 0;
            if (InputHelper.IsKeyDown(Keys.Left))
            {
                _FlipX = true;
                if (InputHelper.IsKeyDown(Keys.Down))
                {
                    //this._Rotation = MathHelper.ToRadians(-45);
                    MyDir = CurrentDirection.kDirectionLeft;
                }
                else if (InputHelper.IsKeyDown(Keys.Up))
                {
                    //this._Rotation = MathHelper.ToRadians(45);
                    MyDir = CurrentDirection.kDirectionLeft;

                }
                else
                {
                    MyDir = CurrentDirection.kDirectionLeft;
                }
            }
            else if (InputHelper.IsKeyDown(Keys.Right))
            {
                _FlipX = false;
                if (InputHelper.IsKeyDown(Keys.Up))
                {
                    //this._Rotation = MathHelper.ToRadians(-45);
                    MyDir = CurrentDirection.kDirectionRight;
                }
                else if (InputHelper.IsKeyDown(Keys.Down))
                {
                    //this._Rotation = MathHelper.ToRadians(45);
                    MyDir = CurrentDirection.kDirectionRight;

                }
                else
                {
                    MyDir = CurrentDirection.kDirectionRight;
                }
            }

            if(InputHelper.IsKeyDown(Keys.A))
            {
                this._Scale.X -= 0.03f;
                if(this._Scale.X < 0.1f)
                {
                    this._Scale.X = 0.1f;
                }
            }
            else if (InputHelper.IsKeyDown(Keys.D))
            {
                this._Scale.X += 0.03f;
                if (this._Scale.X > 2f)
                {
                    this._Scale.X = 2f;
                }
            }

            if (InputHelper.IsKeyDown(Keys.W))
            {
                this._Scale.Y -= 0.03f;
                if (this._Scale.Y < 0.1f)
                {
                    this._Scale.Y = 0.1f;
                }
            }
            else if (InputHelper.IsKeyDown(Keys.S))
            {
                this._Scale.Y += 0.03f;
                if (this._Scale.Y > 2f)
                {
                    this._Scale.Y = 2f;
                }
            }

            if(InputHelper.RightButtonClicked)
            {
                this._Position = InputHelper.MouseScreenPos;
            }

            if(InputHelper.IsKeyPressed(Keys.Space))
            {
                this._Scale = Vector2.One;
                this._Rotation = 0;
            }

            if (InputHelper.IsKeyPressed(Keys.LeftControl))
            {
                this._Scale.X = 0.5f;
                this._Scale.Y = 0.5f;
            }

            //if(InputHelper.IsKeyDown(Keys.R))
            //{
            //    this._Rotation += 0.05f;
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(mouthTex, FishMouth, Color.White);
        }
    }
}
