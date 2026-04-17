using Godot;
using System.Collections.Generic;

namespace ManiacMansionReimagined;

public partial class Main : Control
{
    private Button _actionButton = null!;
    private Label _message = null!;
    private Label _roomLabel = null!;

    private readonly Dictionary<string, string> _translations = new();

    [Export]
    public string CurrentLanguage { get; set; } = "fr";

    public override void _Ready()
    {
        _actionButton = GetNode<Button>("ActionButton");
        _message = GetNode<Label>("Message");
        _roomLabel = GetNode<Label>("RoomPanel/RoomLabel");

        LoadTranslations(CurrentLanguage);

        _roomLabel.Text = TrKey("prototype.room_name");
        _actionButton.Text = TrKey("ui.look_around");
        _message.Text = TrKey("ui.ready");

        _actionButton.Pressed += OnActionButtonPressed;
    }

    private void OnActionButtonPressed()
    {
        _message.Text = TrKey("prototype.look_response");
    }

    private void LoadTranslations(string language)
    {
        _translations.Clear();

        using var file = FileAccess.Open($"res://localization/{language}.csv", FileAccess.ModeFlags.Read);
        if (file is null)
        {
            GD.PushWarning($"Missing localization file for language: {language}");
            return;
        }

        var header = true;
        while (!file.EofReached())
        {
            var row = file.GetCsvLine();
            if (header)
            {
                header = false;
                continue;
            }

            if (row.Length >= 2 && !string.IsNullOrEmpty(row[0]))
            {
                _translations[row[0]] = row[1];
            }
        }
    }

    private string TrKey(string key)
    {
        return _translations.TryGetValue(key, out var value) ? value : key;
    }
}

