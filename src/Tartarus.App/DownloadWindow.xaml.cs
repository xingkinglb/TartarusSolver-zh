using System.Windows;
using Tartarus.Vision.Ocr;

namespace Tartarus.App;

/// <summary>
/// The game is in a language whose OCR recognizer isn't installed: asks to download it, then shows the download's
/// progress. Declined or failed, the game's text isn't read (icons still tell the cards and picks apart).
/// </summary>
public partial class DownloadWindow : Window
{
    private readonly OcrPack _pack;
    private CancellationTokenSource? _cancel;

    /// <summary>What happened, for the app's log.</summary>
    public string Result { get; private set; } = "";

    /// <summary>The last try failed (no host gave it), rather than the player saying no.</summary>
    public bool Failed { get; private set; }

    public DownloadWindow(OcrPack pack, string languageCode)
    {
        _pack = pack;
        InitializeComponent();
        string language = OcrPacks.LanguageName(languageCode);
        Message.Text = Loc.F(
            "The game is in {0}. Reading its text needs the {0} text recognition model ({1} MB, PaddleOCR). Download it now?\n\nWithout it the game's text won't be recognized; cards and picks are still told apart by their pictures.",
            language, (pack.Size / 1048576.0).ToString("0.#", System.Globalization.CultureInfo.InvariantCulture));
        Result = $"OCR: {language} recognizer not downloaded, the game's text won't be read";
        if (Loc.Chinese) FontFamily = Loc.Font;
        LocTree.Apply(this);
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        DownloadButton.IsEnabled = false;
        CancelButton.Content = Loc.T("Cancel");
        Bar.Visibility = Status.Visibility = Visibility.Visible;
        _cancel = new CancellationTokenSource();
        Failed = false;
        // Which host it comes from: Hugging Face, else its mirror (blocked in China).
        string from = "";
        var progress = new Progress<(long Done, long Total)>(p =>
        {
            Bar.Value = p.Total > 0 ? (double)p.Done / p.Total : 0;
            Status.Text = Loc.F("{0} of {1} MB from {2}",
                (p.Done / 1048576.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture),
                (p.Total / 1048576.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture), from);
        });
        var host = new Progress<string>(h =>
        {
            from = h;
            Status.Text = Loc.F("Connecting to {0}…", h);
        });
        try
        {
            await OcrDownload.RunAsync(_pack, progress, _cancel.Token, host);
            _cancel = null;
            Result = $"OCR: {_pack.Id} recognizer downloaded from {from} to {OcrPacks.DownloadDir}";
            DialogResult = true;
        }
        catch (OperationCanceledException)
        {
            _cancel = null;
            Result = $"OCR: {_pack.Id} recognizer download cancelled, the game's text won't be read";
            DialogResult = false;
        }
        catch (Exception ex)
        {
            Result = $"OCR: {_pack.Id} recognizer download failed: {ex.Message}";
            Failed = true;
            Status.Text = Loc.F("Download failed: {0}", ex.Message);
            DownloadButton.IsEnabled = true;
            DownloadButton.Content = Loc.T("Retry");
            CancelButton.Content = Loc.T("Not now");
            _cancel = null;
        }
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        // Closed mid-download: cancel it, and the download's end closes the window.
        if (_cancel != null)
        {
            _cancel.Cancel();
            e.Cancel = true;
        }
        base.OnClosing(e);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        if (_cancel != null) _cancel.Cancel();
        else DialogResult = false;
    }
}
