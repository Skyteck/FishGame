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

        enum CurrentDirection
        {
            kDirectionRight,
            kDirectionLeft,
            kDirectionUpRight,
            kDirectionDownRight,
            kDirectionUpLeft,
            kDirectionDownLeft
        }

        CurrentDirection MyDir = CurrentDirection.kDirectionRight;
        Rectangle FishMouth
        {
            get
            {
                Rectangle newRect;
                if(MyDir == CurrentDirection.kDirectionRight)
                {
                    newRect = new Rectangle(this._BoundingBox.Right - (int)(15 * this._Scale.X), (this._BoundingBox.Top + (this.frameWidth / 2)) + (int)(5 * this._Scale.Y), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left + 10, (this._BoundingBox.Top + (this.frameWidth / 2) + 5), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionUpRight)
                {
                    newRect = new Rectangle(this._BoundingBox.Right - 15, (this._BoundingBox.Top + 20), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionDownRight)
                {
                    newRect = new Rectangle(this._BoundingBox.Right - 25, (this._BoundingBox.Bottom - 20), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionUpLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left + 10, (this._BoundingBox.Top + 20), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionDownLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left + 20, (this._BoundingBox.Bottom - 20), 5, 5);
                }
                else
                {
                    newRect = new Rectangle();
                }
                return newRect;
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
            this._Scale.X = 0.5f;
            this._Scale.Y = 0.5f;
        }

        public override void LoadContent(string path, ContentManager content)
        {
            mouthTex = content.Load<Texture2D>(@"Art/Corner");

            base.LoadContent(path, content);
        }

        public void Update(GameTime gt, List<FoodPellet> pList)
        {


            //RealFishMove(gt, pList);
            DebugMove();
            base.Update(gt);

        }

        private void RealFishMove(GameTime gt, List<FoodPellet> pList)
        {
            hungerTimer -= gt.ElapsedGameTime.TotalSeconds;
            if (hungerTimer < 0)
            {
                Hunger++;
                hungerTimer = 0.5f;
                Console.WriteLine(Hunger);
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
                                this._Rotation = MathHelper.ToRadians(-45);
                                MyDir = CurrentDirection.kDirectionDownLeft;
                            }
                            else
                            {
                                this._Rotation = MathHelper.ToRadians(45);
                                MyDir = CurrentDirection.kDirectionDownRight;

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
                                this._Rotation = MathHelper.ToRadians(45);
                                MyDir = CurrentDirection.kDirectionUpLeft;

                            }
                            else
                            {
                                this._Rotation = MathHelper.ToRadians(-45);
                                MyDir = CurrentDirection.kDirectionUpRight;

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
        }

        private void DebugMove()
        {

            this._Rotation = 0;
            if (InputHelper.IsKeyDown(Keys.Left))
            {
                _FlipX = true;
                if (InputHelper.IsKeyDown(Keys.Down))
                {
                    this._Rotation = MathHelper.ToRadians(-45);
                    MyDir = CurrentDirection.kDirectionDownLeft;
                }
                else if (InputHelper.IsKeyDown(Keys.Up))
                {
                    this._Rotation = MathHelper.ToRadians(45);
                    MyDir = CurrentDirection.kDirectionUpLeft;

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
                    this._Rotation = MathHelper.ToRadians(-45);
                    MyDir = CurrentDirection.kDirectionUpRight;
                }
                else if (InputHelper.IsKeyDown(Keys.Down))
                {
                    this._Rotation = MathHelper.ToRadians(45);
                    MyDir = CurrentDirection.kDirectionDownRight;

                }
                else
                {
                    MyDir = CurrentDirection.kDirectionRight;
                }
            }

            if(InputHelper.IsKeyPressed(Keys.Space))
            {
                if(this._Scale.X == 1.0f)
                {
                    this._Scale.X = 0.5f;
                    this._Scale.Y = 0.5f;
                    Console.WriteLine(this._BoundingBox.Right);
                }
                else
                {

                    this._Scale.X = 1f;
                    this._Scale.Y = 1f;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(mouthTex, FishMouth, Color.White);
        }
    }
}
