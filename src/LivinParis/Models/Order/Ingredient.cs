namespace LivinParisRoussilleTeynier.Models.Order
{
    public class Ingredient(
        string name,
        bool isVegetarian,
        bool isVegan,
        bool isGlutenFree,
        bool isHalal,
        bool isKosher
    //HACK: class/struct/enum allergènes ??
    )
    {
        //QUESTION: set ??
        public string Name { get; set; } = name;
        public bool IsVegetarian { get; set; } = isVegetarian;
        public bool IsVegan { get; set; } = isVegan;
        public bool IsGlutenFree { get; set; } = isGlutenFree;
        public bool IsHalal { get; set; } = isHalal;
        public bool IsKosher { get; set; } = isKosher;
    }
}
