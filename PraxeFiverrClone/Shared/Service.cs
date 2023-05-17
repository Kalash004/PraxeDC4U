namespace PraxeFiverrClone.Shared
{
    public class Service
    {
        public int id { get; set;}
        public int price { get; set; }
        public string name { get; set;}
        public string description { get; set;}

        public Service(string n,string d, int i, int p)
        {
            name = n;
            description = d;
            id = i;
            price = p;
        }
    }
}
