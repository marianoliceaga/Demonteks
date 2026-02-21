import os, re

for root, _, files in os.walk('.'):
    for f in files:
        if f.endswith('.tscn'):
            path = os.path.join(root, f)
            with open(path, 'r', encoding='utf-8') as file:
                content = file.read()
                # Find all nodes and their properties
                nodes = re.split(r'\n(?=\[node )', content)
                for n in nodes:
                    if 'type="ColorRect"' in n and 'script = ExtResource' in n:
                        print(f"FOUND IN {path}:\n{n}\n")
