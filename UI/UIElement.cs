using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame.UI
{
    public class UIElement
    {
        public string _Name;
        public Vector2 _Position { get; private set; }
        public Vector2 OffsetPos;
        public UIManager _UIManager;
        public string HoverText;
        public bool _Show = true;
        public Vector2 _Size;
        List<UIElement> childList;
        public UIElement parent;

        public UIElement(UIManager uim)
        {
            _UIManager = uim;
            childList = new List<UIElement>();
        }

        public virtual void LoadContent(string texName)
        {

        }

        public virtual void Setup()
        {

        }

        public void SetPosition(Vector2 pos)
        {
            this._Position = pos;
            foreach(UIElement c in childList)
            {
                c.SetPosition(this._Position + c.OffsetPos);
            }
        }

        public Vector2 PositionOffset(Vector2 v)
        {
            return this._Position + v;
        }

        public Vector2 PositionOffset(int i, int j)
        {
            return PositionOffset(new Vector2(i, j));
        }

        /// <summary>
        /// Adds the passed UIElement as a child element to handle any updates/drawing
        /// on that child.
        /// </summary>
        /// <param name="e">The UIElement to add</param>
        public void AddChild(UIElement e)
        {
            childList.Add(e);
            e.parent = this;
        }

        public UIElement FindChild(string n)
        {
            return childList.Find(x => x._Name == n);
        }
        


        public virtual void Draw(SpriteBatch sb)
        {
            if(this._Name == string.Empty)
            {
                Console.WriteLine("Missing name!!!!!!! Bad!!!!");
            }

            foreach(UIElement e in childList)
            {
                e.Draw(sb);
            }
        }
    }
}
