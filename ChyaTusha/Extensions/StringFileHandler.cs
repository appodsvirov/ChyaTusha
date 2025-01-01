using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChyaTusha.Extensions
{
    public class StringFileHandler
    {
        public string GetDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            return Path.GetDirectoryName(filePath) ?? string.Empty;
        }

        public string TrimPath(string path, string trimAfter)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(trimAfter))
                return path;

            string directory = path.GetDirectory();

            while (!string.IsNullOrEmpty(directory))
            {
                if (directory.EndsWith(trimAfter, StringComparison.OrdinalIgnoreCase))
                    return directory;

                directory = directory.GetDirectory();
            }

            return path;
        }

        public static string GetProjectDirectory()
        {
            // Получаем путь к каталогу, где находится исполняемый файл
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Переходим на один уровень вверх, чтобы попасть в корень проекта
            string projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.FullName;

            return projectDirectory ?? baseDirectory;  // Возвращаем путь, если не удалось подняться
        }
    }
}
