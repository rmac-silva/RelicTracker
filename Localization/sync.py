import os
import re
from pathlib import Path

def parse_localization_file(file_path):
    """
    Parses a .loc file into active relics, excluded relics, and tracks 
    where the excluded section begins.
    """
    with open(file_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    active_entries = {}
    excluded_entries = {}
    
    # Track the line index where the excluded section starts
    excluded_section_line_idx = len(lines)
    
    current_key = None
    current_val_lines = []
    is_excluded_section = False

    for idx, line in enumerate(lines):
        # Detect the excluded section header
        if "EXCLUDED RELICS" in line:
            is_excluded_section = True
            excluded_section_line_idx = idx
            
        # Check for a new entry line containing '|' (ignoring comments)
        if '|' in line and not line.strip().startswith('//'):
            # Save previous entry if there was one
            if current_key:
                val_str = "".join(current_val_lines)
                if is_excluded_section or not val_str.strip():
                    excluded_entries[current_key] = val_str
                else:
                    active_entries[current_key] = val_str
            
            # Start new entry
            parts = line.split('|', 1)
            current_key = parts[0].strip()
            current_val_lines = [parts[1]]
        else:
            # If it's a continuation of a multi-line value
            if current_key:
                current_val_lines.append(line)

    # Save the very last entry
    if current_key:
        val_str = "".join(current_val_lines)
        if is_excluded_section or not val_str.strip():
            excluded_entries[current_key] = val_str
        else:
            active_entries[current_key] = val_str

    return active_entries, excluded_entries, excluded_section_line_idx

def sync_localization():
    current_dir = Path(__file__).parent
    eng_path = current_dir / "eng.loc"

    if not eng_path.exists():
        print("Error: eng.loc (source of truth) not found in the current directory.")
        return

    print("Parsing source of truth (eng.loc)...")
    eng_active, eng_excluded, _ = parse_localization_file(eng_path)
    
    # Find all other .loc files
    loc_files = [f for f in current_dir.glob("*.loc") if f.name != "eng.loc"]

    if not loc_files:
        print("No other .loc files found to sync.")
        return

    for loc_file in loc_files:
        print(f"Syncing {loc_file.name}...")
        target_active, target_excluded, excluded_idx = parse_localization_file(loc_file)
        
        # Determine what is missing
        missing_active = {k: v for k, v in eng_active.items() if k not in target_active}
        missing_excluded = {k: v for k, v in eng_excluded.items() if k not in target_excluded}
        
        if not missing_active and not missing_excluded:
            print(f"-> {loc_file.name} is already up to date.")
            continue

        # Read original lines to reconstruct the file safely
        with open(loc_file, 'r', encoding='utf-8') as f:
            original_lines = f.readlines()

        new_lines = []
        
        # 1. Add everything up to the excluded section, plus missing active entries
        for idx, line in enumerate(original_lines):
            if idx == excluded_idx:
                # Insert missing active entries right before the Excluded section
                if missing_active:
                    new_lines.append("\n// Missing active entries added by sync script\n")
                    for k, v in missing_active.items():
                        # Ensure the format matches: KEY | value
                        new_lines.append(f"{k} |{v}")
                    new_lines.append("\n")
            new_lines.append(line)
            
        # 2. Append missing excluded entries to the very end of the file
        if missing_excluded:
            # Ensure there's a newline buffer at the end of the file before appending
            if new_lines and not new_lines[-1].endswith('\n'):
                new_lines.append('\n')
            new_lines.append("// Missing excluded entries added by sync script\n")
            for k, v in missing_excluded.items():
                new_lines.append(f"{k} |{v}")

        # Write back the updated file
        with open(loc_file, 'w', encoding='utf-8') as f:
            f.writelines(new_lines)
            
        print(f"-> Updated {loc_file.name}: Added {len(missing_active)} active and {len(missing_excluded)} excluded entries.")

if __name__ == "__main__":
    sync_localization()