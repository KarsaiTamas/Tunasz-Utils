 
using UnityEngine;

namespace TunaszUtils
{
    public static class ConvertToSpriteExtensiton
    {
        public static Sprite ConvertToSprite(this Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
        }
        public static Sprite ConvertToSprite(this Texture2D texture,SpriteMeshType meshType)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero,100,0, meshType);

        }
    }
}
