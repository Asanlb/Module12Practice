using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate double MathOperation(double a, double b);

public interface IPropertyChanged
{
    event PropertyEventHandler PropertyChanged;
}

public delegate void PropertyEventHandler(object sender, PropertyEventArgs e);

public class PropertyEventArgs : EventArgs
{
    public string PropertyName { get; }

    public PropertyEventArgs(string propertyName)
    {
        PropertyName = propertyName;
    }
}

public abstract class Car
{
    public string Model { get; set; }
    public int Speed { get; set; }
    public int Position { get; set; }

    public event EventHandler<string> Finish;

    public Car(string model, int speed)
    {
        Model = model;
        Speed = speed;
        Position = 0;
    }

    public virtual void Move()
    {
        Position += Speed;

        if (Position >= 100)
        {
            OnFinish($"{Model} финишировал!");
        }
    }

    protected virtual void OnFinish(string message)
    {
        Finish?.Invoke(this, message);
    }
}

public class SportsCar : Car
{
    public SportsCar(string model) : base(model, new Random().Next(10, 20)) { }
}

public class PassengerCar : Car
{
    public PassengerCar(string model) : base(model, new Random().Next(8, 15)) { }
}

public class Truck : Car
{
    public Truck(string model) : base(model, new Random().Next(5, 12)) { }
}

public class Bus : Car
{
    public Bus(string model) : base(model, new Random().Next(4, 10)) { }
}

public class RaceGame
{
    public delegate void RaceStartDelegate();
    public delegate void RaceFinishDelegate(string message);

    public event RaceStartDelegate RaceStart;
    public event RaceFinishDelegate RaceFinish;

    public void StartRace(params Car[] cars)
    {
        RaceStart?.Invoke();

        while (true)
        {
            foreach (var car in cars)
            {
                car.Move();
            }
        }
    }

    public void SubscribeToEvents(Car car)
    {
        car.Finish += (sender, message) => RaceFinish?.Invoke(message);
    }
}

public class MyClass : IPropertyChanged
{
    private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChange(nameof(Name));
            }
        }
    }

    public event PropertyEventHandler PropertyChanged;

    protected virtual void OnPropertyChange(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyEventArgs(propertyName));
    }
}

public class Program
{
    static void Main()
    {
        
        MyClass myObject = new MyClass();
        myObject.PropertyChanged += (sender, e) =>
        {
            Console.WriteLine($"Свойство {e.PropertyName} было изменено.");
        };
        myObject.Name = "Новое имя"; 

        
        MathOperation addOperation = Calculator.Add;
        MathOperation subtractOperation = Calculator.Subtract;
        MathOperation multiplyOperation = Calculator.Multiply;
        MathOperation divideOperation = Calculator.Divide;

        PerformOperation(5, 3, addOperation);      
        PerformOperation(5, 3, subtractOperation);  
        PerformOperation(5, 3, multiplyOperation);  
        PerformOperation(5, 3, divideOperation);    

        
        MathOperation anonymousMultiply = delegate (double a, double b) { return a * b; };
        PerformOperation(5, 3, anonymousMultiply);  

        
        MathOperation lambdaDivide = (a, b) => b != 0 ? a / b : double.NaN;
        PerformOperation(5, 3, lambdaDivide);        

        MathOperation chainedOperation = addOperation + multiplyOperation;
        PerformOperation(5, 3, chainedOperation);    
    }

    static void PerformOperation(double a, double b, MathOperation operation)
    {
        double result = operation(a, b);
        Console.WriteLine($"Результат операции: {result}");
    }
}

public class Calculator
{
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;
    public static double Divide(double a, double b) => b != 0 ? a / b : double.NaN;
}

