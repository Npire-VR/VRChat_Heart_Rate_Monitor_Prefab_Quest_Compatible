import bpy
import numpy as np
import bmesh

# =========================
# USER VARIABLES
# =========================
preview_u_value = 4  # Set your desired "Resolution Preview U"
font_path = ""  # Change to your font file path

# Load font
font = bpy.data.fonts.load(font_path)

# Store processed objects for merging later
processed_objects = []

# =========================
# MAIN LOOP
# =========================
for i in range(10):
    obj_name = str(i)
    obj = bpy.data.objects.get(obj_name)

    if obj is None:
        print(f"Object '{obj_name}' not found, skipping.")
        continue

    # Ensure it's active
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)

    # -------------------------
    # Set Resolution Preview U
    # -------------------------
    if obj.type == 'FONT':
        obj.data.resolution_u = preview_u_value

        # -------------------------
        # Set Font
        # -------------------------
        obj.data.font = font

    else:
        print(f"Object '{obj_name}' is not a FONT object, skipping.")
        obj.select_set(False)
        continue

    # -------------------------
    # Convert to Mesh
    # -------------------------
    bpy.ops.object.convert(target='MESH')

    # Update reference after conversion
    obj = bpy.context.view_layer.objects.active

    # -------------------------
    # Create Vertex Group
    # -------------------------
    vg_name = "1_" + obj_name
    vg = obj.vertex_groups.new(name=vg_name)

    # Assign all vertices to the group
    verts = [v.index for v in obj.data.vertices]
    vg.add(verts, 1.0, 'ADD')

    # -------------------------
    # Create Shape Key
    # -------------------------
    # Basis key
    if not obj.data.shape_keys:
        obj.shape_key_add(name="Basis")

    sk_name = "1_" + obj_name + "_on"
    sk = obj.shape_key_add(name=sk_name)

    # Move all vertices to 3D cursor
    cursor_loc = bpy.context.scene.cursor.location
    for v in sk.data:
        v.co = obj.matrix_world.inverted() @ cursor_loc

    obj.data.update()

    # -------------------------
    # Invert Shape Key
    # -------------------------
    d = bpy.context.object.data

    shape_keys = d.shape_keys.key_blocks

    def get_shape_key_cos(shape):
        arr = np.empty(len(d.vertices) * 3, dtype=np.single)
        shape.data.foreach_get("co", arr)
        return arr

    basis_key = shape_keys[0]
    basis_co = get_shape_key_cos(shape_keys[0])
    for shape in shape_keys[1:]:
        if not shape.mute and shape.relative_key != shape:
            shape_co = get_shape_key_cos(shape)
            difference = shape_co - basis_co
            inverted_co = basis_co - difference
            shape.data.foreach_set("co", inverted_co)

    d.update()
    
    # -------------------------
    # move verts to cursor
    # -------------------------
    # Ensure we're in edit mode
    bpy.ops.object.mode_set(mode='EDIT')

    # Get mesh data
    bm = bmesh.from_edit_mesh(obj.data)

    # Get cursor location
    cursor_loc = bpy.context.scene.cursor.location

    # Move all vertices to cursor
    for v in bm.verts:
        v.co = obj.matrix_world.inverted() @ cursor_loc

    # Update mesh
    bmesh.update_edit_mesh(obj.data)
    
    # -------------------------
    # Rename Object
    # -------------------------
    obj.name = "1_" + obj_name
    
    if i != 10:
        processed_objects.append(obj)
        obj.select_set(False)
    else:
        obj.delete()
        

# =========================
# MERGE (JOIN) ALL OBJECTS
# =========================
bpy.ops.object.mode_set(mode='OBJECT')
if processed_objects:
    # Deselect everything first
    bpy.ops.object.select_all(action='DESELECT')

    # Select all processed objects
    for obj in processed_objects:
        obj.select_set(True)

    # Set active object (important for join)
    bpy.context.view_layer.objects.active = processed_objects[0]

    # Join them into one object
    bpy.ops.object.join()

    # Optional: rename final merged object
    bpy.context.view_layer.objects.active.name = "1"

print("Done.")