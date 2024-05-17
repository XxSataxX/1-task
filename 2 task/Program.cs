using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooSearchApp
{
    // Класс Зоопарк
    public class Zoo
    {
        public string Name { get; set; }
        public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();
    }

    // Класс Вольер
    public class Enclosure
    {
        public int Number { get; set; }
        public double Size { get; set; } // Размер в квадратных метрах
        public int MaxAnimals => (int)(Size / 10); // Максимальное количество животных, исходя из размера
        public int CurrentAnimals { get; set; }
        public List<Animal> Animals { get; set; } = new List<Animal>();
        public string Type { get; set; } // Тип вольера: "Морской", "Хищники", "Травоядные", "Смешанный"

        public string GetDescription()
        {
            return $"Номер вольера: {Number}, Размер: {Size} м^2, Тип: {Type}, Максимальное количество животных: {MaxAnimals}, Текущее количество животных: {CurrentAnimals}";
        }
    }

    // Базовый класс Животное
    public class Animal
    {
        public string Name { get; set; }
        public bool IsPredator { get; set; }
        public virtual string GetDescription() => $"Имя: {Name}, Хищник: {IsPredator}";
    }

    // Класс Рыба
    public class Fish : Animal
    {
        public bool IsDeepWater { get; set; }
        public override string GetDescription() => $"{base.GetDescription()}, Глубоководная: {IsDeepWater}";
    }

    // Класс Птица
    public class Bird : Animal
    {
        public double FlightSpeed { get; set; }
        public override string GetDescription() => $"{base.GetDescription()}, Скорость полета: {FlightSpeed} км/ч";
    }

    // Класс Зверь
    public class Beast : Animal
    {
        public string Habitat { get; set; }
        public override string GetDescription() => $"{base.GetDescription()}, Среда обитания: {Habitat}";
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Создаем пример данных
                var zoo = new Zoo
                {
                    Name = "Mistical Zoo",
                    Enclosures = new List<Enclosure>
                    {
                        new Enclosure
                        {
                            Number = 1,
                            Size = 100,
                            CurrentAnimals = 2,
                            Type = "Морской",
                            Animals = new List<Animal>
                            {
                                new Fish { Name = "Золотая рыбка", IsPredator = false, IsDeepWater = false },
                                new Fish { Name = "Клоунская рыба", IsPredator = false, IsDeepWater = false }
                            }
                        },
                        new Enclosure
                        {
                            Number = 2,
                            Size = 200,
                            CurrentAnimals = 3,
                            Type = "Хищники",
                            Animals = new List<Animal>
                            {
                                new Bird { Name = "Орел", IsPredator = true, FlightSpeed = 50 },
                                new Beast { Name = "Лев", IsPredator = true, Habitat = "Саванна" },
                                new Beast { Name = "Тигр", IsPredator = true, Habitat = "Джунгли" }
                            }
                        },
                        new Enclosure
                        {
                            Number = 3,
                            Size = 150,
                            CurrentAnimals = 3,
                            Type = "Травоядные",
                            Animals = new List<Animal>
                            {
                                new Bird { Name = "Попугай", IsPredator = false, FlightSpeed = 20 },
                                new Beast { Name = "Кролик", IsPredator = false, Habitat = "Лес" },
                                new Beast { Name = "Слон", IsPredator = false, Habitat = "Саванна" }
                            }
                        },
                        new Enclosure
                        {
                            Number = 4,
                            Size = 180,
                            CurrentAnimals = 2,
                            Type = "Травоядные",
                            Animals = new List<Animal>
                            {
                                new Bird { Name = "Фламинго", IsPredator = false, FlightSpeed = 30 },
                                new Beast { Name = "Зебра", IsPredator = false, Habitat = "Саванна" }
                            }
                        }
                    }
                };

                // Интерактивный интерфейс командной строки
                Console.WriteLine("Добро пожаловать в зоопарк!");
                Console.WriteLine($"Вы находитесь в {zoo.Name}.");

                while (true)
                {
                    Console.WriteLine("Хотите ли осуществить поиск? (y/n)");
                    if (Console.ReadLine()?.ToLower() != "y")
                        break;

                    Console.WriteLine("Уточните класс экземпляра (Зоопарк, Вольер, Животное, Рыба, Птица, Зверь):");
                    var className = Console.ReadLine();

                    Console.WriteLine("Введите атрибут для поиска (например, Имя, Номер, Размер, Тип, Среда обитания, Скорость полета):");
                    var attribute = Console.ReadLine();

                    Console.WriteLine("Введите значение для поиска:");
                    var value = Console.ReadLine();

                    try
                    {
                        switch (className.ToLower())
                        {
                            case "зоопарк":
                                SearchZoo(zoo, attribute, value);
                                break;
                            case "вольер":
                                SearchEnclosures(zoo.Enclosures, attribute, value);
                                break;
                            case "животное":
                            case "рыба":
                            case "птица":
                            case "зверь":
                                SearchAnimals(zoo.Enclosures.SelectMany(e => e.Animals), attribute, value);
                                break;
                            default:
                                Console.WriteLine("Неверное название класса.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Произошла ошибка: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла критическая ошибка: {ex.Message}");
            }
        }

        static void SearchZoo(Zoo zoo, string attribute, string value)
        {
            try
            {
                switch (attribute.ToLower())
                {
                    case "имя":
                        if (zoo.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                            Console.WriteLine($"Зоопарк найден: {zoo.Name}");
                        else
                            Console.WriteLine("Зоопарк не найден.");
                        break;
                    default:
                        Console.WriteLine("Неверный атрибут для поиска в зоопарке.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске зоопарка: {ex.Message}");
            }
        }

        static void SearchEnclosures(IEnumerable<Enclosure> enclosures, string attribute, string value)
        {
            try
            {
                var foundEnclosures = enclosures.Where(e =>
                {
                    switch (attribute.ToLower())
                    {
                        case "номер":
                            return int.TryParse(value, out int number) && e.Number == number;
                        case "размер":
                            return double.TryParse(value, out double size) && e.Size == size;
                        case "тип":
                            return e.Type.Equals(value, StringComparison.OrdinalIgnoreCase);
                        default:
                            Console.WriteLine("Неверный атрибут для поиска в вольерах.");
                            return false;
                    }
                }).ToList();

                if (foundEnclosures.Any())
                {
                    Console.WriteLine($"Найдено {foundEnclosures.Count} вольеров:");
                    foreach (var enclosure in foundEnclosures)
                    {
                        Console.WriteLine(enclosure.GetDescription());
                    }
                }
                else
                {
                    Console.WriteLine("Вольеры не найдены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске вольеров: {ex.Message}");
            }
        }

        static void SearchAnimals(IEnumerable<Animal> animals, string attribute, string value)
        {
            try
            {
                var foundAnimals = animals.Where(a =>
                {
                    switch (attribute.ToLower())
                    {
                        case "имя":
                            return a.Name.Equals(value, StringComparison.OrdinalIgnoreCase);
                        case "хищник":
                            return bool.TryParse(value, out bool isPredator) && a.IsPredator == isPredator;
                        case "глубоководная":
                            return a is Fish fish && bool.TryParse(value, out bool isDeepWater) && fish.IsDeepWater == isDeepWater;
                        case "скорость полета":
                            return a is Bird bird && double.TryParse(value, out double flightSpeed) && bird.FlightSpeed == flightSpeed;
                        case "среда обитания":
                            return a is Beast beast && beast.Habitat.Equals(value, StringComparison.OrdinalIgnoreCase);
                        default:
                            Console.WriteLine("Неверный атрибут для поиска в животных.");
                            return false;
                    }
                }).ToList();

                if (foundAnimals.Any())
                {
                    Console.WriteLine($"Найдено {foundAnimals.Count} животных:");
                    foreach (var foundAnimal in foundAnimals)
                    {
                        Console.WriteLine(foundAnimal.GetDescription());
                    }
                }
                else
                {
                    Console.WriteLine("Животные не найдены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске животных: {ex.Message}");
            }
        }
    }
}
