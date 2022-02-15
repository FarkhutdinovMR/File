using System;
using System.Collections.Generic;

namespace Store
{
    class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            //Вывод всех товаров на складе с их остатком
            warehouse.ShowInfo();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            //Вывод всех товаров в корзине
            cart.ShowInfo();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    class Good
    {
        public Good(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    class Cell
    {
        public Cell(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Good = good;
            Count = count;
        }

        public Good Good { get; private set; }
        public int Count { get; private set; }

        public void Merge(Cell newCell)
        {
            if (newCell == null)
                throw new ArgumentNullException(nameof(newCell));

            if (newCell.Good != Good)
                throw new InvalidOperationException();

            Count += newCell.Count;
        }

        public void Extract(Cell cell)
        {
            if (cell == null)
                throw new ArgumentNullException(nameof(cell));

            if (cell.Good != Good)
                throw new InvalidOperationException();

            Count -= cell.Count;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Good.Name} {Count}");
        }
    }

    class Warehouse
    {
        private readonly List<Cell> _goods = new List<Cell>();

        public Warehouse()
        {
            _goods = new List<Cell>();
        }

        public void Delive(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            var newCell = new Cell(good, count);

            int goodIndex = _goods.FindIndex(cell => cell.Good == good);

            if (goodIndex == -1)
                _goods.Add(newCell);
            else
                _goods[goodIndex].Merge(newCell);
        }

        public void ShowInfo()
        {
            Console.WriteLine("Остатки на складе:");

            foreach (Cell cell in _goods)
                cell.ShowInfo();
        }

        public bool TryTake(Cell selectedCell)
        {
            if (selectedCell == null)
                throw new ArgumentNullException(nameof(selectedCell));

            int goodIndex = _goods.FindIndex(cell => cell.Good == selectedCell.Good);

            if (goodIndex == -1)
            {
                Console.WriteLine("Товар на складе не найден.");
                return false;
            }

            if (selectedCell.Count > _goods[goodIndex].Count)
            {
                Console.WriteLine($"Не достаточное количество на складе: {selectedCell.Good.Name}");
                return false;
            }

            _goods[goodIndex].Extract(selectedCell);

            return true;
        }
    }

    class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    class Cart
    {
        private readonly List<Cell> _goods;
        private readonly Warehouse _warehouse;

        public Cart(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse));

            _warehouse = warehouse;
            _goods = new List<Cell>();
            Paylink = "Товары куплены.";
        }

        public string Paylink { get; private set; }

        public void Add(Good good, int count)
        {
            if (good == null)
                throw new ArgumentNullException(nameof(good));

            var newCell = new Cell(good, count);

            if (_warehouse.TryTake(newCell) == false)
                return;

            int goodIndex = _goods.FindIndex(cell => cell.Good == good);

            if (goodIndex == -1)
                _goods.Add(newCell);
            else
                _goods[goodIndex].Merge(newCell);
        }

        public Cart Order()
        {
            _goods.Clear();

            return this;
        }

        public void ShowInfo()
        {
            Console.WriteLine("Товары в корзине:");

            foreach (Cell cell in _goods)
                cell.ShowInfo();
        }
    }
}