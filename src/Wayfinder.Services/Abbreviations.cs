namespace Wayfinder.Services;

public static class Abbreviations
{
    private static readonly Dictionary<string, string> _abbreviationDictionary = new()
    {
        { "BB-8", "BB8" },
        { "C-3PO", "3PO" },
        { "Chewbacca", "Chewie" },
        { "Commander Luke Skywalker", "CLS" },
        { "Darth Revan", "DR" },
        { "Doctor Aphra", "Aphra" },
        { "Emperor Palpatine", "EP" },
        { "Executor", "Exec" },
        { "General Kenobi", "GK" },
        { "General Skywalker", "GAS" },
        { "Grand Inquisitor", "GI" },
        { "Han Solo", "Han" },
        { "Jabba the Hutt", "Jabba" },
        { "Jedi Knight Cal Kestis", "JKCK" },
        { "Jedi Knight Luke Skywalker", "JKLS" },
        { "Jedi Knight Revan", "JKR" },
        { "Jedi Master Kenobi", "JMK" },
        { "Jedi Master Luke Skywalker", "JMLS" },
        { "Leia Organa", "Leia" },
        { "Leviathan", "Levi" },
        { "Lord Vader", "LV" },
        { "Padmé Amidala", "Padmé" },
        { "Profundity", "Prof" },
        { "R2-D2", "R2" },
        { "Rey", "Rey" },
        { "Rey (Jedi Training)", "JTR" },
        { "Sith Eternal Emperor", "SEE" },
        { "Starkiller", "SK" },
        { "Supreme Leader Kylo Ren", "SLKR" }
    };

    public static string GetAbbreviation(string word) =>
        _abbreviationDictionary.TryGetValue(word, out var abbreviation) ? abbreviation : word;
}
