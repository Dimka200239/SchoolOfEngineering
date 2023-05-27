using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

            string[] text = File.ReadAllLines(inputFilePath); // Читаем весь текст из файла и приводим его к нижнему регистру

            #region Работа с System.Reflection

            // Создаем экземпляр класса Parser из библиотеки TextParserLib
            var parserInstance = new Parser();

            // Получаем тип внутреннего класса Parser
            var type = typeof(Parser);

            // Получаем метод Parse с флагом BindingFlags.NonPublic | BindingFlags.Instance
            var parserMethod = type.GetMethod("Parse", BindingFlags.NonPublic | BindingFlags.Instance);

            var fisrtStopwatch = new Stopwatch();

            fisrtStopwatch.Start();

            // Передаем строку для парсинга в метод Parse через объект parserInstance
            var firstResult = parserMethod.Invoke(parserInstance, new object[] { text }) as Dictionary<string, int>;

            fisrtStopwatch.Stop();

            Console.WriteLine("Прямой метод выполнился за {0} ms", fisrtStopwatch.ElapsedMilliseconds);

            #endregion

            #region Работа с многопоточным методом

            var secondStopwatch = new Stopwatch();

            secondStopwatch.Start();

            // Вызываем многопоточный метод MultithreadedParser для парсинга текста

            var secondResult = GetResultFromServise(text).Result;

            secondStopwatch.Stop();

            Console.WriteLine("Многопоточный метод выполнился за {0} ms", secondStopwatch.ElapsedMilliseconds);

            #endregion

            var resultString = new StringBuilder("");
            foreach (var value in secondResult)
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

        private static async Task<Dictionary<string, int>> GetResultFromServise(string[] text)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44320");

                var response = await client.PostAsJsonAsync("/api/textController/ParseText", text);

                string responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Dictionary<string, int>>(responseContent);
            }
        }
    }
}
