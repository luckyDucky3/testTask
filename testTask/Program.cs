using System;
using System.Collections.Generic;

//Данная задача является "задачей о рюкзаке" следовательно решается с помощью динамического программирования. Оно реализуется в функции CalculateOptimalAttractions
//Так как у меня 48 часов свободного времени за вычетом времени на сон, я подразумеваю, что бодрствовать могу 16 часов. 
namespace testTask
{
    internal class Program
    {
        static int CalculateOptimalAttractions(Dictionary<string, (double time, int importance)> dictionaryOfAttractions, double totalTime, ref List<string> attractions)
        {
            if (totalTime == 0 || dictionaryOfAttractions.Count == 0)
            {
                return 0;
            }
            var firstAttraction = dictionaryOfAttractions.FirstOrDefault();
            string attractionName = firstAttraction.Key;
            double attractionTime = firstAttraction.Value.time;
            int attractionImportance = firstAttraction.Value.importance;

            Dictionary<string, (double time, int importance)> remainingAttractions = new(dictionaryOfAttractions);
            remainingAttractions.Remove(attractionName);

            if (totalTime < attractionTime)
            {
                return CalculateOptimalAttractions(remainingAttractions, totalTime, ref attractions);
            }
            else
            {
                List<string> includeAttractions = new List<string>(attractions);
                double remainingTime = totalTime - attractionTime;
                int impOfAt = attractionImportance;
                int importanceWithAttraction = impOfAt + CalculateOptimalAttractions(remainingAttractions, remainingTime, ref includeAttractions);
                List<string> excludeAttractions = new List<string>(attractions);
                int importanceWithoutAttraction = CalculateOptimalAttractions(remainingAttractions, totalTime, ref excludeAttractions);
                if (importanceWithAttraction > importanceWithoutAttraction)
                {
                    attractions = includeAttractions;
                    attractions.Add(attractionName);
                    //totalTime = remainingTime;
                    //dictionaryOfAttractions.Remove(attractionName);

                    return importanceWithAttraction;
                }
                else
                {
                    attractions = excludeAttractions;
                    return importanceWithoutAttraction;
                }
            }
        }

        static void Main()
        {
            Dictionary<string, (double time, int importance)> dictionaryOfAttractions = new()
            {
                { "Исаакиевский собор", (5, 10) },
                { "Эрмитаж", (8, 11) },
                { "Кунсткамера", (3.5, 4) },
                { "Петропавловская крепость", (10, 7) },
                { "Ленинградский зоопарк", (9, 15) },
                { "Медный всадник", (1, 17) },
                { "Казанский собор", (4, 3) },
                { "Спас на Крови", (2, 9) },
                { "Зимний дворец Петра I", (7, 12) },
                { "Зоологический музей", (5.5, 6) },
                { "Музей обороны и блокады Ленинграда", (2, 19) },
                { "Русский музей", (5, 8) },
                { "Навестить друзей", (12, 20) },
                { "Музей восковых фигур", (2, 13) },
                { "Литературно-мемориальный музей Ф.М. Достоевского", (4, 2) },
                { "Екатерининский дворец", (1.5, 5) },
                { "Петербургский музей кукол", (1, 14) },
                { "Музей микроминиатюры «Русский Левша»", (3, 18) },
                { "Всероссийский музей А.С. Пушкина и филиалы", (6, 1) },
                { "Музей современного искусства Эрарта", (7, 16) }
            };
            dictionaryOfAttractions = dictionaryOfAttractions.OrderByDescending(x => x.Value.importance).ToDictionary();
            const int freeTime = 48;
            const int sleepTime = 16;
            int freeTimeOnFirstDay = 18;
            int freeTimeOnSecondDay = 16;
            List<string> attractions1 = new List<string>();
            int result1 = CalculateOptimalAttractions(dictionaryOfAttractions, freeTimeOnFirstDay, ref attractions1);
            Dictionary<string, (double time, int importance)> tempDictionary = new(dictionaryOfAttractions.Where(x => !attractions1.Contains(x.Key)));
            List<string> attractions2 = new List<string>();
            int result2 = CalculateOptimalAttractions(tempDictionary, freeTimeOnSecondDay, ref attractions2);
            OutputResult(result1, result2, attractions1, attractions2);
        }

        static void OutputResult(int result1, int result2, List<string> attractions1, List<string> attractions2)
        {
            Console.WriteLine("Что просмотреть в первый день:");
            attractions1.ForEach(x =>
            {
                Console.WriteLine(x);
            });
            Console.WriteLine($"Сумма важности всех достопримечательностей за 1 день: {result1}");
            Console.WriteLine("==============================");
            Console.WriteLine("Что просмотреть во второй день:");
            attractions2.ForEach(x =>
            {
                Console.WriteLine(x);
            });
            Console.WriteLine($"Сумма важности всех достопримечательностей за 2 день: {result2}");
            Console.WriteLine();
            Console.WriteLine($"Общая сумма важности: {result1 + result2}");
        }
    }
}