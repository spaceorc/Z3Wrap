#!/usr/bin/env python3
"""
Z3Library Generator

This script generates Z3Library partial class files from NativeZ3Library.
"""

import re
from dataclasses import dataclass, field
from pathlib import Path
from typing import List, Dict, Tuple, Set, Optional


# Configuration: Functions that should NOT have string overloads for Z3_symbol parameters
# These are typically getter/query functions that inspect existing symbols rather than create new ones
SKIP_SYMBOL_STRING_OVERLOAD = {
    "GetSymbolKind",
    "GetSymbolString",
    "GetSymbolInt",
}

# Configuration: Functions that should be excluded from generation entirely
# These are manually maintained in Z3Library2.cs with custom error handling logic
EXCLUDE_FUNCTIONS = {
    "DelContext",  # Cannot check error after context deletion
    "GetErrorCode",  # Error handling methods - manually maintained
    "SetErrorHandler",
    "SetError",
    "GetErrorMsg",
}

# Configuration: cref attributes to convert to plain text (unresolved or ambiguous references)
# These methods either don't exist in Z3Library or have ambiguous overloads
CONVERT_CREF_TO_TEXT = {
    # Unresolved (CS1574) - Methods excluded from Z3Library
    "MkContext",       # Only MkContextRc is included
    "GlobalParamSet",  # Global params excluded
    "OpenLog",         # Not context-based
    "GetErrorCode",    # Error handling - manually maintained
    "SetErrorHandler",
}


@dataclass
class XmlNode:
    """Represents a node in an XML documentation tree."""
    tag: Optional[str]  # None for text nodes, tag name for element nodes
    attributes: Dict[str, str] = field(default_factory=dict)
    text: str = ""  # Direct text content
    children: List['XmlNode'] = field(default_factory=list)

    def is_text_node(self) -> bool:
        """Check if this is a text node (no tag)."""
        return self.tag is None

    def find_children_by_tag(self, tag: str) -> List['XmlNode']:
        """Find all direct children with the given tag."""
        return [child for child in self.children if child.tag == tag]

    def find_child_by_tag(self, tag: str) -> Optional['XmlNode']:
        """Find first direct child with the given tag."""
        children = self.find_children_by_tag(tag)
        return children[0] if children else None


@dataclass
class EnumValue:
    """Represents a single enum value."""
    name: str
    value: str
    summary: str
    remarks: str


@dataclass
class EnumDefinition:
    """Represents a parsed enum definition."""
    name: str
    summary: str
    see_also: List[str]
    values: List[EnumValue]


@dataclass
class ParameterInfo:
    """Represents a function parameter with C type information."""
    name: str
    csharp_type: str
    c_type: str


@dataclass
class FunctionDefinition:
    """Represents a parsed function definition."""
    name: str
    return_type: str
    return_c_type: str  # C type of return value (from ctype attribute)
    parameters: List[ParameterInfo]
    doc_comment: XmlNode  # Full parsed XML documentation tree


def parse_xml_doc_comment(doc_block: str) -> XmlNode:
    """
    Parse XML documentation comment into a tree structure.
    Takes raw doc block with /// prefixes and returns root XmlNode.
    Preserves original formatting including whitespace.
    """
    # Remove /// prefixes but preserve spacing after it
    lines = []
    for line in doc_block.split('\n'):
        line = line.strip()
        if line.startswith('///'):
            # Remove /// but keep everything after (including leading space)
            content = line[3:]
            # Only strip the single leading space that's conventional after ///
            if content.startswith(' '):
                content = content[1:]
            lines.append(content)

    xml_text = '\n'.join(lines)

    # Simple XML parser - handles nested tags
    root = XmlNode(tag='doc')

    def parse_content(text: str, parent: XmlNode):
        """Recursively parse XML content."""
        pos = 0
        while pos < len(text):
            # Find next tag
            tag_start = text.find('<', pos)

            if tag_start == -1:
                # No more tags - rest is text
                remaining = text[pos:]
                if remaining:
                    parent.children.append(XmlNode(tag=None, text=remaining))
                break

            # Text before tag
            if tag_start > pos:
                text_content = text[pos:tag_start]
                if text_content:
                    parent.children.append(XmlNode(tag=None, text=text_content))

            # Check if it's a closing tag
            if text[tag_start:tag_start+2] == '</':
                # Closing tag - end of current element
                tag_end = text.find('>', tag_start)
                return tag_end + 1

            # Parse opening tag
            tag_end = text.find('>', tag_start)
            if tag_end == -1:
                break

            tag_full = text[tag_start+1:tag_end]

            # Check for self-closing tag
            is_self_closing = tag_full.endswith('/')
            if is_self_closing:
                tag_full = tag_full[:-1].strip()

            # Parse tag name and attributes
            parts = tag_full.split(None, 1)
            tag_name = parts[0]
            attrs = {}

            if len(parts) > 1:
                # Parse attributes
                attr_text = parts[1]
                # Simple attribute parsing: name="value"
                attr_pattern = r'(\w+)="([^"]*)"'
                for match in re.finditer(attr_pattern, attr_text):
                    attrs[match.group(1)] = match.group(2)

            # Create node
            node = XmlNode(tag=tag_name, attributes=attrs)
            parent.children.append(node)

            if is_self_closing:
                pos = tag_end + 1
            else:
                # Parse content until closing tag
                content_start = tag_end + 1
                content_end = parse_content(text[content_start:], node)
                if content_end == -1:
                    # No closing tag found - treat rest as content
                    pos = len(text)
                else:
                    # Skip past closing tag
                    closing_tag = f'</{tag_name}>'
                    pos = content_start + content_end
                    if text[pos:pos+len(closing_tag)] == closing_tag:
                        pos += len(closing_tag)

        return -1  # End of content

    parse_content(xml_text, root)
    return root


def render_xml_node(node: XmlNode, indent: str = "    ") -> str:
    """
    Render XmlNode tree back to C# XML documentation format.
    Returns formatted string with /// prefixes.
    Preserves original formatting exactly as it appears in source.
    """
    lines = []

    def serialize_node_content(n: XmlNode) -> str:
        """Recursively serialize node and children preserving exact formatting."""
        if n.is_text_node():
            return n.text

        # Build attributes string
        attrs_str = ""
        if n.attributes:
            attrs_parts = [f'{k}="{v}"' for k, v in n.attributes.items()]
            attrs_str = " " + " ".join(attrs_parts)

        # Self-closing tag
        if not n.children and not n.text:
            return f"<{n.tag}{attrs_str}/>"

        # Tag with content - preserve exact structure
        result = f"<{n.tag}{attrs_str}>"
        if n.text:
            result += n.text
        for child in n.children:
            result += serialize_node_content(child)
        result += f"</{n.tag}>"

        return result

    # For each top-level child (summary, param, remarks, etc), render line by line
    for child in node.children:
        if child.is_text_node():
            continue  # Skip text nodes at root level

        # Serialize the entire node (opening tag + content + closing tag)
        full_content = serialize_node_content(child)

        # Split by newlines and add /// prefix to each line
        for line in full_content.split('\n'):
            lines.append(f"{indent}/// {line}")

    return '\n'.join(lines)


def parse_native_functions_file(file_path: Path) -> List[FunctionDefinition]:
    """
    Parse a NativeZ3Library.*.generated.cs file and extract function definitions.
    Only includes functions where first parameter has ctype="Z3_context".
    """
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    functions = []

    # Pattern to match function with documentation
    # Looks for: XML docs + [Z3Function] + internal ReturnType MethodName(params) { ... }
    function_pattern = r'((?:[ \t]*///.*?\n)+)[ \t]*\[Z3Function\([^\]]+\)\]\s*internal\s+(\w+)\s+(\w+)\s*\((.*?)\)'

    for match in re.finditer(function_pattern, content, re.MULTILINE | re.DOTALL):
        doc_block = match.group(1)
        return_type = match.group(2)
        method_name = match.group(3)
        params_str = match.group(4)

        # Parse parameters
        parameters = []
        if params_str.strip():
            # Split by comma (simple split, assumes no complex nested types)
            param_parts = [p.strip() for p in params_str.split(',')]
            for param_part in param_parts:
                # Parse: "Type name" or "out Type name"
                parts = param_part.split()
                if len(parts) >= 2:
                    # Check if first part is 'out' modifier
                    if parts[0] == 'out':
                        # Format: "out Type name"
                        if len(parts) >= 3:
                            param_type = f"out {parts[1]}"
                            param_name = parts[2]
                        else:
                            # Invalid format - skip
                            continue
                    else:
                        # Format: "Type name"
                        param_type = parts[0]
                        param_name = parts[1]

                    # Extract ctype from documentation (for now, still use regex)
                    ctype_pattern = rf'<param name="{re.escape(param_name)}" ctype="([^"]+)"'
                    ctype_match = re.search(ctype_pattern, doc_block)
                    c_type = ctype_match.group(1) if ctype_match else ''

                    parameters.append(ParameterInfo(
                        name=param_name,
                        csharp_type=param_type,
                        c_type=c_type
                    ))

        # Filter: only include if first parameter is Z3_context
        if not parameters or parameters[0].c_type != 'Z3_context':
            continue

        # Filter: exclude functions in EXCLUDE_FUNCTIONS configuration
        if method_name in EXCLUDE_FUNCTIONS:
            continue

        # Parse documentation into tree structure
        doc_tree = parse_xml_doc_comment(doc_block)

        # Extract return C type from documentation
        return_ctype_pattern = r'<returns ctype="([^"]+)"'
        return_ctype_match = re.search(return_ctype_pattern, doc_block)
        return_c_type = return_ctype_match.group(1) if return_ctype_match else ''

        functions.append(FunctionDefinition(
            name=method_name,
            return_type=return_type,
            return_c_type=return_c_type,
            parameters=parameters,
            doc_comment=doc_tree
        ))

    return functions


def parse_native_enums_file(file_path: Path) -> List[EnumDefinition]:
    """
    Parse NativeZ3Library.Enums.generated.cs and extract all enum definitions.
    """
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()

    enums = []

    # Find all enum blocks: from "/// <summary>" to the closing "}"
    # Pattern: summary comments + "internal enum Name" + "{" + values + "}"
    enum_pattern = r'((?:[ \t]*///.*?\n)+)[ \t]*internal enum (\w+)\s*\{(.*?)\n[ \t]*\}'

    for match in re.finditer(enum_pattern, content, re.MULTILINE | re.DOTALL):
        doc_block = match.group(1)
        enum_name = match.group(2)
        values_block = match.group(3)

        # Parse documentation block
        summary_lines = []
        see_also = []
        in_summary = False

        for line in doc_block.split('\n'):
            line = line.strip()
            if not line.startswith('///'):
                continue
            line = line[3:].strip()

            if '<summary>' in line:
                in_summary = True
                continue
            elif '</summary>' in line:
                in_summary = False
                continue
            elif in_summary:
                summary_lines.append(line)
            elif '<seealso cref=' in line:
                # Extract cref value
                cref_match = re.search(r'<seealso cref="([^"]+)"', line)
                if cref_match:
                    see_also.append(cref_match.group(1))

        summary = '\n'.join(summary_lines).strip()

        # Parse enum values
        values = []
        # Pattern: optional doc comments + "Name = value,"
        value_pattern = r'((?:[ \t]*///.*?\n)+)?[ \t]*(\w+)\s*=\s*([^,\n]+),'

        for value_match in re.finditer(value_pattern, values_block, re.MULTILINE | re.DOTALL):
            value_doc_block = value_match.group(1) or ''
            value_name = value_match.group(2)
            value_value = value_match.group(3).strip()

            # Parse value documentation
            value_summary_lines = []
            value_remarks_lines = []
            in_value_summary = False
            in_value_remarks = False

            for line in value_doc_block.split('\n'):
                line = line.strip()
                if not line.startswith('///'):
                    continue
                line = line[3:].strip()

                if '<summary>' in line:
                    in_value_summary = True
                    # Check if it's single-line <summary>...</summary>
                    single_match = re.search(r'<summary>(.*?)</summary>', line)
                    if single_match:
                        value_summary_lines.append(single_match.group(1))
                        in_value_summary = False
                    continue
                elif '</summary>' in line:
                    in_value_summary = False
                    continue
                elif '<remarks>' in line:
                    in_value_remarks = True
                    continue
                elif '</remarks>' in line:
                    in_value_remarks = False
                    continue
                elif in_value_summary:
                    value_summary_lines.append(line)
                elif in_value_remarks:
                    value_remarks_lines.append(line)

            value_summary = '\n'.join(value_summary_lines).strip()
            value_remarks = '\n'.join(value_remarks_lines).strip()

            values.append(EnumValue(
                name=value_name,
                value=value_value,
                summary=value_summary,
                remarks=value_remarks
            ))

        enums.append(EnumDefinition(
            name=enum_name,
            summary=summary,
            see_also=see_also,
            values=values
        ))

    return enums


def clone_and_modify_doc(doc_tree: XmlNode, func_name: str, parameters: List[ParameterInfo],
                         param_type_overrides: Dict[str, str] = None) -> XmlNode:
    """
    Clone doc tree and optionally modify parameter names/types for overloads.
    """
    import copy
    cloned = copy.deepcopy(doc_tree)

    # Find all param nodes and update if needed
    if param_type_overrides:
        for param_node in cloned.find_children_by_tag('param'):
            param_name = param_node.attributes.get('name', '')
            if param_name in param_type_overrides:
                # Remove ctype attribute for string overloads
                if 'ctype' in param_node.attributes:
                    del param_node.attributes['ctype']

    # Fix @ prefix in parameter names (C# syntax for reserved keywords)
    for param_node in cloned.find_children_by_tag('param'):
        param_name = param_node.attributes.get('name', '')
        if param_name.startswith('@'):
            # Remove @ prefix from XML documentation (use bare name)
            param_node.attributes['name'] = param_name[1:]

    # Remove duplicate param nodes (can occur after @ prefix stripping)
    seen_params = set()
    params_to_remove = []
    for param_node in cloned.find_children_by_tag('param'):
        param_name = param_node.attributes.get('name', '')
        if param_name in seen_params:
            params_to_remove.append(param_node)
        else:
            seen_params.add(param_name)

    # Remove duplicates from children list
    for node_to_remove in params_to_remove:
        cloned.children.remove(node_to_remove)

    # Ensure we have a summary (add generic one if missing)
    if not cloned.find_child_by_tag('summary'):
        summary_node = XmlNode(
            tag='summary',
            children=[XmlNode(tag=None, text=func_name)]
        )
        cloned.children.insert(0, summary_node)

    return cloned


def filter_invalid_cref_references(doc_tree: XmlNode) -> XmlNode:
    """
    Convert cref/see/seealso tags with invalid references to plain text.
    Walks the XML tree and replaces tags like <see cref="MkContext"/> with just "MkContext".
    """
    import copy
    filtered = copy.deepcopy(doc_tree)

    def process_node(node: XmlNode):
        """Recursively process nodes, replacing invalid cref references."""
        if node.is_text_node():
            return

        # Check if this is a cref-based tag (see, seealso, etc.)
        if node.tag in ['see', 'seealso'] and 'cref' in node.attributes:
            cref_value = node.attributes['cref']

            # Extract just the method name (strip any namespace/class prefix)
            method_name = cref_value.split('.')[-1].split('(')[0]

            # Check if this reference should be converted to text
            if method_name in CONVERT_CREF_TO_TEXT:
                # Convert to text node with just the method name
                node.tag = None  # Make it a text node
                node.text = method_name
                node.attributes.clear()
                node.children.clear()
                return  # Don't process children (there are none now)

        # Process all children recursively
        for child in node.children:
            process_node(child)

    process_node(filtered)
    return filtered


def generate_functions_file(output_dir: Path, functions: List[FunctionDefinition], group_name: str, enum_types: Set[str]):
    """
    Generate Z3Library.{GroupName}.generated.cs with public method wrappers.
    """
    file_path = output_dir / f"Z3Library.{group_name}.generated.cs"

    with open(file_path, 'w', encoding='utf-8') as f:
        # Header
        f.write("// <auto-generated>\n")
        f.write("// This file was generated by scripts/generate_library.py\n")
        f.write(f"// Source: NativeZ3Library.{group_name}.generated.cs\n")
        f.write("// DO NOT EDIT - Changes will be overwritten\n")
        f.write("// </auto-generated>\n\n")

        f.write("using System.Runtime.InteropServices;\n")
        f.write("using Spaceorc.Z3Wrap.Core.Interop;\n\n")
        f.write("namespace Spaceorc.Z3Wrap.Core;\n\n")

        f.write("public sealed partial class Z3Library\n")
        f.write("{\n")

        # Generate each function
        for func in functions:
            # Check if function has Z3_symbol parameters (for overload generation)
            symbol_params = [p for p in func.parameters if p.c_type == 'Z3_symbol']
            has_symbols = len(symbol_params) > 0

            # Check if this function should skip string overload (configured list)
            skip_string_overload = func.name in SKIP_SYMBOL_STRING_OVERLOAD

            # Generate string overload first (if symbols present and not in skip list)
            if has_symbols and not skip_string_overload:
                # Clone and prepare documentation for string overload
                param_overrides = {}
                for param in symbol_params:
                    param_overrides[param.name] = 'string'

                doc_clone = clone_and_modify_doc(func.doc_comment, func.name, func.parameters, param_overrides)
                doc_clone = filter_invalid_cref_references(doc_clone)

                # Render documentation
                doc_output = render_xml_node(doc_clone, "    ")
                f.write(doc_output + "\n")

                # Parameters - convert Z3_symbol to string, Z3_string to string
                public_params = []
                for param in func.parameters:
                    if param.c_type == 'Z3_string':
                        public_type = 'string'
                    elif param.c_type == 'Z3_symbol':
                        public_type = 'string'  # String overload for symbols
                    else:
                        public_type = param.csharp_type

                    public_params.append(f"{public_type} {param.name}")

                # Determine public return type (convert Z3_string to string)
                public_return_type = 'string' if func.return_c_type == 'Z3_string' else func.return_type

                # Method signature
                params_str = ", ".join(public_params)
                f.write(f"    public {public_return_type} {func.name}({params_str})\n")
                f.write("    {\n")

                context_param = func.parameters[0].name

                # Convert string parameters to AnsiStringPtr
                string_params = [p for p in func.parameters if p.c_type == 'Z3_string']
                for param in string_params:
                    f.write(f"        using var {param.name}Ansi = new AnsiStringPtr({param.name});\n")

                # Convert symbol string parameters to Z3_symbol
                for param in symbol_params:
                    f.write(f"        using var {param.name}Ansi = new AnsiStringPtr({param.name});\n")
                    f.write(f"        var {param.name}Symbol = nativeLibrary.MkStringSymbol({context_param}, {param.name}Ansi);\n")
                    f.write(f"        CheckError({context_param});\n")

                # Call native method
                native_args = []
                for param in func.parameters:
                    # Check if parameter has 'out' modifier
                    has_out_modifier = param.csharp_type.startswith('out ')
                    param_name_only = param.name

                    if param.c_type == 'Z3_string':
                        arg = f"{param.name}Ansi"
                    elif param.c_type == 'Z3_symbol':
                        arg = f"{param.name}Symbol"
                    elif param.csharp_type in enum_types:
                        # Enum parameter - cast to internal enum
                        arg = f"(NativeZ3Library.{param.csharp_type}){param.name}"
                    else:
                        arg = param_name_only

                    # Add 'out' keyword if needed
                    if has_out_modifier:
                        native_args.append(f"out {arg}")
                    else:
                        native_args.append(arg)

                native_args_str = ", ".join(native_args)

                # Check if this function should skip error checking
                # Skip for: IncRef/DecRef (reference counting)
                skip_error_check = func.name.endswith('IncRef') or func.name.endswith('DecRef')

                # Generate call based on return type
                if func.return_c_type == 'Z3_string':
                    # Z3_string return type - marshal to C# string
                    f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                    if not skip_error_check:
                        f.write(f"        CheckError({context_param});\n")
                    f.write(f"        result = CheckHandle(result, nameof({func.name}));\n")
                    f.write(f"        return Marshal.PtrToStringAnsi(result) ?? throw new InvalidOperationException(\"Failed to marshal string from native code.\");\n")
                elif func.return_type == 'IntPtr':
                    f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                    if not skip_error_check:
                        f.write(f"        CheckError({context_param});\n")
                    f.write(f"        return CheckHandle(result, nameof({func.name}));\n")
                elif func.return_type == 'void':
                    f.write(f"        nativeLibrary.{func.name}({native_args_str});\n")
                    if not skip_error_check:
                        f.write(f"        CheckError({context_param});\n")
                elif func.return_type in enum_types:
                    # Enum return type - cast from internal enum to public enum
                    f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                    if not skip_error_check:
                        f.write(f"        CheckError({context_param});\n")
                    f.write(f"        return ({func.return_type})result;\n")
                else:
                    f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                    if not skip_error_check:
                        f.write(f"        CheckError({context_param});\n")
                    f.write(f"        return result;\n")

                f.write("    }\n\n")

            # Generate original overload with "Original" suffix (always, or only if no symbols)
            # Clone documentation (no overrides for IntPtr version)
            doc_clone = clone_and_modify_doc(func.doc_comment, func.name, func.parameters)
            doc_clone = filter_invalid_cref_references(doc_clone)

            # Render documentation
            doc_output = render_xml_node(doc_clone, "    ")
            f.write(doc_output + "\n")

            # Parameters - convert only Z3_string to string, keep Z3_symbol as IntPtr
            public_params = []
            for param in func.parameters:
                if param.c_type == 'Z3_string':
                    public_type = 'string'
                else:
                    public_type = param.csharp_type

                public_params.append(f"{public_type} {param.name}")

            # Determine public return type (convert Z3_string to string)
            public_return_type = 'string' if func.return_c_type == 'Z3_string' else func.return_type

            # Method signature - add "Original" suffix if this function has symbols
            method_name = f"{func.name}Original" if has_symbols and not skip_string_overload else func.name
            params_str = ", ".join(public_params)
            f.write(f"    public {public_return_type} {method_name}({params_str})\n")
            f.write("    {\n")

            # Convert string parameters to AnsiStringPtr
            string_params = [p for p in func.parameters if p.c_type == 'Z3_string']
            for param in string_params:
                f.write(f"        using var {param.name}Ansi = new AnsiStringPtr({param.name});\n")

            # Call native method
            native_args = []
            for param in func.parameters:
                # Check if parameter has 'out' modifier
                has_out_modifier = param.csharp_type.startswith('out ')
                param_name_only = param.name

                if param.c_type == 'Z3_string':
                    arg = f"{param.name}Ansi"
                elif param.csharp_type in enum_types:
                    # Enum parameter - cast to internal enum
                    arg = f"(NativeZ3Library.{param.csharp_type}){param.name}"
                else:
                    arg = param_name_only

                # Add 'out' keyword if needed
                if has_out_modifier:
                    native_args.append(f"out {arg}")
                else:
                    native_args.append(arg)

            native_args_str = ", ".join(native_args)

            # Check if this function should skip error checking
            # Skip for: IncRef/DecRef (reference counting)
            skip_error_check = func.name.endswith('IncRef') or func.name.endswith('DecRef')

            # Generate call based on return type
            context_param = func.parameters[0].name
            if func.return_c_type == 'Z3_string':
                # Z3_string return type - marshal to C# string
                f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                if not skip_error_check:
                    f.write(f"        CheckError({context_param});\n")
                f.write(f"        result = CheckHandle(result, nameof({func.name}));\n")
                f.write(f"        return Marshal.PtrToStringAnsi(result) ?? throw new InvalidOperationException(\"Failed to marshal string from native code.\");\n")
            elif func.return_type == 'IntPtr':
                f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                if not skip_error_check:
                    f.write(f"        CheckError({context_param});\n")
                f.write(f"        return CheckHandle(result, nameof({func.name}));\n")
            elif func.return_type == 'void':
                f.write(f"        nativeLibrary.{func.name}({native_args_str});\n")
                if not skip_error_check:
                    f.write(f"        CheckError({context_param});\n")
            elif func.return_type in enum_types:
                # Enum return type - cast from internal enum to public enum
                f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                if not skip_error_check:
                    f.write(f"        CheckError({context_param});\n")
                f.write(f"        return ({func.return_type})result;\n")
            else:
                f.write(f"        var result = nativeLibrary.{func.name}({native_args_str});\n")
                if not skip_error_check:
                    f.write(f"        CheckError({context_param});\n")
                f.write(f"        return result;\n")

            f.write("    }\n\n")

        f.write("}\n")

    return file_path


def generate_enums_file(output_dir: Path, enums: List[EnumDefinition]):
    """
    Generate Z3Library.Enums.generated.cs with actual public enum definitions.
    """
    file_path = output_dir / "Z3Library.Enums.generated.cs"

    with open(file_path, 'w', encoding='utf-8') as f:
        # Header
        f.write("// <auto-generated>\n")
        f.write("// This file was generated by scripts/generate_library.py\n")
        f.write("// Source: NativeZ3Library.Enums.generated.cs\n")
        f.write("// DO NOT EDIT - Changes will be overwritten\n")
        f.write("// </auto-generated>\n\n")

        f.write("namespace Spaceorc.Z3Wrap.Core;\n\n")

        f.write("public sealed partial class Z3Library\n")
        f.write("{\n")

        # Generate each enum
        for i, enum_def in enumerate(enums):
            # Add blank line between enums
            if i > 0:
                f.write("\n")

            # Enum summary - filter invalid cref references
            if enum_def.summary:
                f.write("    /// <summary>\n")
                for line in enum_def.summary.split('\n'):
                    # Convert <see cref="ExcludedMethod"/> to plain text
                    import re
                    def replace_see_cref(match):
                        cref_value = match.group(1)
                        method_name = cref_value.split('.')[-1].split('(')[0]
                        if method_name in CONVERT_CREF_TO_TEXT:
                            return method_name  # Plain text
                        return match.group(0)  # Keep original

                    line = re.sub(r'<see cref="([^"]+)"\s*/>', replace_see_cref, line)
                    f.write(f"    /// {line}\n" if line else "    ///\n")
                f.write("    /// </summary>\n")

            # See also references - filter out excluded functions
            for see_ref in enum_def.see_also:
                method_name = see_ref.split('.')[-1].split('(')[0]
                if method_name not in CONVERT_CREF_TO_TEXT:
                    f.write(f'    /// <seealso cref="{see_ref}"/>\n')

            # Enum declaration
            f.write(f"    public enum {enum_def.name}\n")
            f.write("    {\n")

            # Generate enum values
            for j, value in enumerate(enum_def.values):
                # Add blank line between values if there's documentation
                if j > 0 and (value.summary or value.remarks):
                    f.write("\n")

                # Value documentation
                if value.summary:
                    if value.remarks:
                        # Multi-line: summary + remarks
                        f.write("        /// <summary>\n")
                        for line in value.summary.split('\n'):
                            f.write(f"        /// {line}\n" if line else "        ///\n")
                        f.write("        /// </summary>\n")
                        f.write("        /// <remarks>\n")
                        for line in value.remarks.split('\n'):
                            f.write(f"        /// {line}\n" if line else "        ///\n")
                        f.write("        /// </remarks>\n")
                    else:
                        # Single-line summary
                        if '\n' in value.summary:
                            f.write("        /// <summary>\n")
                            for line in value.summary.split('\n'):
                                f.write(f"        /// {line}\n" if line else "        ///\n")
                            f.write("        /// </summary>\n")
                        else:
                            f.write(f"        /// <summary>{value.summary}</summary>\n")
                elif value.remarks:
                    # Only remarks
                    f.write("        /// <remarks>\n")
                    for line in value.remarks.split('\n'):
                        f.write(f"        /// {line}\n" if line else "        ///\n")
                    f.write("        /// </remarks>\n")

                # Value declaration
                f.write(f"        {value.name} = {value.value},\n")

            f.write("    }\n")

        f.write("}\n")

    return file_path


def main():
    """Main entry point."""
    import argparse

    # Parse command-line arguments
    parser = argparse.ArgumentParser(description='Generate Z3Library partial classes from NativeZ3Library')
    parser.add_argument('--enums-only', action='store_true', help='Generate only the enums file (faster)')
    args = parser.parse_args()

    # Paths
    script_dir = Path(__file__).parent
    project_root = script_dir.parent
    interop_dir = project_root / "Z3Wrap" / "Core" / "Interop"
    output_dir = project_root / "Z3Wrap" / "Core"

    print("Z3Library Generator")
    print("=" * 80)
    print(f"Source: {interop_dir}")
    print(f"Output: {output_dir}")
    print()

    # Clean up old generated files (unless enums-only mode)
    if not args.enums_only:
        print("Cleaning up old generated files...")
        old_files = list(output_dir.glob("*.generated.cs"))
        for old_file in old_files:
            old_file.unlink()
        print(f"Removed {len(old_files)} old generated files")
        print()

    # Parse native enums
    native_enums_file = interop_dir / "NativeZ3Library.Enums.generated.cs"
    print("Parsing NativeZ3Library enums...")
    enums = parse_native_enums_file(native_enums_file)
    print(f"✓ Found {len(enums)} enum definitions")

    # Extract enum type names for return type casting
    enum_types = {enum.name for enum in enums}

    for enum_def in enums:
        print(f"  - {enum_def.name} ({len(enum_def.values)} values)")
    print()

    # Generate enums file
    print("Generating Z3Library.Enums.generated.cs...")
    generated_enums_file = generate_enums_file(output_dir, enums)
    print(f"✓ Generated {generated_enums_file.name}")
    print()

    if args.enums_only:
        print("✅ Enums-only mode: Skipping function generation")
        print(f"ℹ️  Generated 1 file: {generated_enums_file.name}")
        return

    # Parse and generate function files
    print("Parsing NativeZ3Library function files...")
    native_function_files = sorted(interop_dir.glob("NativeZ3Library.*.generated.cs"))
    # Exclude the enums file
    native_function_files = [f for f in native_function_files if f.name != "NativeZ3Library.Enums.generated.cs"]

    print(f"✓ Found {len(native_function_files)} function files")
    print()

    generated_function_files = []
    total_functions = 0

    for native_file in native_function_files:
        # Extract group name from filename: NativeZ3Library.{GroupName}.generated.cs
        group_name = native_file.stem.replace("NativeZ3Library.", "").replace(".generated", "")

        print(f"Processing {group_name}...")

        # Parse functions
        functions = parse_native_functions_file(native_file)

        if functions:
            print(f"  ✓ Found {len(functions)} functions with Z3_context parameter")

            # Generate public wrapper file
            generated_file = generate_functions_file(output_dir, functions, group_name, enum_types)
            generated_function_files.append(generated_file.name)
            total_functions += len(functions)
        else:
            print(f"  ⊘ No functions with Z3_context parameter (skipped)")
        print()

    print("=" * 80)
    print(f"✅ Done! Generated {len(generated_function_files) + 1} files:")
    print(f"   - 1 enums file with {len(enums)} enums")
    print(f"   - {len(generated_function_files)} function files with {total_functions} methods")
    print(f"   Location: {output_dir}")


if __name__ == "__main__":
    main()
