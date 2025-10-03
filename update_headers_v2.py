#!/usr/bin/env python3
"""
Update NativeLibrary file headers with missing function lists.
"""

import json
import re
from pathlib import Path
from typing import Dict, List, Tuple

def get_function_descriptions(header_file: Path) -> Dict[str, str]:
    """Extract function descriptions from Z3 header files."""
    descriptions = {}
    with open(header_file, 'r') as f:
        for line in f:
            line = line.strip()
            if line and not line.startswith('#'):
                # Format: Z3_function_name - Description
                parts = line.split(' - ', 1)
                if len(parts) == 2:
                    descriptions[parts[0].strip()] = parts[1].strip()
    return descriptions

def create_missing_section(missing_functions: List[str], descriptions: Dict[str, str]) -> List[str]:
    """Create the 'Missing Functions' section lines."""
    if not missing_functions:
        return [
            "//\n",
            "// Missing Functions (0 functions):\n",
            "// None - all functions implemented ✓\n"
        ]
    
    lines = [
        "//\n",
        f"// Missing Functions ({len(missing_functions)} functions):\n"
    ]
    
    for func in missing_functions:
        desc = descriptions.get(func, "")
        if desc:
            lines.append(f"// - {func} - {desc}\n")
        else:
            lines.append(f"// - {func}\n")
    
    return lines

def update_file_header(cs_file: Path, missing_functions: List[str], 
                       header_file: Path, expected_count: int, implemented_count: int) -> bool:
    """Update a NativeLibrary file header with missing functions list."""
    
    # Read descriptions from header file
    descriptions = get_function_descriptions(header_file)
    
    # Read the file
    with open(cs_file, 'r') as f:
        lines = f.readlines()
    
    # Find the end of the main header comment block (before "using" statements or blank line)
    header_end = 0
    in_header = False
    found_main_header = False
    
    for i, line in enumerate(lines):
        stripped = line.strip()
        
        # Skip ReSharper comments at the top
        if stripped.startswith('// ReSharper'):
            continue
        
        # Skip blank comment lines after ReSharper
        if stripped == '//':
            if i > 0 and not in_header:
                continue
        
        # Found start of main header
        if stripped.startswith('//') and not stripped.startswith('// ReSharper'):
            in_header = True
            found_main_header = True
            header_end = i
        elif in_header and (not stripped.startswith('//') or stripped == '//'):
            # End of header block
            break
    
    if not found_main_header:
        print(f"  WARNING: No main header comment found in {cs_file.name}")
        return False
    
    # Check if "Missing Functions" section already exists
    has_missing_section = False
    missing_section_start = -1
    missing_section_end = -1
    
    for i in range(header_end + 1):
        if 'Missing Functions' in lines[i]:
            has_missing_section = True
            missing_section_start = i
            # Find the end of the missing section
            for j in range(i + 1, len(lines)):
                if not lines[j].strip().startswith('//'):
                    missing_section_end = j
                    break
            break
    
    # Create the missing section
    missing_section = create_missing_section(missing_functions, descriptions)
    
    # Build the new file
    if has_missing_section:
        # Replace existing section
        new_lines = lines[:missing_section_start] + missing_section + lines[missing_section_end:]
    else:
        # Insert after main header (header_end + 1)
        new_lines = lines[:header_end + 1] + missing_section + lines[header_end + 1:]
    
    # Write back
    with open(cs_file, 'w') as f:
        f.writelines(new_lines)
    
    return True

def main():
    base_dir = Path(__file__).parent.resolve()
    interop_dir = base_dir / 'Z3Wrap' / 'Core' / 'Interop'
    headers_dir = base_dir / 'c_headers'
    
    # Load analysis results
    with open(base_dir / 'missing_functions_data.json', 'r') as f:
        results = json.load(f)
    
    updated_count = 0
    total_missing = 0
    
    print("=" * 80)
    print("UPDATING FILE HEADERS")
    print("=" * 80)
    print()
    
    for result in results:
        if 'error' in result:
            continue
        
        cs_file = interop_dir / result['cs_file']
        header_file = headers_dir / result['header_file']
        missing_functions = result['missing_functions']
        total_missing += result['missing_count']
        
        print(f"Processing {result['cs_file']}...")
        print(f"  Implemented: {result['implemented_count']}/{result['expected_count']}")
        print(f"  Missing: {result['missing_count']}")
        
        if update_file_header(cs_file, missing_functions, header_file, 
                             result['expected_count'], result['implemented_count']):
            updated_count += 1
            print(f"  ✓ Updated")
        else:
            print(f"  ✗ Failed")
        print()
    
    print("=" * 80)
    print(f"SUMMARY")
    print("=" * 80)
    print(f"Files updated: {updated_count}/{len(results)}")
    print(f"Total missing functions: {total_missing}")
    print()
    
    # Calculate top 5
    files_with_missing = [r for r in results if 'error' not in r and r['missing_count'] > 0]
    top5 = sorted(files_with_missing, key=lambda x: x['missing_count'], reverse=True)[:5]
    
    print("TOP 5 FILES WITH MOST MISSING FUNCTIONS:")
    print("-" * 80)
    for result in top5:
        print(f"{result['cs_file']:50s} {result['missing_count']:3d} missing")
    print()

if __name__ == '__main__':
    main()
