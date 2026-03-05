import os

def find_unused_sfx(project_dir):
    sfx_dir = os.path.join(project_dir, 'Assets', 'audio', 'sfx')
    sfx_files = []
    
    # 1. Gather all SFX files
    for root, _, files in os.walk(sfx_dir):
        for f in files:
            if f.endswith(('.mp3', '.ogg', '.wav')) and not f.endswith('.import'):
                sfx_files.append(f)
                
    print(f"Total SFX files found: {len(sfx_files)}")
                
    # 2. Gather all .tscn and .cs contents to memory (there shouldn't be too many for memory)
    code_contents = []
    scan_exts = ('.tscn', '.cs')
    scan_dirs = [os.path.join(project_dir, 'Scenes'), os.path.join(project_dir, 'Scripts')]
    
    for s_dir in scan_dirs:
        for root, _, files in os.walk(s_dir):
            for f in files:
                if f.endswith(scan_exts):
                    path = os.path.join(root, f)
                    try:
                        with open(path, 'r', encoding='utf-8') as file:
                            code_contents.append(file.read())
                    except Exception as e:
                        pass
                        
    print(f"Total .tscn and .cs files scanned: {len(code_contents)}")
    
    unused_sfx = []
    used_sfx = []
    
    # 3. Check usage
    for sfx in sfx_files:
        is_used = False
        sfx_no_ext = os.path.splitext(sfx)[0]
        # We check both exact filename and filename without extension
        for content in code_contents:
            if sfx in content or sfx_no_ext in content:
                is_used = True
                break
                
        if is_used:
            used_sfx.append(sfx)
        else:
            unused_sfx.append(sfx)
            
    print(f"Used SFX: {len(used_sfx)}")
    print(f"Unused SFX: {len(unused_sfx)}")
    
    if len(unused_sfx) > 0:
        print("\n--- UNUSED SFX FILES ---")
        for u in sorted(unused_sfx):
            print(u)
            
if __name__ == '__main__':
    find_unused_sfx(r"d:\Demonteks")
