#!/usr/bin/env python3
"""
Doxygen Integration Module

Provides structured access to Doxygen XML documentation for Z3 C API.
Handles preprocessing, Doxygen execution, XML parsing, and C# XML comment generation.
"""

import re
import subprocess
import tempfile
import xml.etree.ElementTree as ET
from dataclasses import dataclass, field
from pathlib import Path
from typing import List, Dict, Optional


# ============================================================================
# Data Classes
# ============================================================================

@dataclass
class ParamDoc:
    """Documentation for a single parameter."""
    name: str
    ctype: str  # Original C type
    description: str


@dataclass
class FunctionDoc:
    """Structured documentation for a function from Doxygen XML."""
    name: str  # Original C name (e.g., Z3_mk_int)
    brief: str
    params: List[ParamDoc] = field(default_factory=list)
    returns: str = ""
    body: str = ""  # Additional detail paragraphs
    preconditions: List[str] = field(default_factory=list)
    warnings: List[str] = field(default_factory=list)
    remarks: List[str] = field(default_factory=list)
    see_also: List[str] = field(default_factory=list)  # Z3 function names


@dataclass
class EnumValueDoc:
    """Documentation for a single enum value."""
    name: str  # Original C name (e.g., Z3_L_TRUE)
    description: str


@dataclass
class EnumDoc:
    """Structured documentation for an enum from Doxygen XML."""
    name: str  # Original C name (e.g., Z3_lbool)
    brief: str
    values: List[EnumValueDoc] = field(default_factory=list)
    see_also: List[str] = field(default_factory=list)  # Z3 function names


# ============================================================================
# XML Element Extraction
# ============================================================================

def extract_text_from_element(element, convert_refs_to_see_cref=False) -> str:
    """Extract all text content from an XML element and its children.

    Args:
        element: The XML element to extract text from
        convert_refs_to_see_cref: If True, convert <ref> to <see cref="..."/> and handle code blocks

    Returns:
        Extracted text with optional XML tags for .NET documentation
    """
    if element is None:
        return ""

    # Handle special cases where the element itself needs conversion
    if convert_refs_to_see_cref:
        if element.tag == 'ref':
            # Doxygen reference to another function/type
            ref_text = element.text if element.text else ""
            return f'<see cref="{ref_text}"/>'
        elif element.tag == 'computeroutput':
            # Inline code
            code_text = element.text if element.text else ""
            code_text = code_text.replace('‹ccode›', '')
            return f'<c>{code_text}</c>'
        elif element.tag == 'verbatim':
            # Code block (may contain nicebox marker)
            verbatim_text = element.text if element.text else ""
            # Check if this is a nicebox
            if '<!-- nicebox -->' in verbatim_text:
                # Remove the nicebox marker and format as nicebox
                verbatim_text = verbatim_text.replace('<!-- nicebox -->', '').strip()
                lines = verbatim_text.split('\n')
                # Find max width for box
                max_width = max(len(line) for line in lines) if lines else 0
                # Build nicebox
                box_lines = ['╔' + '═' * (max_width + 2) + '╗']
                for line in lines:
                    padded = line.ljust(max_width)
                    box_lines.append('║ ' + padded + ' ║')
                box_lines.append('╚' + '═' * (max_width + 2) + '╝')
                return '<code>\n' + '\n'.join(box_lines) + '\n</code>'
            else:
                # Regular verbatim - just wrap in code tags
                return f'<code>\n{verbatim_text}\n</code>'
        elif element.tag == 'programlisting':
            # Multi-line code block from Doxygen
            code_lines = []
            for codeline in element.findall('.//codeline'):
                # Extract text from highlight elements
                line_text = ''
                for elem in codeline.iter():
                    if elem.tag == 'sp':
                        line_text += ' '
                    elif elem.text:
                        line_text += elem.text
                    if elem.tail:
                        line_text += elem.tail
                if line_text.strip():
                    code_lines.append(line_text.strip())

            if code_lines:
                code_content = '\n'.join(code_lines)
                return f'<code>\n{code_content}\n</code>'
            return ''

    # Get text from this element and all children recursively
    text_parts = []
    if element.text:
        # Escape XML special characters in plain text
        escaped_text = element.text.strip()
        if convert_refs_to_see_cref:
            escaped_text = escaped_text.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
        text_parts.append(escaped_text)

    for child in element:
        # Check if this is a <ref> element (Doxygen reference to another function/type)
        if convert_refs_to_see_cref and child.tag == 'ref':
            ref_text = child.text if child.text else ""
            if ref_text:
                text_parts.append(f'<see cref="{ref_text}"/>')
            # Add tail text if present (escaped)
            if child.tail:
                tail_escaped = child.tail.strip().replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                text_parts.append(tail_escaped)
        # Check if this is a <computeroutput> element (inline code)
        elif convert_refs_to_see_cref and child.tag == 'computeroutput':
            code_text = child.text if child.text else ""
            if code_text:
                code_text = code_text.replace('‹ccode›', '')
                text_parts.append(f'<c>{code_text}</c>')
            # Add tail text if present (escaped)
            if child.tail:
                tail_escaped = child.tail.strip().replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                text_parts.append(tail_escaped)
        # Check if this is a <verbatim> element (code block)
        elif convert_refs_to_see_cref and child.tag == 'verbatim':
            verbatim_text = child.text if child.text else ""
            # Check if this is a nicebox
            if '<!-- nicebox -->' in verbatim_text:
                # Remove the nicebox marker and format as nicebox
                verbatim_text = verbatim_text.replace('<!-- nicebox -->', '').strip()
                lines = verbatim_text.split('\n')
                # Find max width for box
                max_width = max(len(line) for line in lines) if lines else 0
                # Build nicebox
                box_lines = ['╔' + '═' * (max_width + 2) + '╗']
                for line in lines:
                    padded = line.ljust(max_width)
                    box_lines.append('║ ' + padded + ' ║')
                box_lines.append('╚' + '═' * (max_width + 2) + '╝')
                text_parts.append('<code>\n' + '\n'.join(box_lines) + '\n</code>')
            else:
                # Regular verbatim - just wrap in code tags
                text_parts.append(f'<code>\n{verbatim_text}\n</code>')
            # Add tail text if present (escaped)
            if child.tail:
                tail_escaped = child.tail.strip().replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                text_parts.append(tail_escaped)
        # Check if this is a <programlisting> element
        elif convert_refs_to_see_cref and child.tag == 'programlisting':
            code_lines = []
            for codeline in child.findall('.//codeline'):
                line_text = ''
                for elem in codeline.iter():
                    if elem.tag == 'sp':
                        line_text += ' '
                    elif elem.text:
                        line_text += elem.text
                    if elem.tail:
                        line_text += elem.tail
                if line_text.strip():
                    code_lines.append(line_text.strip())
            if code_lines:
                code_content = '\n'.join(code_lines)
                text_parts.append(f'<code>\n{code_content}\n</code>')
            # Add tail text if present (escaped)
            if child.tail:
                tail_escaped = child.tail.strip().replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                text_parts.append(tail_escaped)
        else:
            child_text = extract_text_from_element(child, convert_refs_to_see_cref)
            if child_text:
                text_parts.append(child_text)
            if child.tail:
                tail_escaped = child.tail.strip().replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
                text_parts.append(tail_escaped)

    return ' '.join(text_parts)


# ============================================================================
# Doxygen XML Parsing
# ============================================================================

def parse_function_doc(xml_path: Path, func_name: str) -> Optional[FunctionDoc]:
    """Parse function documentation from Doxygen XML.

    Args:
        xml_path: Path to group__capi.xml file
        func_name: C function name (e.g., Z3_mk_int)

    Returns:
        FunctionDoc object or None if not found
    """
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this function
    for memberdef in root.findall(".//memberdef[@kind='function']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == func_name:
            doc = FunctionDoc(name=func_name, brief="")

            # Brief description
            brief_elem = memberdef.find("briefdescription")
            if brief_elem is not None:
                doc.brief = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)

            # Detailed description
            detail_elem = memberdef.find("detaileddescription")
            if detail_elem is not None:
                # Parameters from parameterlist
                param_list = detail_elem.find(".//parameterlist[@kind='param']")
                if param_list is not None:
                    for param_item in param_list.findall("parameteritem"):
                        param_name_elem = param_item.find(".//parametername")
                        param_name = param_name_elem.text if param_name_elem is not None else ""
                        param_desc_elem = param_item.find(".//parameterdescription")
                        param_desc = extract_text_from_element(param_desc_elem, convert_refs_to_see_cref=True) if param_desc_elem is not None else ""

                        if param_name:
                            # We'll set ctype later when caller provides it
                            doc.params.append(ParamDoc(name=param_name, ctype="", description=param_desc))

                # Returns
                return_sections = detail_elem.findall(".//simplesect[@kind='return']")
                if return_sections:
                    return_texts = []
                    for return_section in return_sections:
                        return_text = extract_text_from_element(return_section, convert_refs_to_see_cref=True)
                        if return_text:
                            return_texts.append(return_text)
                    doc.returns = ' '.join(return_texts)

                # Body (detail paragraphs after params, before special sections)
                body_parts = []
                for para in detail_elem.findall("para"):
                    has_paramlist = para.find("parameterlist") is not None
                    has_simplesect = para.find("simplesect") is not None

                    if not has_paramlist and not has_simplesect:
                        para_text = extract_text_from_element(para, convert_refs_to_see_cref=True)
                        # Skip def_API lines
                        if para_text and not para_text.startswith("def_API"):
                            # Skip paragraphs that are only see cref tags
                            text_without_refs = re.sub(r'<see cref=\"[^\"]+\"/>\s*', '', para_text).strip()
                            if text_without_refs:
                                body_parts.append(para_text)
                    elif has_paramlist:
                        # Extract text after parameterlist
                        paramlist = para.find("parameterlist")
                        if paramlist is not None:
                            remaining_text_parts = []
                            if paramlist.tail and paramlist.tail.strip():
                                remaining_text_parts.append(paramlist.tail.strip())
                            found_paramlist = False
                            for child in para:
                                if found_paramlist:
                                    if child.tag == 'simplesect':
                                        break
                                    child_text = extract_text_from_element(child, convert_refs_to_see_cref=True)
                                    if child_text:
                                        remaining_text_parts.append(child_text)
                                    if child.tail and child.tail.strip():
                                        remaining_text_parts.append(child.tail.strip())
                                if child == paramlist:
                                    found_paramlist = True
                            remaining_text = ' '.join(remaining_text_parts)
                            if remaining_text and not remaining_text.startswith("def_API"):
                                body_parts.append(remaining_text)

                if body_parts:
                    doc.body = ' '.join(body_parts)

                # Preconditions
                pre_sections = detail_elem.findall(".//simplesect[@kind='pre']")
                for pre_section in pre_sections:
                    pre_text = extract_text_from_element(pre_section, convert_refs_to_see_cref=True)
                    if pre_text:
                        doc.preconditions.append(pre_text)

                # Warnings
                warning_sections = detail_elem.findall(".//simplesect[@kind='warning']")
                for warning_section in warning_sections:
                    warning_text = extract_text_from_element(warning_section, convert_refs_to_see_cref=True)
                    if warning_text:
                        doc.warnings.append(warning_text)

                # Remarks
                remark_sections = detail_elem.findall(".//simplesect[@kind='remark']")
                for remark_section in remark_sections:
                    remark_text = extract_text_from_element(remark_section, convert_refs_to_see_cref=True)
                    if remark_text:
                        doc.remarks.append(remark_text)

                # See also
                see_sections = detail_elem.findall(".//simplesect[@kind='see']")
                for see_section in see_sections:
                    refs = see_section.findall(".//ref")
                    for ref in refs:
                        ref_text = ref.text if ref.text else ""
                        if ref_text:
                            doc.see_also.append(ref_text)

            return doc

    return None


def parse_enum_doc(xml_path: Path, enum_name: str) -> Optional[EnumDoc]:
    """Parse enum documentation from Doxygen XML.

    Args:
        xml_path: Path to group__capi.xml file
        enum_name: C enum name (e.g., Z3_lbool)

    Returns:
        EnumDoc object or None if not found
    """
    tree = ET.parse(xml_path)
    root = tree.getroot()

    # Find the memberdef for this enum
    for memberdef in root.findall(".//memberdef[@kind='enum']"):
        name_elem = memberdef.find("name")
        if name_elem is not None and name_elem.text == enum_name:
            doc = EnumDoc(name=enum_name, brief="")

            # Brief description
            brief_elem = memberdef.find("briefdescription")
            if brief_elem is not None:
                doc.brief = extract_text_from_element(brief_elem, convert_refs_to_see_cref=True)

            # Parse value descriptions from detaileddescription
            detail_elem = memberdef.find("detaileddescription")
            value_descriptions = _parse_enum_value_descriptions(detail_elem)

            # Extract values from enumvalue elements
            for enumvalue in memberdef.findall("enumvalue"):
                value_name_elem = enumvalue.find("name")
                if value_name_elem is not None:
                    value_name = value_name_elem.text
                    description = value_descriptions.get(value_name, "")
                    doc.values.append(EnumValueDoc(name=value_name, description=description))

            # See also references
            if detail_elem is not None:
                see_sections = detail_elem.findall(".//simplesect[@kind='see']")
                for see_section in see_sections:
                    refs = see_section.findall(".//ref")
                    for ref in refs:
                        ref_text = ref.text if ref.text else ""
                        if ref_text:
                            doc.see_also.append(ref_text)

            return doc

    return None


def _parse_enum_value_descriptions(detail_elem) -> Dict[str, str]:
    """Parse itemizedlist in detaileddescription to extract per-value descriptions.

    Returns:
        dict: Mapping from enum value name to description text
    """
    value_descriptions = {}

    if detail_elem is None:
        return value_descriptions

    # Find itemizedlist elements
    for itemlist in detail_elem.findall('.//itemizedlist'):
        for listitem in itemlist.findall('listitem'):
            # Each listitem may have multiple para elements
            # The first para should start with the enum value name
            paras = listitem.findall('para')
            if not paras:
                continue

            # Extract text from first para to get the value name
            first_para = paras[0]
            first_para_text = first_para.text or ''

            # Pattern: "Z3_VALUE_NAME: description text" or "Z3_VALUE_NAME description"
            match = re.match(r'^(Z3_[A-Z_0-9]+):?\s*', first_para_text)
            if match:
                value_name = match.group(1)

                # Extract full content from ALL paras in this listitem
                all_para_texts = []
                for para in paras:
                    para_text = extract_text_from_element(para, convert_refs_to_see_cref=True)
                    if para_text:
                        all_para_texts.append(para_text)

                # Join all paragraphs
                full_text = ' '.join(all_para_texts)

                # Remove the value name prefix from the beginning
                description = re.sub(r'^' + re.escape(value_name) + r':?\s*', '', full_text).strip()

                if description:
                    value_descriptions[value_name] = description

    return value_descriptions


# ============================================================================
# Doxygen Execution
# ============================================================================

def preprocess_headers_for_doxygen(headers_dir: Path, output_dir: Path) -> Path:
    """Preprocess Z3 headers to convert custom Doxygen commands to standard ones.

    Converts:
    - \\nicebox{...} to \\verbatim with <!-- nicebox --> marker
    - \\ccode{text} to <tt>‹ccode›text</tt>

    Args:
        headers_dir: Directory containing Z3 header files
        output_dir: Output directory for Doxygen files

    Returns:
        Path to preprocessed headers directory
    """
    preprocessed_dir = output_dir / "preprocessed_headers"
    preprocessed_dir.mkdir(parents=True, exist_ok=True)

    # Process each header file
    for header_file in headers_dir.glob("*.h"):
        with open(header_file, 'r', encoding='utf-8') as f:
            content = f.read()

        # Convert \nicebox{...} to \verbatim with marker inside
        content = re.sub(r'\\nicebox\{', r'\\verbatim\n<!-- nicebox -->\n', content)
        # Replace closing } with \endverbatim
        content = re.sub(r'(\\verbatim\n<!-- nicebox -->.*?)\n\s*\}', r'\1\n\\endverbatim', content, flags=re.DOTALL)

        # Convert \ccode{text} to <tt> with marker prefix
        content = re.sub(r'\\ccode\{([^}]+)\}', r'<tt>‹ccode›\1</tt>', content)

        # Write preprocessed file
        output_file = preprocessed_dir / header_file.name
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(content)

    return preprocessed_dir


def run_doxygen(headers_dir: Path, output_dir: Path) -> Path:
    """Run Doxygen to generate XML documentation.

    Args:
        headers_dir: Directory containing Z3 header files
        output_dir: Output directory for Doxygen XML

    Returns:
        Path to the generated XML directory

    Raises:
        FileNotFoundError: If Doxygen is not installed
        subprocess.CalledProcessError: If Doxygen fails
    """
    # Check if Doxygen is installed
    try:
        subprocess.run(['doxygen', '--version'], capture_output=True, check=True)
    except FileNotFoundError:
        raise FileNotFoundError(
            "Doxygen not found. Please install it:\n"
            "  macOS:  brew install doxygen\n"
            "  Linux:  sudo apt-get install doxygen"
        )

    # Preprocess headers
    print(f"Preprocessing headers to convert custom Doxygen commands...")
    preprocessed_dir = preprocess_headers_for_doxygen(headers_dir, output_dir)
    print(f"✓ Preprocessed headers in {preprocessed_dir}")

    # Create output directory for Doxygen XML
    doxygen_xml_dir = output_dir / "xml"
    doxygen_xml_dir.mkdir(parents=True, exist_ok=True)

    # Create a Doxyfile configuration
    doxyfile_content = f"""
INPUT = {preprocessed_dir}
OUTPUT_DIRECTORY = {output_dir}
GENERATE_XML = YES
GENERATE_HTML = NO
GENERATE_LATEX = NO
OPTIMIZE_OUTPUT_FOR_C = YES
EXTRACT_ALL = YES
EXTRACT_STATIC = YES
RECURSIVE = NO
FILE_PATTERNS = *.h
QUIET = YES
WARNINGS = NO
"""

    # Write Doxyfile to a temporary file
    with tempfile.NamedTemporaryFile(mode='w', suffix='.doxyfile', delete=False) as f:
        doxyfile_path = f.name
        f.write(doxyfile_content)

    try:
        # Run Doxygen
        print(f"Running Doxygen on headers in {headers_dir}...")
        result = subprocess.run(
            ['doxygen', doxyfile_path],
            capture_output=True,
            text=True,
            check=True
        )

        if result.stderr:
            print(f"Doxygen stderr: {result.stderr}")

        print(f"✓ Generated Doxygen XML in {doxygen_xml_dir}")

        # List generated XML files
        xml_files = sorted(doxygen_xml_dir.glob("*.xml"))
        print(f"  Generated {len(xml_files)} XML files")

        return doxygen_xml_dir

    except subprocess.CalledProcessError as e:
        print(f"ERROR: Doxygen failed with exit code {e.returncode}")
        print(f"stdout: {e.stdout}")
        print(f"stderr: {e.stderr}")
        raise
    finally:
        # Clean up temporary Doxyfile
        import os
        os.unlink(doxyfile_path)


def validate_doxygen_xml(xml_dir: Path) -> Path:
    """Validate that Doxygen XML exists and return path to group__capi.xml.

    Args:
        xml_dir: Directory containing Doxygen XML files

    Returns:
        Path to group__capi.xml

    Raises:
        FileNotFoundError: If XML directory or group__capi.xml doesn't exist
    """
    if not xml_dir.exists():
        raise FileNotFoundError(
            f"Doxygen XML directory not found: {xml_dir}\n"
            f"Please run Doxygen first or check the path."
        )

    group_capi_xml = xml_dir / "group__capi.xml"
    if not group_capi_xml.exists():
        raise FileNotFoundError(
            f"group__capi.xml not found in {xml_dir}\n"
            f"Please ensure Doxygen has generated the XML files."
        )

    return group_capi_xml


# ============================================================================
# C# XML Comment Generation
# ============================================================================

def format_xml_doc_lines(text: str, indent: str = "    ") -> List[str]:
    """Format text as XML documentation lines, preserving original line breaks.

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


def generate_function_xml_doc(
    doc: FunctionDoc,
    param_name_map: Optional[Dict[str, str]] = None,
    csharp_name_converter: Optional[callable] = None,
    indent: str = "    "
) -> List[str]:
    """Generate C# XML documentation for a function.

    Args:
        doc: FunctionDoc object with parsed documentation
        param_name_map: Optional mapping of actual param names to doc param names
        csharp_name_converter: Optional function to convert Z3 names to C# names
        indent: Indentation prefix for XML comment lines

    Returns:
        List of XML comment lines (including /// prefix)
    """
    lines = []

    # Apply parameter name mapping if provided
    if param_name_map:
        # Update param names in doc.params to match actual parameter names
        param_docs_by_doc_name = {p.name: p for p in doc.params}
        mapped_params = []
        for actual_name, doc_name in param_name_map.items():
            if doc_name in param_docs_by_doc_name:
                param_doc = param_docs_by_doc_name[doc_name]
                # Create new ParamDoc with actual name
                mapped_params.append(ParamDoc(
                    name=actual_name,
                    ctype=param_doc.ctype,
                    description=param_doc.description
                ))
        # Keep params that weren't in the mapping
        for param in doc.params:
            if param.name not in [d for a, d in param_name_map.items()]:
                mapped_params.append(param)
        doc.params = mapped_params

    # Helper to convert see cref names
    def convert_see_cref(text: str) -> str:
        if not csharp_name_converter:
            return text
        # Replace Z3 function names in see cref tags
        def replace_cref(match):
            z3_name = match.group(1)
            csharp_name = csharp_name_converter(z3_name)
            return f'<see cref="{csharp_name}"/>'
        return re.sub(r'<see cref="(Z3_\w+)"/>', replace_cref, text)

    # Summary (brief description)
    if doc.brief:
        lines.append(f"{indent}/// <summary>")
        brief_converted = convert_see_cref(doc.brief)
        for line in format_xml_doc_lines(brief_converted, indent):
            lines.append(line)
        lines.append(f"{indent}/// </summary>")

    # Parameters
    for param in doc.params:
        param_desc = param.description if param.description else f"{param.ctype} parameter"
        param_desc_converted = convert_see_cref(param_desc)

        if '\n' in param_desc_converted:
            # Multi-line
            lines.append(f'{indent}/// <param name="{param.name}" ctype="{param.ctype}">')
            for line in format_xml_doc_lines(param_desc_converted, indent):
                lines.append(line)
            lines.append(f'{indent}/// </param>')
        else:
            # Single line
            lines.append(f'{indent}/// <param name="{param.name}" ctype="{param.ctype}">{param_desc_converted}</param>')

    # Returns
    if doc.returns:
        returns_converted = convert_see_cref(doc.returns)
        if '\n' in returns_converted:
            lines.append(f"{indent}/// <returns>")
            for line in format_xml_doc_lines(returns_converted, indent):
                lines.append(line)
            lines.append(f"{indent}/// </returns>")
        else:
            lines.append(f"{indent}/// <returns>{returns_converted}</returns>")

    # Remarks (body, preconditions, warnings, remarks)
    remarks_parts = []
    if doc.body:
        remarks_parts.append(convert_see_cref(doc.body))
    if doc.preconditions:
        for pre in doc.preconditions:
            remarks_parts.append(f"Precondition: {convert_see_cref(pre)}")
    if doc.warnings:
        for warning in doc.warnings:
            remarks_parts.append(f"Warning: {convert_see_cref(warning)}")
    if doc.remarks:
        remarks_parts.extend([convert_see_cref(r) for r in doc.remarks])

    if remarks_parts:
        lines.append(f"{indent}/// <remarks>")
        for remark in remarks_parts:
            # Handle code blocks specially
            if '<code>' in remark and '</code>' in remark:
                parts = remark.split('<code>')
                for i, part in enumerate(parts):
                    if i == 0:
                        if part.strip():
                            for line in format_xml_doc_lines(part, indent):
                                lines.append(line)
                    else:
                        if '</code>' in part:
                            code_part, after_code = part.split('</code>', 1)
                            lines.append(f"{indent}/// <code>")
                            for line in code_part.strip().split('\n'):
                                lines.append(f"{indent}/// {line}")
                            lines.append(f"{indent}/// </code>")
                            if after_code.strip():
                                for line in format_xml_doc_lines(after_code.strip(), indent):
                                    lines.append(line)
            else:
                for line in format_xml_doc_lines(remark, indent):
                    lines.append(line)
        lines.append(f"{indent}/// </remarks>")

    # See also references
    for sa_func in doc.see_also:
        sa_csharp = csharp_name_converter(sa_func) if csharp_name_converter else sa_func
        lines.append(f'{indent}/// <seealso cref="{sa_csharp}"/>')

    return lines


def generate_enum_xml_doc(
    doc: EnumDoc,
    csharp_name_converter: Optional[callable] = None,
    indent: str = "    "
) -> List[str]:
    """Generate C# XML documentation for an enum.

    Args:
        doc: EnumDoc object with parsed documentation
        csharp_name_converter: Optional function to convert Z3 names to C# names
        indent: Indentation prefix for XML comment lines

    Returns:
        List of XML comment lines for the enum declaration
    """
    lines = []

    # Helper to convert see cref names
    def convert_see_cref(text: str) -> str:
        if not csharp_name_converter:
            return text
        # Replace Z3 function names in see cref tags
        def replace_cref(match):
            z3_name = match.group(1)
            csharp_name = csharp_name_converter(z3_name)
            return f'<see cref="{csharp_name}"/>'
        return re.sub(r'<see cref="(Z3_\w+)"/>', replace_cref, text)

    # Enum summary
    lines.append(f"{indent}/// <summary>")
    if doc.brief:
        brief_converted = convert_see_cref(doc.brief)
        for line in format_xml_doc_lines(brief_converted, indent):
            lines.append(line)
    else:
        lines.append(f"{indent}/// {doc.name}")
    lines.append(f"{indent}/// </summary>")

    # See also references
    for sa_func in doc.see_also:
        sa_csharp = csharp_name_converter(sa_func) if csharp_name_converter else sa_func
        lines.append(f'{indent}/// <seealso cref="{sa_csharp}"/>')

    return lines


def generate_enum_value_xml_doc(
    value: EnumValueDoc,
    csharp_name_converter: Optional[callable] = None,
    indent: str = "        "
) -> List[str]:
    """Generate C# XML documentation for an enum value.

    Args:
        value: EnumValueDoc object with parsed documentation
        csharp_name_converter: Optional function to convert Z3 names to C# names
        indent: Indentation prefix for XML comment lines

    Returns:
        List of XML comment lines for the enum value
    """
    lines = []

    # Helper to convert see cref names
    def convert_see_cref(text: str) -> str:
        if not csharp_name_converter:
            return text
        # Replace Z3 function names in see cref tags
        def replace_cref(match):
            z3_name = match.group(1)
            csharp_name = csharp_name_converter(z3_name)
            return f'<see cref="{csharp_name}"/>'
        return re.sub(r'<see cref="(Z3_\w+)"/>', replace_cref, text)

    if value.description:
        description_converted = convert_see_cref(value.description)
        lines.append(f"{indent}/// <summary>")
        for line in format_xml_doc_lines(description_converted, indent):
            lines.append(line)
        lines.append(f"{indent}/// </summary>")
    else:
        # Simple one-line documentation with just the name
        lines.append(f"{indent}/// <summary>{value.name}</summary>")

    return lines
