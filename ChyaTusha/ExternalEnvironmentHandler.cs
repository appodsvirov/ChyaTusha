using ChyaTusha.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChyaTusha
{
    public static class ExternalEnvironmentHandler
    {
        public static string GetProjectName()
        {
            // Получаем информацию о текущей сборке
            var assembly = Assembly.GetExecutingAssembly();

            // Получаем атрибут с информацией о сборке
            var assemblyName = assembly.GetName();

            // Имя сборки будет соответствовать имени проекта
            return assemblyName.Name;
        }

        public static string GetProjectDirectory()
        {
            // Получаем путь к каталогу, где находится исполняемый файл
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Переходим на один уровень вверх, чтобы попасть в корень проекта
            string projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.FullName;

            return projectDirectory ?? baseDirectory;  // Возвращаем путь, если не удалось подняться
        }

        public static string GetOutputFolderPath()
        {
            return GetProjectDirectory().TrimPath(GetProjectName());
        }
        public static string GetResourceFolderPath()
        {
            return Path.Combine(GetOutputFolderPath(), "Resources");
        }
    }
}
