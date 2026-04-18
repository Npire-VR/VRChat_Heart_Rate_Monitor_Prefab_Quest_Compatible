import bpy
import numpy as np

# & function to rename stuff
def duplicate_and_rename_stuff(obj, starts_with, rename_to):

    # ~ duplicate object
    new_obj = obj.copy()
    new_obj.data = obj.data.copy()  # make mesh unique
    new_obj.name = rename_to
    bpy.context.collection.objects.link(new_obj)
        
    shape_keys = new_obj.data.shape_keys.key_blocks
    vertex_groups = new_obj.vertex_groups

    # ~ shape key rename
    for shape in shape_keys:
        if shape.name.startswith(starts_with):
            shape.name = rename_to + shape.name[1:]            
            
    # ~ vertex group rename
    for group in vertex_groups:
        if group.name.startswith(starts_with):
            group.name = rename_to + group.name[1:]    

# & get and duplicate object
obj = bpy.data.objects.get("1")
duplicate_and_rename_stuff(obj, "1", "2")
duplicate_and_rename_stuff(obj, "1", "3")

# & return to object mode
bpy.ops.object.mode_set(mode='OBJECT')