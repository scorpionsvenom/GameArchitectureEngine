using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameArchitectureEngine
{
    /// <summary>
    /// sets up camera to follow player
    /// zooming and rotating are able to be changed here too.
    /// </summary>
    public class Camera
    {
        private Viewport viewport;

        private Vector2 centre;
        
        public float X
        {
            get { return centre.X; }
            set { centre.X = value; }
        }

        public float Y
        {
            get { return centre.Y; }
            set { centre.Y = value; }
        }
        
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
            set { transform = value; }
        }

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(Vector2 position, float rotation, float zoom, int mapWidth, int mapHeight)
        {
            centre = new Vector2(position.X, position.Y);

            Vector3 lockedCentre = LockValuesToLevel(mapWidth, mapHeight);

            transform = Matrix.CreateTranslation(new Vector3(-lockedCentre.X, -lockedCentre.Y, 0)) * 
                                                Matrix.CreateRotationZ(rotation) * 
                                                Matrix.CreateScale(zoom) * 
                                                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }

        private Vector3 LockValuesToLevel(int mapWidth, int mapHeight)
        {
            Vector3 lockedCentre = Vector3.Zero;
            float halfVPWidth = viewport.Width / 2;
            float halfVPHeight = viewport.Height / 2;

            if (centre.X - halfVPWidth > 0 && centre.X + halfVPWidth < mapWidth)
                lockedCentre.X = centre.X;
            else if (centre.X - halfVPWidth < 0)
                lockedCentre.X = halfVPWidth;
            else if (centre.X + halfVPWidth > mapWidth)
                lockedCentre.X = mapWidth - halfVPWidth;

            if (centre.Y- halfVPHeight > 0 && centre.Y + halfVPHeight < mapHeight)
                lockedCentre.Y = centre.Y;
            else if (centre.Y - halfVPHeight < 0)
                lockedCentre.Y = halfVPHeight;
            else if (centre.Y + halfVPHeight > mapHeight)
                lockedCentre.Y = mapHeight - halfVPHeight;

            return lockedCentre;
        }        
    }
}
