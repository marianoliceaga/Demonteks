import sys

def build_tilemap():
    # Tilemap grid is 16x16
    # 4000px wide = 250 tiles (0 to 249)
    # Target floor Y = 256px -> Row 16
    # Let's say the level height spans from Row 0 (top) to Row 32 (bottom, y=512)
    
    # We will collect cell coordinates (x, y)
    tiles = set()
    
    # helper
    def rect(x_start, x_end, y_start, y_end):
        for x in range(x_start, x_end + 1):
            for y in range(y_start, y_end + 1):
                tiles.add((x, y))

    # ==========================
    # SECTION 1: Dungeon Cell (0-600px -> Tiles 0-38)
    # ==========================
    # Floor (Row 16)
    rect(0, 38, 16, 17)
    # Left Wall
    rect(0, 2, 0, 15)
    # Right Wall (Breakable/Door proxy) -> leaving a gap, but let's build ceiling
    rect(0, 38, 0, 2)
    # A wall to separate cell, but open enough for exit
    rect(36, 38, 0, 11) # Gap between Row 12 and 15 for player to exit
    
    # ==========================
    # SECTION 2: Tunnel Escape (600-2400px -> Tiles 38-150)
    # ==========================
    # Ceiling continues
    rect(38, 150, 0, 2)
    
    # Tunnel Floor with Pits
    # Segment 1
    rect(40, 60, 16, 17)
    # Pit 1 (61-69)
    # Segment 2
    rect(70, 90, 16, 17)
    # Pit 2 (91-105) Wide gap -> needs platforms
    # Segment 3
    rect(106, 150, 16, 17)
    
    # Add random platforms above pits
    rect(63, 67, 12, 12)
    rect(93, 97, 13, 13)
    rect(100, 103, 10, 10)
    
    # Checkpoint 2 platform at x=1500 -> ~Tile 94
    # Tunnel tight spots (stalactites/stalagmites)
    rect(110, 115, 3, 10)
    rect(120, 125, 12, 17)

    # ==========================
    # SECTION 3: Ancient Ruins (2400-4000px -> Tiles 150-250)
    # ==========================
    # Open ceiling area
    
    # Floor drops down slightly to Row 18
    rect(150, 250, 18, 19)
    # Wall forming the ruins structure
    rect(180, 182, 10, 18)
    rect(180, 200, 10, 11) # Roof of ruins
    
    # Hidden Chamber (Tiles 200-220, enclosed)
    rect(202, 222, 10, 11) # Chamber ceiling
    rect(220, 222, 10, 18) # Chamber right wall
    
    # The left wall of the chamber (Tiles 200-202) is missing here,
    # as it will be placed as a StaticBody2D that the StoneButton removes.
    
    # Generate the PackedInt32Array payload
    # Format: [x, y, source_id, atlas_x, atlas_y, alternative_tile]
    # For simplicity, we assume source_id=0, atlas=(0,0), alt=0 (just a block)
    # Since it's Godot 4, the format is slightly different.
    # Actually, let's just make it a simple tile at (0,0) in the atlas.
    # Tile Data format Godot 4: 
    # [ (x << 16) | (y & 0xFFFF), source_id, (atlas_x << 16) | (atlas_y & 0xFFFF) ]
    
    array = []
    for (x, y) in sorted(tiles):
        coord = (x << 16) | (y & 0xFFFF)
        source_id = 0
        atlas_coord = 0 # (0,0) in atlas
        array.extend([coord, source_id, atlas_coord])
        
    print(f"[{', '.join(map(str, array))}]")

if __name__ == "__main__":
    build_tilemap()
