using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TextParserLib;

namespace FileWorkerEXE
{
    /// <summary>
    /// Класс для работы с файлами.
    /// </summary>
    public class FileWorker
    {
        static void Main(string[] args)
        {
            try
            {
                Work();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Метод для работы с файлами.
        /// </summary>
        private static void Work()
        {
            Console.Write("Введите путь к файлу с текстом: ");
            var inputFilePath = Console.ReadLine();

            string text = File.ReadAllText(inputFilePath).ToLower(); // Читаем весь текст из файла и приводим его к нижнему регистру

            #region Работа с System.Reflection

            // Создаем экземпляр класса Parser из библиотеки TextParserLib
            var parserInstance = new Parser();

            // Получаем тип внутреннего класса Parser
            var type = typeof(Parser);

            // Получаем метод Parse с флагом BindingFlags.NonPublic | BindingFlags.Instance
            var parserMethod = type.GetMethod("Parse", BindingFlags.NonPublic | BindingFlags.Instance);

            // Передаем строку для парсинга в метод Parse через объект parserInstance
            var result = parserMethod.Invoke(parserInstance, new object[] { text }) as Dictionary<string, int>;

            #endregion

            var resultString = new StringBuilder("");
            foreach (var value in result)
            {
                resultString.Append($"{value.Key}\t{value.Value}\n");
            }

            Console.Write("Введите путь к файлу, в который будет выведен ответ: ");
            var outputFilePath = Console.ReadLine();

            using (FileStream fs = File.Create(outputFilePath)) // Записываем результат в файл
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(resultString.ToString());
                fs.Write(title, 0, title.Length);
            }

            Console.WriteLine("Работа программы завершена...");
        }
    }
}
