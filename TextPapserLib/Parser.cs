using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextParserLib
{
    /// <summary>
    /// Класс из dll для парсинга большого текста.
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Метод для парсинга большого текста.
        /// </summary>
        /// <param name="text">Передаваемый массив строк текста.</param>
        /// <returns>Словарь с ключами и значениями, где ключ - слово, значение - его кол-во в тексте.</returns>
        private Dictionary<string, int> Parse(string[] text)
        {
            var wordsDict = new Dictionary<string, int>();
            
            Regex regex = new Regex(@"\b(?![×÷])[A-Za-zÀ-ÿа-яА-Я']+\b");

            foreach (var line in text)
            {
                foreach (Match word in regex.Matches(line.ToLower()))
                {
                    if (wordsDict.ContainsKey(word.Value) == false)
                    {
                        wordsDict.Add(word.Value, 1);
                    }
                    else
                    {
                        wordsDict[word.Value]++;
                    }
                }
            }

            var ordered = wordsDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return ordered;
        }

        /// <summary>
        /// Метод для парсинга большого текста.
        /// </summary>
        /// <param name="text">Передаваемый массив строк текста.</param>
        /// <returns>Словарь с ключами и значениями, где ключ - слово, значение - его кол-во в тексте.</returns>
        public Dictionary<string, int> MultithreadedParser(string[] text)
        {
            var wordsDict = new Dictionary<string, int>();

            Regex regex = new Regex(@"\b(?![×÷])[A-Za-zÀ-ÿа-яА-Я']+\b");

            object locker = new object();

            Parallel.ForEach(text, line =>
            {
                foreach (Match word in regex.Matches(line.ToLower()))
                {
                    lock (locker)
                    {
                        if (wordsDict.ContainsKey(word.Value) == false)
                        {
                            wordsDict.Add(word.Value, 1);
                        }
                        else
                        {
                            wordsDict[word.Value]++;
                        }
                    }
                }
            });

            var ordered = wordsDict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return ordered;
        }
    }
}
