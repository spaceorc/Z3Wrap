#!/usr/bin/env python3
"""
Z3 Native Library Generator

This script analyzes Z3 C API header files and generates a plan for creating
NativeZ3Library partial class files that match the header file groups 1-to-1.
"""

import os
import re
import urllib.request
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
class EnumDefinition:
    """Represents a parsed C enum definition."""
    name: str  # Original C name (e.g., Z3_param_kind)
    csharp_name: str  # C# name (e.g., ParamKind)
    values: List[Tuple[str, str, str, str]]  # List of (original_name, csharp_name, value, doc) tuples
    brief: str = ""  # Brief description from header comments
    see_also: List[str] = None  # List of related items

    def __post_init__(self):
        """Initialize mutable default values."""
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


# Z3 GitHub repository configuration
Z3_GITHUB_REPO = "Z3Prover/z3"
Z3_DEFAULT_BRANCH = "master"
Z3_HEADER_FILES = [
    "src/api/z3_api.h",
    "src/api/z3_algebraic.h",
    "src/api/z3_ast_containers.h",
    "src/api/z3_fpa.h",
    "src/api/z3_optimization.h"
]

# Parameter name mapping for documentation bugs in Z3 headers
# Maps: function_name -> { actual_param_name: doc_param_name }
# This fixes cases where \param tags use different names than the actual function signature
PARAM_NAME_FIXES = {
    "Z3_solver_propagate_consequence": {
        "cb": "solver_cb",
        "num_fixed": "num_ids",
        "fixed": "ids",
        "eq_lhs": "lhs",
        "eq_rhs": "rhs",
        "conseq": "consequence"
    }
}

# Function name typo fixes in Z3 header documentation
# Maps: incorrect_name_in_docs -> correct_name
# This fixes cases where \sa tags or other references have typos
FUNCTION_NAME_TYPO_FIXES = {
    "Z3_goal_to_diamcs_string": "Z3_goal_to_dimacs_string"  # Missing 'i' after 'm'
}


def download_header_from_github(file_path: str, branch: str = Z3_DEFAULT_BRANCH) -> str:
    """
    Download a header file from Z3 GitHub repository.
    Returns the file content as a string.
    """
    url = f"https://raw.githubusercontent.com/{Z3_GITHUB_REPO}/{branch}/{file_path}"
    try:
        with urllib.request.urlopen(url) as response:
            return response.read().decode('utf-8')
    except Exception as e:
        raise RuntimeError(f"Failed to download {file_path} from GitHub: {e}")


def download_and_cache_headers(cache_dir: Path, branch: str = Z3_DEFAULT_BRANCH, force_download: bool = False) -> List[Path]:
    """
    Download Z3 header files from GitHub and cache them locally.
    Returns list of cached header file paths.
    """
    cache_dir.mkdir(parents=True, exist_ok=True)
    cached_files = []

    print(f"Downloading Z3 headers from GitHub ({Z3_GITHUB_REPO} @ {branch})...")

    for i, file_path in enumerate(Z3_HEADER_FILES, 1):
        file_name = Path(file_path).name
        local_path = cache_dir / file_name

        if local_path.exists() and not force_download:
            print(f"  [{i}/{len(Z3_HEADER_FILES)}] {file_name} (cached)")
            cached_files.append(local_path)
            continue

        print(f"  [{i}/{len(Z3_HEADER_FILES)}] {file_name} (downloading...)")
        content = download_header_from_github(file_path, branch)

        with open(local_path, 'w', encoding='utf-8') as f:
            f.write(content)

        cached_files.append(local_path)

    print()
    return cached_files


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


# Global set of all enum value names (populated by find_enums_in_headers)
ALL_ENUM_VALUES = set()


def clean_documentation_text(text: str, preserve_formatting: bool = False) -> str:
    """Clean up documentation text with support for paragraphs and bullet lists."""
    if preserve_formatting:
        # Preserve line breaks and formatting - only do minimal cleaning
        raw_lines = text.split('\n')

        # First pass: strip and track empty lines, remove asterisks
        processed_lines = []
        for line in raw_lines:
            stripped = line.strip()
            # Remove leading asterisks from continuation lines
            stripped = re.sub(r'^\*\s*', '', stripped)
            processed_lines.append(stripped)

        cleaned_lines = []
        in_code_block = False
        in_nicebox = False
        nicebox_lines = []
        in_bullet_list = False
        current_bullet_item = []

        i = 0
        while i < len(processed_lines):
            line = processed_lines[i]

            # Skip empty lines - they'll be used for paragraph separation
            if not line:
                # Close bullet list if we were in one
                if in_bullet_list:
                    if current_bullet_item:
                        cleaned_lines.append('<item><description>' + ' '.join(current_bullet_item) + '</description></item>')
                        current_bullet_item = []
                    cleaned_lines.append('</list>')
                    in_bullet_list = False
                # Add paragraph break if next non-empty line exists
                if i + 1 < len(processed_lines):
                    # Look ahead to see if there's more content
                    has_more = any(processed_lines[j] for j in range(i + 1, len(processed_lines)))
                    if has_more:
                        cleaned_lines.append('</para>')
                        cleaned_lines.append('<para>')
                i += 1
                continue

            # Handle nicebox blocks - convert to code blocks with box drawing
            if '\\nicebox{' in line:
                in_nicebox = True
                nicebox_lines = []
                i += 1
                continue
            if in_nicebox and '}' in line:
                in_nicebox = False
                # Find the longest line for box width
                max_width = max(len(l) for l in nicebox_lines) if nicebox_lines else 0
                # Add code block with box drawing
                cleaned_lines.append('<code>')
                cleaned_lines.append('╔' + '═' * (max_width + 2) + '╗')
                for box_line in nicebox_lines:
                    padded = box_line.ljust(max_width)
                    cleaned_lines.append('║ ' + padded + ' ║')
                cleaned_lines.append('╚' + '═' * (max_width + 2) + '╝')
                cleaned_lines.append('</code>')
                i += 1
                continue
            if in_nicebox:
                nicebox_lines.append(line)
                i += 1
                continue

            # Handle code blocks
            if '\\code' in line:
                in_code_block = True
                line = line.replace('\\code', '<code>')
            if '\\endcode' in line:
                in_code_block = False
                line = line.replace('\\endcode', '</code>')

            # Check if this is a bullet point (starts with -)
            is_bullet = line.startswith('-') and not in_code_block

            # Check if this is a continuation of a bullet item (indented)
            is_continuation = False
            if in_bullet_list and not is_bullet and not in_code_block:
                # Look at original line to check indentation
                original_line = raw_lines[i] if i < len(raw_lines) else ''
                # Continuation lines typically have leading whitespace (at least 2 spaces)
                is_continuation = len(original_line) > 0 and (original_line[0] == ' ' or original_line[0] == '\t')

            if is_bullet:
                # Start bullet list if not already in one
                if not in_bullet_list:
                    cleaned_lines.append('<list type="bullet">')
                    in_bullet_list = True
                else:
                    # Close previous bullet item
                    if current_bullet_item:
                        cleaned_lines.append('<item><description>' + ' '.join(current_bullet_item) + '</description></item>')
                        current_bullet_item = []

                # Remove leading dash and whitespace
                line = re.sub(r'^-\s*', '', line)

                # Process the line content (Doxygen markers, XML escaping)
                line = re.sub(r'\\c\s+(\w+)', r'\1', line)  # Remove \c markers
                line = re.sub(r'\\ccode\{([^}]+)\}', r'<code>\1</code>', line)  # Convert \ccode{...}
                # Convert #Z3_name references - check if it's an enum value or function
                def replace_reference(match):
                    z3_name = match.group(1)
                    if z3_name in ALL_ENUM_VALUES:
                        # Keep enum value name unchanged
                        return f'<see cref="{z3_name}"/>'
                    else:
                        # Convert function name to C# style
                        csharp_name = generate_csharp_method_name(z3_name)
                        return f'<see cref="{csharp_name}"/>'
                line = re.sub(r'#(Z3_\w+)', replace_reference, line)

                # Escape XML
                line = line.replace('<code>', '\x00CODE_OPEN\x00')
                line = line.replace('</code>', '\x00CODE_CLOSE\x00')
                see_cref_placeholders = {}
                def protect_see_cref(match):
                    placeholder = f'\x00SEE_CREF_{len(see_cref_placeholders)}\x00'
                    see_cref_placeholders[placeholder] = match.group(0)
                    return placeholder
                line = re.sub(r'<see cref="[^"]+"/>', protect_see_cref, line)
                line = line.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                line = line.replace('\x00CODE_OPEN\x00', '<code>')
                line = line.replace('\x00CODE_CLOSE\x00', '</code>')
                for placeholder, original in see_cref_placeholders.items():
                    line = line.replace(placeholder, original)

                current_bullet_item = [line]
            elif is_continuation and in_bullet_list:
                # Process the continuation line
                line = re.sub(r'\\c\s+(\w+)', r'\1', line)
                line = re.sub(r'\\ccode\{([^}]+)\}', r'<code>\1</code>', line)
                def replace_reference(match):
                    z3_name = match.group(1)
                    if z3_name in ALL_ENUM_VALUES:
                        # Keep enum value name unchanged
                        return f'<see cref="{z3_name}"/>'
                    else:
                        # Convert function name to C# style
                        csharp_name = generate_csharp_method_name(z3_name)
                        return f'<see cref="{csharp_name}"/>'
                line = re.sub(r'#(Z3_\w+)', replace_reference, line)

                # Escape XML
                line = line.replace('<code>', '\x00CODE_OPEN\x00')
                line = line.replace('</code>', '\x00CODE_CLOSE\x00')
                see_cref_placeholders = {}
                def protect_see_cref(match):
                    placeholder = f'\x00SEE_CREF_{len(see_cref_placeholders)}\x00'
                    see_cref_placeholders[placeholder] = match.group(0)
                    return placeholder
                line = re.sub(r'<see cref="[^"]+"/>', protect_see_cref, line)
                line = line.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                line = line.replace('\x00CODE_OPEN\x00', '<code>')
                line = line.replace('\x00CODE_CLOSE\x00', '</code>')
                for placeholder, original in see_cref_placeholders.items():
                    line = line.replace(placeholder, original)

                # Add to current bullet item
                current_bullet_item.append(line)
            else:
                # Close bullet list if we were in one
                if in_bullet_list:
                    if current_bullet_item:
                        cleaned_lines.append('<item><description>' + ' '.join(current_bullet_item) + '</description></item>')
                        current_bullet_item = []
                    cleaned_lines.append('</list>')
                    in_bullet_list = False

                # Regular line processing
                # Remove Doxygen markers (but not inside code blocks)
                if not in_code_block:
                    line = re.sub(r'\\c\s+(\w+)', r'\1', line)  # Remove \c markers
                    line = re.sub(r'\\ccode\{([^}]+)\}', r'<code>\1</code>', line)  # Convert \ccode{...}
                    # Convert #Z3_name references - check if it's an enum value or function
                    def replace_reference(match):
                        z3_name = match.group(1)
                        if z3_name in ALL_ENUM_VALUES:
                            # Keep enum value name unchanged
                            return f'<see cref="{z3_name}"/>'
                        else:
                            # Convert function name to C# style
                            csharp_name = generate_csharp_method_name(z3_name)
                            return f'<see cref="{csharp_name}"/>'
                    line = re.sub(r'#(Z3_\w+)', replace_reference, line)

                # Escape XML special characters (but not our own tags)
                # First, protect our tags
                line = line.replace('<code>', '\x00CODE_OPEN\x00')
                line = line.replace('</code>', '\x00CODE_CLOSE\x00')
                # Protect see cref tags - use a unique placeholder
                see_cref_placeholders = {}
                def protect_see_cref(match):
                    placeholder = f'\x00SEE_CREF_{len(see_cref_placeholders)}\x00'
                    see_cref_placeholders[placeholder] = match.group(0)
                    return placeholder
                line = re.sub(r'<see cref="[^"]+"/>', protect_see_cref, line)
                # Escape XML
                line = line.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                # Restore our tags
                line = line.replace('\x00CODE_OPEN\x00', '<code>')
                line = line.replace('\x00CODE_CLOSE\x00', '</code>')
                # Restore see cref tags
                for placeholder, original in see_cref_placeholders.items():
                    line = line.replace(placeholder, original)

                cleaned_lines.append(line)

            i += 1

        # Close any open bullet list at the end
        if in_bullet_list:
            if current_bullet_item:
                cleaned_lines.append('<item><description>' + ' '.join(current_bullet_item) + '</description></item>')
            cleaned_lines.append('</list>')

        # Wrap entire content in a paragraph if we added paragraph breaks
        result = '\n'.join(cleaned_lines)
        if '</para>' in result:
            result = '<para>\n' + result + '\n</para>'

        return result
    else:
        # Old behavior: collapse to single line
        text = text.strip()
        text = re.sub(r'\s+', ' ', text)  # Normalize whitespace
        text = re.sub(r'\\c\s+(\w+)', r'\1', text)  # Remove \c markers
        text = re.sub(r'\\ccode\{([^}]+)\}', r'<code>\1</code>', text)  # Convert \ccode{...}
        text = re.sub(r'#(\w+)', r'\1', text)  # Remove # references
        # Handle code blocks
        text = text.replace('\\code', '<code>').replace('\\endcode', '</code>')
        # Escape XML special characters (but not our own tags)
        text = text.replace('<code>', '\x00CODE_OPEN\x00')
        text = text.replace('</code>', '\x00CODE_CLOSE\x00')
        text = text.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
        text = text.replace('\x00CODE_OPEN\x00', '<code>')
        text = text.replace('\x00CODE_CLOSE\x00', '</code>')
        return text


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
        # Enums - map to C# enum types
        'Z3_lbool': 'Lbool',
        'Z3_bool': 'int',  # Z3_bool is actually a typedef for int, not an enum
        'Z3_ast_kind': 'AstKind',
        'Z3_ast_print_mode': 'AstPrintMode',
        'Z3_decl_kind': 'DeclKind',
        'Z3_error_code': 'ErrorCode',
        'Z3_goal_prec': 'GoalPrec',
        'Z3_param_kind': 'ParamKind',
        'Z3_parameter_kind': 'ParameterKind',
        'Z3_sort_kind': 'SortKind',
        'Z3_symbol_kind': 'SymbolKind',
    }

    # Clean the type (remove const, spaces)
    cleaned = c_type.strip()
    # Remove 'const' qualifier
    cleaned = cleaned.replace('const ', '').replace(' const', '').strip()

    # Handle array types (e.g., "Z3_ast[]")
    if cleaned.endswith('[]'):
        base_type = cleaned[:-2].strip()
        # Map the base type
        mapped_base = map_c_type_to_csharp(base_type)
        # Return as array type
        return f'{mapped_base}[]'

    # Handle pointer types
    if '*' in cleaned:
        # Check if it's a string type
        if 'char' in cleaned:
            return 'IntPtr'
        # Other pointers - also IntPtr
        return 'IntPtr'

    # Look up in the type map
    if cleaned in type_map:
        return type_map[cleaned]

    # Unknown type - raise error with clear message
    raise ValueError(f"Unknown C type: '{c_type}' (cleaned: '{cleaned}'). Add mapping to type_map in map_c_type_to_csharp().")


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
        result['brief'] = clean_documentation_text(brief_match.group(1), preserve_formatting=True)

    # Extract \param tags (can have multiple)
    param_matches = re.finditer(r'\\param\s+(\w+)\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for param_match in param_matches:
        param_name = param_match.group(1)
        param_desc_raw = param_match.group(2)
        # Strip leading "- " separator that's used in Z3 docs (\param name - description)
        param_desc_raw = re.sub(r'^\s*-\s*', '', param_desc_raw)
        param_desc = clean_documentation_text(param_desc_raw, preserve_formatting=True)
        result['param_docs'][param_name] = param_desc

    # Extract \returns
    returns_match = re.search(r'\\returns\s+(.+?)(?=\\param|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    if returns_match:
        result['returns_doc'] = clean_documentation_text(returns_match.group(1), preserve_formatting=True)

    # Extract \pre tags (can have multiple)
    pre_matches = re.finditer(r'\\pre\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for pre_match in pre_matches:
        result['preconditions'].append(clean_documentation_text(pre_match.group(1), preserve_formatting=True))

    # Extract \warning tags (can have multiple)
    warning_matches = re.finditer(r'\\warning\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for warning_match in warning_matches:
        result['warnings'].append(clean_documentation_text(warning_match.group(1), preserve_formatting=True))

    # Extract \remark tags (can have multiple)
    remark_matches = re.finditer(r'\\remark\s+(.+?)(?=\\param|\\returns|\\pre|\\post|\\warning|\\remark|\\sa|def_API|$)', comment_text, re.DOTALL)
    for remark_match in remark_matches:
        result['remarks'].append(clean_documentation_text(remark_match.group(1), preserve_formatting=True))

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
                # Check if it's an array parameter
                is_array = '[]' in param_name
                # Clean parameter name (remove [], *, etc.)
                param_name = param_name.rstrip('[]').lstrip('*')
                # If it's an array, append [] to the type
                if is_array:
                    param_type = param_type + '[]'
            else:
                param_type = parts[0]
                param_name = 'param'

            parameters.append((param_type, param_name))

    # Extract documentation
    docs = extract_function_documentation(header_path, func_name)

    # Apply parameter name fixes if configured
    param_docs = docs.get('param_docs', {})
    if func_name in PARAM_NAME_FIXES:
        fixes = PARAM_NAME_FIXES[func_name]
        fixed_param_docs = {}
        for actual_param, doc_param in fixes.items():
            if doc_param in param_docs:
                fixed_param_docs[actual_param] = param_docs[doc_param]
        # Merge with any params that don't need fixing
        for param_name, param_doc in param_docs.items():
            if param_name not in fixes.values():
                fixed_param_docs[param_name] = param_doc
        param_docs = fixed_param_docs

    return FunctionSignature(
        name=func_name,
        return_type=return_type_c,
        parameters=parameters,
        brief=docs.get('brief', ''),
        param_docs=param_docs,
        returns_doc=docs.get('returns_doc', ''),
        preconditions=docs.get('preconditions', []),
        warnings=docs.get('warnings', []),
        remarks=docs.get('remarks', []),
        see_also=docs.get('see_also', [])
    )


def convert_enum_name_to_csharp(enum_name: str) -> str:
    """
    Convert Z3 enum name to C# class name.
    Examples:
    - Z3_param_kind -> ParamKind
    - Z3_lbool -> Lbool
    - Z3_symbol_kind -> SymbolKind
    - Z3_ast_kind -> AstKind
    """
    # Remove Z3_ prefix
    name = enum_name[3:] if enum_name.startswith('Z3_') else enum_name

    # Split on underscores and capitalize each part
    parts = name.split('_')
    return ''.join(part.capitalize() for part in parts)


def convert_enum_value_to_csharp(value_name: str, enum_name: str) -> str:
    """
    Keep original Z3 enum value name unchanged.
    Examples:
    - Z3_PK_UINT -> Z3_PK_UINT
    - Z3_L_TRUE -> Z3_L_TRUE
    - Z3_OP_LE -> Z3_OP_LE
    """
    return value_name


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


def extract_enum_documentation(content: str, enum_position: int) -> dict:
    """
    Extract documentation comment before an enum definition.
    Returns dict with: brief, see_also, value_docs (dict of value_name -> description)
    """
    # Look backwards from enum position to find the /** ... */ comment
    before_enum = content[:enum_position]

    # Find the last /** ... */ comment before the enum
    comment_pattern = r'/\*\*\s*(.*?)\s*\*/'
    comments = list(re.finditer(comment_pattern, before_enum, re.DOTALL))

    if not comments:
        return {'brief': '', 'see_also': [], 'value_docs': {}}

    last_comment = comments[-1]
    comment_text = last_comment.group(1)

    result = {
        'brief': '',
        'see_also': [],
        'value_docs': {}
    }

    # Extract \brief description
    brief_match = re.search(r'\\brief\s+(.+?)(?=\\sa|-\s+Z3_|$)', comment_text, re.DOTALL)
    if brief_match:
        result['brief'] = clean_documentation_text(brief_match.group(1), preserve_formatting=True)

    # Extract \sa tags
    sa_matches = re.finditer(r'\\sa\s+(Z3_\w+)', comment_text)
    for sa_match in sa_matches:
        result['see_also'].append(sa_match.group(1))

    # Extract value descriptions (bullet points like "- Z3_VALUE_NAME description..." or "- Z3_VALUE_NAME: description...")
    # Note: descriptions can span multiple lines with indented content
    # Pattern matches: "- Z3_NAME" followed by optional ":" and then the description
    value_pattern = r'-\s+(Z3_\w+):?\s+(.+?)(?=\n\s*-\s+Z3_|\*/$)'
    for value_match in re.finditer(value_pattern, comment_text, re.DOTALL):
        value_name = value_match.group(1)
        value_desc_raw = value_match.group(2).strip()
        value_desc = clean_documentation_text(value_desc_raw, preserve_formatting=True)
        result['value_docs'][value_name] = value_desc

    return result


def find_enums_in_headers(headers_dir: Path) -> List[EnumDefinition]:
    """
    Find all enum definitions in header files.
    Returns list of EnumDefinition objects.
    Also populates the global ALL_ENUM_VALUES set.

    Uses a two-pass approach:
    1. First pass: Extract all enum values to populate ALL_ENUM_VALUES
    2. Second pass: Process documentation (which may reference enum values)
    """
    global ALL_ENUM_VALUES
    ALL_ENUM_VALUES.clear()

    header_files = sorted(headers_dir.glob('*.h'))

    # PASS 1: Extract all enum values without processing documentation
    # This populates ALL_ENUM_VALUES so documentation processing can distinguish
    # enum values from function names in #Z3_name references
    enum_raw_data = []  # Store raw enum data for second pass
    seen_names = set()

    for header_path in header_files:
        with open(header_path, 'r', encoding='utf-8') as f:
            content = f.read()

        # Find enum typedefs: typedef enum { ... } Z3_enum_name;
        # Pattern: typedef enum ... { values } Z3_xxx;
        enum_pattern = r'typedef\s+enum\s+(?:\w+\s+)?\{([^}]+)\}\s+(Z3_\w+)\s*;'
        matches = re.finditer(enum_pattern, content, re.DOTALL)

        for match in matches:
            values_block = match.group(1)
            enum_name = match.group(2)

            if enum_name in seen_names:
                continue
            seen_names.add(enum_name)

            # Parse enum values (without documentation)
            # Pattern: VALUE_NAME or VALUE_NAME = number
            value_pattern = r'(Z3_\w+)\s*(?:=\s*([^,\n]+))?'

            for value_match in re.finditer(value_pattern, values_block):
                value_name = value_match.group(1).strip()
                if value_name:  # Skip empty matches
                    # Add to global set of all enum values
                    ALL_ENUM_VALUES.add(value_name)

            # Store raw data for second pass
            enum_raw_data.append((header_path, content, match, enum_name, values_block))

    # PASS 2: Process documentation and create EnumDefinition objects
    # Now ALL_ENUM_VALUES is fully populated, so documentation cleaning
    # can correctly distinguish enum values from function names
    enums = []

    for header_path, content, match, enum_name, values_block in enum_raw_data:
        # Extract documentation (now with ALL_ENUM_VALUES populated)
        docs = extract_enum_documentation(content, match.start())

        # Parse enum values with documentation
        values = []
        value_pattern = r'(Z3_\w+)\s*(?:=\s*([^,\n]+))?'
        current_value = 0  # Track current enum value

        for value_match in re.finditer(value_pattern, values_block):
            value_name = value_match.group(1).strip()
            explicit_value = value_match.group(2)

            if value_name:  # Skip empty matches
                if explicit_value:
                    # Explicit value provided - parse it
                    explicit_value = explicit_value.strip()
                    # Try to evaluate simple expressions (like -1, 0x100, etc.)
                    try:
                        current_value = int(explicit_value, 0)  # 0 allows parsing hex, octal, etc.
                    except ValueError:
                        # If it's not a simple integer, keep the expression as-is
                        pass
                else:
                    # No explicit value - use current counter
                    explicit_value = str(current_value)

                csharp_value = convert_enum_value_to_csharp(value_name, enum_name)

                # Get documentation for this value
                value_doc = docs['value_docs'].get(value_name, '')

                values.append((value_name, csharp_value, explicit_value, value_doc))

                # Increment for next value (if this was a simple integer)
                try:
                    current_value = int(explicit_value, 0) + 1
                except ValueError:
                    # If it's an expression, we can't auto-increment reliably
                    current_value += 1

        # Create enum definition
        csharp_name = convert_enum_name_to_csharp(enum_name)
        enum_def = EnumDefinition(
            name=enum_name,
            csharp_name=csharp_name,
            values=values,
            brief=docs['brief'],
            see_also=docs['see_also']
        )
        enums.append(enum_def)

    return sorted(enums, key=lambda e: e.name)


def analyze_headers(headers_dir: Path, verbose: bool = False) -> List[HeaderGroup]:
    """
    Analyze all header files in the c_headers directory.
    Returns list of HeaderGroup objects with functions extracted.
    """
    import sys

    header_files = sorted(headers_dir.glob('*.h'))
    all_groups = []

    for header_path in header_files:
        groups = find_groups_in_header(header_path)

        # Read file once to get total lines
        with open(header_path, 'r', encoding='utf-8') as f:
            total_lines = len(f.readlines())

        # Filter out "Types" groups and count total functions
        valid_groups = []
        for i, (line_num, group_name) in enumerate(groups):
            if group_name.strip() == "Types":
                continue
            start_line = line_num
            end_line = groups[i + 1][0] if i + 1 < len(groups) else total_lines
            functions = extract_functions_from_group(header_path, start_line, end_line)
            valid_groups.append((line_num, group_name, start_line, end_line, functions))

        total_funcs = sum(len(funcs) for _, _, _, _, funcs in valid_groups)
        processed_funcs = 0

        for group_idx, (line_num, group_name, start_line, end_line, functions) in enumerate(valid_groups):
            # Parse signatures for each function
            signatures = []

            for func_idx, func_name in enumerate(functions, 1):
                processed_funcs += 1

                # Progress display
                if verbose:
                    if func_idx == 1:
                        print(f"  Processing {header_path.name} - {group_name}...")
                    print(f"    [{func_idx}/{len(functions)}] {func_name}")
                else:
                    # Compact progress bar
                    progress = processed_funcs / total_funcs
                    bar_width = 30
                    filled = int(bar_width * progress)
                    bar = '█' * filled + '░' * (bar_width - filled)
                    # Truncate group name if too long
                    display_name = group_name if len(group_name) <= 40 else group_name[:37] + "..."
                    sys.stdout.write(f"\r  Processing {header_path.name:20s} [{bar}] {display_name:43s}")
                    sys.stdout.flush()

                try:
                    sig = parse_function_signature(header_path, func_name)
                    signatures.append(sig)
                except Exception as e:
                    print(f"\nERROR parsing {func_name} in {header_path.name}: {e}")
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

        if not verbose:
            # Clear the progress line after finishing the file
            sys.stdout.write("\r" + " " * 120 + "\r")
            sys.stdout.flush()

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


def generate_default_param_description(c_type: str) -> str:
    """
    Generate a default parameter description from C type.
    Examples:
    - Z3_context -> context parameter
    - Z3_string -> string parameter
    - Z3_ast -> ast parameter
    - unsigned -> unsigned parameter
    - int -> int parameter
    """
    # Remove Z3_ prefix if present
    type_name = c_type
    if type_name.startswith('Z3_'):
        type_name = type_name[3:]

    # Remove const, *, [], whitespace
    type_name = type_name.replace('const', '').replace('*', '').replace('[]', '').strip()

    # Convert to lowercase for description
    type_name = type_name.lower()

    return f"{type_name} parameter"


def convert_param_name_to_camel_case(param_name: str) -> str:
    """
    Convert snake_case parameter name to camelCase.
    Examples:
    - backtrack_level -> backtrackLevel
    - var_name -> varName
    - c -> c
    """
    # If no underscores, return as-is
    if '_' not in param_name:
        return param_name

    # Split on underscores
    parts = param_name.split('_')

    # First part stays lowercase, capitalize first letter of remaining parts
    return parts[0] + ''.join(part.capitalize() for part in parts[1:])


def format_xml_doc_lines(text: str, indent: str = "    ") -> List[str]:
    """
    Format text as XML documentation lines, preserving original line breaks.

    Args:
        text: The text to format (may contain newlines)
        indent: The indentation prefix (default: "    ")

    Returns:
        List of lines with proper XML comment prefix
    """
    if not text:
        return []

    lines = text.split('\n')
    return [f"{indent}/// {line}" if line else f"{indent}///" for line in lines]


def generate_enums_file(enums: List[EnumDefinition], output_dir: Path):
    """
    Generate NativeZ3Library.Enums.generated.cs with actual enum definitions.
    """
    file_path = output_dir / "NativeZ3Library.Enums.generated.cs"

    with open(file_path, 'w', encoding='utf-8') as f:
        # Header comment
        f.write("// <auto-generated>\n")
        f.write("// This file was generated by scripts/generate_native_library.py\n")
        f.write("// Source: All Z3 header files\n")
        f.write("// DO NOT EDIT - Changes will be overwritten\n")
        f.write("// </auto-generated>\n\n")

        # Using statements
        f.write("using System;\n\n")

        # Namespace
        f.write("namespace Spaceorc.Z3Wrap.Core.Interop;\n\n")

        # Class declaration
        f.write("internal sealed partial class NativeZ3Library\n")
        f.write("{\n")

        # Generate nested enum classes
        for enum_def in enums:
            f.write(f"    /// <summary>\n")
            if enum_def.brief:
                # Preserve original line breaks
                for line in format_xml_doc_lines(enum_def.brief, "    "):
                    f.write(f"{line}\n")
            else:
                # Fallback to just the enum name
                f.write(f"    /// {enum_def.name}\n")
            f.write(f"    /// </summary>\n")

            # Add see also references
            if enum_def.see_also:
                for sa_item in enum_def.see_also:
                    # Apply typo fixes if needed
                    sa_item_fixed = FUNCTION_NAME_TYPO_FIXES.get(sa_item, sa_item)
                    # Try to convert to C# method name
                    sa_csharp = generate_csharp_method_name(sa_item_fixed)
                    f.write(f'    /// <seealso cref="{sa_csharp}"/>\n')

            f.write(f"    internal enum {enum_def.csharp_name}\n")
            f.write("    {\n")

            # Generate enum values
            for i, (original_name, csharp_name, explicit_value, value_doc) in enumerate(enum_def.values):
                # Add documentation
                if value_doc:
                    # Multi-line documentation with description
                    f.write(f"        /// <summary>\n")
                    f.write(f"        /// {original_name}\n")
                    f.write(f"        /// </summary>\n")
                    f.write(f"        /// <remarks>\n")
                    # Preserve original line breaks
                    for line in format_xml_doc_lines(value_doc, "        "):
                        f.write(f"{line}\n")
                    f.write(f"        /// </remarks>\n")
                else:
                    # Simple one-line documentation
                    f.write(f"        /// <summary>{original_name}</summary>\n")

                f.write(f"        {csharp_name} = {explicit_value},\n")

            f.write("    }\n\n")

        f.write("}\n")

    return file_path


def generate_partial_class(group: HeaderGroup, output_dir: Path):
    """
    Generate a partial class file with delegates and P/Invoke implementations.
    """
    file_name = f"NativeZ3Library.{group.group_name_clean}.generated.cs"
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
        f.write("namespace Spaceorc.Z3Wrap.Core.Interop;\n\n")

        # Class declaration
        f.write("internal sealed partial class NativeZ3Library\n")
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
                # Convert snake_case to camelCase
                camel_case_name = convert_param_name_to_camel_case(param_name)
                # Escape C# keywords by prefixing with @
                safe_param_name = f"@{camel_case_name}" if camel_case_name in csharp_keywords else camel_case_name
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
            # Always generate documentation if there are parameters (to include ctype attributes)
            has_docs = sig.brief or sig.param_docs or sig.returns_doc or sig.preconditions or sig.warnings or sig.remarks or sig.see_also or sig.parameters

            if has_docs:
                # Summary (brief description)
                if sig.brief:
                    f.write("    /// <summary>\n")
                    for line in format_xml_doc_lines(sig.brief, "    "):
                        f.write(f"{line}\n")
                    f.write("    /// </summary>\n")

                # Parameter documentation - ALWAYS generate for all parameters with ctype attribute
                for param_type_c, param_name in sig.parameters:
                    # Convert snake_case to camelCase
                    camel_case_name = convert_param_name_to_camel_case(param_name)
                    safe_param_name = f"@{camel_case_name}" if camel_case_name in csharp_keywords else camel_case_name
                    # XML documentation uses the logical name without @ prefix
                    xml_param_name = camel_case_name

                    # Get documentation if available, or generate default from C type
                    param_doc = sig.param_docs.get(param_name, '')
                    if not param_doc:
                        param_doc = generate_default_param_description(param_type_c)

                    # Always include ctype attribute with original C type
                    if '\n' in param_doc:
                        # Multi-line - put content on separate lines
                        f.write(f'    /// <param name="{xml_param_name}" ctype="{param_type_c}">\n')
                        for line in format_xml_doc_lines(param_doc, "    "):
                            f.write(f"{line}\n")
                        f.write(f'    /// </param>\n')
                    else:
                        # Single line
                        f.write(f'    /// <param name="{xml_param_name}" ctype="{param_type_c}">{param_doc}</param>\n')

                # Returns documentation
                if sig.returns_doc:
                    if '\n' in sig.returns_doc:
                        # Multi-line
                        f.write(f"    /// <returns>\n")
                        for line in format_xml_doc_lines(sig.returns_doc, "    "):
                            f.write(f"{line}\n")
                        f.write(f"    /// </returns>\n")
                    else:
                        # Single line
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
                        for line in format_xml_doc_lines(remark, "    "):
                            f.write(f"{line}\n")
                    f.write("    /// </remarks>\n")

                # See also references
                for sa_func in sig.see_also:
                    # Apply typo fixes if needed
                    sa_func_fixed = FUNCTION_NAME_TYPO_FIXES.get(sa_func, sa_func)
                    # Convert Z3_function_name to CSharp MethodName
                    sa_csharp = generate_csharp_method_name(sa_func_fixed)
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


def hide_cursor():
    """Hide terminal cursor."""
    import sys
    sys.stdout.write('\033[?25l')
    sys.stdout.flush()

def show_cursor():
    """Show terminal cursor."""
    import sys
    sys.stdout.write('\033[?25h')
    sys.stdout.flush()

def main():
    """Main entry point."""
    import sys
    import argparse
    import atexit

    # Ensure cursor is shown on exit
    atexit.register(show_cursor)

    # Hide cursor during operation
    hide_cursor()

    try:
        # Parse command-line arguments
        parser = argparse.ArgumentParser(description='Generate NativeZ3Library partial classes from Z3 headers')
        parser.add_argument('--verbose', '-v', action='store_true', help='Enable verbose output with all function names')
        parser.add_argument('--branch', '-b', default=Z3_DEFAULT_BRANCH, help=f'Z3 GitHub branch to use (default: {Z3_DEFAULT_BRANCH})')
        parser.add_argument('--force-download', '-f', action='store_true', help='Force re-download headers even if cached')
        parser.add_argument('--enums-only', action='store_true', help='Generate only the enums file (faster)')
        args = parser.parse_args()

        # Paths
        script_dir = Path(__file__).parent
        project_root = script_dir.parent
        headers_cache_dir = project_root / ".cache" / "z3_headers"
        output_dir = project_root / "Z3Wrap" / "Core" / "Interop"

        print("Z3 Native Library Generator")
        print("=" * 80)
        print(f"GitHub repository: {Z3_GITHUB_REPO} @ {args.branch}")
        print(f"Cache directory: {headers_cache_dir}")
        print()

        # Download headers from GitHub
        header_files = download_and_cache_headers(headers_cache_dir, args.branch, args.force_download)
        headers_dir = headers_cache_dir

        print(f"Output directory: {output_dir}")
        print()

        # Clean up old generated files (unless enums-only mode)
        if not args.enums_only:
            print("Cleaning up old generated files...")
            old_files = list(output_dir.glob("*.generated.cs"))
            for old_file in old_files:
                old_file.unlink()
            print(f"Removed {len(old_files)} old generated files")
            print()

        # Find enums
        print("Finding enum definitions...")
        enums = find_enums_in_headers(headers_dir)
        total_enum_values = sum(len(e.values) for e in enums)
        print(f"✓ Found {len(enums)} enum definitions with {total_enum_values} total values")
        print()

        # Generate enums file
        print("Generating enums file...")
        enums_file = generate_enums_file(enums, output_dir)
        print(f"✓ Generated {enums_file.name} ({len(enums)} enums)")
        print()

        if args.enums_only:
            print("✅ Enums-only mode: Skipping function generation")
            print(f"ℹ️  Generated 1 file: {enums_file.name}")
            return

        # Analyze headers
        print("Analyzing header files...")
        groups = analyze_headers(headers_dir, verbose=args.verbose)
        if not args.verbose:
            print(f"✓ Analyzed {len(groups)} groups across {len(set(g.header_file for g in groups))} header files")
        else:
            print(f"Found {len(groups)} groups across {len(set(g.header_file for g in groups))} header files")
        print()

        # Generate partial class files with delegates and P/Invoke
        print("Generating partial class files with P/Invoke implementations...")
        generated_files = []

        if args.verbose:
            for i, group in enumerate(groups, 1):
                print(f"  [{i}/{len(groups)}] {group.group_name_clean} ({len(group.functions)} functions)...")
                file_path = generate_partial_class(group, output_dir)
                generated_files.append(file_path.name)
        else:
            # Compact progress for generation
            for i, group in enumerate(groups, 1):
                progress = i / len(groups)
                bar_width = 30
                filled = int(bar_width * progress)
                bar = '█' * filled + '░' * (bar_width - filled)
                sys.stdout.write(f"\r  Progress [{bar}] {i}/{len(groups)} groups")
                sys.stdout.flush()
                file_path = generate_partial_class(group, output_dir)
                generated_files.append(file_path.name)
            sys.stdout.write("\r" + " " * 80 + "\r")
            sys.stdout.flush()

        print(f"✅ Generated {len(generated_files)} partial class files")
        print(f"   Total functions: {sum(len(g.functions) for g in groups)}")
        print(f"   Location: {output_dir}")
        print()
        print("ℹ️  Functions loaded via reflection using [Z3Function] attributes")
    except Exception as e:
        show_cursor()
        raise


if __name__ == "__main__":
    main()
