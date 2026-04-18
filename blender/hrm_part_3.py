import bpy
import bmesh

# & config vars
# ~ set the max number the display will hold
max_display_number = 255
# ~ whether digits should be centered
center_digits = True

# & Get the active object
obj = bpy.context.object

# & Check if the object has shape keys
if not obj.data.shape_keys:
    raise Exception("Object has no shape keys")

# & Get the shape key block
key_blocks = obj.data.shape_keys.key_blocks

# & loop through display numbers
for display_number in range(max_display_number + 1):
    
    # ~ determine the list of shape keys we need to use 
    # ? determine number of needed digits
    # ^ if less than 10 then 1 digit
    if display_number < 10:
        
        # * get shape keys based on if center digits is set or not
        if center_digits:
            shape_keys = ["2_" + str(display_number) + "_on"]
        else:
            shape_keys = ["1_" + str(display_number) + "_on"]

    # ^ if between 10 and 99 then 2 digits
    elif display_number < 100:
    
        # * make the number a string so it can be split
        display_number_str = str(display_number)

        # * get shape keys based on if center digits is set or not
        shape_keys = ["2_" + display_number_str[0] + "_on", "1_" + display_number_str[1] + "_on"]
        if center_digits:
            shape_keys.append("2digit")
        
    # ^ if over 100 then assume it's 3 digits for now... this will be fun if i need a commically high number display in the future....
    else:

        # * make the number a string so it can be split
        display_number_str = str(display_number)

        # * get shape keys
        shape_keys = ["3_" + display_number_str[0] + "_on", "2_" + display_number_str[1] + "_on", "1_" + display_number_str[2] + "_on"]

    print(shape_keys)

    # ~ create the new shape key
    # ? set all shape keys to value 0
    for key in key_blocks:
        key.value = 0.0

    # ? set shape keys from the selected list to max value
    for name in shape_keys:
        key_blocks[name].value = 1.0

    # ? create a new shape key that is a mix of the selected shape keys
    bpy.ops.object.shape_key_add(from_mix=True)
    key_blocks[-1].name = str(display_number)
    
    
# & clean up old shape keys after merge is complete
# ~ list of things to check for at the start of shape key name, removes all matching.
starts_with = ["1_","2_","3_"]

# ~ re-get object/keys because i'm too lazy to read my previous code
object = bpy.context.active_object
shape_keys = object.data.shape_keys.key_blocks

# ~ shape key removal
for shape in shape_keys:
    for string in starts_with:
        if shape.name.startswith(string) or shape.name == "2digit":
            object.shape_key_remove(shape)
            break

# & remove extra verts for performance
# ~ determine groups to delete
# ? get the max position
length = str(len(str(max_display_number)))
# ? get the starting number
start = int(str(max_display_number)[0])

# ? initilize groups to delete
groups_to_delete = [ length + "_0" ]

# ? loop threw and add deletion on groups if needed
if start != 9:
    for group_number in range(start + 1, 10, 1):
        groups_to_delete.append(length  + "_" + str(group_number))
 

# ~ get object and delete groups
obj = bpy.context.object

if obj and obj.type == 'MESH':
    bpy.ops.object.mode_set(mode='OBJECT')

    mesh = obj.data
    bm = bmesh.new()
    bm.from_mesh(mesh)

    # Map group names to indices
    name_to_index = {vg.name: vg.index for vg in obj.vertex_groups}
    target_indices = {name_to_index[name] for name in groups_to_delete if name in name_to_index}

    verts_to_delete = []

    # Find vertices belonging to target groups
    for v in mesh.vertices:
        for g in v.groups:
            if g.group in target_indices:
                verts_to_delete.append(v.index)
                break

    # Rebuild lookup table again (safe practice before indexed access)
    bm.verts.ensure_lookup_table()

    # Delete vertices
    bm_verts = [bm.verts[i] for i in verts_to_delete]
    bmesh.ops.delete(bm, geom=bm_verts, context='VERTS')

    # Write mesh back
    bm.to_mesh(mesh)
    bm.free()

    print(f"Deleted {len(verts_to_delete)} vertices.")

    # Remove the vertex groups
    for vg in list(obj.vertex_groups):
        if vg.name in groups_to_delete:
            obj.vertex_groups.remove(vg)

    print("Specified vertex groups removed.")

else:
    print("No active mesh object selected.")