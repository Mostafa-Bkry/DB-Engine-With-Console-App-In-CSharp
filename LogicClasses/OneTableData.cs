namespace LogicClasses
{
    public class OneTableData
    {
        public Type Table { get; private set; }
        public List<string[]> Data { get; private set; }

        public OneTableData(Type tab, List<string[]> data)
        {
            Table = tab;
            Data = data;
        }
    }
}
