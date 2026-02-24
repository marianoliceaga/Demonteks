#!/usr/bin/env python3
import os
import time
import requests
import json
import sys

# Set UTF-8 encoding for Windows console
if sys.platform == "win32":
    import codecs

    sys.stdout = codecs.getwriter("utf-8")(sys.stdout.buffer, "strict")
    sys.stderr = codecs.getwriter("utf-8")(sys.stderr.buffer, "strict")

API_KEY = "sk_fc340fbff7ca1191a7463f1c7a8d216ccd5d8f4840dd6d98"
OUTPUT_DIR = r"D:\Demonteks\Assets\audio\sfx"
API_URL = "https://api.elevenlabs.io/v1/sound-generation"

SOUND_EFFECTS = [
    # Player Sounds
    (
        "Footstep_Grass",
        "Rhythmic, crisp footsteps of leather boots on dry, crunchy jungle grass.",
    ),
    ("Footstep_Stone", "Clean, echoing boot footsteps on a solid, cold stone floor."),
    ("Dash", "A sharp, pressurized air whoosh combined with a quick friction slide."),
    ("Jump", "A quick fabric rustle and a short, low-effort human vocal grunt."),
    (
        "Land_Soft",
        "A muffled, organic thud of a body landing on soft soil and foliage.",
    ),
    (
        "Land_Hard",
        "A sharp, solid thwack of boots hitting a stone surface with a tiny echo.",
    ),
    ("Player_Punch", "A punchy, meaty thwack of a fist hitting skin and muscle."),
    ("Knife_Slash", "A sharp, clean metallic swish cutting through the air rapidly."),
    (
        "Knife_Hit_Enemy",
        "A quick, wet puncture sound followed by a faint metallic ring.",
    ),
    ("Machete_Slash", "A heavy, weighted blade swing with a mid-range whoosh sound."),
    (
        "Machete_Hit_Enemy",
        "A brutal, heavy chop into thick meat and bone, deep and impactful.",
    ),
    (
        "Sword_Swing",
        "A classic, resonant shring sound of a steel sword slicing the air.",
    ),
    (
        "Player_Hurt_Light",
        "A quick, sharp intake of breath and a short ungh vocal grunt.",
    ),
    (
        "Player_Hurt_Medium",
        "A strained, painful groan of a warrior being struck in the chest.",
    ),
    (
        "Player_Hurt_Heavy",
        "A loud, visceral cry of agony with a slight metallic armor rattle.",
    ),
    (
        "Player_Death",
        "A dramatic, exhaling final breath followed by a heavy body collapse.",
    ),
    (
        "Player_Heal",
        "A shimmering, magical rising shimmer sound with a soft wind whoosh.",
    ),
    ("Respawn", "A low-frequency hum building up into a bright, ethereal chime."),
    (
        "Checkpoint_Activate",
        "A satisfying mechanical click followed by a low, magical pulse hum.",
    ),
    ("Lever_Pull", "Heavy iron gears grinding together with a loud, metallic clunk."),
    # Enemy Sounds
    (
        "Demontek_Idle",
        "Low, wet guttural breathing mixed with a faint, rhythmic mechanical hum.",
    ),
    (
        "Demontek_Attack",
        "A ferocious snarl followed by a sharp, tearing sound of claws.",
    ),
    (
        "Demontek_Hit",
        "A high-pitched demonic screech mixed with a metallic spark sound.",
    ),
    (
        "Demontek_Death",
        "A gurgling, distorted scream that fades into a power-down whine.",
    ),
    (
        "Guard_Baton_Swing",
        "A heavy electrical crackle and a blunt whoosh of a weighted club.",
    ),
    (
        "Mutant_Attack_Heavy",
        "A massive, wet splat and a deep-frequency bone-crushing impact.",
    ),
    (
        "Mutant_Death_Roar",
        "A massive, multi-layered roar of a beast, echoing and deep.",
    ),
    (
        "Goliath_Ground_Pound",
        "A massive explosion of debris and a low-frequency shockwave.",
    ),
    (
        "Goliath_Stagger",
        "The sound of heavy metal plates buckling and a loud, pained groan.",
    ),
    (
        "King_Fire_Explosion",
        "A roaring magical fireball impact followed by a crackling inferno.",
    ),
    # Environment Sounds
    ("Birds_Chirping", "Eerie, multi-toned alien bird calls in a dense, humid jungle."),
    ("Bush_Rustle", "Rhythmic, organic rustling of thick leaves and foliage."),
    ("Chains_Rattle", "Heavy, metallic iron chains clinking and dragging on stone."),
    (
        "Distant_Screams",
        "Faint, echoing human screams bouncing off dungeon walls, haunting.",
    ),
    ("Prison_Door_Creak", "A long, slow, agonizing heavy metal door hinge creaking."),
    (
        "Fire_Roaring",
        "A large, aggressive bonfire crackling and hissing with intense heat.",
    ),
    (
        "Demon_Whispers",
        "Multiple layered, distorted voices whispering in a dark language.",
    ),
    (
        "Hell_Portal_Hum",
        "A deep, vibrating energy hum with occasional electrical pulses.",
    ),
    # UI Sounds
    ("Button_Hover", "A very short, clean wooden tick or soft digital blip."),
    ("Button_Click", "A sharp, tactile mechanical click, like a high-end keyboard."),
    ("Menu_Open", "A quick, snappy whoosh with a rising tonal chime."),
    ("Menu_Close", "A quick, snappy whoosh with a falling tonal chime."),
    ("Menu_Navigate", "A short, neutral mid-range organic thump or pulse."),
    ("Error_Buzz", "A short, dissonant electronic buzz, negative and sharp."),
    ("Success_Chime", "A bright, rising major-chord chime, positive and rewarding."),
    ("Level_Complete", "A triumphant, short orchestral swell with a shimmering bell."),
    (
        "Achievement_Unlock",
        "A sparkly, rapid ascending magical scale, light and celebratory.",
    ),
    ("Stat_Increase", "A quick, high-pitched ding with a rising frequency sweep."),
    (
        "Health_Low_Warning",
        "A rhythmic, muffled heartbeat sound mixed with a soft alert pulse.",
    ),
    (
        "New_Weapon_Notif",
        "A bright, metallic weapon clink followed by a short fanfare.",
    ),
    ("Save_Game", "A soft, shimmering magical chime, clean and satisfying."),
    ("Load_Game", "A clean air whoosh with a positive, bright ending note."),
    ("Volume_Test", "A simple, clear sine wave beep that is pleasant to the ear."),
]


def generate_sound_effect(title, description, max_retries=3):
    """Generate a sound effect using ElevenLabs API with retry logic."""

    for attempt in range(max_retries):
        try:
            headers = {"xi-api-key": API_KEY, "Content-Type": "application/json"}

            payload = {
                "text": description,
                "duration_seconds": 1.0,
                "prompt_influence": 0.3,
                "model_id": "eleven_text_to_sound_v2",
            }

            print(f"[{title}] Attempt {attempt + 1}/{max_retries}...")

            response = requests.post(API_URL, headers=headers, json=payload, timeout=30)

            if response.status_code == 200:
                # Save the audio file
                output_filename = f"{title}.mp3"
                output_path = os.path.join(OUTPUT_DIR, output_filename)

                with open(output_path, "wb") as f:
                    f.write(response.content)

                file_size = os.path.getsize(output_path) / 1024
                print(f"[{title}] ✓ Success ({file_size:.1f} KB)")
                return True
            else:
                error_msg = response.text
                print(f"[{title}] ✗ Failed: {response.status_code} - {error_msg}")
                if response.status_code == 429:
                    print(f"[{title}] Rate limited, waiting 10 seconds...")
                    time.sleep(10)
                elif response.status_code >= 500:
                    print(f"[{title}] Server error, waiting 5 seconds...")
                    time.sleep(5)

        except requests.exceptions.Timeout:
            print(f"[{title}] ✗ Timeout on attempt {attempt + 1}")
            time.sleep(5)
        except requests.exceptions.RequestException as e:
            print(f"[{title}] ✗ Request error: {e}")
            time.sleep(5)
        except Exception as e:
            print(f"[{title}] ✗ Unexpected error: {e}")
            time.sleep(5)

    print(f"[{title}] ✗ Failed after {max_retries} attempts")
    return False


def main():
    """Generate all sound effects."""
    os.makedirs(OUTPUT_DIR, exist_ok=True)

    print(f"Starting sound effect generation...")
    print(f"Output directory: {OUTPUT_DIR}")
    print(f"Total sounds to generate: {len(SOUND_EFFECTS)}")
    print()

    success_count = 0
    failed_sounds = []

    for title, description in SOUND_EFFECTS:
        if generate_sound_effect(title, description):
            success_count += 1
        else:
            failed_sounds.append(title)

        # Small delay between requests to avoid rate limiting
        time.sleep(0.5)

    print()
    print("=" * 50)
    print(f"Generation complete!")
    print(f"Successful: {success_count}/{len(SOUND_EFFECTS)}")
    print(f"Failed: {len(failed_sounds)}")

    if failed_sounds:
        print()
        print("Failed sounds:")
        for sound in failed_sounds:
            print(f"  - {sound}")


if __name__ == "__main__":
    main()
