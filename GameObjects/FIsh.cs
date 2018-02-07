using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.GameObjects
{
    class Fish : Sprite
    {
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
                    newRect = new Rectangle(this._BoundingBox.Right - 10, (this._BoundingBox.Top + (this.frameWidth / 2)), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left - 5, (this._BoundingBox.Top + (this.frameWidth / 2) - 5), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionUpRight)
                {
                    newRect = new Rectangle(this._BoundingBox.Right - 20, (this._BoundingBox.Top + 10), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionDownRight)
                {
                    newRect = new Rectangle(this._BoundingBox.Right - 25, (this._BoundingBox.Bottom - 20), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionUpLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left, (this._BoundingBox.Top + 10), 5, 5);
                }
                else if (MyDir == CurrentDirection.kDirectionDownLeft)
                {
                    newRect = new Rectangle(this._BoundingBox.Left + 10, (this._BoundingBox.Bottom - 20), 5, 5);
                }
                else
                {
                    newRect = new Rectangle();
                }
                return newRect;
            }
        }

        bool moving = false;

        public Fish()
        {
            moveTimer = new Random().Next(0, 5);
        }

        public override void LoadContent(string path, ContentManager content)
        {
            mouthTex = content.Load<Texture2D>(@"Art/Corner");

            base.LoadContent(path, content);
        }

        public void Update(GameTime gt, List<FoodPellet> pList)
        {




            //moveTimer -= gt.ElapsedGameTime.TotalSeconds;

            //if (moveTimer <= 0)
            //{
            //    moving = true;
            //    Random ran = new Random();

            //    moveTimer = ran.Next(0, 5);


            //    targetPos.X = ran.Next(50, 700);
            //    targetPos.Y = ran.Next(50, 400);



            //}

            //if (InputHelper.LeftButtonClicked)
            //{
            //    moving = true;
            //    targetPos = InputHelper.MouseScreenPos;
            //}
            if(pList.FindAll(x=>x._CurrentState == SpriteState.kStateActive).Count > 0)
            {

                float closestDistance = 100000;
                foreach (FoodPellet p in pList.FindAll(x => x._CurrentState == SpriteState.kStateActive))
                {
                    //find closest pellet.
                    float currentDistance = Vector2.Distance(this.FishMouth.Location.ToVector2(), p._Position);
                    if (currentDistance < closestDistance)
                    {
                        closestPellet = p;
                        closestDistance = currentDistance;
                    }
                }
                if (closestPellet != null)
                {
                    if (closestPellet._BoundingBox.Intersects(this.FishMouth))
                    {
                        closestPellet.Deactivate();
                        moving = false;
                    }
                }
            }

            if (closestPellet != null)
            {
                targetPos = closestPellet._Position;
                moving = true;
                if (closestPellet._BoundingBox.Intersects(this.FishMouth))
                {
                    closestPellet.Deactivate();
                }


                if (closestPellet._CurrentState == SpriteState.kStateInActive)
                {
                    moving = false;
                }

            }


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


                if ( Math.Abs((this.FishMouth.Location.ToVector2().Y - targetPos.Y)) > 5)
                {

                    if (this.FishMouth.Location.ToVector2().Y < targetPos.Y)
                    {
                        this._Position.Y += speed * (float)gt.ElapsedGameTime.TotalSeconds;

                        if(Math.Abs((this.FishMouth.Location.ToVector2().Y - targetPos.Y)) > 15 )
                        {

                            if(_FlipX)
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


                if(Vector2.Distance(this.FishMouth.Location.ToVector2(), targetPos) < 10)
                {
                    moving = false;
                    this._Rotation = 0;

                    if(_FlipX)
                    {
                        MyDir = CurrentDirection.kDirectionLeft;
                    }
                    else
                    {
                        MyDir = CurrentDirection.kDirectionRight;
                    }
                }
            }

            base.Update(gt);

        }
    }
}
