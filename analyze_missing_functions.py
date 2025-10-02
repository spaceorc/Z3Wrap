#!/usr/bin/env python3
"""
Analyze Z3 API coverage by comparing z3_api.h against our NativeLibrary implementation.
"""

import re
import subprocess
from pathlib import Path

def get_z3_api_functions():
    """Download and parse z3_api.h to extract all Z3 API functions."""
    import urllib.request

    url = "https://raw.githubusercontent.com/Z3Prover/z3/master/src/api/z3_api.h"

    print(f"Downloading {url}...")
    with urllib.request.urlopen(url) as response:
        content = response.read().decode('utf-8')

    # Find all Z3_API function declarations
    # Pattern: <return_type> Z3_API Z3_<function_name>(...);
    # The format is: void Z3_API Z3_global_param_set(...) or Z3_config Z3_API Z3_mk_config(void)
    pattern = r'\s+Z3_API\s+(Z3_\w+)\s*\('
    matches = re.findall(pattern, content, re.MULTILINE)

    return sorted(set(matches))

def get_implemented_functions():
    """Extract all Z3 functions we've already implemented in NativeLibrary."""
    interop_dir = Path("Z3Wrap/Core/Interop")

    all_functions = set()

    for cs_file in interop_dir.glob("NativeLibrary.*.cs"):
        content = cs_file.read_text()

        # Find all LoadFunctionOrNull and LoadFunctionInternal calls
        pattern = r'Load(?:FunctionOrNull|FunctionInternal)\([^,]+,\s*[^,]+,\s*"(Z3_\w+)"\)'
        matches = re.findall(pattern, content)
        all_functions.update(matches)

    return sorted(all_functions)

def categorize_missing_functions(missing):
    """Categorize missing functions by their naming patterns."""
    categories = {
        'fixedpoint': [],
        'algebraic': [],
        'substitute': [],
        'simplify': [],
        'parse': [],
        'stats': [],
        'solver_propagate': [],
        'ast_vector': [],
        'ast_map': [],
        'version': [],
        'log': [],
        'other': []
    }

    for func in missing:
        func_lower = func.lower()

        if 'fixedpoint' in func_lower:
            categories['fixedpoint'].append(func)
        elif 'algebraic' in func_lower:
            categories['algebraic'].append(func)
        elif 'substitute' in func_lower:
            categories['substitute'].append(func)
        elif 'simplify' in func_lower:
            categories['simplify'].append(func)
        elif 'parse' in func_lower or 'smtlib' in func_lower or 'eval_' in func_lower:
            categories['parse'].append(func)
        elif 'stats' in func_lower and 'statistics' not in func_lower:
            categories['stats'].append(func)
        elif 'solver_propagate' in func_lower:
            categories['solver_propagate'].append(func)
        elif 'ast_vector' in func_lower:
            categories['ast_vector'].append(func)
        elif 'ast_map' in func_lower:
            categories['ast_map'].append(func)
        elif 'version' in func_lower:
            categories['version'].append(func)
        elif 'log' in func_lower or 'trace' in func_lower or 'reset_memory' in func_lower or 'finalize_memory' in func_lower:
            categories['log'].append(func)
        else:
            categories['other'].append(func)

    return {k: v for k, v in categories.items() if v}  # Remove empty categories

def main():
    print("=" * 80)
    print("Z3 API Coverage Analysis")
    print("=" * 80)
    print()

    # Get all Z3 API functions
    z3_functions = get_z3_api_functions()
    print(f"Total Z3 API functions: {len(z3_functions)}")

    # Get implemented functions
    implemented = get_implemented_functions()
    print(f"Implemented functions: {len(implemented)}")

    # Find missing functions
    missing = sorted(set(z3_functions) - set(implemented))
    print(f"Missing functions: {len(missing)}")
    print()

    # Categorize missing functions
    categories = categorize_missing_functions(missing)

    print("=" * 80)
    print("Missing Functions by Category")
    print("=" * 80)
    print()

    for category, functions in sorted(categories.items()):
        print(f"{category.upper()} ({len(functions)} functions):")
        for func in functions:
            print(f"  - {func}")
        print()

    # Summary for each category
    print("=" * 80)
    print("Summary")
    print("=" * 80)
    print()
    for category, functions in sorted(categories.items()):
        print(f"{category:20s}: {len(functions):3d} functions")
    print(f"{'TOTAL':20s}: {len(missing):3d} functions")
    print()

    # Calculate completion percentage
    completion = (len(implemented) / len(z3_functions)) * 100
    print(f"API Coverage: {completion:.1f}% ({len(implemented)}/{len(z3_functions)})")

    # Check for conversion functions (known to be skipped)
    conversion_funcs = [f for f in missing if any(x in f for x in ['_to_ast', 'app_to_ast', 'func_decl_to_ast', 'sort_to_ast', 'pattern_to_ast'])]
    if conversion_funcs:
        print()
        print(f"Note: {len(conversion_funcs)} conversion functions are intentionally skipped (no-ops in C#):")
        for func in conversion_funcs:
            print(f"  - {func}")

if __name__ == "__main__":
    main()
