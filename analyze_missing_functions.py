#!/usr/bin/env python3
"""
Analyze missing Z3 functions in NativeLibrary partial files.
"""

import os
import re
from pathlib import Path
from typing import Dict, List, Set, Tuple

# Mapping of NativeLibrary files to their corresponding c_headers files
FILE_MAPPINGS = {
    'NativeLibrary.Accessors.cs': 'z3_api_accessors.txt',
    'NativeLibrary.Arrays.cs': 'z3_api_arrays.txt',
    'NativeLibrary.BitVectors.cs': 'z3_api_bit_vectors.txt',
    'NativeLibrary.Configuration.cs': 'z3_api_create_configuration.txt',
    'NativeLibrary.ConstantsAndApplications.cs': 'z3_api_constants_and_applications.txt',
    'NativeLibrary.Context.cs': 'z3_api_context_and_ast_reference_counting.txt',
    'NativeLibrary.ErrorHandling.cs': 'z3_api_error_handling.txt',
    'NativeLibrary.GlobalParameters.cs': 'z3_api_global_parameters.txt',
    'NativeLibrary.Goals.cs': 'z3_api_goals.txt',
    'NativeLibrary.IntegersAndReals.cs': 'z3_api_integers_and_reals.txt',
    'NativeLibrary.InteractionLogging.cs': 'z3_api_interaction_logging.txt',
    'NativeLibrary.Miscellaneous.cs': 'z3_api_miscellaneous.txt',
    'NativeLibrary.Models.cs': 'z3_api_models.txt',
    'NativeLibrary.Modifiers.cs': 'z3_api_modifiers.txt',
    'NativeLibrary.Numerals.cs': 'z3_api_numerals.txt',
    'NativeLibrary.ParameterDescriptions.cs': 'z3_api_parameter_descriptions.txt',
    'NativeLibrary.Parameters.cs': 'z3_api_parameters.txt',
    'NativeLibrary.Parsing.cs': 'z3_api_parser_interface.txt',
    'NativeLibrary.PropositionalLogicAndEquality.cs': 'z3_api_propositional_logic_and_equality.txt',
    'NativeLibrary.Quantifiers.cs': 'z3_api_quantifiers.txt',
    'NativeLibrary.Sets.cs': 'z3_api_sets.txt',
    'NativeLibrary.Solvers.cs': 'z3_api_solvers.txt',
    'NativeLibrary.Sorts.cs': 'z3_api_sorts.txt',
    'NativeLibrary.SpecialTheories.cs': 'z3_api_special_relations.txt',
    'NativeLibrary.Statistics.cs': 'z3_api_statistics.txt',
    'NativeLibrary.StringConversion.cs': 'z3_api_string_conversion.txt',
    'NativeLibrary.Symbols.cs': 'z3_api_symbols.txt',
    'NativeLibrary.Tactics.cs': 'z3_api_tactics_simplifiers_and_probes.txt',
}

def read_header_functions(header_file: Path) -> Set[str]:
    """Read Z3 function names from header file."""
    functions = set()
    with open(header_file, 'r') as f:
        for line in f:
            line = line.strip()
            if line and not line.startswith('#'):
                # Extract function name (first word)
                match = re.match(r'^(Z3_\w+)', line)
                if match:
                    functions.add(match.group(1))
    return functions

def read_implemented_functions(cs_file: Path) -> Set[str]:
    """Read implemented function names from NativeLibrary partial file."""
    functions = set()
    with open(cs_file, 'r') as f:
        content = f.read()
        # Find LoadFunctionOrNull, LoadFunctionInternal, or LoadFunction calls
        patterns = [
            r'LoadFunctionOrNull\([^,]+,\s*[^,]+,\s*"(Z3_\w+)"\)',
            r'LoadFunctionInternal\([^,]+,\s*[^,]+,\s*"(Z3_\w+)"\)',
            r'LoadFunction<[^>]+>\("(Z3_\w+)"\)',
        ]
        for pattern in patterns:
            for match in re.finditer(pattern, content):
                functions.add(match.group(1))
    return functions

def analyze_file(cs_file: Path, header_file: Path) -> Dict:
    """Analyze a single NativeLibrary file for missing functions."""
    if not header_file.exists():
        return {
            'cs_file': cs_file.name,
            'header_file': header_file.name,
            'error': 'Header file not found'
        }

    if not cs_file.exists():
        return {
            'cs_file': cs_file.name,
            'header_file': header_file.name,
            'error': 'CS file not found'
        }

    expected = read_header_functions(header_file)
    implemented = read_implemented_functions(cs_file)
    missing = expected - implemented

    return {
        'cs_file': cs_file.name,
        'header_file': header_file.name,
        'expected_count': len(expected),
        'implemented_count': len(implemented),
        'missing_count': len(missing),
        'missing_functions': sorted(missing),
        'expected_functions': sorted(expected),
        'implemented_functions': sorted(implemented)
    }

def main():
    base_dir = Path(__file__).parent.resolve()
    interop_dir = base_dir / 'Z3Wrap' / 'Core' / 'Interop'
    headers_dir = base_dir / 'c_headers'

    results = []

    for cs_filename, header_filename in sorted(FILE_MAPPINGS.items()):
        cs_file = interop_dir / cs_filename
        header_file = headers_dir / header_filename

        result = analyze_file(cs_file, header_file)
        results.append(result)

    # Print summary
    print("=" * 80)
    print("Z3 API COVERAGE ANALYSIS")
    print("=" * 80)
    print()

    total_expected = 0
    total_implemented = 0
    total_missing = 0
    files_with_missing = []

    for result in results:
        if 'error' in result:
            print(f"⚠️  {result['cs_file']}: {result['error']}")
            continue

        total_expected += result['expected_count']
        total_implemented += result['implemented_count']
        total_missing += result['missing_count']

        if result['missing_count'] > 0:
            files_with_missing.append(result)

        status = "✓" if result['missing_count'] == 0 else "⚠"
        print(f"{status} {result['cs_file']:50s} "
              f"{result['implemented_count']:3d}/{result['expected_count']:3d} "
              f"({result['missing_count']:3d} missing)")

    print()
    print("=" * 80)
    print(f"TOTAL: {total_implemented}/{total_expected} functions implemented "
          f"({total_missing} missing)")
    if total_expected > 0:
        print(f"Coverage: {total_implemented/total_expected*100:.1f}%")
    print("=" * 80)
    print()

    # Top 5 files with most missing functions
    if files_with_missing:
        print("TOP 5 FILES WITH MOST MISSING FUNCTIONS:")
        print("-" * 80)
        for result in sorted(files_with_missing, key=lambda x: x['missing_count'], reverse=True)[:5]:
            print(f"{result['cs_file']:50s} {result['missing_count']:3d} missing")
            for func in result['missing_functions'][:10]:
                print(f"  - {func}")
            if len(result['missing_functions']) > 10:
                print(f"  ... and {len(result['missing_functions']) - 10} more")
            print()

    # Write detailed report
    report_file = base_dir / 'MISSING_FUNCTIONS_REPORT.txt'
    with open(report_file, 'w') as f:
        f.write("=" * 80 + "\n")
        f.write("DETAILED MISSING FUNCTIONS REPORT\n")
        f.write("=" * 80 + "\n\n")

        for result in results:
            if 'error' in result:
                continue

            f.write(f"\n{'=' * 80}\n")
            f.write(f"{result['cs_file']}\n")
            f.write(f"{'=' * 80}\n")
            f.write(f"Header: {result['header_file']}\n")
            f.write(f"Implemented: {result['implemented_count']}/{result['expected_count']}\n")
            f.write(f"Missing: {result['missing_count']}\n\n")

            if result['missing_count'] > 0:
                f.write("Missing Functions:\n")
                for func in result['missing_functions']:
                    f.write(f"  - {func}\n")
            else:
                f.write("✓ All functions implemented!\n")
            f.write("\n")

    print(f"Detailed report written to: {report_file}")
    
    # Return results for use by update script
    import json
    results_file = base_dir / 'missing_functions_data.json'
    with open(results_file, 'w') as f:
        json.dump(results, f, indent=2)
    print(f"Data file written to: {results_file}")

if __name__ == '__main__':
    main()
