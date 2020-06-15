namespace dotnetCampus.YamlToCSharp
{
    class CSharpProperty : ICSharpNode
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