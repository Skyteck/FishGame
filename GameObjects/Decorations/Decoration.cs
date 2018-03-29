using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FishGame.GameObjects
{
    class Decoration : Sprite
    {

        //Placeable variables
        public bool Clicked = false;
        public Vector2 ClickOffset = Vector2.Zero;
        public bool HasGravity = true;
        public int FallSpeed = 125;

        protected override void UpdateActive(GameTime gameTime)
        {
            //Placeable logic
            if (Clicked)
            {
                if (InputHelper.LeftButtonReleased)
                {
                    Clicked = false;
                }
                else
                {
                    this._Position = (InputHelper.MouseScreenPos + ClickOffset);
                }
            }
            else
            {
                if (this._Position.Y < 450)
                {
                    if(HasGravity)
                    {
                        this._Position.Y += (float)(FallSpeed * gameTime.ElapsedGameTime.TotalSeconds);
                    }
                }

                if (InputHelper.LeftButtonClicked && this._BoundingBox.Contains(InputHelper.MouseScreenPos))
                {
                    Clicked = true;
                    ClickOffset.X = this._Position.X - InputHelper.MouseScreenPos.X;
                    ClickOffset.Y = this._Position.Y - InputHelper.MouseScreenPos.Y;
                }
            }

            base.UpdateActive(gameTime);
        }
    }
}
