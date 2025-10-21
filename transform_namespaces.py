#!/usr/bin/env python3
"""
NAMESPACE TRANSFORMATION SCRIPT
Systematically updates all namespaces for Brute Gaming Macros
"""

import os
import re
from pathlib import Path

# Namespace transformation mapping
NAMESPACE_MAPPINGS = {
    r'namespace _4RTools\.Utils': 'namespace BruteGamingMacros.Core.Utils',
    r'namespace _4RTools\.Model': 'namespace BruteGamingMacros.Core.Model',
    r'namespace _4RTools\.Forms': 'namespace BruteGamingMacros.UI.Forms',
    r'namespace _4RTools\.Properties': 'namespace BruteGamingMacros.Properties',
    r'namespace _4RTools\.Resources\._4RTools': 'namespace BruteGamingMacros.Resources.BruteGaming',
    r'namespace _4RTools(?!\.)': 'namespace BruteGamingMacros.Core',
}

USING_MAPPINGS = {
    r'using _4RTools\.Utils': 'using BruteGamingMacros.Core.Utils',
    r'using _4RTools\.Model': 'using BruteGamingMacros.Core.Model',
    r'using _4RTools\.Forms': 'using BruteGamingMacros.UI.Forms',
    r'using _4RTools\.Properties': 'using BruteGamingMacros.Properties',
    r'using _4RTools\.Resources\._4RTools': 'using BruteGamingMacros.Resources.BruteGaming',
    r'using _4RTools(?!\.)': 'using BruteGamingMacros.Core',
}

def transform_file(filepath):
    """Transform namespaces and using statements in a single file."""
    try:
        with open(filepath, 'r', encoding='utf-8-sig') as f:
            content = f.read()

        original_content = content
        modified = False

        # Update namespace declarations
        for old_pattern, new_text in NAMESPACE_MAPPINGS.items():
            new_content = re.sub(old_pattern, new_text, content)
            if new_content != content:
                content = new_content
                modified = True

        # Update using statements
        for old_pattern, new_text in USING_MAPPINGS.items():
            new_content = re.sub(old_pattern, new_text, content)
            if new_content != content:
                content = new_content
                modified = True

        # Write back if modified
        if modified:
            with open(filepath, 'w', encoding='utf-8-sig', newline='\r\n') as f:
                f.write(content)
            return True

        return False

    except Exception as e:
        print(f"ERROR processing {filepath}: {e}")
        return False

def main():
    print("=" * 70)
    print("  BRUTE GAMING MACROS - NAMESPACE TRANSFORMATION")
    print("=" * 70)
    print()

    base_dir = Path('.')
    cs_files = list(base_dir.glob('**/*.cs'))

    # Filter out bin and obj directories
    cs_files = [f for f in cs_files if 'bin' not in str(f) and 'obj' not in str(f)]

    print(f"Found {len(cs_files)} C# files to process")
    print()

    updated_count = 0

    # Process files by category
    categories = {
        'Utils': [],
        'Model': [],
        'Forms': [],
        'Properties': [],
        'Resources': [],
        'Root': []
    }

    for file in cs_files:
        if 'Utils' in str(file):
            categories['Utils'].append(file)
        elif 'Model' in str(file):
            categories['Model'].append(file)
        elif 'Forms' in str(file):
            categories['Forms'].append(file)
        elif 'Properties' in str(file):
            categories['Properties'].append(file)
        elif 'Resources' in str(file):
            categories['Resources'].append(file)
        else:
            categories['Root'].append(file)

    # Process each category
    for category, files in categories.items():
        if not files:
            continue

        print(f"→ Processing {category}/ ({len(files)} files)...")

        for filepath in sorted(files):
            if transform_file(filepath):
                print(f"  ✓ {filepath}")
                updated_count += 1

    print()
    print("=" * 70)
    print(f"✅ TRANSFORMATION COMPLETE!")
    print("=" * 70)
    print(f"Total files processed: {len(cs_files)}")
    print(f"Files updated: {updated_count}")
    print()
    print("Next steps:")
    print("  1. Review changes with: git diff")
    print("  2. Update AppConfig.cs with new branding")
    print("  3. Update AssemblyInfo.cs with version info")
    print("  4. Rename project files")
    print()

if __name__ == '__main__':
    main()
