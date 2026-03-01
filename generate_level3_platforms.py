import re

def update_platforms(filepath):
    with open(filepath, 'r') as f:
        content = f.read()

    # Function to replace position for a given node name
    def replace_pos(node_name, new_pos):
        nonlocal content
        pattern = r'(name="' + node_name + '".*?position = Vector2\()[^\)]+(\))'
        content = re.sub(pattern, r'\g<1>' + new_pos + r'\g<2>', content, flags=re.DOTALL)

    replace_pos('F1_Plat1', '960, 400')
    replace_pos('F1_Plat2', '1200, 360')
    replace_pos('F1_Plat3', '1440, 400')
    replace_pos('F1_Plat4', '1680, 340')

    replace_pos('F2_Plat1', '960, 96')
    replace_pos('F2_Plat2', '1200, 56')
    replace_pos('F2_Plat3', '1440, 96')
    replace_pos('F2_Plat4', '1680, 56')

    replace_pos('F3_Plat1', '960, -208')
    replace_pos('F3_Plat2', '1200, -248')
    replace_pos('F3_Plat3', '1440, -180')
    replace_pos('F3_Plat4', '1680, -260')

    # Fixed stair zigzag layout to prevent vertical occlusion
    replace_pos('Stair1_A', '1920, 368')
    replace_pos('Stair1_B', '2060, 304')
    replace_pos('Stair1_C', '1920, 240')
    replace_pos('Stair1_D', '2060, 176')

    replace_pos('Stair2_A', '1920, 64')
    replace_pos('Stair2_B', '2060, 0')
    replace_pos('Stair2_C', '1920, -64')
    replace_pos('Stair2_D', '2060, -128')

    replace_pos('Stair3_A', '1920, -240')
    replace_pos('Stair3_B', '2060, -304')
    replace_pos('Stair3_C', '1920, -368')
    replace_pos('Stair3_D', '2060, -432')

    with open(filepath, 'w') as f:
        f.write(content)
        
    print(f"Updated {filepath} platforms successfully!")

if __name__ == '__main__':
    update_platforms('d:/Demonteks/Scenes/Levels/Level3_Castle.tscn')
