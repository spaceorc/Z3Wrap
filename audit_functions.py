#!/usr/bin/env python3
"""
Comprehensive audit of Z3 function placement in NativeLibrary files.
Compares functions in NativeLibrary.*.cs against c_headers/*.txt source files.
"""

import os
import re
from collections import defaultdict
from pathlib import Path

def extract_functions_from_cs(file_path):
    """Extract Z3 function names from LoadFunction calls in C# files."""
    functions = []
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
        # Match LoadFunctionInternal and LoadFunctionOrNull calls
        pattern = r'LoadFunction(?:Internal|OrNull)\s*\(\s*handle\s*,\s*functionPointers\s*,\s*"(Z3_[^"]+)"'
        matches = re.findall(pattern, content)
        functions.extend(matches)
    return functions

def extract_functions_from_txt(file_path):
    """Extract Z3 function names from c_headers txt files."""
    functions = []
    with open(file_path, 'r', encoding='utf-8') as f:
        for line in f:
            line = line.strip()
            # Skip empty lines, headers, and separator lines
            if not line or '=' in line or not line.startswith('Z3_'):
                continue
            # Each line is just a function name
            functions.append(line)
    return functions

def map_txt_to_cs_name(txt_filename):
    """Map c_headers txt filename to expected NativeLibrary.*.cs filename."""
    # Remove z3_api_ prefix and .txt suffix
    base = txt_filename.replace('z3_api_', '').replace('.txt', '')

    # Convert to PascalCase
    parts = base.split('_')
    pascal = ''.join(word.capitalize() for word in parts)

    # Special mappings
    mappings = {
        'CreateConfiguration': 'Configuration',
        'ContextAndAstReferenceCounting': 'Context',  # Context management
        'SequencesAndRegularExpressions': 'StringTheory',
        'SpecialRelations': 'SpecialTheories',
        'TacticsSimplifiersAndProbes': 'Tactics',  # Split across multiple files
        'ParserInterface': 'Parsing',
    }

    return mappings.get(pascal, pascal)

def main():
    base_dir = Path('/Users/spaceorc/work/temp/z3lib')
    cs_dir = base_dir / 'Z3Wrap' / 'Core' / 'Interop'
    headers_dir = base_dir / 'c_headers'

    # Read all NativeLibrary.*.cs files
    cs_functions = {}  # filename -> [functions]
    for cs_file in cs_dir.glob('NativeLibrary.*.cs'):
        functions = extract_functions_from_cs(cs_file)
        if functions:
            cs_functions[cs_file.stem] = functions

    # Read all c_headers/*.txt files
    txt_functions = {}  # filename -> [functions]
    for txt_file in headers_dir.glob('*.txt'):
        functions = extract_functions_from_txt(txt_file)
        if functions:
            txt_functions[txt_file.name] = functions

    # Build lookup: function -> txt_file
    function_to_txt = {}
    for txt_file, funcs in txt_functions.items():
        for func in funcs:
            function_to_txt[func] = txt_file

    # Build lookup: function -> cs_file
    function_to_cs = {}
    for cs_file, funcs in cs_functions.items():
        for func in funcs:
            if func in function_to_cs:
                function_to_cs[func].append(cs_file)
            else:
                function_to_cs[func] = [cs_file]

    # Analysis
    print("=" * 80)
    print("FUNCTION PLACEMENT AUDIT")
    print("=" * 80)
    print()

    # Find misplaced functions
    misplaced = []
    for func, cs_files in function_to_cs.items():
        if func in function_to_txt:
            expected_txt = function_to_txt[func]
            expected_cs_suffix = map_txt_to_cs_name(expected_txt)
            expected_cs = f"NativeLibrary.{expected_cs_suffix}"

            for actual_cs in cs_files:
                if actual_cs != expected_cs:
                    misplaced.append((func, actual_cs, expected_cs, expected_txt))

    # Find missing functions
    missing_by_txt = defaultdict(list)
    for txt_file, txt_funcs in txt_functions.items():
        for func in txt_funcs:
            if func not in function_to_cs:
                missing_by_txt[txt_file].append(func)

    # Find duplicate functions
    duplicates = [(func, files) for func, files in function_to_cs.items() if len(files) > 1]

    # Report
    if not misplaced and not missing_by_txt and not duplicates:
        print("✓ ALL FUNCTIONS CORRECTLY PLACED!")
        print()

    if misplaced:
        print("## FUNCTIONS IN WRONG FILES")
        print()
        for func, current, expected, txt_file in sorted(misplaced):
            print(f"  {func}:")
            print(f"    Currently in: {current}.cs")
            print(f"    Should be in: {expected}.cs")
            print(f"    Source: {txt_file}")
            print()

    if missing_by_txt:
        print("## MISSING FUNCTIONS")
        print()
        total_missing = sum(len(funcs) for funcs in missing_by_txt.values())
        print(f"Total missing functions: {total_missing}")
        print()
        for txt_file, funcs in sorted(missing_by_txt.items()):
            if funcs:
                expected_cs = map_txt_to_cs_name(txt_file)
                print(f"{txt_file} -> NativeLibrary.{expected_cs}.cs: Missing {len(funcs)} functions")
                for func in sorted(funcs)[:10]:  # Show first 10
                    print(f"  - {func}")
                if len(funcs) > 10:
                    print(f"  ... and {len(funcs) - 10} more")
                print()

    if duplicates:
        print("## DUPLICATE FUNCTIONS")
        print()
        for func, files in sorted(duplicates):
            print(f"  {func}:")
            for f in files:
                print(f"    - {f}.cs")
            print()

    # Summary statistics
    print("=" * 80)
    print("SUMMARY STATISTICS")
    print("=" * 80)
    print()

    total_txt_funcs = sum(len(funcs) for funcs in txt_functions.values())
    total_cs_funcs = len(set(func for funcs in cs_functions.values() for func in funcs))

    print(f"Functions in c_headers/*.txt: {total_txt_funcs}")
    print(f"Functions in NativeLibrary*.cs: {total_cs_funcs}")
    print(f"Misplaced functions: {len(misplaced)}")
    print(f"Missing functions: {total_missing if missing_by_txt else 0}")
    print(f"Duplicate functions: {len(duplicates)}")
    print()

if __name__ == '__main__':
    main()
