using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChyaTusha.Extensions
{
    public static class StringFileExtension
    {
        public static string GetDirectory(this string filePath)
        {
            var handler = new StringFileHandler();

            return handler.GetDirectory(filePath);
        }

        public static string TrimPath(this string path, string trimAfter)
        {
            var handler = new StringFileHandler();

            return handler.TrimPath(path, trimAfter);
        }

    }
}
