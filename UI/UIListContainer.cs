using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace FishGame.UI
{
    class UIListContainer : UIElement
    {
        public List<UIElement> itemsList;

        public int buffer = 37;
        public UIListContainer(UIManager uim): base(uim)
        {
            itemsList = new List<UIElement>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            /* thinking through scroll bar
             * scroll down one click to increment items drawn by columns?
             * 
             * 
             * */
            base.Draw(spriteBatch);

            int columns = (int)(_Size.X / buffer);
            int rows = (int)(_Size.Y / buffer);
            int toDraw = columns * rows;
            int itemsDrawn = 0;
            int currentRow = 0;
            int currentColumn = 0;

            //if (toDraw < _InventoryManager.itemSlots.Count)
            //{
            //    if (this._BoundingBox.Contains(InputHelper.MouseScreenPos) && InputHelper.MouseScrolled)
            //    {
            //        if (InputHelper.MouseScrolledUp)
            //        {
            //            scrollPos--;
            //            if (scrollPos < 0)
            //            {
            //                scrollPos = 0;
            //            }
            //        }
            //        else if (InputHelper.MouseScrolledDown)
            //        {
            //            scrollPos++;
            //            if (scrollPos > rows)
            //            {
            //                scrollPos = rows;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    scrollPos = 0;
            //}

            //itemsDrawn = columns * scrollPos;
            //if (itemsDrawn >= _InventoryManager.itemSlots.Count)
            //{
            //    itemsDrawn = _InventoryManager.itemSlots.Count - columns;
            //}

            //Vector2 StartPos = HelperFunctions.PointToVector(_TopEdge.Location);
            Vector2 StartPos = this._Position;
            StartPos.X += 16;
            StartPos.Y += 16;
            

            while (itemsDrawn < itemsList.Count)
            {
                //where to draw?
                Vector2 pos = new Vector2(StartPos.X + (currentColumn * buffer), StartPos.Y + (currentRow * buffer));
                itemsList[itemsDrawn].SetPosition(pos);
                itemsList[itemsDrawn].Draw(spriteBatch);

                currentColumn++;
                if (currentColumn >= columns)
                {
                    currentColumn = 0;
                    currentRow++;
                    if (currentRow >= rows)
                    {
                        break;
                    }
                }
                itemsDrawn++;
            }
        }
    }
}
