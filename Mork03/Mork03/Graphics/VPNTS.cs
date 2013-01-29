using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mork.Graphics {
    [Serializable]
    public struct VertexPositionNormalTextureShade : IVertexType {
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof (float)*6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1));

        public Vector3 Normal;
        public Vector3 Position;
        public float Shade;
        public Vector2 TextureCoordinate;

        public VertexPositionNormalTextureShade(Vector3 position, Vector3 normal, Vector2 textureCoordinate, float shade) {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
            Shade = shade;
        }

        public static int SizeInBytes {
            get { return sizeof (float)*9; }
        }

        #region IVertexType Members

        VertexDeclaration IVertexType.VertexDeclaration {
            get { return VertexDeclaration; }
        }

        #endregion
    }
}