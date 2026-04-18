using Godot;
using System;
using System.Collections.Generic;

namespace ManiacMansionReimagined;

public partial class Main : Control
{
    private const string RoomTexturePath = "res://assets/rooms/room_07.png";
    private const string BernardSheetPath = "res://assets/sprites/bernard/bernard_walk_cycle_44_58.png";
    private const string WalkAnimation = "walk";

    private TextureRect _roomView = null!;
    private AnimatedSprite2D _bernard = null!;

    private Vector2 _roomTextureSize;
    private Vector2 _roomDisplayOffset;
    private Vector2 _targetFeetPosition;
    private Vector2 _currentFeetPosition;
    private float _groundY;
    private bool _isWalking;

    [Export]
    public float WalkSpeed { get; set; } = 180f;

    public override void _Ready()
    {
        _roomView = GetNode<TextureRect>("RoomView");
        _bernard = GetNode<AnimatedSprite2D>("Bernard");

        var roomTexture = GD.Load<Texture2D>(RoomTexturePath);
        if (roomTexture == null)
        {
            GD.PushError($"Missing room texture: {RoomTexturePath}");
            return;
        }

        _roomTextureSize = roomTexture.GetSize();
        _roomView.Texture = roomTexture;

        _bernard.SpriteFrames = BuildBernardFrames(BernardSheetPath);
        _bernard.Animation = WalkAnimation;
        _bernard.Play();

        UpdateRoomLayout();

        _groundY = _roomDisplayOffset.Y + _roomTextureSize.Y * 0.84f;
        _currentFeetPosition = new Vector2(_roomDisplayOffset.X + _roomTextureSize.X * 0.18f, _groundY);
        _targetFeetPosition = _currentFeetPosition;
        PlaceBernardAtFeet(_currentFeetPosition);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationResized)
        {
            UpdateRoomLayout();
            _groundY = _roomDisplayOffset.Y + _roomTextureSize.Y * 0.84f;
            _currentFeetPosition.Y = _groundY;
            _targetFeetPosition.Y = _groundY;
            PlaceBernardAtFeet(_currentFeetPosition);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton
            && mouseButton.Pressed
            && mouseButton.ButtonIndex == MouseButton.Left)
        {
            var click = mouseButton.Position;
            var roomRect = GetRoomDisplayRect();
            if (!roomRect.HasPoint(click))
            {
                return;
            }

            var roomX = Mathf.Clamp(click.X - roomRect.Position.X, 24f, _roomTextureSize.X - 24f);
            var targetX = roomRect.Position.X + roomX;
            _targetFeetPosition = new Vector2(targetX, _groundY);

            if (!_isWalking)
            {
                StartWalking();
            }
        }
    }

    public override void _Process(double delta)
    {
        if (!_isWalking)
        {
            return;
        }

        var step = WalkSpeed * (float)delta;
        var next = _currentFeetPosition.MoveToward(_targetFeetPosition, step);

        if (next.X < _currentFeetPosition.X)
        {
            _bernard.FlipH = true;
        }
        else if (next.X > _currentFeetPosition.X)
        {
            _bernard.FlipH = false;
        }

        _currentFeetPosition = next;
        PlaceBernardAtFeet(_currentFeetPosition);

        if (_currentFeetPosition.DistanceTo(_targetFeetPosition) < 1.0f)
        {
            _currentFeetPosition = _targetFeetPosition;
            PlaceBernardAtFeet(_currentFeetPosition);
            StopWalking();
        }
    }

    private void StartWalking()
    {
        _isWalking = true;
        _bernard.Play(WalkAnimation);
    }

    private void StopWalking()
    {
        _isWalking = false;
        _bernard.Stop();
        _bernard.Frame = 0;
    }

    private void PlaceBernardAtFeet(Vector2 feetPosition)
    {
        var frameSize = GetBernardFrameSize();
        _bernard.Position = new Vector2(
            feetPosition.X - (frameSize.X / 2f),
            feetPosition.Y - frameSize.Y);
    }

    private Vector2 GetBernardFrameSize()
    {
        var frames = _bernard.SpriteFrames;
        if (frames == null || frames.GetFrameCount(WalkAnimation) == 0)
        {
            return Vector2.Zero;
        }

        var texture = frames.GetFrameTexture(WalkAnimation, 0);
        return texture.GetSize();
    }

    private void UpdateRoomLayout()
    {
        var viewportSize = GetViewportRect().Size;
        var scale = Mathf.Min(viewportSize.X / _roomTextureSize.X, viewportSize.Y / _roomTextureSize.Y);
        var displaySize = _roomTextureSize * scale;
        _roomDisplayOffset = (viewportSize - displaySize) * 0.5f;

        _roomView.AnchorLeft = 0f;
        _roomView.AnchorTop = 0f;
        _roomView.AnchorRight = 1f;
        _roomView.AnchorBottom = 1f;
        _roomView.OffsetLeft = 0f;
        _roomView.OffsetTop = 0f;
        _roomView.OffsetRight = 0f;
        _roomView.OffsetBottom = 0f;
        _roomView.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
    }

    private Rect2 GetRoomDisplayRect()
    {
        var viewportSize = GetViewportRect().Size;
        var scale = Mathf.Min(viewportSize.X / _roomTextureSize.X, viewportSize.Y / _roomTextureSize.Y);
        var displaySize = _roomTextureSize * scale;
        var position = (viewportSize - displaySize) * 0.5f;
        return new Rect2(position, displaySize);
    }

    private static SpriteFrames BuildBernardFrames(string sheetPath)
    {
        var sheet = Image.LoadFromFile(sheetPath);
        if (sheet == null)
        {
            GD.PushError($"Missing Bernard sheet: {sheetPath}");
            return new SpriteFrames();
        }

        var components = ExtractComponents(sheet);
        if (components.Count == 0)
        {
            GD.PushError($"No Bernard frames were detected in: {sheetPath}");
            return new SpriteFrames();
        }

        var maxArea = 0;
        foreach (var component in components)
        {
            maxArea = Math.Max(maxArea, component.Area);
        }

        var minArea = Math.Max(1000, (int)(maxArea * 0.75f));
        var frameComponents = components.FindAll(component => component.Area >= minArea);
        if (frameComponents.Count > 0)
        {
            components = frameComponents;
        }

        var maxWidth = 0;
        var maxHeight = 0;
        foreach (var component in components)
        {
            maxWidth = Math.Max(maxWidth, component.Rect.Size.X);
            maxHeight = Math.Max(maxHeight, component.Rect.Size.Y);
        }

        var frames = new SpriteFrames();
        frames.AddAnimation(WalkAnimation);

        foreach (var component in components)
        {
            var padded = Image.CreateEmpty(maxWidth, maxHeight, false, Image.Format.Rgba8);
            padded.Fill(Colors.Transparent);

            var crop = sheet.GetRegion(component.Rect);
            var x = (maxWidth - crop.GetWidth()) / 2;
            var y = maxHeight - crop.GetHeight();
            padded.BlitRect(crop, new Rect2I(0, 0, crop.GetWidth(), crop.GetHeight()), new Vector2I(x, y));

            var texture = ImageTexture.CreateFromImage(padded);
            frames.AddFrame(WalkAnimation, texture);
        }

        frames.SetAnimationSpeed(WalkAnimation, 10f);
        return frames;
    }

    private static List<ComponentBounds> ExtractComponents(Image image)
    {
        var width = image.GetWidth();
        var height = image.GetHeight();
        var visited = new bool[width * height];
        var components = new List<ComponentBounds>();
        var queue = new Queue<Vector2I>();

        bool IsForeground(int x, int y)
        {
            var color = image.GetPixel(x, y);
            return color.A > 0 && (color.R < 0.96f || color.G < 0.96f || color.B < 0.96f);
        }

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var index = y * width + x;
                if (visited[index] || !IsForeground(x, y))
                {
                    continue;
                }

                visited[index] = true;
                queue.Enqueue(new Vector2I(x, y));

                var minX = x;
                var maxX = x;
                var minY = y;
                var maxY = y;
                var area = 0;

                while (queue.Count > 0)
                {
                    var point = queue.Dequeue();
                    area++;
                    minX = Math.Min(minX, point.X);
                    maxX = Math.Max(maxX, point.X);
                    minY = Math.Min(minY, point.Y);
                    maxY = Math.Max(maxY, point.Y);

                    var neighbors = new[]
                    {
                        new Vector2I(point.X - 1, point.Y),
                        new Vector2I(point.X + 1, point.Y),
                        new Vector2I(point.X, point.Y - 1),
                        new Vector2I(point.X, point.Y + 1)
                    };

                    foreach (var neighbor in neighbors)
                    {
                        if (neighbor.X < 0 || neighbor.X >= width || neighbor.Y < 0 || neighbor.Y >= height)
                        {
                            continue;
                        }

                        var neighborIndex = neighbor.Y * width + neighbor.X;
                        if (visited[neighborIndex] || !IsForeground(neighbor.X, neighbor.Y))
                        {
                            continue;
                        }

                        visited[neighborIndex] = true;
                        queue.Enqueue(neighbor);
                    }
                }

                components.Add(new ComponentBounds(
                    new Rect2I(minX, minY, maxX - minX + 1, maxY - minY + 1),
                    area));
            }
        }

        components.Sort((left, right) =>
        {
            var yCompare = left.Rect.Position.Y.CompareTo(right.Rect.Position.Y);
            return yCompare != 0 ? yCompare : left.Rect.Position.X.CompareTo(right.Rect.Position.X);
        });

        return components;
    }

    private readonly record struct ComponentBounds(Rect2I Rect, int Area);
}
