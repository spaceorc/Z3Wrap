#!/usr/bin/env python3
"""
Update NativeLibrary file headers with missing function lists.
"""

import json
import re
from pathlib import Path
from typing import Dict, List

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

def create_missing_section(missing_functions: List[str], descriptions: Dict[str, str]) -> str:
    """Create the 'Missing Functions' section text."""
    if not missing_functions:
        return "None - all functions implemented ✓"
    
    lines = []
    for func in missing_functions:
        desc = descriptions.get(func, "")
        if desc:
            lines.append(f"// - {func} - {desc}")
        else:
            lines.append(f"// - {func}")
    
    return '\n'.join(lines)

def update_file_header(cs_file: Path, missing_functions: List[str], 
                       header_file: Path, expected_count: int, implemented_count: int) -> bool:
    """Update a NativeLibrary file header with missing functions list."""
    
    # Read descriptions from header file
    descriptions = get_function_descriptions(header_file)
    
    # Read the file
    with open(cs_file, 'r') as f:
        lines = f.readlines()
    
    # Find the header comment block
    header_start = 0
    header_end = 0
    in_header = False
    
    for i, line in enumerate(lines):
        if line.strip().startswith('//'):
            if not in_header:
                header_start = i
                in_header = True
            header_end = i
        elif in_header:
            # Found the end of header comments
            break
    
    if not in_header:
        print(f"  WARNING: No header comment found in {cs_file.name}")
        return False
    
    # Extract header lines
    header_lines = lines[header_start:header_end + 1]
    
    # Remove existing "Missing Functions" section if present
    filtered_lines = []
    skip_missing_section = False
    
    for line in header_lines:
        if 'Missing Functions' in line:
            skip_missing_section = True
            continue
        elif skip_missing_section:
            # Skip until we hit a non-comment or empty comment line after section start
            if line.strip() == '//' or not line.strip().startswith('//'):
                skip_missing_section = False
                if line.strip() != '//':
                    filtered_lines.append(line)
            continue
        else:
            filtered_lines.append(line)
    
    # Remove trailing empty comment lines
    while filtered_lines and filtered_lines[-1].strip() == '//':
        filtered_lines.pop()
    
    # Add missing functions section before the last line
    new_header = filtered_lines[:-1] if filtered_lines else []
    
    # Add empty line before missing section
    new_header.append('//\n')
    
    # Add missing functions section
    new_header.append(f'// Missing Functions ({len(missing_functions)} functions):\n')
    missing_section = create_missing_section(missing_functions, descriptions)
    new_header.append(f'{missing_section}\n')
    
    # Add back the last line if it exists
    if filtered_lines:
        new_header.append(filtered_lines[-1])
    
    # Reconstruct file
    new_lines = new_header + lines[header_end + 1:]
    
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
    print(f"Files updated: {updated_count}")
    print(f"Total missing functions: {total_missing}")
    print()

if __name__ == '__main__':
    main()
