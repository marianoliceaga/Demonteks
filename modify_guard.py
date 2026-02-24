import codecs

filepath = r'd:\Demonteks\Scenes\Characters\Guard.tscn'
with codecs.open(filepath, 'r', 'utf-8') as f: 
    lines = f.readlines()

new_ext = []
for d in ['east', 'west']:
    for i in range(6):
        res_id = f'attack_{d}_{i}'
        path = f'res://Assets/characters/guard_demontek/animations/cross-punch/{d}/frame_00{i}.png'
        new_ext.append(f'[ext_resource type=\"Texture2D\" path=\"{path}\" id=\"{res_id}\"]\n')

# find last ext_resource
last_ext = -1
for i, l in enumerate(lines):
    if l.startswith('[ext_resource'): 
        last_ext = i

lines = lines[:last_ext+1] + new_ext + lines[last_ext+1:]

# find animations list
anim_idx = -1
for i, l in enumerate(lines):
    if l.startswith('animations = [{'): 
        anim_idx = i
        break

new_anims = ''
for d in ['east', 'west']:
    anim = '{\n\"frames\": ['
    for i in range(6):
        anim += '{\n\"duration\": 1.0,\n'
        anim += f'\"texture\": ExtResource(\"attack_{d}_{i}\")\n'
        if i < 5: 
            anim += '}, '
        else: 
            anim += '}],\n'
    anim += '\"loop\": false,\n'
    anim += f'\"name\": &\"attack_{d}\",\n'
    anim += '\"speed\": 10.0\n}, '
    new_anims += anim

lines[anim_idx] = lines[anim_idx].replace('animations = [{', 'animations = [' + new_anims + '{')

with codecs.open(filepath, 'w', 'utf-8') as f: 
    f.writelines(lines)
print("Done modifying Guard.tscn")
