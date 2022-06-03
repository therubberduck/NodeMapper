using System.Linq;
using Microsoft.Msagl.Drawing;

namespace NodeMapper.DataRepository.Schema
{
    public static class DbColorConverter
    {
        public static Color ColorFromDbString(string colorString)
        {
            var colorComponents = colorString.Split(',').Select(byte.Parse).ToArray();
            return new Color(colorComponents[0], colorComponents[1], colorComponents[2]);
        }

        public static string DbStringFromColor(Color color)
        {
            return color.R + "," + color.G + "," + color.B;
        }
    }
}