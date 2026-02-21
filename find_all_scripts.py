import os, re

for root, _, files in os.walk('.'):
    if '.godot' in root: continue
    for f in files:
        if f.endswith('.tscn') or f.endswith('.scn'):
            path = os.path.join(root, f)
            with open(path, 'r', encoding='utf-8') as file:
                content = file.read()
                nodes = re.split(r'\n(?=\[node )', content)
                for n in nodes:
                    if 'script =' in n:
                        match = re.search(r'\[node.*?type="([^"]+)".*?\]|\[node.*?\]', n)
                        node_type = match.group(1) if match and match.group(1) else "Unknown (Instance?)"
                        print(f"File: {path} | Type: {node_type}")
                        # Look for script
                        for line in n.split('\n'):
                            if 'script = ExtResource' in line or 'script = SubResource' in line:
                                print(f"  -> {line.strip()}")
