#!/usr/bin/env python3
"""
Move a Z3 P/Invoke function from one NativeLibrary partial file to another.

Usage: python move_function.py <function_name> <source_file> <target_file>
Example: python move_function.py MkStringSymbol NativeLibrary.Expressions.cs NativeLibrary.Symbols.cs
"""

import sys
import re
from pathlib import Path
from typing import Optional, Tuple, List


def find_function_block(lines: List[str], func_name: str) -> Optional[Tuple[int, int, str, Optional[Tuple[int, int, str]]]]:
    """
    Find a complete function block including:
    - XML documentation (/// comments)
    - The actual method
    - Optionally the delegate (returned separately)

    Returns: (method_start, method_end, method_text, (delegate_start, delegate_end, delegate_text)) or None
    """
    # Find the method declaration line
    method_pattern = rf'internal\s+\w+\s+{re.escape(func_name)}\s*\('
    method_line = None

    for i, line in enumerate(lines):
        if re.search(method_pattern, line):
            method_line = i
            break

    if method_line is None:
        return None

    # Find the start (include XML docs above)
    start_line = method_line

    # Walk backwards to find XML documentation
    i = method_line - 1
    while i >= 0:
        line = lines[i].strip()
        if line.startswith('///') or line.startswith('[') or line == '':
            start_line = i
            i -= 1
        else:
            break

    # Find the end (closing brace of the method)
    brace_count = 0
    in_method = False
    end_line = method_line

    for i in range(method_line, len(lines)):
        line = lines[i]
        if '{' in line:
            in_method = True
            brace_count += line.count('{')
        if '}' in line:
            brace_count -= line.count('}')

        if in_method and brace_count == 0:
            end_line = i
            break

    # Include trailing blank line if present
    if end_line + 1 < len(lines) and lines[end_line + 1].strip() == '':
        end_line += 1

    method_text = ''.join(lines[start_line:end_line + 1])

    # Now look for the delegate (walk backwards from XML docs start)
    delegate_info = None
    delegate_name = func_name + 'Delegate'

    i = start_line - 1
    while i >= 0 and lines[i].strip() == '':
        i -= 1

    if i >= 0 and delegate_name in lines[i]:
        delegate_end = i
        # Delegate is a single line
        delegate_start = i
        delegate_text = lines[delegate_start]
        delegate_info = (delegate_start, delegate_end, delegate_text)

    return (start_line, end_line, method_text, delegate_info)


def find_load_function_call(lines: List[str], func_name: str) -> Optional[Tuple[int, str]]:
    """
    Find the LoadFunctionInternal/LoadFunctionOrNull call for this function.
    Returns (line_index, z3_function_name) or None
    """
    # Look for the actual Z3 function name in LoadFunction calls
    # The method name might be MkStringSymbol but Z3 call is Z3_mk_string_symbol

    # Try to find by searching for any LoadFunction call that might reference this function
    # Pattern 1: Direct match
    pattern1 = rf'Load(?:FunctionInternal|FunctionOrNull)\(handle,\s*functionPointers,\s*"(Z3_[^"]+)"\)'

    for i, line in enumerate(lines):
        match = re.search(pattern1, line)
        if match:
            z3_name = match.group(1)
            # Check if this Z3 name corresponds to our function
            # Convert Z3_mk_string_symbol -> MkStringSymbol
            method_name_from_z3 = ''.join(word.capitalize() for word in z3_name.replace('Z3_', '').split('_'))
            if method_name_from_z3 == func_name:
                return (i, z3_name)

    return None


def remove_function(file_path: Path, func_name: str) -> Optional[Tuple[str, Optional[str], Optional[str]]]:
    """
    Remove function from source file.
    Returns (func_text, delegate_text, z3_function_name) or None
    """
    with open(file_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    # Find and extract function block
    func_result = find_function_block(lines, func_name)
    if func_result is None:
        print(f"Error: Function {func_name} not found in {file_path.name}", file=sys.stderr)
        return None

    method_start, method_end, func_text, delegate_info = func_result

    delegate_text = None
    if delegate_info:
        delegate_start, delegate_end, delegate_text = delegate_info

    # Find and remove LoadFunction call
    load_result = find_load_function_call(lines, func_name)
    z3_func_name = None
    if load_result:
        load_line, z3_func_name = load_result
    else:
        print(f"Warning: LoadFunction call for {func_name} not found in {file_path.name}", file=sys.stderr)
        load_line = None

    # Remove in reverse order to maintain line numbers
    indices_to_remove = []

    if load_line is not None:
        indices_to_remove.append(load_line)

    if delegate_info:
        indices_to_remove.append(delegate_info[0])

    indices_to_remove.append((method_start, method_end))

    # Sort in reverse order
    indices_to_remove.sort(reverse=True, key=lambda x: x if isinstance(x, int) else x[0])

    # Remove from end to start
    for idx in indices_to_remove:
        if isinstance(idx, tuple):
            start, end = idx
            del lines[start:end + 1]
        else:
            del lines[idx]

    # Write back
    with open(file_path, 'w', encoding='utf-8') as f:
        f.writelines(lines)

    return (func_text, delegate_text, z3_func_name)


def add_function(file_path: Path, func_name: str, func_text: str, delegate_text: Optional[str], z3_func_name: Optional[str]) -> bool:
    """Add function to target file."""
    with open(file_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    # Find the LoadFunctions method
    load_method_pattern = r'private static void LoadFunctions\w+\(IntPtr handle, Dictionary<string, IntPtr> functionPointers\)'
    load_method_line = None

    for i, line in enumerate(lines):
        if re.search(load_method_pattern, line):
            load_method_line = i
            break

    if load_method_line is None:
        print(f"Error: LoadFunctions method not found in {file_path.name}", file=sys.stderr)
        return False

    # Find the opening brace and closing brace of LoadFunctions method
    opening_brace_line = None
    for i in range(load_method_line, len(lines)):
        if '{' in lines[i]:
            opening_brace_line = i
            break

    if opening_brace_line is None:
        print(f"Error: Could not find opening brace of LoadFunctions method", file=sys.stderr)
        return False

    # Find closing brace
    brace_count = 0
    closing_brace_line = None
    for i in range(opening_brace_line, len(lines)):
        brace_count += lines[i].count('{')
        brace_count -= lines[i].count('}')
        if brace_count == 0:
            closing_brace_line = i
            break

    if closing_brace_line is None:
        print(f"Error: Could not find closing brace of LoadFunctions method", file=sys.stderr)
        return False

    # Check if method body is empty (only whitespace/comments)
    method_body = ''.join(lines[opening_brace_line + 1:closing_brace_line])
    is_empty = not method_body.strip() or 'TODO' in method_body

    # Add LoadFunction call
    if z3_func_name:
        # Determine if we should use LoadFunctionInternal or LoadFunctionOrNull
        # Use Internal for now (can be adjusted later)
        load_call = f'        LoadFunctionInternal(handle, functionPointers, "{z3_func_name}");\n'

        if is_empty:
            # Replace the empty body
            insert_pos = opening_brace_line + 1
            # Remove TODO lines if present
            while insert_pos < closing_brace_line and (lines[insert_pos].strip() == '' or 'TODO' in lines[insert_pos]):
                del lines[insert_pos]
                closing_brace_line -= 1
            lines.insert(insert_pos, load_call)
        else:
            # Insert before closing brace
            lines.insert(closing_brace_line, load_call)

    # Add delegate if present
    if delegate_text:
        # Find where to insert delegate (after LoadFunctions method closing brace)
        insert_delegate_pos = closing_brace_line + (1 if not is_empty else 2)  # Adjust for added load call
        # Add blank line before delegate if needed
        if lines[insert_delegate_pos - 1].strip() != '':
            lines.insert(insert_delegate_pos, '\n')
            insert_delegate_pos += 1
        lines.insert(insert_delegate_pos, delegate_text)
        if not delegate_text.endswith('\n'):
            lines[insert_delegate_pos] += '\n'

    # Find the end of the partial class (last closing brace)
    class_end = len(lines) - 1
    while class_end >= 0 and lines[class_end].strip() != '}':
        class_end -= 1

    if class_end < 0:
        print(f"Error: Could not find end of class in {file_path.name}", file=sys.stderr)
        return False

    # Insert function before the class closing brace
    # Add blank line before function if needed
    if lines[class_end - 1].strip() != '':
        lines.insert(class_end, '\n')
        class_end += 1

    if not func_text.endswith('\n'):
        func_text += '\n'

    lines.insert(class_end, func_text)

    # Write back
    with open(file_path, 'w', encoding='utf-8') as f:
        f.writelines(lines)

    return True


def main():
    if len(sys.argv) != 4:
        print("Usage: python move_function.py <function_name> <source_file> <target_file>", file=sys.stderr)
        print("Example: python move_function.py MkStringSymbol NativeLibrary.Expressions.cs NativeLibrary.Symbols.cs", file=sys.stderr)
        sys.exit(1)

    func_name = sys.argv[1]
    source_file = Path(sys.argv[2])
    target_file = Path(sys.argv[3])

    # Resolve paths - assume files are in Z3Wrap/Core/Interop if not absolute
    if not source_file.is_absolute():
        source_file = Path('Z3Wrap/Core/Interop') / source_file
    if not target_file.is_absolute():
        target_file = Path('Z3Wrap/Core/Interop') / target_file

    if not source_file.exists():
        print(f"Error: Source file not found: {source_file}", file=sys.stderr)
        sys.exit(1)

    if not target_file.exists():
        print(f"Error: Target file not found: {target_file}", file=sys.stderr)
        sys.exit(1)

    print(f"Moving {func_name}")
    print(f"  From: {source_file.name}")
    print(f"  To:   {target_file.name}")

    # Remove from source
    result = remove_function(source_file, func_name)
    if result is None:
        sys.exit(1)

    func_text, delegate_text, z3_func_name = result

    if delegate_text:
        print(f"  Found delegate: {delegate_text.strip()}")
    if z3_func_name:
        print(f"  Z3 function: {z3_func_name}")

    # Add to target
    if not add_function(target_file, func_name, func_text, delegate_text, z3_func_name):
        print("Error: Failed to add function to target file", file=sys.stderr)
        print("The function has been removed from source but not added to target!", file=sys.stderr)
        print("You may need to manually restore or fix the files.", file=sys.stderr)
        sys.exit(1)

    print(f"✓ Successfully moved {func_name}")


if __name__ == "__main__":
    main()
