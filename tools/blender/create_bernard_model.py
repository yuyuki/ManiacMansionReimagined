"""Create a static stylized Bernard model in Blender.

Run from the repository root:
blender --background --python tools/blender/create_bernard_model.py

Outputs:
- assets/models/bernard/bernard_model.blend
- assets/models/bernard/bernard_model.glb
"""

from __future__ import annotations

from math import radians
from pathlib import Path

import bpy
from mathutils import Vector


ROOT = Path(__file__).resolve().parents[2]
OUT_DIR = ROOT / "assets" / "models" / "bernard"
BLEND_PATH = OUT_DIR / "bernard_model.blend"
GLB_PATH = OUT_DIR / "bernard_model.glb"


def clear_scene() -> None:
    bpy.ops.object.select_all(action="SELECT")
    bpy.ops.object.delete()


def material(name: str, color: tuple[float, float, float, float], roughness: float = 0.55) -> bpy.types.Material:
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    bsdf.inputs["Base Color"].default_value = color
    bsdf.inputs["Roughness"].default_value = roughness
    return mat


def finish(obj: bpy.types.Object, mat: bpy.types.Material, smooth: bool = True) -> bpy.types.Object:
    obj.data.materials.append(mat)
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    if smooth:
        bpy.ops.object.shade_smooth()
    obj.select_set(False)
    return obj


def add_bevel(obj: bpy.types.Object, width: float, segments: int = 3) -> bpy.types.Object:
    bevel = obj.modifiers.new("soft rounded edges", "BEVEL")
    bevel.width = width
    bevel.segments = segments
    obj.modifiers.new("weighted normals", "WEIGHTED_NORMAL")
    return obj


def cube(
    name: str,
    loc: tuple[float, float, float],
    scale: tuple[float, float, float],
    mat: bpy.types.Material,
    bevel: float = 0.02,
    rot: tuple[float, float, float] = (0, 0, 0),
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    obj.dimensions = scale
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    if bevel:
        add_bevel(obj, bevel)
    return finish(obj, mat)


def sphere(
    name: str,
    loc: tuple[float, float, float],
    scale: tuple[float, float, float],
    mat: bpy.types.Material,
    segments: int = 40,
    rot: tuple[float, float, float] = (0, 0, 0),
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_uv_sphere_add(segments=segments, ring_count=20, radius=1, location=loc, rotation=rot)
    obj = bpy.context.object
    obj.name = name
    obj.scale = scale
    return finish(obj, mat)


def cyl_between(
    name: str,
    start: tuple[float, float, float],
    end: tuple[float, float, float],
    radius: float,
    mat: bpy.types.Material,
    vertices: int = 24,
) -> bpy.types.Object:
    a = Vector(start)
    b = Vector(end)
    d = b - a
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=d.length, location=a + d * 0.5)
    obj = bpy.context.object
    obj.name = name
    obj.rotation_euler = d.to_track_quat("Z", "Y").to_euler()
    return finish(obj, mat)


def cone(
    name: str,
    loc: tuple[float, float, float],
    radius1: float,
    radius2: float,
    depth: float,
    mat: bpy.types.Material,
    rot: tuple[float, float, float] = (0, 0, 0),
    vertices: int = 24,
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_cone_add(
        vertices=vertices,
        radius1=radius1,
        radius2=radius2,
        depth=depth,
        location=loc,
        rotation=rot,
    )
    obj = bpy.context.object
    obj.name = name
    return finish(obj, mat)


def torus(
    name: str,
    loc: tuple[float, float, float],
    mat: bpy.types.Material,
    major: float,
    minor: float,
    rot: tuple[float, float, float] = (0, 0, 0),
) -> bpy.types.Object:
    bpy.ops.mesh.primitive_torus_add(
        major_radius=major,
        minor_radius=minor,
        major_segments=48,
        minor_segments=10,
        location=loc,
        rotation=rot,
    )
    obj = bpy.context.object
    obj.name = name
    return finish(obj, mat)


def build_static_bernard() -> None:
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    clear_scene()

    skin = material("skin peach", (0.93, 0.55, 0.33, 1))
    skin_dark = material("skin warm shadow", (0.66, 0.29, 0.16, 1))
    white = material("white short sleeve shirt", (0.94, 0.97, 1.0, 1))
    dark = material("black tie and belt", (0.005, 0.004, 0.006, 1))
    pants = material("dark blue high waist pants", (0.025, 0.035, 0.13, 1))
    shoe = material("red magenta sneakers", (0.70, 0.025, 0.25, 1))
    sole = material("white rubber soles", (0.94, 0.93, 0.88, 1))
    hair = material("almost black blue hair", (0.004, 0.007, 0.04, 1))
    lens = material("opaque black glasses lenses", (0.0, 0.004, 0.014, 1), roughness=0.2)
    metal = material("dull metal buckle", (0.76, 0.69, 0.47, 1), roughness=0.35)
    pocket = material("pocket protector blue pens", (0.08, 0.22, 0.55, 1))

    # Overall silhouette: large head, thin neck, narrow shoulders, long skinny limbs.
    sphere("long oval head", (0, -0.035, 2.05), (0.24, 0.18, 0.32), skin)
    sphere("pointed chin", (0, -0.07, 1.77), (0.13, 0.11, 0.08), skin, segments=24)
    sphere("left large ear", (-0.235, -0.015, 2.05), (0.045, 0.03, 0.075), skin_dark, segments=20)
    sphere("right large ear", (0.235, -0.015, 2.05), (0.045, 0.03, 0.075), skin_dark, segments=20)
    cone("long nerd nose", (0, -0.205, 2.05), 0.047, 0.012, 0.16, skin_dark, rot=(radians(90), 0, 0), vertices=20)
    cube("small flat mouth", (0, -0.215, 1.89), (0.12, 0.012, 0.018), skin_dark, bevel=0.004)

    # Bernard's most readable feature: big dark glasses.
    torus("left thick square glasses rim", (-0.095, -0.205, 2.11), dark, major=0.074, minor=0.012, rot=(radians(90), 0, 0))
    torus("right thick square glasses rim", (0.095, -0.205, 2.11), dark, major=0.074, minor=0.012, rot=(radians(90), 0, 0))
    sphere("left black lens", (-0.095, -0.21, 2.11), (0.065, 0.012, 0.052), lens, segments=24)
    sphere("right black lens", (0.095, -0.21, 2.11), (0.065, 0.012, 0.052), lens, segments=24)
    cube("glasses bridge", (0, -0.218, 2.11), (0.055, 0.018, 0.018), dark, bevel=0.005)
    cyl_between("left glasses arm", (-0.158, -0.205, 2.12), (-0.255, -0.03, 2.11), 0.009, dark, vertices=8)
    cyl_between("right glasses arm", (0.158, -0.205, 2.12), (0.255, -0.03, 2.11), 0.009, dark, vertices=8)

    # Angular dark hair cap and swept-up quiff.
    sphere("helmet dark hair cap", (0, 0.005, 2.29), (0.255, 0.19, 0.13), hair)
    cube("flat sideburn left", (-0.225, -0.025, 2.08), (0.055, 0.055, 0.20), hair, bevel=0.025)
    cube("flat sideburn right", (0.225, -0.025, 2.08), (0.055, 0.055, 0.20), hair, bevel=0.025)
    for i, x in enumerate([-0.16, -0.08, 0.0, 0.08, 0.16], start=1):
        tuft = cone(
            f"swept high hair spike {i}",
            (x, -0.095 - abs(x) * 0.18, 2.43 - abs(x) * 0.2),
            0.07,
            0.012,
            0.30 - abs(x) * 0.25,
            hair,
            rot=(radians(68), radians(x * 45), radians(-x * 45)),
            vertices=16,
        )
        tuft.scale.x = 0.75

    cyl_between("thin neck", (0, -0.005, 1.72), (0, -0.005, 1.58), 0.055, skin, vertices=20)
    cube("white shirt torso tapered", (0, 0, 1.31), (0.48, 0.24, 0.58), white, bevel=0.075)
    cube("left collar triangle", (-0.07, -0.15, 1.60), (0.15, 0.035, 0.10), white, bevel=0.012, rot=(0, 0, radians(-18)))
    cube("right collar triangle", (0.07, -0.15, 1.60), (0.15, 0.035, 0.10), white, bevel=0.012, rot=(0, 0, radians(18)))
    cone("black tie knot", (0, -0.165, 1.56), 0.065, 0.035, 0.08, dark, vertices=4)
    cube("long skinny black tie", (0, -0.16, 1.30), (0.075, 0.028, 0.43), dark, bevel=0.01)
    cube("shirt pocket", (0.14, -0.155, 1.39), (0.12, 0.018, 0.11), white, bevel=0.008)
    cube("pocket pen blue", (0.11, -0.171, 1.44), (0.018, 0.012, 0.13), pocket, bevel=0.004)
    cube("pocket pen black", (0.15, -0.171, 1.44), (0.018, 0.012, 0.13), dark, bevel=0.004)

    cube("high dark pants block", (0, 0.0, 0.97), (0.48, 0.25, 0.30), pants, bevel=0.05)
    cube("black belt", (0, -0.02, 1.13), (0.52, 0.27, 0.055), dark, bevel=0.01)
    cube("gold square belt buckle", (0, -0.165, 1.13), (0.09, 0.025, 0.065), metal, bevel=0.008)

    # Slightly stiff, inward posture.
    cyl_between("left upper arm white sleeve", (-0.30, -0.005, 1.50), (-0.47, -0.02, 1.25), 0.07, white)
    cyl_between("left skinny forearm", (-0.47, -0.02, 1.25), (-0.55, -0.06, 0.99), 0.046, skin)
    sphere("left clenched hand", (-0.56, -0.07, 0.94), (0.065, 0.045, 0.07), skin_dark, segments=20)
    cyl_between("right upper arm white sleeve", (0.30, -0.005, 1.50), (0.47, -0.02, 1.25), 0.07, white)
    cyl_between("right skinny forearm", (0.47, -0.02, 1.25), (0.55, -0.06, 0.99), 0.046, skin)
    sphere("right clenched hand", (0.56, -0.07, 0.94), (0.065, 0.045, 0.07), skin_dark, segments=20)

    cyl_between("left narrow thigh", (-0.14, 0, 0.86), (-0.18, -0.005, 0.49), 0.085, pants)
    cyl_between("left narrow shin", (-0.18, -0.005, 0.49), (-0.20, -0.035, 0.17), 0.07, pants)
    cyl_between("right narrow thigh", (0.14, 0, 0.86), (0.18, -0.005, 0.49), 0.085, pants)
    cyl_between("right narrow shin", (0.18, -0.005, 0.49), (0.20, -0.035, 0.17), 0.07, pants)
    cube("left bright sneaker", (-0.22, -0.16, 0.07), (0.20, 0.35, 0.11), shoe, bevel=0.045)
    cube("left sneaker toe cap", (-0.22, -0.30, 0.075), (0.17, 0.12, 0.10), shoe, bevel=0.06)
    cube("left white sole", (-0.22, -0.16, 0.015), (0.22, 0.37, 0.035), sole, bevel=0.02)
    cube("right bright sneaker", (0.22, -0.16, 0.07), (0.20, 0.35, 0.11), shoe, bevel=0.045)
    cube("right sneaker toe cap", (0.22, -0.30, 0.075), (0.17, 0.12, 0.10), shoe, bevel=0.06)
    cube("right white sole", (0.22, -0.16, 0.015), (0.22, 0.37, 0.035), sole, bevel=0.02)

    # Group objects for a cleaner outliner. No rig, no pose controls.
    collection = bpy.data.collections.new("Bernard static model only")
    bpy.context.scene.collection.children.link(collection)
    for obj in list(bpy.context.scene.objects):
        if obj.type == "MESH":
            for source_collection in list(obj.users_collection):
                source_collection.objects.unlink(obj)
            collection.objects.link(obj)

    bpy.ops.object.light_add(type="AREA", location=(0, -3.5, 3.6))
    key = bpy.context.object
    key.name = "preview softbox key light"
    key.data.energy = 550
    key.data.size = 4
    bpy.ops.object.camera_add(location=(0, -4.6, 1.55), rotation=(radians(76), 0, 0))
    bpy.context.scene.camera = bpy.context.object

    bpy.context.scene.render.engine = "CYCLES"
    bpy.context.scene.view_settings.view_transform = "Filmic"
    bpy.context.scene.unit_settings.system = "METRIC"

    bpy.ops.wm.save_as_mainfile(filepath=str(BLEND_PATH))
    bpy.ops.export_scene.gltf(filepath=str(GLB_PATH), export_format="GLB", export_yup=True)


if __name__ == "__main__":
    build_static_bernard()
