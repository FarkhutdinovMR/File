public static void CreateNewObject()
{
    //Создание объекта на карте
}

public static void DefineChance()
{
    _chance = Random.Range(0, 100);
}

public static int CalculateSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}