using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Mork.Local_Map.Sector
{
    public class MapSector
    {
        const byte dimS = 16;

        MNode[] data = new MNode[dimS * dimS * dimS];

        public int sx, sy, sz;

        public bool builded = false;

        internal VertexBuffer VertexBuffer;

        public MapSector(int x, int y, int z)
        {
            sx = x;
            sy = y;
            sz = z;
            for (int i = 0; i <= dimS*dimS*dimS - 1; i++)
                    {
                        data[i] = new MNode();
                    }
        }

        public void RebuildSectorGeo(GraphicsDevice gd)
        {
            VertexPositionColor[] VertexArray = new VertexPositionColor[dimS*dimS*dimS*6];

            int index = 0;

            for (int i = 0; i <= dimS - 1;i++ )
                for (int j = 0; j<= dimS - 1; j++)
                    for (int k = 0; k <= dimS - 1; k++)
                    {
                        VertexArray[index] = new VertexPositionColor(new Vector3(i + 1, j + 1, k + 1), Color.Yellow);
                        VertexArray[index + 1] = new VertexPositionColor(new Vector3(i + 1, j + 1, k), Color.Yellow);
                        VertexArray[index + 2] = new VertexPositionColor(new Vector3(i + 1, j, k + 1), Color.Yellow);
                        VertexArray[index + 3] = new VertexPositionColor(new Vector3(i + 1, j, k + 1), Color.Yellow);
                        VertexArray[index + 4] = new VertexPositionColor(new Vector3(i + 1, j + 1, k), Color.Yellow);
                        VertexArray[index + 5] = new VertexPositionColor(new Vector3(i + 1, j, k), Color.Yellow);

                        index += 6;
                    }



            VertexBuffer = new VertexBuffer(gd, typeof(VertexPositionColor), VertexArray.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(VertexArray);
            builded = true;
        }
    }
}
