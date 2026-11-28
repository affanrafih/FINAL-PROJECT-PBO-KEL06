namespace Phrasia.Logika;

public class CompleteSentenceQuestion : Question
{
    private const int MinChoices = 2;
    private readonly List<string> _choices;
    public IReadOnlyList<string> Choices => _choices.AsReadOnly();
    public int CorrectIndex { get; }
    public string CorrectAnswer => _choices[CorrectIndex];


    public CompleteSentenceQuestion(
        string prompt,
        string explanation,
        IEnumerable<string> choices,
        int correctIndex,
        int points = 10)
        : base(prompt, explanation, points)
    {
        if (choices is null)
            throw new ArgumentNullException(nameof(choices));

        var list = choices.Select(c => c?.Trim() ?? string.Empty).ToList();

        if (list.Count < MinChoices)
            throw new ArgumentException($"Minimal {MinChoices} pilihan diperlukan.", nameof(choices));

        if (list.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Pilihan tidak boleh kosong.", nameof(choices));

        if (list.Distinct(StringComparer.OrdinalIgnoreCase).Count() != list.Count)
            throw new ArgumentException("Pilihan tidak boleh kembar.", nameof(choices));

        if (correctIndex < 0 || correctIndex >= list.Count)
            throw new ArgumentOutOfRangeException(
                nameof(correctIndex), correctIndex, "Indeks jawaban benar di luar rentang pilihan.");

        _choices = list;
        CorrectIndex = correctIndex;
    }

    public override bool Evaluate(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
            return false;

        return answer.Trim().Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
    }

    public override string BuildFeedback(bool isCorrect)
    {
        string feedback = base.BuildFeedback(isCorrect);

        return isCorrect
            ? feedback
            : $"{feedback} Jawaban yang benar: \"{CorrectAnswer}\".";
    }
}
