#nullable enable
using Godot;
using System;
using System.Collections.Generic;

namespace ManiacMansionReimagined;

public partial class Main : Control
{
	private const string RoomTexturePath = "res://assets/rooms/room_07.png";
	private const string BernardSheetPath = "res://assets/sprites/bernard/bernard_walk_cycle_44_58.png";
	private const string WalkAnimation = "walk";

	private TextureRect? _roomView;
	private AnimatedSprite2D? _bernard;

	private Vector2 _roomTextureSize;
	private Vector2 _roomDisplayOffset;
	private float _roomScale = 1f;
	private Vector2 _targetFeetPosition;
	private Vector2 _currentFeetPosition;
	private float _groundY;
	private bool _isWalking;

	[Export]
	public float WalkSpeed { get; set; } = 180f;

	public override void _Ready()
	{
		_roomView = GetNodeOrNull<TextureRect>("RoomView");
		_bernard = GetNodeOrNull<AnimatedSprite2D>("Bernard");
		if (_roomView == null || _bernard == null)
		{
			GD.PushError("Main scene is missing required nodes: RoomView and/or Bernard.");
			return;
		}

		// Handle clicks directly on the room view so GUI controls don't swallow them before _UnhandledInput.
		_roomView.GuiInput += OnRoomGuiInput;

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

		_groundY = _roomDisplayOffset.Y + (_roomTextureSize.Y * 0.84f * _roomScale);
		_currentFeetPosition = new Vector2(
			_roomDisplayOffset.X + (_roomTextureSize.X * 0.18f * _roomScale),
			_groundY);
		_targetFeetPosition = _currentFeetPosition;
		PlaceBernardAtFeet(_currentFeetPosition);
	}

	public override void _Notification(int what)
	{
		if (what == NotificationResized)
		{
			// Can be triggered before _Ready(); keep it safe.
			if (_roomView == null || _roomTextureSize == Vector2.Zero)
			{
				return;
			}

			UpdateRoomLayout();
			_groundY = _roomDisplayOffset.Y + (_roomTextureSize.Y * 0.84f * _roomScale);
			_currentFeetPosition.Y = _groundY;
			_targetFeetPosition.Y = _groundY;
			PlaceBernardAtFeet(_currentFeetPosition);
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Kept as a fallback for non-GUI contexts; main click-to-move is handled by RoomView.GuiInput.
		if (@event is InputEventMouseButton mouseButton
			&& mouseButton.Pressed
			&& mouseButton.ButtonIndex == MouseButton.Left)
		{
			HandleRoomClick(mouseButton.Position);
		}
	}

	public override void _Process(double delta)
	{
		if (!_isWalking)
		{
			return;
		}

		if (_bernard == null)
		{
			_isWalking = false;
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
		_bernard?.Play(WalkAnimation);
	}

	private void StopWalking()
	{
		_isWalking = false;
		if (_bernard != null)
		{
			_bernard.Stop();
			_bernard.Frame = 0;
		}
	}

	private void PlaceBernardAtFeet(Vector2 feetPosition)
	{
		var frameSize = GetBernardFrameSize();
		if (_bernard == null)
		{
			return;
		}

		_bernard.Position = new Vector2(
			feetPosition.X - (frameSize.X / 2f),
			feetPosition.Y - frameSize.Y);
	}

	private Vector2 GetBernardFrameSize()
	{
		if (_bernard == null)
		{
			return Vector2.Zero;
		}

		var frames = _bernard.SpriteFrames;
		if (frames == null || frames.GetFrameCount(WalkAnimation) == 0)
		{
			return Vector2.Zero;
		}

		var texture = frames.GetFrameTexture(WalkAnimation, 0);
		if (texture == null)
		{
			return Vector2.Zero;
		}
		return texture.GetSize();
	}

	private void UpdateRoomLayout()
	{
		if (_roomView == null || _roomTextureSize == Vector2.Zero)
		{
			return;
		}

		var viewportSize = GetViewportRect().Size;
		_roomScale = Mathf.Min(viewportSize.X / _roomTextureSize.X, viewportSize.Y / _roomTextureSize.Y);
		var displaySize = _roomTextureSize * _roomScale;
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
		var scale = _roomTextureSize == Vector2.Zero
			? 1f
			: Mathf.Min(viewportSize.X / _roomTextureSize.X, viewportSize.Y / _roomTextureSize.Y);
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

		// This sheet is authored as a fixed grid (8x2). We only use the top row for the side-walk cycle.
		const int columns = 8;
		const int rows = 2;
		const int walkRow = 0;

		if (sheet.GetWidth() % columns != 0 || sheet.GetHeight() % rows != 0)
		{
			GD.PushError($"Unexpected Bernard sheet dimensions: {sheet.GetWidth()}x{sheet.GetHeight()} (expected divisible by {columns}x{rows}).");
			return new SpriteFrames();
		}

		var cellWidth = sheet.GetWidth() / columns;
		var cellHeight = sheet.GetHeight() / rows;

		var frameRects = new List<Rect2I>(columns);
		for (var col = 0; col < columns; col++)
		{
			var cell = new Rect2I(col * cellWidth, walkRow * cellHeight, cellWidth, cellHeight);
			if (TryGetTightAlphaRect(sheet, cell, out var tight))
			{
				frameRects.Add(tight);
			}
		}

		if (frameRects.Count == 0)
		{
			GD.PushError($"No Bernard frames were detected in: {sheetPath}");
			return new SpriteFrames();
		}

		var maxWidth = 0;
		var maxHeight = 0;
		foreach (var rect in frameRects)
		{
			maxWidth = Math.Max(maxWidth, rect.Size.X);
			maxHeight = Math.Max(maxHeight, rect.Size.Y);
		}

		var frames = new SpriteFrames();
		frames.AddAnimation(WalkAnimation);

		foreach (var rect in frameRects)
		{
			var padded = Image.CreateEmpty(maxWidth, maxHeight, false, Image.Format.Rgba8);
			padded.Fill(Colors.Transparent);

			var crop = sheet.GetRegion(rect);
			var x = (maxWidth - crop.GetWidth()) / 2;
			var y = maxHeight - crop.GetHeight();
			padded.BlitRect(crop, new Rect2I(0, 0, crop.GetWidth(), crop.GetHeight()), new Vector2I(x, y));

			var texture = ImageTexture.CreateFromImage(padded);
			frames.AddFrame(WalkAnimation, texture);
		}

		frames.SetAnimationSpeed(WalkAnimation, 10f);
		return frames;
	}

	private void OnRoomGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButton
			&& mouseButton.Pressed
			&& mouseButton.ButtonIndex == MouseButton.Left)
		{
			// In GUI callbacks, Position can be local to the Control; use the viewport mouse position.
			HandleRoomClick(GetViewport().GetMousePosition());
		}
	}

	private void HandleRoomClick(Vector2 click)
	{
		if (_roomTextureSize == Vector2.Zero)
		{
			return;
		}

		var roomRect = GetRoomDisplayRect();
		if (!roomRect.HasPoint(click))
		{
			return;
		}

		// Convert click (display space) to room-texture space, clamp, then convert back.
		var scale = roomRect.Size.X / _roomTextureSize.X;
		if (scale <= 0.0001f)
		{
			return;
		}

		var roomX = (click.X - roomRect.Position.X) / scale;
		roomX = Mathf.Clamp(roomX, 24f, _roomTextureSize.X - 24f);

		var targetX = roomRect.Position.X + (roomX * scale);
		_targetFeetPosition = new Vector2(targetX, _groundY);

		if (!_isWalking)
		{
			StartWalking();
		}
	}

	private static bool TryGetTightAlphaRect(Image sheet, Rect2I bounds, out Rect2I rect)
	{
		const float alphaThreshold = 0.01f;
		var minX = int.MaxValue;
		var maxX = int.MinValue;
		var minY = int.MaxValue;
		var maxY = int.MinValue;
		var count = 0;

		var xStart = Math.Max(0, bounds.Position.X);
		var yStart = Math.Max(0, bounds.Position.Y);
		var xEnd = Math.Min(sheet.GetWidth() - 1, bounds.Position.X + bounds.Size.X - 1);
		var yEnd = Math.Min(sheet.GetHeight() - 1, bounds.Position.Y + bounds.Size.Y - 1);

		for (var y = yStart; y <= yEnd; y++)
		{
			for (var x = xStart; x <= xEnd; x++)
			{
				if (sheet.GetPixel(x, y).A <= alphaThreshold)
				{
					continue;
				}

				count++;
				minX = Math.Min(minX, x);
				maxX = Math.Max(maxX, x);
				minY = Math.Min(minY, y);
				maxY = Math.Max(maxY, y);
			}
		}

		if (count < 50 || minX > maxX || minY > maxY)
		{
			rect = default;
			return false;
		}

		rect = new Rect2I(minX, minY, maxX - minX + 1, maxY - minY + 1);
		return true;
	}
}
