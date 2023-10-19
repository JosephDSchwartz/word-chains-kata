using static System.Console;

WriteLine("Enter the STARTING word in the chain and then press enter:");
var chainStartWord = ReadLine().ToString();

WriteLine("Enter the ENDING word in the chain and then press enter:");
var chainEndWord = ReadLine().ToString();

var words = File.ReadAllLines("words_alpha.txt");
var endWordCharacters = chainEndWord.ToCharArray();

var chains = GetChains(new[] { chainStartWord }).Where(chain => !chain.Contains(string.Empty)).ToList();

WriteLine("Potential Chains:\n\n");
foreach (var chain in chains)
{
    WriteLine(string.Join(" -> ", chain));
}

WriteLine($"\n\nShortest Chain: {string.Join(" -> ", chains.Aggregate(Enumerable.Empty<string>(), (shortestChain, chain) => shortestChain.Count() > chain.Count() || !shortestChain.Any() ? chain : shortestChain))}");

IEnumerable<IEnumerable<string>> GetChains(IEnumerable<string> previousWords)
{
    if (previousWords.Contains(string.Empty) || previousWords.Last().Equals(chainEndWord)) yield return previousWords;

    var nextWords = GetNextWords(previousWords.Last());

    foreach (var word in nextWords)
    {
        foreach (var chain in GetChains(previousWords.Append(word)))
        {
            yield return chain;
        }
    }
}

IEnumerable<string> GetNextWords(string word)
{
    var wordCharacters = word.ToCharArray();
    for (var i = 0; i < wordCharacters.Length; i++)
    {
        if (wordCharacters[i] == endWordCharacters[i]) continue;

        var potentialWordCharacters = wordCharacters.ToArray();
        potentialWordCharacters[i] = endWordCharacters[i];
        var potentialWord = string.Join("", potentialWordCharacters);

        if (words.Contains(potentialWord)) yield return potentialWord;

        yield return string.Empty;
    }
}
