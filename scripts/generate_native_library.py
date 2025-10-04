#!/usr/bin/env python3
"""
Z3 Native Library Generator

This script analyzes Z3 C API header files and generates a plan for creating
NativeLibrary2 partial class files that match the header file groups 1-to-1.
"""

import os
import re
from dataclasses import dataclass
from pathlib import Path
from typing import List, Tuple


@dataclass
class FunctionSignature:
    """Represents a parsed C function signature."""
    name: str
    return_type: str
    parameters: List[Tuple[str, str]]  # List of (type, name) tuples
    brief: str = ""  # Brief description
    param_docs: dict = None  # Parameter name -> description mapping
    returns_doc: str = ""  # Return value description
    preconditions: List[str] = None  # List of preconditions
    warnings: List[str] = None  # List of warnings
    remarks: List[str] = None  # List of remarks
    see_also: List[str] = None  # List of related function names

    def __post_init__(self):
        """Initialize mutable default values."""
        if self.param_docs is None:
            self.param_docs = {}
        if self.preconditions is None:
            self.preconditions = []
        if self.warnings is None:
            self.warnings = []
        if self.remarks is None:
            self.remarks = []
        if self.see_also is None:
            self.see_also = []


@dataclass
class HeaderGroup:
    """Represents a group of functions in a Z3 header file."""
    header_file: str
    group_name: str
    group_name_clean: str  # Cleaned for C# class name
    functions: List[str]  # List of Z3 function names in this group
    signatures: List[FunctionSignature]  # Parsed function signatures


def find_groups_in_header(header_path: Path) -> List[Tuple[int, str]]:
    """
    Find all @name groups in a header file.
    Returns list of (line_number, group_name) tuples.
    """
    groups = []
    with open(header_path, 'r', encoding='utf-8') as f:
        for line_num, line in enumerate(f, start=1):
            # Look for /** @name GroupName */
            match = re.search(r'/\*\*\s*@name\s+(.+?)\s*\*/', line)
            if match:
                group_name = match.group(1).strip()
                groups.append((line_num, group_name))
    return groups


def extract_functions_from_group(header_path: Path, start_line: int, end_line: int) -> List[str]:
    """
    Extract all Z3 function names from a specific group in a header file.
    Looks for patterns like: type Z3_API function_name(params)
    """
    functions = []
    with open(header_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()

        # Extract lines in the group range
        group_lines = lines[start_line:end_line]

        for line in group_lines:
            # Look for function declarations with Z3_API
            # Pattern: return_type Z3_API function_name(
            match = re.search(r'Z3_API\s+(Z3_\w+)\s*\(', line)
            if match:
                func_name = match.group(1)
                functions.append(func_name)

    return functions


def map_c_type_to_csharp(c_type: str) -> str:
    """
    Map C type to C# type for P/Invoke.
    """
    type_map = {
        'void': 'void',
        'bool': 'bool',
        'int': 'int',
        'signed': 'int',
        'unsigned': 'uint',
        'unsigned int': 'uint',
        'int64_t': 'long',
        'uint64_t': 'ulong',
        'double': 'double',
        'float': 'float',
        'Z3_string': 'IntPtr',  # char* - marshaled as IntPtr
        'Z3_string_ptr': 'IntPtr',  # Z3_string* - pointer to string pointer
        'Z3_char_ptr': 'IntPtr',  # char const* - marshaled as IntPtr
        'char const *': 'IntPtr',
        'const char *': 'IntPtr',
        'char *': 'IntPtr',
        # All Z3 types are opaque pointers
        'Z3_context': 'IntPtr',
        'Z3_config': 'IntPtr',
        'Z3_symbol': 'IntPtr',
        'Z3_ast': 'IntPtr',
        'Z3_sort': 'IntPtr',
        'Z3_func_decl': 'IntPtr',
        'Z3_app': 'IntPtr',
        'Z3_pattern': 'IntPtr',
        'Z3_model': 'IntPtr',
        'Z3_solver': 'IntPtr',
        'Z3_goal': 'IntPtr',
        'Z3_tactic': 'IntPtr',
        'Z3_simplifier': 'IntPtr',
        'Z3_probe': 'IntPtr',
        'Z3_params': 'IntPtr',
        'Z3_param_descrs': 'IntPtr',
        'Z3_ast_vector': 'IntPtr',
        'Z3_ast_map': 'IntPtr',
        'Z3_apply_result': 'IntPtr',
        'Z3_func_interp': 'IntPtr',
        'Z3_func_entry': 'IntPtr',
        'Z3_optimize': 'IntPtr',
        'Z3_stats': 'IntPtr',
        'Z3_parser_context': 'IntPtr',
        'Z3_constructor': 'IntPtr',
        'Z3_constructor_list': 'IntPtr',
        # Function pointers/callbacks - all marshaled as IntPtr
        'Z3_error_handler': 'IntPtr',
        'Z3_push_eh': 'IntPtr',
        'Z3_pop_eh': 'IntPtr',
        'Z3_fresh_eh': 'IntPtr',
        'Z3_fixed_eh': 'IntPtr',
        'Z3_eq_eh': 'IntPtr',
        'Z3_final_eh': 'IntPtr',
        'Z3_created_eh': 'IntPtr',
        'Z3_decide_eh': 'IntPtr',
        'Z3_on_clause_eh': 'IntPtr',
        'Z3_on_binding_eh': 'IntPtr',
        'Z3_solver_callback': 'IntPtr',
        'Z3_model_eh': 'IntPtr',
        # Enums - marshaled as int
        'Z3_lbool': 'int',
        'Z3_bool': 'int',
        'Z3_ast_kind': 'int',
        'Z3_ast_print_mode': 'int',
        'Z3_decl_kind': 'int',
        'Z3_error_code': 'int',
        'Z3_goal_prec': 'int',
        'Z3_param_kind': 'int',
        'Z3_parameter_kind': 'int',
        'Z3_sort_kind': 'int',
        'Z3_symbol_kind': 'int',
    }

    # Clean the type (remove const, spaces)
    cleaned = c_type.strip()
    # Remove 'const' qualifier
    cleaned = cleaned.replace('const ', '').replace(' const', '').strip()

    # Handle pointer types
    if '*' in cleaned:
        # Check if it's a string type
        if 'char' in cleaned:
            return 'IntPtr'
        # Array parameters - also IntPtr
        return 'IntPtr'

    # Look up in the type map
    if cleaned in type_map:
        return type_map[cleaned]

    # Unknown type - raise error
    raise ValueError(f"Unknown C type: '{c_type}' (cleaned: '{cleaned}'). Add mapping to type_map.")


def extract_function_documentation(header_path: Path, func_name: str) -> dict:
    """
    Extract all documentation tags for a function from the header file.
    Returns dict with keys: brief, param_docs, returns_doc, preconditions, warnings, remarks, see_also.
    """
    with open(header_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Find the function declaration
    pattern = rf'Z3_API\s+{re.escape(func_name)}\s*\('
    match = re.search(pattern, content)

    if not match:
        return {}

    # Find the comment block before the function (/** ... */)
    before_func = content[:match.start()]

    # Find the last /** ... */ comment block before the function
    comment_pattern = r'/\*\*\s*(.*?)\s*\*/'
    comments = list(re.finditer(comment_pattern, before_func, re.DOTALL))

    if not comments:
        return {}

    last_comment = comments[-1]
    comment_text = last_comment.group(1)

    def clean_text(text: str) -> str:
        """Clean up documentation text."""
        text = text.strip()
        text = re.sub(r'\s+', ' ', text)  # Normalize whitespace
        text = re.sub(r'\\c\s+(\w+)', r'\1', text)  # Remove \c markers
        text = re.sub(r'\\ccode\{([^}]+)\}', r'\1', text)  # Remove \ccode{...}
        text = re.sub(r'#(\w+)', r'\1', text)  # Remove # references
        # Escape XML special characters
        text = text.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
        return text

    result = {
        'brief': '',
        'param_docs': {},
        'returns_doc': '',
        'preconditions': [],
        'warnings': [],
        'remarks': [],
        'see_also': []
    }

    # Extract \brief description
    brief_match = re.search(r'\\brief\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    if brief_match:
        result['brief'] = clean_text(brief_match.group(1))

    # Extract \param tags (can have multiple)
    param_matches = re.finditer(r'\\param\s+(\w+)\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for param_match in param_matches:
        param_name = param_match.group(1)
        param_desc = clean_text(param_match.group(2))
        result['param_docs'][param_name] = param_desc

    # Extract \returns
    returns_match = re.search(r'\\returns\s+(.+?)(?=\\param|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    if returns_match:
        result['returns_doc'] = clean_text(returns_match.group(1))

    # Extract \pre tags (can have multiple)
    pre_matches = re.finditer(r'\\pre\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for pre_match in pre_matches:
        result['preconditions'].append(clean_text(pre_match.group(1)))

    # Extract \warning tags (can have multiple)
    warning_matches = re.finditer(r'\\warning\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for warning_match in warning_matches:
        result['warnings'].append(clean_text(warning_match.group(1)))

    # Extract \remark tags (can have multiple)
    remark_matches = re.finditer(r'\\remark\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for remark_match in remark_matches:
        result['remarks'].append(clean_text(remark_match.group(1)))

    # Extract \sa tags (can have multiple, often one per line)
    sa_matches = re.finditer(r'\\sa\s+(Z3_\w+)', comment_text)
    for sa_match in sa_matches:
        result['see_also'].append(sa_match.group(1))

    return result


def parse_function_signature(header_path: Path, func_name: str) -> FunctionSignature:
    """
    Parse a function signature from the header file.
    Looks for patterns like: return_type Z3_API function_name(params)
    """
    with open(header_path, 'r', encoding='utf-8') as f:
        content = f.read()

    # Find the function signature - it may span multiple lines
    # Pattern: return_type Z3_API func_name(...)
    # Use DOTALL to match across lines, non-greedy to stop at first semicolon
    pattern = rf'(\w+(?:\s+\w+)*)\s+Z3_API\s+({re.escape(func_name)})\s*\((.*?)\)\s*;'
    match = re.search(pattern, content, re.DOTALL)

    if not match:
        # Return a default signature if parsing fails
        return FunctionSignature(
            name=func_name,
            return_type='IntPtr',
            parameters=[('IntPtr', 'c')]
        )

    return_type_c = match.group(1).strip()
    params_str = match.group(3).strip()

    # Parse parameters
    parameters = []
    if params_str and params_str != 'void':
        # Split by comma, but be careful with nested types
        param_parts = []
        depth = 0
        current = []
        for char in params_str + ',':
            if char == '(':
                depth += 1
            elif char == ')':
                depth -= 1
            elif char == ',' and depth == 0:
                param_parts.append(''.join(current).strip())
                current = []
                continue
            current.append(char)

        for param in param_parts:
            if not param:
                continue
            # Parameter format: "type name" or "type* name" or "type name[]"
            # Split from the right to get the parameter name
            parts = param.strip().rsplit(None, 1)
            if len(parts) == 2:
                param_type, param_name = parts
                # Clean parameter name (remove [], *, etc.)
                param_name = param_name.rstrip('[]').lstrip('*')
            else:
                param_type = parts[0]
                param_name = 'param'

            parameters.append((param_type, param_name))

    # Extract documentation
    docs = extract_function_documentation(header_path, func_name)

    return FunctionSignature(
        name=func_name,
        return_type=return_type_c,
        parameters=parameters,
        brief=docs.get('brief', ''),
        param_docs=docs.get('param_docs', {}),
        returns_doc=docs.get('returns_doc', ''),
        preconditions=docs.get('preconditions', []),
        warnings=docs.get('warnings', []),
        remarks=docs.get('remarks', []),
        see_also=docs.get('see_also', [])
    )


def clean_group_name_for_class(group_name: str) -> str:
    """
    Convert a group name to a valid C# class name.
    Examples:
    - "Algebraic Numbers" -> "AlgebraicNumbers"
    - "Context and AST Reference Counting" -> "ContextAndAstReferenceCounting"
    - "Bit-vectors" -> "BitVectors"
    """
    # Remove special characters and normalize spaces
    cleaned = re.sub(r'[^\w\s-]', '', group_name)
    # Split on spaces and hyphens, capitalize each word, join
    words = re.split(r'[\s-]+', cleaned)
    return ''.join(word.capitalize() for word in words if word)


def analyze_headers(headers_dir: Path) -> List[HeaderGroup]:
    """
    Analyze all header files in the c_headers directory.
    Returns list of HeaderGroup objects with functions extracted.
    """
    header_files = sorted(headers_dir.glob('*.h'))
    all_groups = []

    for header_path in header_files:
        print(f"  Processing {header_path.name}...")
        groups = find_groups_in_header(header_path)

        # Read file once to get total lines
        with open(header_path, 'r', encoding='utf-8') as f:
            total_lines = len(f.readlines())

        for i, (line_num, group_name) in enumerate(groups):
            # Skip "Types" group as it only contains type declarations
            if group_name.strip() == "Types":
                continue

            # Determine the range of lines for this group
            # Group starts at line_num, ends at next group or end of file
            start_line = line_num
            end_line = groups[i + 1][0] if i + 1 < len(groups) else total_lines

            # Extract functions in this group
            functions = extract_functions_from_group(header_path, start_line, end_line)

            # Parse signatures for each function
            signatures = []
            print(f"    Parsing {len(functions)} functions from {group_name}...")
            for func_idx, func_name in enumerate(functions, 1):
                try:
                    if func_idx % 10 == 0 or func_idx == len(functions):
                        print(f"      [{func_idx}/{len(functions)}] {func_name}")
                    sig = parse_function_signature(header_path, func_name)
                    signatures.append(sig)
                except Exception as e:
                    print(f"ERROR parsing {func_name} in {header_path.name}: {e}")
                    raise

            clean_name = clean_group_name_for_class(group_name)
            header_group = HeaderGroup(
                header_file=header_path.name,
                group_name=group_name,
                group_name_clean=clean_name,
                functions=functions,
                signatures=signatures
            )
            all_groups.append(header_group)

    return all_groups




def generate_csharp_method_name(func_name: str) -> str:
    """
    Generate C# method name from Z3 function name.
    Examples:
    - Z3_mk_bvadd_no_overflow -> MkBvaddNoOverflow
    - Z3_get_bv_sort_size -> GetBvSortSize
    - Z3_mk_int -> MkInt
    """
    # Remove Z3_ prefix
    name = func_name[3:] if func_name.startswith('Z3_') else func_name

    # Split on underscores
    parts = name.split('_')

    # Capitalize first letter of each part, keep rest as-is
    result = ''.join(part.capitalize() for part in parts)

    return result


def generate_delegate_name(func_name: str) -> str:
    """Generate delegate type name from function name."""
    csharp_name = generate_csharp_method_name(func_name)
    return f"{csharp_name}Delegate"


def generate_partial_class(group: HeaderGroup, output_dir: Path):
    """
    Generate a partial class file with delegates and P/Invoke implementations.
    """
    file_name = f"NativeLibrary2.{group.group_name_clean}.generated.cs"
    file_path = output_dir / file_name

    with open(file_path, 'w', encoding='utf-8') as f:
        # Header comment
        f.write("// <auto-generated>\n")
        f.write("// This file was generated by scripts/generate_native_library.py\n")
        f.write(f"// Source: {group.header_file} / {group.group_name}\n")
        f.write("// DO NOT EDIT - Changes will be overwritten\n")
        f.write("// </auto-generated>\n\n")

        # Using statements
        f.write("using System;\n")
        f.write("using System.Runtime.InteropServices;\n\n")

        # Namespace
        f.write("namespace Spaceorc.Z3Wrap.Core.Interop2;\n\n")

        # Class declaration
        f.write("internal sealed partial class NativeLibrary2\n")
        f.write("{\n")

        csharp_keywords = {'string', 'object', 'int', 'bool', 'char', 'byte', 'float', 'double', 'decimal', 'long', 'short', 'uint', 'ulong', 'ushort', 'void', 'class', 'struct', 'enum', 'interface', 'delegate', 'event', 'namespace', 'using', 'ref', 'out', 'params', 'base', 'this', 'fixed'}

        # Generate delegates and methods for each function
        for sig in group.signatures:
            # Map C types to C# types
            return_type_cs = map_c_type_to_csharp(sig.return_type)

            # Build parameter lists
            params_cs = []
            param_names = []

            for param_type_c, param_name in sig.parameters:
                param_type_cs = map_c_type_to_csharp(param_type_c)
                # Escape C# keywords by prefixing with @
                safe_param_name = f"@{param_name}" if param_name in csharp_keywords else param_name
                params_cs.append(f"{param_type_cs} {safe_param_name}")
                param_names.append(safe_param_name)

            params_str = ", ".join(params_cs) if params_cs else ""
            param_names_str = ", ".join(param_names) if param_names else ""

            delegate_name = generate_delegate_name(sig.name)
            csharp_method_name = generate_csharp_method_name(sig.name)

            # Delegate declaration
            f.write(f"    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]\n")
            f.write(f"    private delegate {return_type_cs} {delegate_name}({params_str});\n\n")

            # XML documentation for method
            has_docs = sig.brief or sig.param_docs or sig.returns_doc or sig.preconditions or sig.warnings or sig.remarks or sig.see_also

            if has_docs:
                # Summary (brief description)
                if sig.brief:
                    f.write("    /// <summary>\n")
                    f.write(f"    /// {sig.brief}\n")
                    f.write("    /// </summary>\n")

                # Parameter documentation
                for param_type_c, param_name in sig.parameters:
                    safe_param_name = f"@{param_name}" if param_name in csharp_keywords else param_name
                    if param_name in sig.param_docs:
                        param_doc = sig.param_docs[param_name]
                        f.write(f'    /// <param name="{safe_param_name}">{param_doc}</param>\n')

                # Returns documentation
                if sig.returns_doc:
                    f.write(f"    /// <returns>{sig.returns_doc}</returns>\n")

                # Remarks (preconditions, warnings, remarks)
                remarks_parts = []

                if sig.preconditions:
                    for pre in sig.preconditions:
                        remarks_parts.append(f"Precondition: {pre}")

                if sig.warnings:
                    for warning in sig.warnings:
                        remarks_parts.append(f"Warning: {warning}")

                if sig.remarks:
                    remarks_parts.extend(sig.remarks)

                if remarks_parts:
                    f.write("    /// <remarks>\n")
                    for remark in remarks_parts:
                        f.write(f"    /// {remark}\n")
                    f.write("    /// </remarks>\n")

                # See also references
                for sa_func in sig.see_also:
                    # Convert Z3_function_name to CSharp MethodName
                    sa_csharp = generate_csharp_method_name(sa_func)
                    f.write(f'    /// <seealso cref="{sa_csharp}"/>\n')

            # Z3Function attribute
            f.write(f"    [Z3Function(\"{sig.name}\")]\n")

            # Method implementation
            f.write(f"    internal {return_type_cs} {csharp_method_name}({params_str})\n")
            f.write("    {\n")
            f.write(f"        var funcPtr = GetFunctionPointer(\"{sig.name}\");\n")
            f.write(f"        var func = Marshal.GetDelegateForFunctionPointer<{delegate_name}>(funcPtr);\n")
            if return_type_cs == "void":
                f.write(f"        func({param_names_str});\n")
            else:
                f.write(f"        return func({param_names_str});\n")
            f.write("    }\n\n")

        f.write("}\n")

    return file_path


def main():
    """Main entry point."""
    # Paths
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    headers_dir = project_root / "c_headers"
    output_dir = project_root / "Z3Wrap" / "Core" / "Interop2"

    print("Z3 Native Library Generator")
    print("=" * 80)
    print(f"Headers directory: {headers_dir}")
    print(f"Output directory: {output_dir}")
    print()

    # Clean up old generated files
    print("Cleaning up old generated files...")
    old_files = list(output_dir.glob("*.generated.cs"))
    for old_file in old_files:
        old_file.unlink()
    print(f"Removed {len(old_files)} old generated files")
    print()

    # Analyze headers
    print("Analyzing header files...")
    groups = analyze_headers(headers_dir)
    print(f"Found {len(groups)} groups across {len(set(g.header_file for g in groups))} header files")
    print()

    # Generate partial class files with delegates and P/Invoke
    print("Generating partial class files with P/Invoke implementations...")
    generated_files = []
    for i, group in enumerate(groups, 1):
        print(f"  [{i}/{len(groups)}] {group.group_name_clean} ({len(group.functions)} functions)...")
        file_path = generate_partial_class(group, output_dir)
        generated_files.append(file_path.name)

    print()
    print(f"✅ Generated {len(generated_files)} partial class files")
    print(f"   Total functions: {sum(len(g.functions) for g in groups)}")
    print(f"   Location: {output_dir}")
    print()
    print("Sample files:")
    for i, file_name in enumerate(sorted(generated_files)[:5], 1):
        print(f"  {i}. {file_name}")
    print()
    print("ℹ️  Functions loaded via reflection using [Z3Function] attributes")


if __name__ == "__main__":
    main()
