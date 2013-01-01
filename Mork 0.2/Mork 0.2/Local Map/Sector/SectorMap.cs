using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mork.Graphics.MapEngine;

namespace Mork.Local_Map.Sector
{
    public class SectorMap
    {
        const int sectn = 1;
        MapSector[] data = new MapSector[sectn * sectn * sectn];
        GraphicsDevice gd;
        BasicEffect be;

        public SectorMap(GraphicsDevice _gd)
        {
            for (int i = 0; i <= sectn - 1; i++)
                for (int j = 0; j <= sectn - 1; j++)
                    for (int k = 0; k <= sectn - 1; k++)
                        data[i * sectn * sectn + j * sectn + k] = new MapSector(i, j, k);

            gd = _gd;
            be = new BasicEffect(gd);
        }

        public void RebuildAllMapGeo()
        {
            foreach(var a in data)
            {
                a.RebuildSectorGeo(gd);
            }
        }

        public void DrawAllMap(GameTime gt, Camera cam)
        {
            be.World = Matrix.CreateScale(10);
            be.View = cam.View;
            be.Projection = cam.Projection;
            be.AmbientLightColor = new Vector3(1,1,1);
            //be.LightingEnabled = true;

            gd.RasterizerState = RasterizerState.CullCounterClockwise;
            gd.DepthStencilState = DepthStencilState.Default;
            gd.BlendState = BlendState.Opaque;

            foreach (var pass in be.CurrentTechnique.Passes)
            {
                pass.Apply();
                foreach (var a in data)
                {
                    if (!a.builded) a.RebuildSectorGeo(gd);
                    if (a.VertexBuffer.VertexCount != 0)
                    {
                       gd.SetVertexBuffer(a.VertexBuffer);
                       gd.DrawPrimitives(PrimitiveType.TriangleList, 0, a.VertexBuffer.VertexCount / 3);
                    }
                    {

                    }
                }

                //Matrix aa = Matrix.CreateScale(10);
                //Main._teapot.Draw(aa, cam.View, cam.Projection);
            }
        }
    }
}
