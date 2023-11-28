namespace Lesson07.Models;

public class Snack
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
    
    public Snack(string theName, double thePrice, int theStock)
    {
        Name = theName;
        Price = thePrice;
        Stock = theStock;
    }
}


