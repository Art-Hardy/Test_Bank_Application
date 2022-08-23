// See https://aka.ms/new-console-template for more information

var array = new int[] { 1, 2, 3, 4, 5, 6, };
PrintArray(array);
Console.WriteLine();

var arrayClone = array.Clone() as int[];
PrintArray(arrayClone);
Console.WriteLine("\n_____________________________");

array[1] = 102;
Console.WriteLine(array[1]);
Console.WriteLine();
Console.WriteLine(arrayClone[1]);

Console.ReadKey();


static void PrintArray(int[] array)
{
    foreach (var item in array)
    {
        Console.Write($"{item} ");
    }
}