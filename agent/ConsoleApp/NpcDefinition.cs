namespace ConsoleApp
{
    internal class NpcDefinition
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Personality { get; set; } = string.Empty;
        public string Lore { get; set; } = string.Empty;
        public List<string>? Topics { get; set; }
        public string? PromptHint { get; set; }
    }
}