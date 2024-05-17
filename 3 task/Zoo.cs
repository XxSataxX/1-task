using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11Slava
{
    public class Zoo
    {
        public string Name { get; set; }
        public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();
    }

    public class Enclosure
    {
        public int Number { get; set; }
        public double Size { get; set; }
        public int MaxAnimals { get; set; }
        public int CurrentAnimals => Animals.Count;
        public List<Animal> Animals { get; set; } = new List<Animal>();
    }

    public abstract class Animal
    {
        public string Name { get; set; }
        public bool IsPredator { get; set; }
        public abstract string GetDescription();
    }

    public class Fish : Animal
    {
        public bool IsDeepWater { get; set; }
        public override string GetDescription()
        {
            return $"Fish: {Name}, Predator: {IsPredator}, DeepWater: {IsDeepWater}";
        }
    }

    public class Bird : Animal
    {
        public double FlightSpeed { get; set; }
        public override string GetDescription()
        {
            return $"Bird: {Name}, Predator: {IsPredator}, FlightSpeed: {FlightSpeed} km/h";
        }
    }

    public class Mammal : Animal
    {
        public string Habitat { get; set; }
        public override string GetDescription()
        {
            return $"Mammal: {Name}, Predator: {IsPredator}, Habitat: {Habitat}";
        }
    }

}
