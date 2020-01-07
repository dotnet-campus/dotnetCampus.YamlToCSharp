namespace dotnetCampus.YamlToCsharp
{
    class CsharpProperty : ICsharpNode
    {
        public string Key { get; set; }

        public string Value { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Key + ":" + Value;
        }
    }
}