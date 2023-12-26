namespace BuildMaterials.Export
{
    public class ExportColumnNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public ExportColumnNameAttribute(string name)
        {
            Name = name;
        }
    }
}
