# Bernard Blender Model

This project uses Blender as the authoring tool for the first 3D Bernard model.
The current asset is a static stylized model, not a rig. It is meant to be a
recognizable Bernard-like character mesh that can later receive a proper
armature.

## Goal

Create a poseable Bernard inspired by the character direction:

- thin awkward teen silhouette;
- white shirt and black tie;
- dark pants;
- magenta sneakers;
- dark quiff hair;
- sunglasses.

The source sprite folders are treated as inspiration only. This workflow does
not overwrite any original or in-progress 2D resource.

## Files

Input:

- `tools/blender/create_bernard_model.py`

Generated output:

- `assets/models/bernard/bernard_model.blend`
- `assets/models/bernard/bernard_model.glb`

## Requirements

Install Blender and make sure the `blender` command is available in your
terminal.

On Windows, if Blender is not on `PATH`, use the full executable path, for
example:

```powershell
& "C:\Program Files\Blender Foundation\Blender 4.3\blender.exe" --background --python tools\blender\create_bernard_model.py
```

If `blender` is on `PATH`, run:

```powershell
blender --background --python tools\blender\create_bernard_model.py
```

## Model Notes

Open `assets/models/bernard/bernard_model.blend` in Blender.

This file contains only the model pieces. There are no pose pivots, no armature,
and no helper labels. The character is separated into readable mesh parts so a
future rig can be created by hand.

## Godot Import

After generation, import `assets/models/bernard/bernard_model.glb` into the
Godot project if you want to test it in-engine. For production, keep the
`.blend` file as the editable source and treat `.glb` as generated output.

## Validation

Check that:

- the `.blend` opens without missing assets;
- the visible character has Bernard's key readable traits;
- the `.glb` imports into Godot without scale surprises;
- original resource files remain untouched.

## Known Limits

- This is not a final sculpt.
- Limbs are separate mesh pieces, ready to be replaced or joined during rigging.
- No armature or facial animation is included.
