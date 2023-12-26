namespace BuildMaterials.Export
{
    public class BooleanValueAttribute : Attribute
    {
        public string True { get; private set; }
        public string False { get; private set; }

        public BooleanValueAttribute(string tr, string fl)
        {
            True = tr;
            False = fl;
        }

        public string GetValue(bool value) => value ? True : False;
    }
}
