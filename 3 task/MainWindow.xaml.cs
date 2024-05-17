using _11Slava;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ZooApp
{
    public partial class MainWindow : Window
    {
        private Zoo zoo;
        private Animal editingAnimal;

        public MainWindow()
        {
            InitializeComponent();
            zoo = new Zoo { Name = "Мой Зоопарк" };
            InitializeData();
            UpdateAnimalListView();
        }

        private void InitializeData()
        {
            var enclosure1 = new Enclosure { Number = 1, Size = 100, MaxAnimals = 10 };
            enclosure1.Animals.Add(new Fish { Name = "Акула", IsPredator = true, IsDeepWater = true });
            enclosure1.Animals.Add(new Bird { Name = "Орёл", IsPredator = true, FlightSpeed = 80 });

            var enclosure2 = new Enclosure { Number = 2, Size = 200, MaxAnimals = 20 };
            enclosure2.Animals.Add(new Mammal { Name = "Лев", IsPredator = true, Habitat = "Саванна" });

            zoo.Enclosures.Add(enclosure1);
            zoo.Enclosures.Add(enclosure2);
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            ClearAnimalForm();
            editingAnimal = null;
            AnimalFormPanel.Visibility = Visibility.Visible;
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalListView.SelectedItem is AnimalViewModel selectedAnimal)
            {
                foreach (var enclosure in zoo.Enclosures)
                {
                    var animal = enclosure.Animals.FirstOrDefault(a => a.Name == selectedAnimal.Name);
                    if (animal != null)
                    {
                        enclosure.Animals.Remove(animal);
                        UpdateAnimalListView();
                        break;
                    }
                }
            }
        }

        private void EditAnimal_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalListView.SelectedItem is AnimalViewModel selectedAnimal)
            {
                editingAnimal = zoo.Enclosures.SelectMany(enclosure => enclosure.Animals)
                    .FirstOrDefault(a => a.Name == selectedAnimal.Name);

                if (editingAnimal != null)
                {
                    PopulateAnimalForm(editingAnimal);
                    AnimalFormPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (e.AddedItems[0] as ComboBoxItem).Content.ToString();
            if (selectedOption == "Имя по возрастанию")
            {
                AnimalListView.ItemsSource = zoo.Enclosures.SelectMany(enclosure => enclosure.Animals)
                    .OrderBy(animal => animal.Name)
                    .Select(animal => new AnimalViewModel(animal)).ToList();
            }
            else if (selectedOption == "Имя по убыванию")
            {
                AnimalListView.ItemsSource = zoo.Enclosures.SelectMany(enclosure => enclosure.Animals)
                    .OrderByDescending(animal => animal.Name)
                    .Select(animal => new AnimalViewModel(animal)).ToList();
            }
        }

        private void UpdateAnimalListView()
        {
            AnimalListView.ItemsSource = zoo.Enclosures.SelectMany(enclosure => enclosure.Animals)
                .Select(animal => new AnimalViewModel(animal)).ToList();
        }

        private void AnimalTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FishPanel.Visibility = Visibility.Collapsed;
            BirdPanel.Visibility = Visibility.Collapsed;
            MammalPanel.Visibility = Visibility.Collapsed;

            switch (AnimalTypeComboBox.SelectedIndex)
            {
                case 0:
                    FishPanel.Visibility = Visibility.Visible;
                    break;
                case 1:
                    BirdPanel.Visibility = Visibility.Visible;
                    break;
                case 2:
                    MammalPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (editingAnimal == null)
            {
                AddNewAnimal();
            }
            else
            {
                UpdateExistingAnimal();
            }

            AnimalFormPanel.Visibility = Visibility.Collapsed;
            UpdateAnimalListView();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AnimalFormPanel.Visibility = Visibility.Collapsed;
        }

        private void ClearAnimalForm()
        {
            AnimalTypeComboBox.SelectedIndex = -1;
            SetPlaceholder(NameTextBox, "Имя");
            IsPredatorCheckBox.IsChecked = false;
            IsDeepWaterCheckBox.IsChecked = false;
            SetPlaceholder(FlightSpeedTextBox, "Скорость полета");
            SetPlaceholder(HabitatTextBox, "Среда обитания");
            FishPanel.Visibility = Visibility.Collapsed;
            BirdPanel.Visibility = Visibility.Collapsed;
            MammalPanel.Visibility = Visibility.Collapsed;
        }

        private void PopulateAnimalForm(Animal animal)
        {
            NameTextBox.Text = animal.Name;
            NameTextBox.Foreground = Brushes.Black;
            IsPredatorCheckBox.IsChecked = animal.IsPredator;

            if (animal is Fish fish)
            {
                AnimalTypeComboBox.SelectedIndex = 0;
                IsDeepWaterCheckBox.IsChecked = fish.IsDeepWater;
            }
            else if (animal is Bird bird)
            {
                AnimalTypeComboBox.SelectedIndex = 1;
                FlightSpeedTextBox.Text = bird.FlightSpeed.ToString();
                FlightSpeedTextBox.Foreground = Brushes.Black;
            }
            else if (animal is Mammal mammal)
            {
                AnimalTypeComboBox.SelectedIndex = 2;
                HabitatTextBox.Text = mammal.Habitat;
                HabitatTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddNewAnimal()
        {
            Animal newAnimal = null;
            switch (AnimalTypeComboBox.SelectedIndex)
            {
                case 0:
                    newAnimal = new Fish
                    {
                        Name = NameTextBox.Text,
                        IsPredator = IsPredatorCheckBox.IsChecked == true,
                        IsDeepWater = IsDeepWaterCheckBox.IsChecked == true
                    };
                    break;
                case 1:
                    newAnimal = new Bird
                    {
                        Name = NameTextBox.Text,
                        IsPredator = IsPredatorCheckBox.IsChecked == true,
                        FlightSpeed = double.Parse(FlightSpeedTextBox.Text)
                    };
                    break;
                case 2:
                    newAnimal = new Mammal
                    {
                        Name = NameTextBox.Text,
                        IsPredator = IsPredatorCheckBox.IsChecked == true,
                        Habitat = HabitatTextBox.Text
                    };
                    break;
            }

            if (newAnimal != null)
            {
                var enclosure = zoo.Enclosures.First(); // Добавляем в первый вольер
                enclosure.Animals.Add(newAnimal);
            }
        }

        private void UpdateExistingAnimal()
        {
            editingAnimal.Name = NameTextBox.Text;
            editingAnimal.IsPredator = IsPredatorCheckBox.IsChecked == true;

            if (editingAnimal is Fish fish)
            {
                fish.IsDeepWater = IsDeepWaterCheckBox.IsChecked == true;
            }
            else if (editingAnimal is Bird bird)
            {
                bird.FlightSpeed = double.Parse(FlightSpeedTextBox.Text);
            }
            else if (editingAnimal is Mammal mammal)
            {
                mammal.Habitat = HabitatTextBox.Text;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetPlaceholder(textBox, textBox.Tag.ToString());
            }
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.Foreground = Brushes.Gray;
            textBox.Tag = placeholder;
        }
    }

    public class AnimalViewModel
    {
        public string Name { get; set; }
        public string AnimalType { get; set; }
        public string Description { get; set; }

        public AnimalViewModel(Animal animal)
        {
            Name = animal.Name;
            AnimalType = animal.GetType().Name;
            Description = animal.GetDescription();
        }
    }
}
