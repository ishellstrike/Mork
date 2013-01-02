using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork
{
    public partial class Main
    {
        VertexBuffer vertexbuffer;
        IndexBuffer indexbuffer;
        BasicEffect Effect;

        VertexPositionColor[] _vertices;
        UInt16[] _indices;
        private void LRInit()
        {
            vertexbuffer = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, 10000, BufferUsage.WriteOnly);
            indexbuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 40000, BufferUsage.WriteOnly);
            Effect = new BasicEffect(GraphicsDevice);
        }

        private void LocalMapRenderer(GameTime gt, SpriteBatch sb, GraphicsDevice device)
        {
            device.SetVertexBuffer(vertexbuffer);
            device.Indices = indexbuffer;

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
               // device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Length, 0, IndexCount / 3);
            }


            var temp = device.RasterizerState;
            

            //sb.DrawString(Font1, pickRay.ToString(), new Vector2(50, 300), Color.Red);

            

            smap.DrawAllMap(gt, Camera);

            device.RasterizerState = temp;
        }

        private void LRUpdate(GameTime gt)
        {
           // _vertices = new;

            //vertexbuffer.SetData<VertexPositionColor>(_vertices);
            //indexbuffer.SetData<UInt16>(_indices);
        }
    }
}
