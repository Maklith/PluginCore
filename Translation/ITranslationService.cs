using System.Threading;
using System.Threading.Tasks;

namespace PluginCore;

public interface ITranslationService
{
    Task<string> TranslateAsync(
        string text,
        TranslationSourceLanguage sourceLanguage = TranslationSourceLanguage.Auto,
        TranslationTargetLanguage targetLanguage = TranslationTargetLanguage.SimplifiedChinese,
        CancellationToken cancellationToken = default);
}
