using Glitch9.Database;
using UnityEngine;
using UnityEngine.UI;
using Glitch9.DB;

namespace Glitch9.Apis.Google.Sheets
{
    [Type(typeof(Sprite), new string[] { "Sprite", "sprite" })]
    public class SpriteType : IType
    {
        public object DefaultValue => Sprites.Get(0);
        /// <summary>
        /// value는 스프레드 시트에 적혀있는 값
        /// </summary> 
        public object Read(string value)
        {
            if (int.TryParse(value, out int id))
            {
                return Sprites.Get(id);
            }

            return DefaultValue;
        }

        /// <summary>
        /// value write to google sheet
        /// </summary> 
        public string Write(object value)
        {
            if (value is Sprite sprite)
            {
                return Sprites.GetKey(sprite).ToString();
            }
            return string.Empty;
        }
    }
}
